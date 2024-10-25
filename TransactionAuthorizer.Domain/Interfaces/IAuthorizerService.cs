using TransactionAuthorizer.Domain.Entities;

namespace TransactionAuthorizer.Domain.Interfaces;

public interface IAuthorizerService
{
    Task<AuthorizationResponseDomain> AuthorizeAsync(TransactionDomain transaction);
}