using System.Web.Mvc;
using System.Web.Routing;
using Loki.Mvc;
using Loki.Resources;
using Loki.Resources.EF;
using $rootnamespace$.Controllers;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($rootnamespace$.LokiConfig), "SetUp")]

namespace $rootnamespace$
{
	public class LokiConfig
	{
		public static void SetUp()
		{
			Migrator.UpdateDatabaseToLatestVersion();

			ResourceProviders.Current = new CachedResourceEditor();
			ResourceObjectProviders.Current = new CachedResourceObjectProvider();
			
			// TODO: the route is added for demonstration purpose
			RouteTable.Routes.MapRoute(
				name: "Loki",
				url: "Loki/{action}/{id}",
				defaults: new { controller = "LokiLocalization", action = "Index", id = UrlParameter.Optional }
			).InitCultureFromCookie(LokiLocalizationController.CookieName).InitCultureFromHttpHeader();
		}
	}
}