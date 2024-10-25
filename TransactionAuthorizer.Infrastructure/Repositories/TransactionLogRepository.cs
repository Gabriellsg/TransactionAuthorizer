using Dapper;
using System.Data;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Infrastructure.Repositories;

public sealed record TransactionLogRepository(IDbConnection dbConnection) : ITransactionLogRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task AddTransactionLogAsync(TransactionLogDomain log)
    {
        var query = @"
            INSERT INTO TRANSACTION_LOG 
                (ACCOUNT_NUMBER, TRANSACTION_DATE, AMOUNT, BENEFIT_CATEGORY_ID, TRANSACTION_STATUS)
            VALUES 
                (@AccountNumber, @TransactionDate, @Amount, @BenefitCategoryId, @TransactionStatus)";

        await _dbConnection.ExecuteAsync(query, log);
    }

    public async Task<TransactionLogDomain?> GetTransactionLogAsync(int id)
    {
        var query = @"SELECT 
                        ACCOUNT_NUMBER, 
                        TRANSACTION_DATE, 
                        AMOUNT, 
                        BENEFIT_CATEGORY_ID, 
                        TRANSACTION_STATUS 
                      FROM TRANSACTION_LOG 
                      WHERE ID = @Id";

        return await _dbConnection.QueryFirstOrDefaultAsync<TransactionLogDomain>(query, new { Id = id });
    }
}