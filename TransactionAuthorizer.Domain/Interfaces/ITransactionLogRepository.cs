using TransactionAuthorizer.Domain.Entities;

namespace TransactionAuthorizer.Domain.Interfaces;

public interface ITransactionLogRepository
{
    Task AddTransactionLogAsync(TransactionLogDomain log);
}