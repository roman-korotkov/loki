using System;
using System.Web.Routing;
using JetBrains.Annotations;

namespace Loki.Mvc
{
	/// <summary>
	/// Retrieves current culture from a cookie.
	/// </summary>
	public class FromCookieRouteHandler : InitCultureRouteHandler
	{
		private readonly string _cookie;

		/// <summary>
		/// Initializes a new instance of the <see cref="FromRouteRouteHandler"/> class.
		/// </summary>
		/// <param name="routeHandler">The route handler.</param>
		/// <param name="cookie">The culture parameter.</param>
		public FromCookieRouteHandler([NotNull] IRouteHandler routeHandler, [NotNull] string cookie)
			: base(routeHandler)
		{
			if (string.IsNullOrEmpty(cookie))
			{
				throw new ArgumentNullException("cookie");
			}
			_cookie = cookie;
		}

		/// <summary>
		/// Initializes the current culture for the current request using the specified request context.
		/// </summary>
		/// <param name="requestContext">The request context.</param>
		/// <returns>True if the current culture was initialized; false otherwise.</returns>
		protected override bool InitCulture(RequestContext requestContext)
		{
			var httpContext = requestContext.HttpContext;
			if (httpContext == null || httpContext.Request == null)
			{
				return false;
			}

			var cookie = httpContext.Request.Cookies[_cookie];

			return cookie != null && InitCulture(cookie.Value);
		}
	}
}