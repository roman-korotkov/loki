using System.Web.Mvc;

namespace Loki.Mvc
{
	/// <summary>
	/// Represents a Razor view with initialized set of resources.
	/// </summary>
	public abstract class LocalizableWebViewPage : WebViewPage
	{
		/// <summary>
		/// Gets or sets the page resources.
		/// </summary>
		public dynamic Resources { get; set; }

		/// <summary>
		/// Initializes the <see cref="T:System.Web.Mvc.AjaxHelper" />, <see cref="T:System.Web.Mvc.HtmlHelper" />, and <see cref="T:System.Web.Mvc.UrlHelper" /> classes.
		/// </summary>
		public override void InitHelpers()
		{
			base.InitHelpers();

			Resources = MvcLocalization.GetContext(ViewContext).GetResourceObject();
		}
	}

	/// <summary>
	/// Represents a Razor view with initialized set of resources.
	/// </summary>
	public abstract class LocalizableWebViewPage<TModel> : WebViewPage<TModel>
	{
		/// <summary>
		/// Gets or sets the page resources.
		/// </summary>
		public dynamic Resources { get; set; }

		/// <summary>
		/// Initializes the <see cref="T:System.Web.Mvc.AjaxHelper" />, <see cref="T:System.Web.Mvc.HtmlHelper" />, and <see cref="T:System.Web.Mvc.UrlHelper" /> classes.
		/// </summary>
		public override void InitHelpers()
		{
			base.InitHelpers();

			Resources = MvcLocalization.GetContext(ViewContext).GetResourceObject();
		}
	}

	/// <summary>
	/// Represents a Razor view with initialized set of resources.
	/// </summary>
	public abstract class LocalizableWebViewPage<TResources, TModel> : WebViewPage<TModel>
	{
		/// <summary>
		/// Gets or sets the page resources.
		/// </summary>
		public TResources Resources { get; set; }

		/// <summary>
		/// Initializes the <see cref="T:System.Web.Mvc.AjaxHelper" />, <see cref="T:System.Web.Mvc.HtmlHelper" />, and <see cref="T:System.Web.Mvc.UrlHelper" /> classes.
		/// </summary>
		public override void InitHelpers()
		{
			base.InitHelpers();

			Resources = MvcLocalization.GetContext(ViewContext).GetResourceObject<TResources>();
		}
	}
}
