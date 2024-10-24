using TransactionAuthorizer.Application.Extensions;
using TransactionAuthorizer.Application.Interfaces;
using TransactionAuthorizer.Application.Models;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Application.Services
{
    public sealed record AuthorizerAppService(IAuthorizerService Authorizer) : IAuthorizerAppService
    {
        public async Task<string> Authorize(TransactionRequestModel transaction)
        {
            return await Authorizer.AuthorizeAsync(transaction.AsDomain());
        }
    }
}