using System;
using System.Web.Routing;
using System.Web.SessionState;
using JetBrains.Annotations;

namespace Loki.Mvc
{
	/// <summary>
	/// Retrieves current culture from a cookie.
	/// </summary>
	/// <remarks>
	/// For this handler to work it has to be first handler in the chain. Also see http://stackoverflow.com/questions/218057/httpcontext-current-session-is-null-when-routing-requests
	/// </remarks>
	public class FromSessionRouteHandler : InitCultureRouteHandler, IRequiresSessionState
	{
		private readonly string _value;

		/// <summary>
		/// Initializes a new instance of the <see cref="FromRouteRouteHandler"/> class.
		/// </summary>
		/// <param name="routeHandler">The route handler.</param>
		/// <param name="value">The culture parameter.</param>
		public FromSessionRouteHandler([NotNull] IRouteHandler routeHandler, [NotNull] string value)
			: base(routeHandler)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException("value");
			}
			_value = value;
		}

		/// <summary>
		/// Initializes the current culture for the current request using the specified request context.
		/// </summary>
		/// <param name="requestContext">The request context.</param>
		/// <returns>True if the current culture was initialized; false otherwise.</returns>
		protected override bool InitCulture(RequestContext requestContext)
		{
			var httpContext = requestContext.HttpContext;
			if (httpContext == null || httpContext.Session == null)
			{
				return false;
			}

			return InitCulture(httpContext.Session[_value]);
		}
	}
}