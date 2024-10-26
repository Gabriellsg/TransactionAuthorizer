using FluentValidation;
using FluentValidation.Results;
using TransactionAuthorizer.Application.Interfaces;
using TransactionAuthorizer.Application.Models;

namespace TransactionAuthorizer.Application.Validators;

public sealed class TransactionRequestModelValidator : AbstractValidator<TransactionRequestModel>, ITransactionRequestModelValidator
{
    public TransactionRequestModelValidator()
    {
        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("AccountNumber is required.")
            .Length(1, 20).WithMessage("AccountNumber must be between 1 and 20 characters.");

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0).WithMessage("TotalAmount must be greater than zero.");

        RuleFor(x => x.MerchantCategoryCode)
            .NotEmpty().WithMessage("MerchantCategoryCode is required.")
            .Length(1, 4).WithMessage("MerchantCategoryCode must be between 1 and 4 characters.");

        RuleFor(x => x.MerchantName)
            .NotEmpty().WithMessage("MerchantName is required.")
            .MaximumLength(255).WithMessage("MerchantName must be up to 255 characters.");
    }

    public async Task<ValidationResult> ValidateAsync(TransactionRequestModel transaction)
    {
        return await ValidateAsync(transaction);
    }
}