using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using TransactionAuthorizer.CrossCutting.IoC;
using Serilog;
using Microsoft.OpenApi.Models;
using TransactionAuthorizer.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.Debug());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TransactionAuthorizer", Version = "v1" });
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TransactionAuthorizer v1"));

app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();
    await migrationService.ApplyMigrationsAsync();
}

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Internal Error");
        Serilog.Log.Error(ex, "Internal Error");
    }
});

app.Run();