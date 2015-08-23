using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Loki.Resources.EF
{
	/// <summary>
	/// Represents a database string set.
	/// </summary>
	internal sealed class ResourceSet : Loki.Resources.ResourceSet
	{
		private readonly ResourceConverter _converter;

		private readonly ConcurrentDictionary<string, string> _values;

		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceSet" /> class.
		/// </summary>
		/// <param name="values">The set values.</param>
		/// <param name="converter">The resource converter.</param>
		public ResourceSet(IEnumerable<KeyValuePair<string, string>> values, ResourceConverter converter)
		{
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}
			_converter = converter;

			_values = new ConcurrentDictionary<string, string>(values);
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
			string result;
			if (!_values.TryGetValue(key, out result))
			{
				value = default(TValue);

				return false;
			}

			value = _converter.To<TValue>(result);

			return true;
		}
	}
}