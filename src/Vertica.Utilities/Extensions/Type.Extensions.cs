using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Vertica.Utilities_v4.Extensions.StringExt;

namespace Vertica.Utilities_v4.Extensions.TypeExt
{
	public static class TypeExtensions
	{
		public static IEnumerable<Type> AllInterfaces(this Type target)
		{
			foreach (var @interface in target.GetInterfaces())
			{
				yield return @interface;
				foreach (var childInterface in @interface.AllInterfaces())
				{
					yield return childInterface;
				}
			}
		}

		public static IEnumerable<MethodInfo> AllMethods(this Type target)
		{
			return 
				new[]{target}.Concat(target.AllInterfaces())
				.SelectMany(type => type.GetMethods());
		}

		public static string NameWithNamespace(this Type target)
		{
			return target.Namespace.IsNotEmpty() ?
				target.Namespace + "." + target.Name :
				target.Name;
		}

		public static string LongName(this Type target, bool includeNamespace = false)
		{
			var sb = new StringBuilder();

			if (includeNamespace) sb.Append(NameWithNamespace(target));
			else sb.Append(target.Name);

			if (target.IsGenericType)
			{
				// remove generic apostrophes
				sb.Remove(sb.Length - 2, 2);
				sb.Append("<");
				Type[] arguments = target.GetGenericArguments();
				if (!target.IsGenericTypeDefinition)
				{
					foreach (Type argument in arguments)
					{
						if (includeNamespace) sb.Append(NameWithNamespace(argument));
						else sb.Append(argument.Name);
						sb.Append(", ");
					}
					sb.Remove(sb.Length - 2, 2);
				}
				else
				{
					for (int i = 0; i < arguments.Length - 1; i++)
					{
						sb.Append(",");
					}
				}
				sb.Append(">");
			}
			return sb.ToString();
		}

		public static T GetDefault<T>(this Type t)
		{
			return (T)GetDefault(t);
		}

		public static object GetDefault(this Type t)
		{
			if (!t.IsValueType) return null;
			return Activator.CreateInstance(t);
		}

		public static bool CanBeNulled(this Type type)
		{
			if (type == null) return false;

			return !type.IsValueType || type.IsNullable();
		}

		public static bool IsNullable(this Type type)
		{
			if ((type == null) || !type.IsValueType)
			{
				return false;
			}
			return (type.IsGenericType && type.GetGenericTypeDefinition() == (typeof(Nullable<>)));
		}
	}
}