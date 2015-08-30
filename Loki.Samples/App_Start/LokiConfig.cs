using Loki.Resources;
using Loki.Resources.EF;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Loki.Samples.LokiConfig), "SetUp")]

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