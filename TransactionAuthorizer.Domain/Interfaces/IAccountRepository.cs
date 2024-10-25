using TransactionAuthorizer.Domain.Entities;

namespace TransactionAuthorizer.Domain.Interfaces;

public interface IAccountRepository
{
    Task<AccountDomain?> GetAccountAsync(string accountNumber);
    Task UpdateAccountBalanceAsync(string accountNumber, int benefitCategoryId, decimal amount);
    Task InitializeBalancesAsync(Dictionary<string, decimal> initialBalances);
    Task<decimal> GetBenefitBalanceAsync(string accountNumber, BenefitCategoryDomain category);
}