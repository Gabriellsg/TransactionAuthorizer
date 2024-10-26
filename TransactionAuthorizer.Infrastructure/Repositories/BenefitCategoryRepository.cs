using Dapper;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
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
                      INNER JOIN BENEFIT_CATEGORY bc ON mcc.BENEFIT_CATEGORY_ID = bc.ID
                      WHERE mcc.MCC_CODE = @MerchantCategoryCode";

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