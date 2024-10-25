using TransactionAuthorizer.Application.Models;

namespace TransactionAuthorizer.Application.Interfaces;

public interface IAuthorizerAppService
{
    Task<AuthorizationResponseModel> AuthorizeTransactionAsync(TransactionRequestModel transaction);
}