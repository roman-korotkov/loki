using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Routing;
using JetBrains.Annotations;

namespace Loki.Mvc
{
	/// <summary>
	/// Initializes the current culture for the associated route.
	/// </summary>
	public abstract class InitCultureRouteHandler : IRouteHandler
	{
		private readonly IRouteHandler _routeHandler;

		/// <summary>
		/// Initializes a new instance of the <see cref="InitCultureRouteHandler"/> class.
		/// </summary>
		/// <param name="routeHandler">The route handler.</param>
		protected InitCultureRouteHandler([NotNull] IRouteHandler routeHandler)
		{
			if (routeHandler == null)
			{
				throw new ArgumentNullException("routeHandler");
			}
			_routeHandler = routeHandler;
		}

		/// <summary>
		/// Provides the object that processes the request.
		/// </summary>
		/// <param name="requestContext">An object that encapsulates information about the request.</param>
		/// <returns>An object that processes the request.</returns>
		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			var handler = _routeHandler as InitCultureRouteHandler;
			if (handler == null || !handler.InitCulture(requestContext))
			{
				InitCulture(requestContext);
			}

			return handler == null 
				? _routeHandler.GetHttpHandler(requestContext)
				: handler.GetHttpHandlerDirect(requestContext);
		}

		/// <summary>
		/// Initializes the current culture for the current request using the specified request context.
		/// </summary>
		/// <param name="requestContext">The request context.</param>
		/// <returns>True if the current culture was initialized; false otherwise.</returns>
		protected abstract bool InitCulture(RequestContext requestContext);

		/// <summary>
		/// Attempts to convert the specified object value to culture information and set it as current culture for the current request.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>True if the current culture was initialized; false otherwise.</returns>
		protected bool InitCulture(object value)
		{
			var culture = value as CultureInfo;
			if (culture != null)
			{
				return InitCulture(culture);
			}

			var cultureName = value as string;
			if (cultureName != null)
			{
				try
				{
					return InitCulture(CultureInfo.GetCultureInfo(cultureName));
				}
				catch (ArgumentOutOfRangeException) { }
				catch (CultureNotFoundException) { }
			}

			try
			{
				return InitCulture(CultureInfo.GetCultureInfo(Convert.ToInt32(value)));
			}
			catch (FormatException) { }
			catch (InvalidCastException) { }
			catch (OverflowException) { }
			catch (ArgumentOutOfRangeException) { }
			catch (CultureNotFoundException) { }

			return false;
		}

		private static bool InitCulture(CultureInfo culture)
		{
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;

			return true;
		}

		private IHttpHandler GetHttpHandlerDirect(RequestContext requestContext)
		{
			var handler = _routeHandler as InitCultureRouteHandler;
			return handler == null
				? _routeHandler.GetHttpHandler(requestContext)
				: handler.GetHttpHandlerDirect(requestContext);
		}
	}
}