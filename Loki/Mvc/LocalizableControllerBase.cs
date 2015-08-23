using System;
using System.Web.Mvc;
using Loki.Resources;

namespace Loki.Mvc
{
	/// <summary>
	/// Represents a controller that provides base localization facilities.
	/// </summary>
	public class LocalizableControllerBase : Controller
	{
		private LocalizationContext _localizationContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizableController"/> class.
		/// </summary>
		public LocalizableControllerBase()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizableController"/> class.
		/// </summary>
		/// <param name="localizationContext">The localization context.</param>
		/// <remarks>
		/// For dependency injection.
		/// </remarks>
		public LocalizableControllerBase(LocalizationContext localizationContext)
		{
			if (localizationContext == null)
			{
				throw new ArgumentNullException("localizationContext");
			}
			_localizationContext = localizationContext;
		}

		/// <summary>
		/// Gets or sets the localization context.
		/// </summary>
		public virtual LocalizationContext LocalizationContext
		{
			get { return _localizationContext ?? (_localizationContext = MvcLocalization.GetContext(ControllerContext)); }
			set { _localizationContext = value; }
		}

		/// <summary>
		/// Retrieves the resource of the specified type for the current culture, the specified resource set and key.
		/// </summary>
		/// <typeparam name="TResource">The type of the resource.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="set">The set.</param>
		/// <returns>The resource instance.</returns>
		public TResource Resource<TResource>(string key, string set = null)
		{
			return set == null
				? LocalizationContext.ResourceSet.ValueOrDefaultFor<TResource>(key)
				: LocalizationContext.ResourceProvider.ValueOrDefaultFor<TResource>(LocalizationContext.Culture, set, key);
		}

		/// <summary>
		/// Retrieves the string resource for the the current culture, the specified resource set and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="set">The set.</param>
		/// <returns>The string resource.</returns>
		public string SR(string key, string set = null)
		{
			return set == null
				? LocalizationContext.ResourceSet.ValueOrDefaultFor(key)
				: LocalizationContext.ResourceProvider.ValueOrDefaultFor(LocalizationContext.Culture, set, key);
		}

		/// <summary>
		/// Retrieves the formatted string resource for the the current culture, the specified resource set and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="set">The set.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>The string for the current culture associated with the specified key.</returns>
		public string SRFormat(string key, string set, params object[] parameters)
		{
			var value = set == null
				? LocalizationContext.ResourceSet.ValueOrDefaultFor(key)
				: LocalizationContext.ResourceProvider.ValueOrDefaultFor(LocalizationContext.Culture, set, key);

			return parameters == null || parameters.Length == 0
				? value
				: string.Format(LocalizationContext.Culture, value, parameters);
		}
	}
}