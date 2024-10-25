using Dapper;
using System.Data;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Infrastructure.Repositories;

public sealed class BenefitCategoryRepository(IDbConnection dbConnection) : IBenefitCategoryRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<BenefitCategoryDomain?> GetBenefitCategoryAsync(string merchantCategoryCode)
    {
        var query = @"SELECT 
                        bc.ID, 
                        bc.NAME, 
                        bc.BALANCE 
                      FROM MERCHANT_CATEGORY_CODE mcc
                      INNER JOIN BENEFIT_CATEGORY bc ON mcc.BENEFITCATEGORYID = bc.ID
                      WHERE mcc.MCCCODE = @MerchantCategoryCode";

        return await _dbConnection.QueryFirstOrDefaultAsync<BenefitCategoryDomain>(query, new
        {
            MerchantCategoryCode = merchantCategoryCode
        });
    }

    public async Task<BenefitCategoryDomain> GetDefaultBenefitCategoryAsync()
    {
        var query = @"SELECT 
                        ID, 
                        NAME, 
                        BALANCE 
                      FROM BENEFIT_CATEGORY 
                      WHERE NAME = 'CASH'";

        var response = await _dbConnection.QueryFirstOrDefaultAsync<BenefitCategoryDomain>(query);

        return response!;
    }
}