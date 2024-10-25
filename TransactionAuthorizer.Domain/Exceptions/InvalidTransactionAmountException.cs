namespace TransactionAuthorizer.Domain.Exceptions;

public sealed class InvalidTransactionAmountException(string message) : ArgumentException(message) { }