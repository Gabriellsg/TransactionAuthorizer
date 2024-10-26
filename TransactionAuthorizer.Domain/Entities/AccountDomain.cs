namespace TransactionAuthorizer.Domain.Entities;

public sealed class AccountDomain
{
    public int Id { get; }
    public string AccountNumber { get; }
    public decimal BalanceFood { get; }
    public decimal BalanceMeal { get; }
    public decimal BalanceCash { get; }

    public AccountDomain(int id, string accountNumber, decimal balanceFood, decimal balanceMeal, decimal balanceCash)
    {
        Id = id;
        AccountNumber = accountNumber;
        BalanceFood = balanceFood;
        BalanceMeal = balanceMeal;
        BalanceCash = balanceCash;
    }

    public AccountDomain() { }
}