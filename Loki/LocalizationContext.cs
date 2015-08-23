using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Loki.Resources;

namespace Loki
{
	/// <summary>
	/// Represents a localization context.
	/// </summary>
	public class LocalizationContext
	{
		private static readonly MethodInfo GetResourceObjectMethod;

		private readonly ResourceProvider _resourceProvider;
		private readonly CultureInfo _culture;

		private ResourceSet _resourceSet;
		private ResourceObjectProviderBase _resourceObjectProvider;

		static LocalizationContext()
		{
			GetResourceObjectMethod = typeof (LocalizationContext).
				GetMethods().
				Single(x => x.Name == "GetResourceObject" &&
				            x.IsGenericMethodDefinition &&
				            x.GetGenericArguments().Length == 1 &&
				            x.GetParameters().Length == 0);

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizationContext" /> class.
		/// </summary>
		/// <param name="resourceProvider">The resource provider.</param>
		/// <param name="culture">The culture.</param>
		/// <param name="resourceObjectProvider">The resource object provider.</param>
		public LocalizationContext(ResourceProvider resourceProvider, CultureInfo culture, ResourceObjectProviderBase resourceObjectProvider = null)
		{
			if (resourceProvider == null)
			{
				throw new ArgumentNullException("resourceProvider");
			}
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			_resourceProvider = resourceProvider;
			_culture = culture;
			_resourceObjectProvider = resourceObjectProvider ?? ResourceObjectProviders.Current;
		}

		/// <summary>
		/// Gets or sets the resource provider associated with the current context.
		/// </summary>
		public ResourceProvider ResourceProvider
		{
			get { return _resourceProvider; }
		}
		
		/// <summary>
		/// Gets the culture associated with the current context.
		/// </summary>
		public CultureInfo Culture
		{
			get { return _culture; }
		}

		/// <summary>
		/// Gets or sets the resource set.
		/// </summary>
		public ResourceSet ResourceSet
		{
			get { return _resourceSet; }
			set { _resourceSet = value ?? EmptyResourceSet.Instance; }
		}

		/// <summary>
		/// Gets or sets the resource object provider.
		/// </summary>
		public ResourceObjectProviderBase ObjectProvider
		{
			get { return _resourceObjectProvider; }
			set { _resourceObjectProvider = value ?? ResourceObjectProviders.Current; }
		}

		/// <summary>
		/// Returns the default resource object for the current context.
		/// </summary>
		/// <returns>The default resource object.</returns>
		public dynamic GetResourceObject()
		{
			return ObjectProvider.Get<dynamic>(ResourceProvider, Culture);
		}

		/// <summary>
		/// Gets the resource object of the specified type.
		/// </summary>
		/// <typeparam name="TResource">The type of the resource.</typeparam>
		/// <returns>The resource object.</returns>
		public TResource GetResourceObject<TResource>()
		{
			return ObjectProvider.Get<TResource>(ResourceProvider, Culture);
		}

		/// <summary>
		/// Gets the resource object of the specified type.
		/// </summary>
		/// <param name="type">The type of the resource.</param>
		/// <returns>The resource object.</returns>
		public object GetResourceObject(Type type)
		{
			return GetResourceObjectMethod.MakeGenericMethod(type).Invoke(this, new object[0]);
		}
	}
}
