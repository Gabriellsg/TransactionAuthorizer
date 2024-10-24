namespace TransactionAuthorizer.Domain.Entities;

public sealed record TransactionDomain(
    string AccountNumber,
    decimal TotalAmount,
    string MerchantCategoryCode,
    string MerchantName);