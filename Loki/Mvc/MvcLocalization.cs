using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using Loki.Resources;

namespace Loki.Mvc
{
	/// <summary>
	/// Defines default behavior to retrieve localization context in MVC environment.
	/// </summary>
	internal static class MvcLocalization
	{
		private const string ViewDataKey = "CE8F340967DC4717A9B96862818240BE";

		/// <summary>
		/// Retrieves the localization context for the specified controller context.
		/// </summary>
		/// <param name="controllerContext">The controller context.</param>
		/// <returns>The localization context.</returns>
		public static LocalizationContext GetContext(ControllerContext controllerContext)
		{
			var provider = ResourceProviders.Current;
			var culture =  Thread.CurrentThread.CurrentUICulture;

			return new LocalizationContext(provider, culture) {ResourceSet = EvaluateSet(controllerContext, provider, culture)};
		}

		/// <summary>
		/// Retrieves the localization context for the specified view context.
		/// </summary>
		/// <param name="viewContext">The view context.</param>
		/// <returns>The localization context.</returns>
		public static LocalizationContext GetContext(ViewContext viewContext)
		{
			var controller = viewContext.Controller as LocalizableController;
			if (controller != null)
			{
				return controller.LocalizationContext;
			}

			var context = viewContext.ViewData[ViewDataKey] as LocalizationContext;
			if (context != null)
			{
				return context;
			}

			context = GetContext((ControllerContext)viewContext);

			viewContext.ViewData[ViewDataKey] = context;

			return context;
		}

		private static ResourceSet EvaluateSet(ControllerContext controllerContext, ResourceProvider provider, CultureInfo culture)
		{
			var route = controllerContext.RouteData;

			try
			{
				var set = route != null
					? string.Format("{0}/{1}", route.GetRequiredString("controller"), route.GetRequiredString("action"))
					: string.Empty;

				return provider.GetResourceSet(culture, set);
			}
			catch (InvalidOperationException) // no route data
			{
				return provider.GetResourceSet(culture, string.Empty);
			}
		}
	}
}
