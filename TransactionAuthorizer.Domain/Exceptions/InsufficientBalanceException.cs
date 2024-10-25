namespace TransactionAuthorizer.Domain.Exceptions;

public sealed class InsufficientBalanceException : ArgumentException
{
    public InsufficientBalanceException() : base("Insufficient balance.") { }

    public InsufficientBalanceException(string message) : base(message) { }

    public InsufficientBalanceException(string message, Exception innerException) : base(message, innerException) { }
}