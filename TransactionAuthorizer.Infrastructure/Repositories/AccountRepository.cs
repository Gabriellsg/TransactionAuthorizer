using Dapper;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class AccountRepository(IDbConnection dbConnection) : IAccountRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<AccountDomain?> GetAccountAsync(string accountNumber)
    {
        var query = @"SELECT 
                          ID,  
                          ACCOUNT_NUMBER AS AccountNumber, 
                          BALANCE_FOOD AS BalanceFood, 
                          BALANCE_MEAL AS BalanceMeal, 
                          BALANCE_CASH AS BalanceCash 
                      FROM ACCOUNT 
                      WHERE ACCOUNT_NUMBER = @AccountNumber";

        return await _dbConnection
            .QueryFirstOrDefaultAsync<AccountDomain>(query, new { AccountNumber = accountNumber });
    }

    public async Task UpdateAccountBalanceAsync(string accountNumber, int benefitCategoryId, decimal amount)
    {
        string balanceColumn = SelectCategory(benefitCategoryId);

        var updateQuery = $@"UPDATE ACCOUNT 
                             SET {balanceColumn} = {balanceColumn} - @Amount 
                             WHERE ACCOUNT_NUMBER = @AccountNumber";

        await _dbConnection.ExecuteAsync(updateQuery, new
        {
            Amount = amount,
            AccountNumber = accountNumber
        });
    }

    public async Task<decimal> GetBenefitBalanceAsync(string accountNumber, BenefitCategoryDomain category)
    {
        string balanceColumn = SelectCategory(category.Id);

        var query = $@"
                    SELECT {balanceColumn} 
                    FROM ACCOUNT 
                    WHERE ACCOUNT_NUMBER = @AccountNumber";

        return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(query, new { AccountNumber = accountNumber });
    }

    private static string SelectCategory(int benefitCategoryId) =>
        benefitCategoryId switch
        {
            1 => "BALANCE_FOOD",
            2 => "BALANCE_MEAL",
            3 => "BALANCE_CASH",
            _ => throw new ArgumentException($"Invalid benefit category ID."),
        };
}