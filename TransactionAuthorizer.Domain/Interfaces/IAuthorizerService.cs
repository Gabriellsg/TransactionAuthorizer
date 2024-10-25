using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Enums;

namespace TransactionAuthorizer.Domain.Interfaces;

public interface IAuthorizerService
{
    Task<AuthorizationCode> AuthorizeAsync(TransactionDomain transaction);
}