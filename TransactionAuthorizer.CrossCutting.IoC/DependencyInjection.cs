using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using TransactionAuthorizer.Application.Interfaces;
using TransactionAuthorizer.Domain.Interfaces;
using TransactionAuthorizer.Infrastructure.Configurations;
using TransactionAuthorizer.Infrastructure.Repositories;

namespace TransactionAuthorizer.CrossCutting.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Registrar a string de conexão
        services.AddScoped<IDbConnection>(provider =>
            new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

        services.AddSingleton(provider => new SemaphoreSlim(1));
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<IBenefitCategoryRepository, BenefitCategoryRepository>();
        services.AddTransient<IMerchantCategoryCodeRepository, MerchantCategoryCodeRepository>();
        services.AddTransient<IAuthorizerService, Domain.Services.AuthorizerService>();
        services.AddTransient<IAuthorizerAppService, Application.Services.AuthorizerService>();
        services.AddTransient<MigrationService>();

        return services;
    }
}