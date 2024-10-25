using TransactionAuthorizer.Application.Extensions;
using TransactionAuthorizer.Application.Interfaces;
using TransactionAuthorizer.Application.Models;
using TransactionAuthorizer.Domain.Exceptions;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Application.Services;

public sealed class AuthorizerService(IAuthorizerService authorizer) : IAuthorizerAppService
{
    private readonly IAuthorizerService _authorizer = authorizer;

    public async Task<string> AuthorizeTransactionAsync(TransactionRequestModel transaction)
    {
        try
        {
            var result = await _authorizer.AuthorizeAsync(transaction.AsDomain());
            return result.ToString();
        }
        catch (InsufficientBalanceException ex)
        {
            throw new InvalidTransactionException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            throw new InvalidTransactionException("Error during transaction authorization.", ex);
        }
    }
}