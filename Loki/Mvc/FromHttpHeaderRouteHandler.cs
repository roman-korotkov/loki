using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Routing;
using JetBrains.Annotations;

namespace Loki.Mvc
{
	/// <summary>
	/// Retrieves current culture from Accept-Language HTTP header.
	/// </summary>
	public class FromHttpHeaderRouteHandler : InitCultureRouteHandler
	{
		private readonly HashSet<string> _languages;

		/// <summary>
		/// Initializes a new instance of the <see cref="FromRouteRouteHandler"/> class.
		/// </summary>
		/// <param name="routeHandler">The route handler.</param>
		/// <param name="languages">The supported languages.</param>
		public FromHttpHeaderRouteHandler([NotNull] IRouteHandler routeHandler, [NotNull] string[] languages)
			: base(routeHandler)
		{
			if (languages == null)
			{
				throw new ArgumentNullException("languages");
			}
			_languages = new HashSet<string>(languages);
		}

		/// <summary>
		/// Initializes the current culture for the current request using the specified request context.
		/// </summary>
		/// <param name="requestContext">The request context.</param>
		/// <returns>True if the current culture was initialized; false otherwise.</returns>
		protected override bool InitCulture(RequestContext requestContext)
		{
			var httpContext = requestContext.HttpContext;
			if (httpContext == null || httpContext.Request == null || httpContext.Request.UserLanguages == null)
			{
				return false;
			}

			var language = httpContext.Request.UserLanguages.
				Where(x => !string.IsNullOrEmpty(x)).
				Select(x => x.Split(new []{';'}, StringSplitOptions.RemoveEmptyEntries)).
				Where(x => x.Length > 0).
				Select(x => new { Language = x[0], Quality = x.Length > 1 ? ParseQuality(x[1]) : 1d }).
				OrderByDescending(x => x.Quality).
				Select(x => x.Language).
				FirstOrDefault(x => _languages.Contains(x));

			return InitCulture(language);
		}

		private static double ParseQuality(string value)
		{
			double quality;
			double.TryParse(value.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault(), NumberStyles.Number, CultureInfo.InvariantCulture, out quality);
			return quality;
		}
	}
}