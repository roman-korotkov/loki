using System;
using System.Web.Routing;
using JetBrains.Annotations;

namespace Loki.Mvc
{
	/// <summary>
	/// Retrieves current culture from a route parameter.
	/// </summary>
	public class FromRouteRouteHandler : InitCultureRouteHandler
	{
		private readonly string _parameter;

		/// <summary>
		/// Initializes a new instance of the <see cref="FromRouteRouteHandler"/> class.
		/// </summary>
		/// <param name="routeHandler">The route handler.</param>
		/// <param name="parameter">The culture parameter.</param>
		public FromRouteRouteHandler([NotNull] IRouteHandler routeHandler, [NotNull] string parameter) : base(routeHandler)
		{
			if (string.IsNullOrEmpty(parameter))
			{
				throw new ArgumentNullException("parameter");
			}
			_parameter = parameter;
		}

		/// <summary>
		/// Initializes the current culture for the current request using the specified request context.
		/// </summary>
		/// <param name="requestContext">The request context.</param>
		/// <returns>True if the current culture was initialized; false otherwise.</returns>
		protected override bool InitCulture(RequestContext requestContext)
		{
			var routeData = requestContext.RouteData;

			object value;
			if (routeData == null || (!routeData.Values.TryGetValue(_parameter, out value) && !routeData.DataTokens.TryGetValue(_parameter, out value)))
			{
				return false;
			}

			return InitCulture(value);
		}
	}
}