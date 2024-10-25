namespace TransactionAuthorizer.Domain.Exceptions;

public sealed class NonExistentAccountException : ArgumentException
{
    public NonExistentAccountException(string message) : base(message) { }

    public NonExistentAccountException(string message, Exception innerException) : base(message, innerException) { }
}