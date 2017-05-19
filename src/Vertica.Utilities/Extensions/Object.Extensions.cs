using System;
using System.Collections.Generic;
using System.Reflection;
using Vertica.Utilities.Extensions.TypeExt;

namespace Vertica.Utilities.Extensions.ObjectExt
{
	public static class ObjectExtensions
	{
		#region safe

		public static string SafeToString<T>(this T instance, string @default = null) where T : class
		{
			return instance == null ? @default : instance.ToString();
		}

		public static U Safe<T, U>(this T argument, Func<T, U> func, Predicate<T> isSafe = null)
			where T : class
			where U : class
		{
			return notSafe(argument, isSafe) ? null : func(argument);
		}

		public static U? SafeValue<T, U>(this T argument, Func<T, U> func, Predicate<T> isSafe = null)
			where T : class
			where U : struct
		{
			return notSafe(argument, isSafe) ? default(U?) : func(argument);
		}

		public static U? SafeValue<T, U>(this T argument, Func<T, U?> func, Predicate<T> isSafe = null)
			where T : class
			where U : struct
		{
			return notSafe(argument, isSafe) ? default(U?) : func(argument);
		}

		private static bool notSafe<T>(T argument, Predicate<T> isSafe) where T : class
		{
			bool notSafe = isSafe != null ? !isSafe(argument) : EqualityComparer<T>.Default.Equals(argument, default(T));
			return notSafe;
		}

		#endregion

		#region unbox, checking DbNull

		/// <summary>
		/// Returns <c>null</c> if the object cannot be converted somehow to bool.
		/// </summary>
		public static bool? UnboxBool(this object o)
		{
			bool? result = null;
			try
			{
				if (o is bool)
				{
					result = (bool)o;
				}
				else if (o.IsIntegral())
				{
					long l = Convert.ToInt64(o);
					if (l == 0) result = false;
					if (l == 1) result = true;
				}
				else if (o is char)
				{
					var c = (char)o;
					if (c.Equals('0')) result = false;
					if (c.Equals('1')) result = true;
					if (c.Equals('t') || c.Equals('T')) result = true;
					if (c.Equals('f') || c.Equals('F')) result = false;
				}
				else
				{
					var str = o as string;
					bool parsed;
					if (bool.TryParse(str, out parsed))
					{
						result = parsed;
					}
					else
					{
						IEqualityComparer<string> comparer = StringComparer.OrdinalIgnoreCase;
						if (comparer.Equals(str, "1")) result = true;
						else if (comparer.Equals(str, "0")) result = false;
						else if (comparer.Equals(str, "t")) result = true;
						else if (comparer.Equals(str, "f")) result = false;
					}
				}
			}
			// swallow any exception and return null
			catch { }
			return result;
		}

		public static T? Unbox<T>(this object o) where T : struct
		{
			T? result = null;
			try
			{
				if (o != null)
				{
					result = (T?)Convert.ChangeType(o, typeof(T));
				}
			}
			// there is no TryChangeType :-(
			catch (FormatException) { }
			catch (OverflowException) { }
			catch (InvalidCastException) { }
			return result;
		}

		#endregion

		public static bool IsBoxedDefault(this object instance)
		{
			bool isDefault = true;
			if (instance != null)
			{
				Type boxedType = instance.GetType();
				isDefault = boxedType.GetTypeInfo().IsValueType && instance.Equals(boxedType.GetDefault());
			}
			return isDefault;
		}

		public static bool IsNotDefault<T>(this T instance)
		{
			return !IsDefault(instance);
		}

		// ReSharper disable RedundantCast
		public static bool IsDefault<T>(this T instance)
		{
			bool result = true;

			if ((object)instance != null)
			{
				result = instance.Equals(default(T));
			}
			return result;
		}

		public static bool IsIntegral<T>(this T o)
		{
			return (o is byte) || (o is sbyte) ||
				(o is short) || (o is ushort) ||
				(o is int) || (o is uint) ||
				(o is long) || (o is ulong);
		}

		#region Cast

		public static T Cast<T>(this T instance)
		{
			return (T)instance;
		}

		#endregion
	}
}
