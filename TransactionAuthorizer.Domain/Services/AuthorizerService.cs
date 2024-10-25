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
    SemaphoreSlim semaphore) : IAuthorizerService
{
    private readonly ILogger<AuthorizerService> _logger = logger;
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IBenefitCategoryRepository _benefitCategoryRepository = benefitCategoryRepository;
    private readonly ITransactionLogRepository _transactionLogRepository = transactionLogRepository;
    private readonly SemaphoreSlim _semaphore = semaphore;

    public async Task<AuthorizationCode> AuthorizeAsync(TransactionDomain transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        if (transaction.TotalAmount <= 0)
            throw new InvalidTransactionAmountException($"Total amount {transaction.TotalAmount} must be greater than zero.");

        await _semaphore.WaitAsync();

        var account = await _accountRepository.GetAccountAsync(transaction.AccountNumber) ??
            throw new NonExistentAccountException($"There is no account for the number {transaction.AccountNumber} provided");

        try
        {
            var benefitCategory = await _benefitCategoryRepository.GetBenefitCategoryAsync(transaction.MerchantCategoryCode) ??
                await _benefitCategoryRepository.GetDefaultBenefitCategoryAsync();

            _logger.LogInformation("Get the balance for the account");
            var balance = await _accountRepository.GetBenefitBalanceAsync(transaction.AccountNumber, benefitCategory);

            if (balance < transaction.TotalAmount)
            {
                _logger.LogWarning("Insufficient balance for category: {Category}.", benefitCategory);
                throw new InsufficientBalanceException("The balance for the benefit category is insufficient to authorize the transaction.");
            }

            _logger.LogInformation("Update the balance...");
            await _accountRepository.UpdateAccountBalanceAsync(transaction.AccountNumber, benefitCategory.Id, transaction.TotalAmount);

            _logger.LogInformation("Transaction authorized for category: {Category}.", benefitCategory);

            await SaveTransactionLogAsync(account, transaction, nameof(AuthorizationCode.Approved));

            return AuthorizationCode.Approved;
        }
        catch (InsufficientBalanceException ex)
        {
            await SaveTransactionLogAsync(account, transaction, nameof(AuthorizationCode.InsufficientBalance));

            throw new InvalidTransactionException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authorizing transaction for account: {Account}.", transaction.AccountNumber);
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task SaveTransactionLogAsync(AccountDomain account, TransactionDomain transaction, string authorizationCode)
    {
        var log = new TransactionLogDomain(
            Id: 0,
            AccountId: account.Id,
            Amount: transaction.TotalAmount,
            MerchantName: transaction.MerchantName,
            MerchantCategoryCode: transaction.MerchantCategoryCode,
            TransactionDate: DateTime.Now,
            AuthorizationCode: authorizationCode);

        await _transactionLogRepository.AddTransactionLogAsync(log);
    }
}