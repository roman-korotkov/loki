using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Routing;
using JetBrains.Annotations;

namespace Loki.Mvc
{
	/// <summary>
	/// Retrieves current culture from a cookie.
	/// </summary>
	public class FromDomainRouteHandler : InitCultureRouteHandler
	{
		private readonly IDictionary<string, CultureInfo> _map;
		private readonly Func<string, string> _domainTransform;

		/// <summary>
		/// Initializes a new instance of the <see cref="FromRouteRouteHandler" /> class.
		/// </summary>
		/// <param name="routeHandler">The route handler.</param>
		/// <param name="map">The map.</param>
		/// <param name="domainTransform"></param>
		public FromDomainRouteHandler([NotNull] IRouteHandler routeHandler, [NotNull] IDictionary<string, CultureInfo> map,
			[NotNull] Func<string, string> domainTransform)
			: base(routeHandler)
		{
			if (map == null)
			{
				throw new ArgumentNullException("map");
			}
			if (domainTransform == null)
			{
				throw new ArgumentNullException("domainTransform");
			}
			_map = map;
			_domainTransform = domainTransform;
		}

		/// <summary>
		/// Initializes the current culture for the current request using the specified request context.
		/// </summary>
		/// <param name="requestContext">The request context.</param>
		/// <returns>True if the current culture was initialized; false otherwise.</returns>
		protected override bool InitCulture(RequestContext requestContext)
		{
			var httpContext = requestContext.HttpContext;
			if (httpContext == null || httpContext.Request == null || httpContext.Request.Url == null)
			{
				return false;
			}

			CultureInfo culture;

			return _map.TryGetValue(_domainTransform(httpContext.Request.Url.Host), out culture) && InitCulture(culture);
		}
	}
}