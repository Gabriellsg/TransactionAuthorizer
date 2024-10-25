namespace TransactionAuthorizer.Domain.Exceptions;

public class InvalidTransactionAmountException : ArgumentException
{
    public InvalidTransactionAmountException(string message) : base(message) { }

    public InvalidTransactionAmountException(string message, Exception innerException) : base(message, innerException) { }
}