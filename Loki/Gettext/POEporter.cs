﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Loki.Gettext.PortableObject;
using Loki.Resources;

namespace Loki.Gettext
{
	/// <summary>
	/// Provides a way to export a set of resource types into the portable object file.
	/// </summary>
	public class POEporter
	{
		private static readonly int CommentTypeCount;

		private readonly List<Type> _types;

		static POEporter()
		{
			CommentTypeCount = Enum.GetValues(typeof (CommentType)).Length;
		}

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
		public virtual File Export(ResourceProvider provider, ResourceObjectProviderBase objects = null)
		{
			var culture = Culture ?? CultureInfo.InstalledUICulture;

			var originalContext = new LocalizationContext(EmptyResourceProvider.Instance, CultureInfo.InvariantCulture, new ResourceObjectProvider());
			var translatedContext = new LocalizationContext(new GettextResourceProvider(provider), culture, objects);

			var list = new List<Entry>();
			var stringType = typeof (string);

			CachedResourceObjectProvider.RemoveAllCachedObjects();

			foreach (var type in _types)
			{
				var original = originalContext.GetResourceObject(type);
				var translated = translatedContext.GetResourceObject(type);

				var set = type.FullName;

				var comments = GetComments(type.GetCustomAttributes<POCommentAttribute>());

				list.AddRange(ResourceObjectProviderBase.EnumerateFields(type).Where(x => x.FieldType == stringType).Select(field => new Entry
					{
						Context = string.Format("{0}.{1}", set, field.Name), 
						OriginalText = (string) field.GetValue(original), 
						TranslatedText = (string) field.GetValue(translated),
						Comments = GetComments(field.GetCustomAttributes<POCommentAttribute>(), comments)
					}));
				list.AddRange(ResourceObjectProviderBase.EnumerateProperties(type).Where(x => x.PropertyType == stringType).Select(property => new Entry
					{
						Context = string.Format("{0}.{1}", set, property.Name),
						OriginalText = (string)property.GetValue(original, null),
						TranslatedText = (string)property.GetValue(original, null),
						Comments = GetComments(property.GetCustomAttributes<POCommentAttribute>(), comments)
					}));
			}

			CachedResourceObjectProvider.RemoveAllCachedObjects();

			return new File(Name, culture.Name, list);
		}

		private static Comment[] GetComments(IEnumerable<POCommentAttribute> attributes, Comment[] defaults = null)
		{
			var comments = new Comment[CommentTypeCount];

			if (defaults != null)
			{
				foreach (var @default in defaults)
				{
					comments[(int) @default.Type] = @default;
				}
			}

			foreach (var attribute in attributes)
			{
				comments[(int) attribute.Type] = new Comment {Type = attribute.Type, Value = attribute.Value};
			}

			return comments.Where(x => x != null).ToArray();
		}
	}
}
