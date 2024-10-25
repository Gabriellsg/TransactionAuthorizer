namespace TransactionAuthorizer.Domain.Enums;

public enum AuthorizationCode
{
    Approved = 0, // "00"
    InsufficientBalance = 51, // "51"
    OtherError = 7 // "07"
}