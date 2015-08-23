using System.Web.Routing;

namespace Loki.Mvc
{
	/// <summary>
	/// Represents a controller that provides localozation members.
	/// </summary>
	public class LocalizableController : LocalizableControllerBase
	{
		/// <summary>
		/// Gets or sets the controller resources.
		/// </summary>
		public dynamic Resources { get; set; }

		/// <summary>
		/// Initializes data that might not be available when the constructor is called.
		/// </summary>
		/// <param name="requestContext">The HTTP context and route data.</param>
		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);

			Resources = LocalizationContext.GetResourceObject();
		}
	}

	/// <summary>
	/// Represents a controller that provides localozation members.
	/// </summary>
	public class LocalizableController<TResources> : LocalizableControllerBase
	{
		/// <summary>
		/// Gets or sets the controller resources.
		/// </summary>
		public TResources Resources { get; set; }

		/// <summary>
		/// Initializes data that might not be available when the constructor is called.
		/// </summary>
		/// <param name="requestContext">The HTTP context and route data.</param>
		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);

			Resources = LocalizationContext.GetResourceObject<TResources>();
		}
	}
}
