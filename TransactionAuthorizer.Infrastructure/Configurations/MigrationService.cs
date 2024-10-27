using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace TransactionAuthorizer.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class MigrationService
{
    private readonly IConfiguration _configuration;
    private readonly string _dbName = "TransactionAuthorizerDB";
    private readonly IDbConnection _dbConnection;

    public MigrationService(IConfiguration configuration)
    {
        _configuration = configuration;
        _dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

    public async Task ApplyMigrationsAsync()
    {
        await EnsureDatabaseCreatedAsync();
        await CreateMigrationHistoryTableAsync();

        var appliedMigrations = await _dbConnection.QueryAsync<string>("SELECT MigrationName FROM MIGRATION_HISTORY");
        var migrationFiles = Directory.GetFiles("../TransactionAuthorizer.Infrastructure/Migrations", "*.sql").OrderBy(f => f);

        foreach (var file in migrationFiles)
        {
            var migrationName = Path.GetFileName(file);
            if (!appliedMigrations.Contains(migrationName))
            {
                var sql = await File.ReadAllTextAsync(file);
                await _dbConnection.ExecuteAsync(sql);
                await _dbConnection.ExecuteAsync("INSERT INTO MIGRATION_HISTORY (MigrationName) VALUES (@MigrationName)", new { MigrationName = migrationName });
            }
        }
    }

    private async Task EnsureDatabaseCreatedAsync()
    {
        var sql = $@"
            IF DB_ID('{_dbName}') IS NULL
            BEGIN
                CREATE DATABASE [{_dbName}];
            END";

        // Connect to the master database to create the new database
        var masterConnectionString = _dbConnection.ConnectionString.Replace(_dbConnection.Database, "master");
        using (var masterConnection = new SqlConnection(masterConnectionString))
        {
            await masterConnection.ExecuteAsync(sql);
        }

        // Update the connection to use the newly created database
        _dbConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    private async Task CreateMigrationHistoryTableAsync()
    {
        var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MIGRATION_HISTORY')
            BEGIN
                CREATE TABLE MIGRATION_HISTORY (
                    ID INT PRIMARY KEY IDENTITY,
                    MIGRATIONNAME NVARCHAR(100) NOT NULL,
                    APPLIEDON DATETIME NOT NULL DEFAULT GETDATE()
                );
            END";

        await _dbConnection.ExecuteAsync(sql);
    }
}