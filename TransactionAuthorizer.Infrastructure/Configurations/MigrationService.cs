using Dapper;
using System.Data;

namespace TransactionAuthorizer.Infrastructure.Configurations;

public class MigrationService(IDbConnection dbConnection)
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task ApplyMigrationsAsync()
    {
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