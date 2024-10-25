namespace TransactionAuthorizer.Domain.Entities;

public sealed record AccountDomain(
    int Id,
    string AccountNumber, 
    decimal BalanceFood, 
    decimal BalanceMeal,
    decimal BalanceCash);