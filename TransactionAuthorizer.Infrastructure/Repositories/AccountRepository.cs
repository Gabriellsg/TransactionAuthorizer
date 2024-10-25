using Dapper;
using System.Data;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Infrastructure.Repositories;

public sealed class AccountRepository(IDbConnection dbConnection) : IAccountRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<AccountDomain?> GetAccountAsync(string accountNumber)
    {
        var query = @"SELECT 
                        ACCOUNTNUMBER, 
                        BALANCEFOOD, 
                        BALANCEMEAL, 
                        BALANCECASH 
                      FROM Account 
                      WHERE AccountNumber = @AccountNumber";

        return await _dbConnection
            .QueryFirstOrDefaultAsync<AccountDomain>(query, new { AccountNumber = accountNumber });
    }

    public async Task UpdateAccountBalanceAsync(string accountNumber, int benefitCategoryId, decimal amount)
    {
        var updateQuery = @"UPDATE ACCOUNT 
                            SET Balance = Balance - @Amount 
                            WHERE AccountNumber = @AccountNumber AND BenefitCategoryId = @BenefitCategoryId";

        await _dbConnection.ExecuteAsync(updateQuery, new
        {
            Amount = amount,
            AccountNumber = accountNumber,
            BenefitCategoryId = benefitCategoryId
        });
    }

    public async Task InitializeBalancesAsync(Dictionary<string, decimal> initialBalances)
    {
        foreach (var balance in initialBalances)
        {
            var query = @"INSERT INTO ACCOUNT 
                            (AccountNumber, BalanceFood, BalanceMeal, BalanceCash) 
                          VALUES 
                            (@AccountNumber, @BalanceFood, @BalanceMeal, @BalanceCash) 
                          ON DUPLICATE KEY UPDATE BalanceFood = @BalanceFood, BalanceMeal = @BalanceMeal, BalanceCash = @BalanceCash";

            await _dbConnection.ExecuteAsync(query, new
            {
                AccountNumber = balance.Key,
                BalanceFood = balance.Value,
                BalanceMeal = balance.Value,
                BalanceCash = balance.Value
            });
        }
    }

    public async Task<decimal> GetBenefitBalanceAsync(string accountNumber, BenefitCategoryDomain category)
    {
        string balanceColumn = category.Id switch
        {
            1 => "BALANCEFOOD",
            2 => "BALANCEMEAL",
            3 => "BALANCECASH",
            _ => throw new ArgumentException($"Benefit category with ID {category.Id} not found"),
        };

        var query = $@"
                    SELECT {balanceColumn} 
                    FROM ACCOUNT 
                    WHERE ACCOUNTNUMBER = @AccountNumber";

        return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(query, new { AccountNumber = accountNumber });
    }
}