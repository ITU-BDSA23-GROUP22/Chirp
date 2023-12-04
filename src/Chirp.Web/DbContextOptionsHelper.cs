//#define USE_SQLITE // Switch between SqLite- or SqlServer provider

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Chirp.Web
{
    public static class DbContextOptionsHelper
    {
        public static void Configure(DbContextOptionsBuilder options, IConfiguration configuration, ILogger logger, string migrationAssemblyName = "")
        {
            //NOTE: This allows us to seamlessly switch between SqLite and SqlServer datbase for development support

#if USE_SQLITE
			// Configure as SqLite provider..
			var connectionString = configuration.GetConnectionString("ChirpDatabase-sqLite")
				?? throw new ArgumentException("Connection string ChirpDatabase-sqLite cannot be null");

			logger.LogDebug($"Connecition string = {connectionString}");

			//.UseSqlServer(connection, x => x.MigrationsAssembly(this.GetType().Assembly.GetName().Name)).Options,

			if (migrationAssemblyName != string.Empty)
			{
				options.UseSqlite(connectionString, x => x.MigrationsAssembly(migrationAssemblyName));
			}
			else
			{
				options.UseSqlite(connectionString);
			}

#else
            // Configure as SqlServer provider..
            var connectionString = configuration.GetConnectionString("ChirpDatabase")
                ?? throw new ArgumentException("Connection string ChirpDatabase cannot be null");

            logger.LogDebug($"Connecition string = {connectionString}");

            if (migrationAssemblyName != string.Empty)
            {
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly(migrationAssemblyName));
            }
            else
            {
                options.UseSqlServer(connectionString);
            }

#endif
        }
    }
}