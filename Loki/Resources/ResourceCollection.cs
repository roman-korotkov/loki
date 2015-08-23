namespace Loki.Resources
{
	/// <summary>
	/// Represents an editable set of resources associated with a culture.
	/// </summary>
	public abstract class ResourceCollection : ResourceSet
	{
		/// <summary>
		/// Associates the specified value with the specified key for this set culture.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public abstract void Set<TValue>(string key, TValue value);

		/// <summary>
		/// Removes the value associated with the specified key for this set culture.
		/// </summary>
		/// <param name="key">The key.</param>
		public abstract void Remove(string key);
	}
}