using TransactionAuthorizer.Application.Extensions;
using TransactionAuthorizer.Application.Interfaces;
using TransactionAuthorizer.Application.Models;
using TransactionAuthorizer.Domain.Exceptions;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Application.Services;

public sealed class AuthorizerAppService(IAuthorizerService authorizer, ITransactionRequestModelValidator validator) : IAuthorizerAppService
{
    private readonly IAuthorizerService _authorizer = authorizer;
    private readonly ITransactionRequestModelValidator _validator = validator;

    public async Task<AuthorizationResponseModel> AuthorizeTransactionAsync(TransactionRequestModel transaction)
    {
        var validationResult = await _validator.ValidateAsync(transaction);
        
        if (!validationResult.IsValid)
            throw new InvalidTransactionException("Invalid transaction data: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        try
        {
            var result = await _authorizer.AuthorizeAsync(transaction.AsDomain());
            return result.AsModel();
        }
        catch (Exception ex)
        {
            throw new InvalidTransactionException("Error during transaction authorization.", ex);
        }
    }
}