using TransactionAuthorizer.Domain.Entities;

namespace TransactionAuthorizer.Domain.Interfaces;

public interface IMerchantRepository
{
    Task<MerchantDomain?> GetMerchantByNameAsync(string merchantName);
}