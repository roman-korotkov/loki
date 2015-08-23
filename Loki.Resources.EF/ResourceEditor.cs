using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Loki.Resources.EF
{
	/// <summary>
	/// Represents an <see cref="Loki.Resources.ResourceEditor"/> implementation that uses EF storage.
	/// </summary>
	public class ResourceEditor : Loki.Resources.ResourceEditor
	{
		private ResourceConverter _converter;

		private readonly ChangeQueue _queue;

		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceEditor"/> class.
		/// </summary>
		public ResourceEditor(ResourceConverter converter = null)
		{
			_converter = converter ?? ResourceConverter.Default;

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
			return new ResourceSet(GetResources(culture.LCID, set), Converter);
		}

		/// <summary>
		/// Gets the localized resource collection for the specified culture with the specified resource set name. Creates the collection if it doesn't exist.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="set">The resource set name.</param>
		/// <returns>The string for the specified key.</returns>
		public override Loki.Resources.ResourceCollection GetResourceCollection(CultureInfo culture, string set)
		{
			var collection = new ResourceCollection(GetResources(culture.LCID, set), Converter);

			collection.ValueSet += (key, value) => _queue.Enqueue(new SetValue(culture.LCID, set, key, value));
			collection.ValueRemoved += (key, value) => _queue.Enqueue(new RemoveValue(culture.LCID, set, key));

			return collection;
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
			using (var storage = new LokiContext())
			{
				var resource = storage.Resources.FirstOrDefault(x => x.Culture == culture.LCID && x.Set == set && x.Key == key);
				if (resource == null)
				{
					value = default(TValue);
					return false;
				}

				value = Converter.To<TValue>(resource.Value);
				return true;
			}
		}

		/// <summary>
		/// Removes the resource set for the specified culture with the specified resource set name.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="set">The set.</param>
		public override void RemoveResourceSet(CultureInfo culture, string set)
		{
			_queue.Enqueue(new RemoveResourceSet(culture.LCID, set));
		}

		private static IEnumerable<KeyValuePair<string, string>> GetResources(int culture, string set)
		{
			using (var storage = new LokiContext())
			{
				return storage.Resources.
					Where(x => x.Culture == culture && x.Set == set).
					Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).
					ToArray();
			}
		}
	}
}
