using TransactionAuthorizer.Infrastructure.Interfaces;

namespace TransactionAuthorizer.Infrastructure.Repositories;

public sealed record TransactionRepository : ITransactionRepository
{
    public Dictionary<string, decimal> GetBenefitBalances()
    {
        return new()
            {
                {"Food", 1000},
                {"Meal", 500},
                {"Cash", 2000}
            };
    }
}
