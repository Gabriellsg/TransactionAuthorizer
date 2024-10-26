using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using TransactionAuthorizer.Application.Interfaces;
using TransactionAuthorizer.Application.Services;
using TransactionAuthorizer.Application.Validators;
using TransactionAuthorizer.Domain.Interfaces;
using TransactionAuthorizer.Domain.Services;
using TransactionAuthorizer.Infrastructure.Configurations;
using TransactionAuthorizer.Infrastructure.Repositories;
using FluentValidation;

namespace TransactionAuthorizer.CrossCutting.IoC;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnection>(provider =>
            new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

        services.AddSingleton(provider => new SemaphoreSlim(1));
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<IBenefitCategoryRepository, BenefitCategoryRepository>();
        services.AddTransient<IAuthorizerService, AuthorizerService>();
        services.AddTransient<IAuthorizerAppService, AuthorizerAppService>();
        services.AddTransient<ITransactionLogRepository, TransactionLogRepository>();
        services.AddTransient<IMerchantRepository, MerchantRepository>();
        services.AddTransient<MigrationService>();

        services.AddTransient<ITransactionRequestModelValidator, TransactionRequestModelValidator>();

        services.AddValidatorsFromAssemblyContaining<TransactionRequestModelValidator>();

        return services;
    }
}