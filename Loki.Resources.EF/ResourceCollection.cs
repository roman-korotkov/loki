using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Loki.Resources.EF
{
	/// <summary>
	/// Represents a database string set.
	/// </summary>
	internal sealed class ResourceCollection : Loki.Resources.ResourceCollection
	{
		private readonly ResourceConverter _converter;

		private readonly ConcurrentDictionary<string, string> _values;

		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceCollection" /> class.
		/// </summary>
		/// <param name="values">The set values.</param>
		/// <param name="converter">The resource converter.</param>
		public ResourceCollection(IEnumerable<KeyValuePair<string, string>> values, ResourceConverter converter)
		{
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}
			_converter = converter;

			_values = new ConcurrentDictionary<string, string>(values);
		}

		/// <summary>
		/// Occurs when when value was set.
		/// </summary>
		public event Action<string, string> ValueSet;

		/// <summary>
		/// Occurs when when value was removed.
		/// </summary>
		public event Action<string, string> ValueRemoved;

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

		/// <summary>
		/// Associates the specified value with the specified key for this set culture.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public override void Set<TValue>(string key, TValue value)
		{
			var result = _converter.From(value);

			_values.AddOrUpdate(key, result, (k, v) => result);

			OnValueSet(key, result);
		}

		/// <summary>
		/// Removes the value associated with the specified key for this set culture.
		/// </summary>
		/// <param name="key">The key.</param>
		public override void Remove(string key)
		{
			string value;
			if (_values.TryRemove(key, out value))
			{
				OnValueRemoved(key, value);
			}
		}

		private void OnValueSet(string key, string value)
		{
			var handler = ValueSet;
			if (handler != null)
			{
				handler(key, value);
			}
		}

		private void OnValueRemoved(string key, string value)
		{
			var handler = ValueRemoved;
			if (handler != null)
			{
				handler(key, value);
			}
		}
	}
}