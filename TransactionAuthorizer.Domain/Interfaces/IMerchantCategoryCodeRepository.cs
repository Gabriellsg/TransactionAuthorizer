using TransactionAuthorizer.Domain.Entities;

namespace TransactionAuthorizer.Domain.Interfaces;

public interface IMerchantCategoryCodeRepository
{
    Task<MerchantCategoryCodeDomain?> GetMerchantCategoryCodeAsync(string merchantCategoryCode);
}