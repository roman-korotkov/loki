using System;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using Loki.Resources.EF.Migrations;

namespace Loki.Resources.EF
{
	/// <summary>
	/// Represents a database migrator.
	/// </summary>
	public static class Migrator
	{
		/// <summary>
		/// Updates the database to the latest version.
		/// </summary>
		public static void UpdateDatabaseToLatestVersion()
		{
			var migrator = new DbMigrator(new Configuration());
			try
			{
				migrator.Update();
			}
			catch (Exception exception)
			{
				Trace.TraceError("Unable to update database to the latest version: {0}", exception);

				throw;
			}
		}
	}
}
