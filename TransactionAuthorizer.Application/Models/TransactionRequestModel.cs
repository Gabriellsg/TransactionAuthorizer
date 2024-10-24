namespace TransactionAuthorizer.Application.Models;

public sealed record TransactionRequestModel(
    string AccountNumber,
    decimal TotalAmount,
    string MerchantCategoryCode,
    string MerchantName);
