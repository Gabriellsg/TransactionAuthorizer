namespace TransactionAuthorizer.Domain.Entities;

public sealed record AccountDomain(
    string AccountNumber, 
    decimal BalanceFood, 
    decimal BalanceMeal,
    decimal BalanceCash);