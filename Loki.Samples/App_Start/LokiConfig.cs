using Loki.Resources;
using Loki.Resources.EF;

namespace Loki.Samples
{
	public class LokiConfig
	{
		public static void SetUp()
		{
			Migrator.UpdateDatabaseToLatestVersion();

			ResourceProviders.Current = new CachedResourceEditor();
			ResourceObjectProviders.Current = new CachedResourceObjectProvider();
		}
	}
}