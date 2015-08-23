namespace Loki.Resources
{
	/// <summary>
	/// Represents a set of resources associated with a culture.
	/// </summary>
	public abstract class ResourceSet
	{
		/// <summary>
		/// Attempts to get the value for the this set culture associated with the specified key.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>True if the value was found; false otherwise.</returns>
		public abstract bool TryGet<TValue>(string key, out TValue value);
	}
}