using System.ComponentModel.DataAnnotations;

namespace TransactionAuthorizer.Application.Models;

public sealed record TransactionRequestModel(
    [Required(ErrorMessage = "Account number is required.")] 
        string AccountNumber,    
    [Required(ErrorMessage = "Total amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")] 
        decimal TotalAmount,
    [Required(ErrorMessage = "Merchant category code is required.")] 
        string MerchantCategoryCode,
    [Required(ErrorMessage = "Merchant name is required.")] 
        string MerchantName);
