namespace TransactionAuthorizer.Infrastructure.Interfaces;

public interface ITransactionRepository
{
    Dictionary<string, decimal> GetBenefitBalances();
}