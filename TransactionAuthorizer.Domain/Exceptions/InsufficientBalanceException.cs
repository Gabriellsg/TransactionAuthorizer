namespace TransactionAuthorizer.Domain.Exceptions;

public sealed class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException() : base("Insufficient balance.") { }
}