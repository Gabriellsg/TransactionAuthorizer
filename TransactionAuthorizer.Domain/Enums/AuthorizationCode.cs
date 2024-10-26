namespace TransactionAuthorizer.Domain.Enums;

public enum AuthorizationCode
{
    Approved = 00,
    InsufficientBalance = 51,
    OtherError = 07
}