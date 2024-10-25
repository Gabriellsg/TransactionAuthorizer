namespace TransactionAuthorizer.Domain.Exceptions;

public sealed class NonExistentAccountException(string message) : ArgumentException(message) { }