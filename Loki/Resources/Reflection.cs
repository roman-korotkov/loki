using System;
using System.Globalization;
using System.Reflection;

namespace Loki.Resources
{
	internal static class Reflection
	{
		internal static class Types
		{
			public static readonly Type ResourceProvider;
			public static readonly Type CultureInfo;
			public static readonly Type ResourceSet;

			static Types()
			{
				ResourceProvider = typeof (ResourceProvider);
				CultureInfo = typeof (CultureInfo);
				ResourceSet = typeof (ResourceSet);
			}
		}

		internal static class Methods
		{
			public static readonly MethodInfo ResourceProviderGetResourceSet;
			public static readonly MethodInfo ResourceSetTryGet;

			static Methods()
			{
				ResourceProviderGetResourceSet = Types.ResourceProvider.GetMethod("GetResourceSet");
				ResourceSetTryGet = Types.ResourceSet.GetMethod("TryGet");
			}
		}
	}
}