using Dapper;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed record TransactionLogRepository(IDbConnection DbConnection) : ITransactionLogRepository
{
    private readonly IDbConnection _dbConnection = DbConnection;

    public async Task AddTransactionLogAsync(TransactionLogDomain log)
    {
        var query = @"
            INSERT INTO TRANSACTION_LOG 
                (ACCOUNT_ID, AMOUNT, MERCHANT, MCC_CODE, TRANSACTION_DATE, AUTHORIZATION_CODE)
            VALUES 
                (@AccountId, @Amount, @MerchantName,  @MerchantCategoryCode, @TransactionDate, @AuthorizationCode)";

        await _dbConnection.ExecuteAsync(query, log);
    }
}