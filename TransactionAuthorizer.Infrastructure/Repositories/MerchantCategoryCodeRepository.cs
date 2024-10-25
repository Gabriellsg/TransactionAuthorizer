using Dapper;
using System.Data;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Infrastructure.Repositories;

public sealed class MerchantCategoryCodeRepository(IDbConnection dbConnection) : IMerchantCategoryCodeRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<MerchantCategoryCodeDomain?> GetMerchantCategoryCodeAsync(string merchantCategoryCode)
    {
        var query = @"SELECT 
                        bc.ID, 
                        bc.NAME, 
                        bc.BALANCE 
                      FROM MERCHANT_CATEGORY_CODE mcc
                      INNER JOIN BENEFIT_CATEGORY bc ON mcc.BENEFITCATEGORYID = bc.ID
                      WHERE mcc.MCCCODE = @MerchantCategoryCode";

        return await _dbConnection.QueryFirstOrDefaultAsync<MerchantCategoryCodeDomain>(query, new
        {
            MerchantCategoryCode = merchantCategoryCode
        });
    }
}