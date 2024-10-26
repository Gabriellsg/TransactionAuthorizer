using FluentValidation.Results;
using TransactionAuthorizer.Application.Models;

namespace TransactionAuthorizer.Application.Interfaces;

public interface ITransactionRequestModelValidator
{
    Task<ValidationResult> ValidateAsync(TransactionRequestModel transaction);
}
