using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Routing;

namespace Loki.Mvc
{
	/// <summary>
	/// Defines extension methods that helps to initialize localization infrastructure.
	/// </summary>
	public static class Routing
	{
		private static readonly IDictionary<string, CultureInfo> DefaultDomainMap;

		static Routing()
		{
			DefaultDomainMap = new Dictionary<string, CultureInfo>
			{
				{".com", CultureInfo.GetCultureInfo("en")},
				{".uk", CultureInfo.GetCultureInfo("en-GB")},
				{".ru", CultureInfo.GetCultureInfo("ru")},
				{".de", CultureInfo.GetCultureInfo("de")},
				{".fr", CultureInfo.GetCultureInfo("fr")},
				{".se", CultureInfo.GetCultureInfo("sv")},
				{".sp", CultureInfo.GetCultureInfo("es")}
			};
		}

		/// <summary>
		/// Modifies the specified route to initialize current culture from the specified route parameter.
		/// </summary>
		/// <param name="route">The route.</param>
		/// <param name="parameter">The culture parameter.</param>
		/// <returns>The route.</returns>
		public static Route InitCultureFromRoute(this Route route, string parameter = "culture")
		{
			route.RouteHandler = new FromRouteRouteHandler(route.RouteHandler, parameter);

			return route;
		}

		/// <summary>
		/// Modifies the specified route to initialize current culture from the specified cookie.
		/// </summary>
		/// <param name="route">The route.</param>
		/// <param name="cookie">The cookie.</param>
		/// <returns>The route.</returns>
		public static Route InitCultureFromCookie(this Route route, string cookie = "culture")
		{
			route.RouteHandler = new FromCookieRouteHandler(route.RouteHandler, cookie);

			return route;
		}

		/// <summary>
		/// Modifies the specified route to initialize current culture from the specified session value.
		/// </summary>
		/// <param name="route">The route.</param>
		/// <param name="value">The session value.</param>
		/// <returns>The route.</returns>
		public static Route InitCultureFromSession(this Route route, string value = "culture")
		{
			route.RouteHandler = new FromSessionRouteHandler(route.RouteHandler, value);

			return route;
		}

		/// <summary>
		/// Modifies the specified route to initialize current culture from domain name using the specified map.
		/// </summary>
		/// <param name="route">The route.</param>
		/// <param name="map">The map.</param>
		/// <param name="domainTransform">The domain transform.</param>
		/// <returns>The route.</returns>
		public static Route InitCultureFromDomain(this Route route, IDictionary<string, CultureInfo> map = null, Func<string, string> domainTransform = null)
		{
			route.RouteHandler = new FromDomainRouteHandler(route.RouteHandler, map ?? DefaultDomainMap, domainTransform ?? DefaultDomainTransform);

			return route;
		}

		/// <summary>
		/// Modifies the specified route to initialize current culture from Accept-Language HTTP header using the specified collection of supported languages.
		/// </summary>
		/// <param name="route">The route.</param>
		/// <param name="languages">The languages.</param>
		/// <returns>The route.</returns>
		public static Route InitCultureFromHttpHeader(this Route route, params string[] languages)
		{
			route.RouteHandler = new FromHttpHeaderRouteHandler(route.RouteHandler, languages);

			return route;
		}

		private static string DefaultDomainTransform(string domain)
		{
			if (domain == null)
			{
				return string.Empty;
			}

			var index = domain.LastIndexOf('.');

			return index < 0 ? string.Empty : domain.Substring(index);
		}
	}
}
