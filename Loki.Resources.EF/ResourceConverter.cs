using System;

namespace Loki.Resources.EF
{
	public abstract class ResourceConverter
	{
		public static readonly ResourceConverter Default = new DefaultResourceConverter();

		public abstract TValue To<TValue>(string value);
		public abstract string From<TValue>(TValue value);

		private class DefaultResourceConverter : ResourceConverter
		{
			public override TValue To<TValue>(string value)
			{
				try
				{
					return (TValue) (object) value;
				}
				catch (InvalidCastException)
				{
					return default(TValue);
				}
			}

			public override string From<TValue>(TValue value)
			{
				try
				{
					return (string)(object)value;
				}
				catch (InvalidCastException)
				{
					return string.Empty;
				}
			}
		}
	}
}