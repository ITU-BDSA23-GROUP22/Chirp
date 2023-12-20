using Microsoft.EntityFrameworkCore;

namespace Chirp.Web
{
    /// <summary>
    ///     Provides central configuring of selected database provider.
    ///     Is used in both Chirp.Web.Program startup and Chirp.Migrations.
    /// </summary>
    public static class DbContextOptionsHelper
    {
        public static void Configure(DbContextOptionsBuilder options, IConfiguration configuration, ILogger logger, string migrationAssemblyName = "")
        {
            var databaseProviderConfig = new DatabaseProviderConfig();
            configuration.GetSection(nameof(DatabaseProviderConfig)).Bind(databaseProviderConfig);

            if (databaseProviderConfig.DatabaseProviderType == DatabaseProviderType.SqLite)
            {
                // Configure as SqLite provider..
                var connectionString = configuration.GetConnectionString("ChirpDatabase-SqLite")
                    ?? throw new ArgumentException("Connection string ChirpDatabase-SqLite cannot be null");

                logger.LogDebug($"Connecition string = {connectionString}");

                if (migrationAssemblyName != string.Empty)
                {
                    options.UseSqlite(connectionString, x => x.MigrationsAssembly(migrationAssemblyName));
                }
                else
                {
                    options.UseSqlite(connectionString);
                }
            }
            else if (databaseProviderConfig.DatabaseProviderType == DatabaseProviderType.SqlServer)
            {

                // Configure as SqlServer provider..
                var connectionString = configuration.GetConnectionString("ChirpDatabase-SqlServer")
                    ?? throw new ArgumentException("Connection string ChirpDatabase-SqlServer cannot be null");

                logger.LogDebug($"Connecition string = {connectionString}");

                if (migrationAssemblyName != string.Empty)
                {
                    options.UseSqlServer(connectionString, x => x.MigrationsAssembly(migrationAssemblyName));
                }
                else
                {
                    options.UseSqlServer(connectionString);
                }
            }
        }
    }
}