using System;
namespace Chirp.Web
{
	public enum DatabaseProviderType
	{
		SqLite, SqlServer
	}

	public class DatabaseProviderConfig
	{
		public DatabaseProviderType DatabaseProviderType { get; set; }

		/// <summary>
		/// 	If true, the database is dropped before updating and seeding.
		/// </summary>
		public bool EnsureDeletedDatabaseOnStartup { get; set; }

		public bool SeedDatabase { get; set; }

		public bool EnsureCreatedDatabaseOnStartup { get; set; }

    }
}

