namespace TransactionAuthorizer.Domain.Exceptions;

public sealed class InvalidTransactionException : Exception
{
    public InvalidTransactionException(string message) : base(message) { }

    public InvalidTransactionException(string message, Exception innerException) : base(message, innerException) { }
}