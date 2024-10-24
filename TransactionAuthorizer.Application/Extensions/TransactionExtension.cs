using TransactionAuthorizer.Application.Models;
using TransactionAuthorizer.Domain.Entities;

namespace TransactionAuthorizer.Application.Extensions;

public static class TransactionExtension
{
    public static TransactionDomain AsDomain(this TransactionRequestModel self) => new(
        self.AccountNumber,
        self.TotalAmount,
        self.MerchantCategoryCode,
        self.MerchantName);
}