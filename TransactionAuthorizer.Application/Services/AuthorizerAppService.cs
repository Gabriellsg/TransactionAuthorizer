﻿using TransactionAuthorizer.Application.Extensions;
using TransactionAuthorizer.Application.Interfaces;
using TransactionAuthorizer.Application.Models;
using TransactionAuthorizer.Domain.Exceptions;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Application.Services;

public sealed class AuthorizerAppService(IAuthorizerService authorizer) : IAuthorizerAppService
{
    private readonly IAuthorizerService _authorizer = authorizer;

    public async Task<AuthorizationResponseModel> AuthorizeTransactionAsync(TransactionRequestModel transaction)
    {
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