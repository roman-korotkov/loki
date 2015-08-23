using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Loki.Resources
{
	/// <summary>
	/// Converts a resource set into resource object.
	/// </summary>
	public abstract class ResourceObjectProviderBase
	{
		const BindingFlags MemberFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;

		/// <summary>
		/// Gets the resouce object of the specified type from the specified resource provider for the specified culture.
		/// </summary>
		/// <typeparam name="TResource">The type of the resource.</typeparam>
		/// <param name="resourceProvider">The resource provider.</param>
		/// <param name="culture">The culture.</param>
		/// <returns>The resource object.</returns>
		public abstract TResource Get<TResource>(ResourceProvider resourceProvider, CultureInfo culture);

		/// <summary>
		/// Enumerates the localizable members of the specified resource type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The localizable members.</returns>
		protected internal static IEnumerable<MemberInfo> EnumerateMembers(Type type)
		{
			foreach (var field in EnumerateFields(type))
			{
				yield return field;
			}
			foreach (var property in EnumerateProperties(type))
			{
				yield return property;
			}
		}

		/// <summary>
		/// Enumerates the localizable fields of the specified resource type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The localizable fields.</returns>
		protected internal static IEnumerable<FieldInfo> EnumerateFields(Type type)
		{
			return type.GetFields(MemberFlags).Where(x => !x.IsInitOnly);
		}

		/// <summary>
		/// Enumerates the localizable fields of the specified resource type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The localizable fields.</returns>
		protected internal static IEnumerable<PropertyInfo> EnumerateProperties(Type type)
		{
			return type.GetProperties(MemberFlags).Where(x => x.CanWrite);
		}
	}
}