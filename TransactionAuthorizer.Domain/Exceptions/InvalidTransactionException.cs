namespace TransactionAuthorizer.Domain.Exceptions;

public sealed class InvalidTransactionException : Exception
{
    public InvalidTransactionException(string message) : base(message) { }
}