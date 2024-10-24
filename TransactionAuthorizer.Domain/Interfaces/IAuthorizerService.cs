using System.Transactions;
using TransactionAuthorizer.Domain.Entities;

namespace TransactionAuthorizer.Domain.Interfaces;

public interface IAuthorizerService
{
    Task<string> AuthorizeAsync(TransactionDomain transaction);
}