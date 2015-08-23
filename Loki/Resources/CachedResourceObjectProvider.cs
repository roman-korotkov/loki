using System;
using System.Globalization;

namespace Loki.Resources
{
	/// <summary>
	/// Represents a <see cref="ResourceObjectProviderBase"/> implementation that uses static cache to store resource objects.
	/// </summary>
	public class CachedResourceObjectProvider : ResourceObjectProviderBase
	{
		internal static event Action ClearCache;

		/// <summary>
		/// Removes all the cached objects from the cache.
		/// </summary>
		public static void RemoveAllCachedObjects()
		{
			var clearCache = ClearCache;
			if (clearCache != null)
			{
				clearCache();
			}
		}

		/// <summary>
		/// Gets the resouce object of the specified type from the specified resource provider for the specified culture.
		/// </summary>
		/// <typeparam name="TResource">The type of the resource.</typeparam>
		/// <param name="resourceProvider">The resource provider.</param>
		/// <param name="culture">The culture.</param>
		/// <returns>The resource object.</returns>
		public override TResource Get<TResource>(ResourceProvider resourceProvider, CultureInfo culture)
		{
			return ResourceObject<TResource>.GetCached(resourceProvider, culture);
		}
	}
}