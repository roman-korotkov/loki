using System.Globalization;

namespace Loki.Resources
{
	/// <summary>
	/// Represents a default implementation of <see cref="ResourceObjectProviderBase"/> class.
	/// </summary>
	public class ResourceObjectProvider : ResourceObjectProviderBase
	{
		/// <summary>
		/// Gets the resouce object of the specified type from the specified resource provider for the specified culture.
		/// </summary>
		/// <typeparam name="TResource">The type of the resource.</typeparam>
		/// <param name="resourceProvider">The resource provider.</param>
		/// <param name="culture">The culture.</param>
		/// <returns>The resource object.</returns>
		public override TResource Get<TResource>(ResourceProvider resourceProvider, CultureInfo culture)
		{
			return ResourceObject<TResource>.Get(resourceProvider, culture);
		}
	}
}