using System;
using JetBrains.Annotations;
using Loki.Resources;

namespace Loki.Gettext
{
	/// <summary>
	/// Represents an implementation of <see cref="ResourceSet"/> that returns resources for translation.
	/// </summary>
	internal class GettextResourceSet : ResourceSet
	{
		private readonly ResourceSet _set;

		/// <summary>
		/// Initializes a new instance of the <see cref="GettextResourceSet"/> class.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <exception cref="ArgumentNullException">set</exception>
		public GettextResourceSet([NotNull] ResourceSet set)
		{
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}
			_set = set;
		}

		/// <summary>
		/// Attempts to get the value for the this set culture associated with the specified key.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>True if the value was found; false otherwise.</returns>
		public override bool TryGet<TValue>(string key, out TValue value)
		{
			_set.TryGet(key, out value);

			return true;
		}
	}
}