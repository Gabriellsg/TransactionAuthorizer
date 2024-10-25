using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Application.Models;
using TransactionAuthorizer.Domain.Exceptions;

namespace TransactionAuthorizer.Application.Extensions;

public static class TransactionExtension
{
    public static TransactionDomain AsDomain(this TransactionRequestModel self)
    {
        if (string.IsNullOrWhiteSpace(self.AccountNumber))
            throw new InvalidTransactionException("Account number is required.");

        if (self.TotalAmount <= 0)
            throw new InvalidTransactionException("Total amount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(self.MerchantCategoryCode))
            throw new InvalidTransactionException("Merchant category code is required.");

        if (string.IsNullOrWhiteSpace(self.MerchantName))
            throw new InvalidTransactionException("Merchant name is required.");

        return new TransactionDomain(
            self.AccountNumber,
            self.TotalAmount,
            self.MerchantCategoryCode,
            self.MerchantName);
    }

    public static AuthorizationResponseModel AsModel(this AuthorizationResponseDomain self) => new(self.Code);
}