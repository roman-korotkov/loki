using System.Globalization;

namespace Loki.Resources
{
	public static class Resources
	{
		public static TValue ValueOrDefaultFor<TValue>(this ResourceProvider provider, CultureInfo culture, string set, string key, TValue defaultValue = default (TValue))
		{
			TValue value;
			return provider.TryGet(culture, set, key, out value) ? value : defaultValue;
		}

		public static string ValueOrDefaultFor(this ResourceProvider provider, CultureInfo culture, string set, string key)
		{
			return provider.ValueOrDefaultFor(culture, set, key, key);
		}

		public static TValue ValueOrDefaultFor<TValue>(this ResourceSet set, string key, TValue defaultValue = default (TValue))
		{
			TValue value;
			return set.TryGet(key, out value) ? value : defaultValue;
		}

		public static string ValueOrDefaultFor(this ResourceSet set, string key)
		{
			return set.ValueOrDefaultFor(key, key);
		}

	}
}
