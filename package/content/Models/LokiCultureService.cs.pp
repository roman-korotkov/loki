using System.Globalization;
using System.Linq;
using System.Threading;

namespace $rootnamespace$.Models
{
	public static class LokiCultureService
	{
		private static readonly CultureInfo[] SupportedCultures;

		static LokiCultureService()
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

		public static CultureInfo[] Cultures
		{
			get { return SupportedCultures; }
		}

		public static CultureInfo FindCulture(int culture)
		{
			return SupportedCultures.FirstOrDefault(x => x.LCID == culture) ?? SupportedCultures.First();
		}

		public static CultureInfo FindCulture(string culture)
		{
			return SupportedCultures.FirstOrDefault(x => x.Name == culture) ?? SupportedCultures.First();
		}

		public static void SetCulture(int culture)
		{
			Thread.CurrentThread.CurrentCulture = FindCulture(culture);
		}

		public static void SetCulture(string culture)
		{
			Thread.CurrentThread.CurrentCulture = FindCulture(culture);
		}

		public static void SetUICulture(int culture)
		{
			Thread.CurrentThread.CurrentUICulture = FindCulture(culture);
		}

		public static void SetUICulture(string culture)
		{
			Thread.CurrentThread.CurrentUICulture = FindCulture(culture);
		}
	}
}