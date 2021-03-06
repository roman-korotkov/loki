﻿using System;
using System.Globalization;
using JetBrains.Annotations;
using Loki.Resources;

namespace Loki.Gettext
{
	/// <summary>
	/// Represents an implementation of <see cref="ResourceProvider"/> that returns resources for translation.
	/// </summary>
	internal class GettextResourceProvider : ResourceProvider
	{
		private readonly ResourceProvider _provider;

		/// <summary>
		/// Initializes a new instance of the <see cref="GettextResourceProvider"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		public GettextResourceProvider([NotNull] ResourceProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			_provider = provider;
		}

		/// <summary>
		/// Gets the localized resource set for the specified culture with the specified resource set name.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="set">The resource set name.</param>
		/// <returns>The string for the specified key.</returns>
		public override ResourceSet GetResourceSet(CultureInfo culture, string set)
		{
			var resourceSet = _provider.GetResourceSet(culture, set);

			return resourceSet != null ? new GettextResourceSet(resourceSet) : null;
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
			_provider.TryGet(culture, set, key, out value);

			return true;
		}
	}
}
