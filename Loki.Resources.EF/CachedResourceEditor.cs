using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Loki.Resources.EF
{
	/// <summary>
	/// Represents a cached <see cref="Loki.Resources.ResourceEditor"/> implementation that uses EF storage.
	/// </summary>
	public class CachedResourceEditor : Loki.Resources.ResourceEditor
	{
		private ResourceConverter _converter;

		private readonly ConcurrentDictionary<int, ConcurrentDictionary<string, ResourceCollection>> _cache;
		private readonly ChangeQueue _queue;

		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceEditor"/> class.
		/// </summary>
		public CachedResourceEditor(ResourceConverter converter = null)
		{
			_converter = converter ?? ResourceConverter.Default;

			_cache = new ConcurrentDictionary<int, ConcurrentDictionary<string, ResourceCollection>>();
			_queue = new ChangeQueue();
		}

		/// <summary>
		/// Gets or sets the converter.
		/// </summary>
		public ResourceConverter Converter
		{
			get { return _converter; }
			set { _converter = value ?? ResourceConverter.Default; }
		}

		/// <summary>
		/// Gets the localized resource set for the specified culture with the specified resource set name.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="set">The resource set name.</param>
		/// <returns>The string for the specified key.</returns>
		public override Loki.Resources.ResourceSet GetResourceSet(CultureInfo culture, string set)
		{
			return GetResourceCollection(culture, set);
		}

		/// <summary>
		/// Gets the localized resource collection for the specified culture with the specified resource set name. Creates the collection if it doesn't exist.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="set">The resource set name.</param>
		/// <returns>The string for the specified key.</returns>
		public override Loki.Resources.ResourceCollection GetResourceCollection(CultureInfo culture, string set)
		{
			return _cache.
				GetOrAdd(culture.LCID, GetResources).
				GetOrAdd(set, x => CreateCollection(new KeyValuePair<string, string>[0], culture.LCID, set));
		}

		/// <summary>
		/// Attempts to get the value for the specified culture associated with the specified key in the reseource set with the specified resource set name.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="culture">The culture.</param>
		/// <param name="set">The resource set name.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>True if the value was found; false otherwise.</returns>
		public override bool TryGet<TValue>(CultureInfo culture, string set, string key, out TValue value)
		{
			return GetResourceCollection(culture, set).TryGet(key, out value);
		}

		/// <summary>
		/// Removes the resource set for the specified culture with the specified resource set name.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="set">The set.</param>
		public override void RemoveResourceSet(CultureInfo culture, string set)
		{
			ResourceCollection collection;
			if (_cache.GetOrAdd(culture.LCID, GetResources).TryRemove(set, out collection))
			{
				_queue.Enqueue(new RemoveResourceSet(culture.LCID, set));
			}
		}

		/// <summary>
		/// Clears the cache for the specified culture.
		/// </summary>
		/// <param name="culture">The culture.</param>
		public void ClearCache(CultureInfo culture)
		{
			ConcurrentDictionary<string, ResourceCollection> sets;

			_cache.TryRemove(culture.LCID, out sets);
		}

		/// <summary>
		/// Clear all the cached instances.
		/// </summary>
		public void Clear()
		{
			_cache.Clear();
		}

		private ConcurrentDictionary<string, ResourceCollection> GetResources(int culture)
		{
			using (var storage = new LokiContext())
			{
				return new ConcurrentDictionary<string, ResourceCollection>(storage.Resources.
					Where(x => x.Culture == culture).
					GroupBy(x => x.Set).
					ToArray().
					Select(x => new KeyValuePair<string, ResourceCollection>(x.Key, CreateCollection(x.Select(y => new KeyValuePair<string, string>(y.Key, y.Value)), culture, x.Key))));
			}
		}

		private ResourceCollection CreateCollection(IEnumerable<KeyValuePair<string, string>> values, int culture, string set)
		{
			var collection = new ResourceCollection(values, Converter);

			collection.ValueSet += (key, value) => _queue.Enqueue(new SetValue(culture, set, key, value));
			collection.ValueRemoved += (key, value) => _queue.Enqueue(new RemoveValue(culture, set, key));

			return collection;
		}
	}
}