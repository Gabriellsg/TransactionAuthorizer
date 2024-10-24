using Microsoft.Extensions.DependencyInjection;
using TransactionAuthorizer.Application.Interfaces;
using TransactionAuthorizer.Application.Services;
using TransactionAuthorizer.Domain.Interfaces;
using TransactionAuthorizer.Domain.Services;
using TransactionAuthorizer.Infrastructure.Interfaces;
using TransactionAuthorizer.Infrastructure.Repositories;

namespace TransactionAuthorizer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<SemaphoreSlim>(provider => new SemaphoreSlim(1));
        services.AddTransient<ITransactionRepository, TransactionRepository>();
        services.AddTransient<IAuthorizerService, AuthorizerService>();
        services.AddTransient<IAuthorizerAppService, AuthorizerAppService>();
        services.AddTransient<Dictionary<string, decimal>>(provider =>
        new Dictionary<string, decimal>()
        {
            // Adicione itens ao dicionário
            {"Chave1", 10.99m},
            {"Chave2", 5.49m}
        });

        return services;
    }
}