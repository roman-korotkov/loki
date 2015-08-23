using System.Globalization;

namespace Loki.Resources
{
	/// <summary>
	/// Defines provider of localizable resources.
	/// </summary>
	public abstract class ResourceProvider
	{
		/// <summary>
		/// Gets the localized resource set for the specified culture with the specified resource set name.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="set">The resource set name.</param>
		/// <returns>The string for the specified key.</returns>
		public abstract ResourceSet GetResourceSet(CultureInfo culture, string set);

		/// <summary>
		/// Attempts to get the value for the specified culture associated with the specified key in the reseource set with the specified resource set name.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="culture">The culture.</param>
		/// <param name="set">The resource set name.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>True if the value was found; false otherwise.</returns>
		public abstract bool TryGet<TValue>(CultureInfo culture, string set, string key, out TValue value);
	}
}