namespace TransactionAuthorizer.Domain.Entities;

public sealed record TransactionLogDomain(
    int Id,
    int? AccountId,
    decimal Amount,
    string MerchantName,
    string MerchantCategoryCode,
    DateTime TransactionDate,
    int AuthorizationCode);