using System;
namespace Chirp.Web
{
	public enum DatabaseProviderType
	{
		// Specifies use of SqLite database provider
		SqLite,

		// Specifies use of SqlServer database provider
		SqlServer
	}

	/// <summary>
	///		Provides appsettings configuration for database provider selection 
	/// </summary>
	public class DatabaseProviderConfig
	{
		/// <summary>
		///		Specifies database provider selection
		/// </summary>
		public DatabaseProviderType DatabaseProviderType { get; set; }

		/// <summary>
		/// 	If true, the database is dropped before updating and seeding
		/// </summary>
		public bool RecreateDatabaseOnStartup { get; set; }

		/// <summary>
		///		If true, the database is seeded upon creation
		/// </summary>
		public bool SeedDatabase { get; set; }
	}
}

