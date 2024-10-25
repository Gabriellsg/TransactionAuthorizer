using TransactionAuthorizer.Application.Models;

namespace TransactionAuthorizer.Application.Interfaces;

public interface IAuthorizerAppService
{
    Task<string> AuthorizeTransactionAsync(TransactionRequestModel transaction);
}