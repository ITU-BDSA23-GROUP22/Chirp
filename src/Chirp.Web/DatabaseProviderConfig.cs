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
	}
}

