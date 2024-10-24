using Microsoft.Extensions.Logging;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Exceptions;
using TransactionAuthorizer.Domain.Interfaces;

namespace TransactionAuthorizer.Domain.Services;

public sealed record AuthorizerService(
    Dictionary<string, decimal> BenefitBalances, 
    ILogger<AuthorizerService> Logger,
    SemaphoreSlim Semaphore) : IAuthorizerService
{
    public async Task<string> AuthorizeAsync(TransactionDomain transaction)
    {
        await Semaphore.WaitAsync();

        try
        {
            var benefitCategory = GetBenefitCategory(transaction.MerchantCategoryCode);

            if (BenefitBalances[benefitCategory] < transaction.TotalAmount)
            {
                Logger.LogWarning("Insufficient balance.");
                throw new InsufficientBalanceException();
            }

            BenefitBalances[benefitCategory] -= transaction.TotalAmount;
            Logger.LogInformation("Transaction authorized.");
            return "00";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error authorizing transaction.");
            throw;
        }
        finally
        {
            Semaphore.Release();
        }
    }

    private string GetBenefitCategory(string merchantCategoryCode)
    {
        return merchantCategoryCode switch
        {
            "5411" or "5412" => "Food",
            "5811" or "5812" => "Meal",
            _ => "Cash"
        };
    }
}