using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Loki.Gettext.PortableObject;
using Loki.Resources;

namespace Loki.Gettext
{
	/// <summary>
	/// Provides a way to export a set of resource types into the portable object file.
	/// </summary>
	public class POEporter
	{
		private readonly List<Type> _types;

		/// <summary>
		/// Initializes a new instance of the <see cref="POEporter"/> class.
		/// </summary>
		public POEporter()
		{
			_types = new List<Type>();
		}

		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the culture of the file.
		/// </summary>
		public CultureInfo Culture { get; set; }

		/// <summary>
		/// Gets the types.
		/// </summary>
		public List<Type> Types
		{
			get { return _types; }
		}

		/// <summary>
		/// Adds all the resource types from the specified namespace
		/// </summary>
		/// <param name="namespace">The namespace.</param>
		/// <remarks>
		/// Make sure all assemblies defining the namespace are loaded prior calling this method.
		/// </remarks>
		public void AddNamespace(string @namespace)
		{
			_types.AddRange(AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.Namespace == @namespace));
		}

		/// <summary>
		/// Exports a set of resource types into the portable object file.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="objects">The objects.</param>
		/// <returns>The PO file.</returns>
		public File Export(ResourceProvider provider, ResourceObjectProvider objects = null)
		{
			var culture = Culture ?? CultureInfo.InstalledUICulture;

			var originalContext = new LocalizationContext(EmptyResourceProvider.Instance, CultureInfo.InvariantCulture, objects);
			var translatedContext = new LocalizationContext(new GettextResourceProvider(provider), culture, objects);

			var list = new List<Entry>();
			var stringType = typeof (string);

			foreach (var type in _types)
			{
				var original = originalContext.GetResourceObject(type);
				var translated = translatedContext.GetResourceObject(type);

				var set = type.FullName;

				list.AddRange(ResourceObjectProviderBase.EnumerateFields(type).Where(x => x.FieldType == stringType).Select(field => new Entry
					{
						Context = string.Format("{0}.{1}", set, field.Name), 
						OriginalText = (string) field.GetValue(original), 
						TranslatedText = (string) field.GetValue(translated)
					}));
				list.AddRange(ResourceObjectProviderBase.EnumerateProperties(type).Where(x => x.PropertyType == stringType).Select(property => new Entry
					{
						Context = string.Format("{0}.{1}", set, property.Name),
						OriginalText = (string)property.GetValue(original, null),
						TranslatedText = (string)property.GetValue(original, null)
					}));
			}

			return new File(Name, culture.Name, list);
		}
	}
}
