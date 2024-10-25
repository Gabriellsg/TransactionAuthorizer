using Microsoft.Extensions.Logging;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Enums;
using TransactionAuthorizer.Domain.Exceptions;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Domain.Services;

public sealed class AuthorizerService(
    IAccountRepository accountRepository,
    IBenefitCategoryRepository benefitCategoryRepository,
    ILogger<AuthorizerService> logger,
    SemaphoreSlim semaphore) : IAuthorizerService
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IBenefitCategoryRepository _benefitCategoryRepository = benefitCategoryRepository;
    private readonly ILogger<AuthorizerService> _logger = logger;
    private readonly SemaphoreSlim _semaphore = semaphore;

    public async Task<AuthorizationCode> AuthorizeAsync(TransactionDomain transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        if (transaction.TotalAmount <= 0)
            throw new InvalidTransactionAmountException($"Total amount {transaction.TotalAmount} must be greater than zero.");

        await _semaphore.WaitAsync();

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
            return AuthorizationCode.Approved;
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
}