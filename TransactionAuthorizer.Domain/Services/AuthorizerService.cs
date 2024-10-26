using Microsoft.Extensions.Logging;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Enums;
using TransactionAuthorizer.Domain.Exceptions;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Domain.Services;

public sealed class AuthorizerService(
    ILogger<AuthorizerService> logger,
    IAccountRepository accountRepository,
    IBenefitCategoryRepository benefitCategoryRepository,
    ITransactionLogRepository transactionLogRepository,
    IMerchantRepository merchantRepository,
    SemaphoreSlim semaphore) : IAuthorizerService
{
    private readonly ILogger<AuthorizerService> _logger = logger;
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IBenefitCategoryRepository _benefitCategoryRepository = benefitCategoryRepository;
    private readonly ITransactionLogRepository _transactionLogRepository = transactionLogRepository;
    private readonly IMerchantRepository _merchantRepository = merchantRepository;
    private readonly SemaphoreSlim _semaphore = semaphore;

    public async Task<AuthorizationResponseDomain> AuthorizeAsync(TransactionDomain transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        if (transaction.TotalAmount <= 0)
            throw new InvalidTransactionAmountException($"Total amount {transaction.TotalAmount} must be greater than zero.");

        await _semaphore.WaitAsync();

        AccountDomain? account = null;

        try
        {
            account = await _accountRepository.GetAccountAsync(transaction.AccountNumber) ??
                throw new NonExistentAccountException($"There is no account for the number {transaction.AccountNumber} provided");

            var merchantCategoryCode = transaction.MerchantCategoryCode;
            var merchant = await _merchantRepository.GetMerchantByNameAsync(transaction.MerchantName);

            if (merchant != null)
            {
                merchantCategoryCode = merchant.MerchantCategoryCode;
                _logger.LogInformation("Merchant-specific MCC applied for {MerchantName}: {MccCode}", transaction.MerchantName, merchantCategoryCode);
            }

            var benefitCategory = await _benefitCategoryRepository.GetBenefitCategoryAsync(merchantCategoryCode) ??
                                  await _benefitCategoryRepository.GetDefaultBenefitCategoryAsync();

            _logger.LogInformation("Get the balance for the account");
            var (balance, benefity) = GetBenefitBalance(transaction.AccountNumber, benefitCategory, transaction.TotalAmount);

            benefitCategory = benefitCategory with { Id = benefity.Id, Balance = balance, Name = benefity.Name };

            if (balance < transaction.TotalAmount)
            {
                _logger.LogWarning("Insufficient balance for category: {Category}.", benefitCategory);
                await SaveTransactionLogAsync(account, transaction, (int)AuthorizationCode.InsufficientBalance);
                return new AuthorizationResponseDomain(((int)AuthorizationCode.InsufficientBalance).ToString("D2"));
            }

            _logger.LogInformation("Update the balance...");
            await _accountRepository.UpdateAccountBalanceAsync(transaction.AccountNumber, benefitCategory.Id, transaction.TotalAmount);

            _logger.LogInformation("Transaction authorized for category: {Category}.", benefitCategory);
            await SaveTransactionLogAsync(account, transaction, (int)AuthorizationCode.Approved);

            return new AuthorizationResponseDomain(((int)AuthorizationCode.Approved).ToString("D2"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authorizing transaction for account: {Account}.", transaction.AccountNumber);
            await SaveTransactionLogAsync(account, transaction, (int)AuthorizationCode.OtherError);
            return new AuthorizationResponseDomain(((int)AuthorizationCode.OtherError).ToString("D2"));
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private (decimal, BenefitCategoryDomain) GetBenefitBalance(string accountNumber, BenefitCategoryDomain benefitCategory, decimal totalAmount)
    {
        var balance = _accountRepository.GetBenefitBalanceAsync(accountNumber, benefitCategory).Result;

        if (balance < totalAmount)
        {
            benefitCategory = _benefitCategoryRepository.GetDefaultBenefitCategoryAsync().Result;
            balance = _accountRepository.GetBenefitBalanceAsync(accountNumber, benefitCategory).Result;
        }

        return (balance, benefitCategory);
    }

    private async Task SaveTransactionLogAsync(AccountDomain? account, TransactionDomain transaction, int authorizationCode)
    {
        var log = new TransactionLogDomain(
            Id: 0,
            AccountId: account?.Id,
            Amount: transaction.TotalAmount,
            MerchantName: transaction.MerchantName,
            MerchantCategoryCode: transaction.MerchantCategoryCode,
            TransactionDate: DateTime.Now,
            AuthorizationCode: authorizationCode);

        await _transactionLogRepository.AddTransactionLogAsync(log);
    }
}