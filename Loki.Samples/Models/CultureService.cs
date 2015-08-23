using System.Globalization;
using System.Linq;
using System.Threading;

namespace Loki.Samples.Models
{
	/// <summary>
	/// Provides a way to obtain supported cultures.
	/// </summary>
	public static class CultureService
	{
		private static readonly CultureInfo[] SupportedCultures;

		static CultureService()
		{
			SupportedCultures = new[]
				{
					CultureInfo.GetCultureInfo("en"), 
					CultureInfo.GetCultureInfo("ru"), 
					CultureInfo.GetCultureInfo("es"),
					CultureInfo.GetCultureInfo("fr"), 
					CultureInfo.GetCultureInfo("de")
				};
		}

		/// <summary>
		/// Gets the cultures.
		/// </summary>
		public static CultureInfo[] Cultures
		{
			get { return SupportedCultures; }
		}

		/// <summary>
		/// Finds the culture.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <returns></returns>
		public static CultureInfo FindCulture(int culture)
		{
			return SupportedCultures.FirstOrDefault(x => x.LCID == culture) ?? SupportedCultures.First();
		}

		/// <summary>
		/// Finds the culture.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <returns></returns>
		public static CultureInfo FindCulture(string culture)
		{
			return SupportedCultures.FirstOrDefault(x => x.Name == culture) ?? SupportedCultures.First();
		}

		/// <summary>
		/// Sets the current culture on the current thread.
		/// </summary>
		/// <param name="culture">The culture.</param>
		public static void SetCulture(int culture)
		{
			Thread.CurrentThread.CurrentCulture = FindCulture(culture);
		}

		/// <summary>
		/// Sets the current culture on the current thread.
		/// </summary>
		/// <param name="culture">The culture.</param>
		public static void SetCulture(string culture)
		{
			Thread.CurrentThread.CurrentCulture = FindCulture(culture);
		}

		/// <summary>
		/// Sets the current culture on the current thread.
		/// </summary>
		/// <param name="culture">The culture.</param>
		public static void SetUICulture(int culture)
		{
			Thread.CurrentThread.CurrentUICulture = FindCulture(culture);
		}

		/// <summary>
		/// Sets the current culture on the current thread.
		/// </summary>
		/// <param name="culture">The culture.</param>
		public static void SetUICulture(string culture)
		{
			Thread.CurrentThread.CurrentUICulture = FindCulture(culture);
		}
	}
}