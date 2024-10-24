using TransactionAuthorizer.Application.Models;

namespace TransactionAuthorizer.Application.Interfaces;

public interface IAuthorizerAppService
{
    Task<string> Authorize(TransactionRequestModel transaction);
}