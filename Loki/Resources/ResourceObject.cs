using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Loki.Resources
{
	internal static class ResourceObject<TResource>
	{
		private static readonly ConcurrentDictionary<int, TResource> Cache;

		private static readonly Func<ResourceProvider, CultureInfo, TResource> Implementation;

		static ResourceObject()
		{
			var resourceType = typeof (TResource);

			var constructor = resourceType.GetConstructor(new Type[0]);

			Implementation = constructor == null ? Default : BuildExpression(resourceType, constructor).Compile();

			Cache = new ConcurrentDictionary<int, TResource>();

			CachedResourceObjectProvider.ClearCache += OnClearCache;
		}

		internal static TResource Get(ResourceProvider resourceProvider, CultureInfo culture)
		{
			return Implementation(resourceProvider, culture);
		}

		internal static TResource GetCached(ResourceProvider resourceProvider, CultureInfo culture)
		{
			return Cache.GetOrAdd(culture.LCID, _ => Get(resourceProvider, culture));
		}

		private static Expression<Func<ResourceProvider, CultureInfo, TResource>> BuildExpression(Type resourceType, ConstructorInfo constructor)
		{
			var provider = Expression.Parameter(Reflection.Types.ResourceProvider);
			var culture = Expression.Parameter(Reflection.Types.CultureInfo);

			var resource = Expression.Variable(resourceType);

			var variables = new List<ParameterExpression> { resource };
			var expressions = new List<Expression> { Expression.Assign(resource, Expression.New(constructor)) };

			var type = resourceType;
			while (type != null)
			{
				InitMembers(provider, culture, resource, type, variables, expressions);

				type = type.BaseType != typeof (object) ? type.BaseType : null;
			}

			expressions.Add(resource);

			var body = Expression.Block(variables, expressions);

			return Expression.Lambda<Func<ResourceProvider, CultureInfo, TResource>>(body, provider, culture);
		}

		private static void InitMembers(Expression provider, Expression culture, Expression resource, Type type, List<ParameterExpression> variables, List<Expression> expressions)
		{
			var set = Expression.Variable(Reflection.Types.ResourceSet);
			var setName = Expression.Constant(type.FullName);
			var getResourceSetCall = Expression.Call(provider, Reflection.Methods.ResourceProviderGetResourceSet, culture, setName);

			variables.Add(set);
			expressions.Add(Expression.Assign(set, getResourceSetCall));

			foreach (var member in ResourceObjectProviderBase.EnumerateMembers(type).Select(x => Expression.MakeMemberAccess(resource, x)))
			{
				var variable = Expression.Variable(member.Type);

				variables.Add(variable);

				var tryGetCall = Expression.Call(set, Reflection.Methods.ResourceSetTryGet.MakeGenericMethod(member.Type), Expression.Constant(member.Member.Name), variable);
				var assignField = Expression.Assign(member, variable);

				if (member.Type == typeof(string))
				{
					expressions.Add(Expression.IfThen(tryGetCall, assignField));
				}
				else
				{
					var resourceObjectType = typeof(ResourceObject<>).MakeGenericType(member.Type);
					var get = resourceObjectType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic).Single(x => x.Name == "Get");
					var getCall = Expression.Call(get, provider, culture);

					expressions.Add(Expression.IfThenElse(tryGetCall, assignField, Expression.Assign(member, getCall)));
				}
			}
		}

		private static TResource Default(ResourceProvider resourceProvider, CultureInfo culture)
		{
			return default(TResource);
		}

		private static void OnClearCache()
		{
			Cache.Clear();
		}

	}
}