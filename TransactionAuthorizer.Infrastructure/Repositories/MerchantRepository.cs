using Dapper;
using System.Data;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Infrastructure.Repositories;

public class MerchantRepository(IDbConnection dbConnection) : IMerchantRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<MerchantDomain?> GetMerchantByNameAsync(string merchantName)
    {
        var query = @"SELECT 
                          ID, 
                          NAME, 
                          MCC_CODE AS MerchantCategoryCode
                      FROM MERCHANT
                      WHERE NAME = @MerchantName";

        return await _dbConnection.QueryFirstOrDefaultAsync<MerchantDomain>(
            query, new { MerchantName = merchantName });
    }
}