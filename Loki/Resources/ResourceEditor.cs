using System.Globalization;

namespace Loki.Resources
{
	/// <summary>
	/// Defines a resource editor.
	/// </summary>
	public abstract class ResourceEditor : ResourceProvider
	{
		/// <summary>
		/// Gets the localized resource collection for the specified culture with the specified resource set name. Creates the collection if it doesn't exist.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="set">The resource set name.</param>
		/// <returns>The string for the specified key.</returns>
		public abstract ResourceCollection GetResourceCollection(CultureInfo culture, string set);

		/// <summary>
		/// Removes the resource set for the specified culture with the specified resource set name.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="set">The set.</param>
		public abstract void RemoveResourceSet(CultureInfo culture, string set);
	}
}