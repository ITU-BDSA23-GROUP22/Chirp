// Inspiration: https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Chirp.Infrastructure;
using Chirp.Web;
using Microsoft.Extensions.Logging;

namespace Chirp.Migrations
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ChirpDbContext>
    {
        public ChirpDbContext CreateDbContext(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole());
            var loggerDuringStartUp = loggerFactory.CreateLogger<Program>();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            loggerDuringStartUp.LogInformation($"Creating DbContext for env : {env}");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .Build();

            var dbContextOptions = new DbContextOptionsBuilder<ChirpDbContext>();
            DbContextOptionsHelper.Configure(dbContextOptions, configuration, loggerDuringStartUp, this.GetType().Assembly.GetName().Name);

            return new ChirpDbContext(dbContextOptions.Options);
        }
    }
}