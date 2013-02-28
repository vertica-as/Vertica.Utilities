using System;
using System.Collections.Generic;
using Vertica.Utilities_v4.Extensions.Infrastructure;
using Vertica.Utilities_v4.Extensions.StringExt;

namespace Vertica.Utilities_v4.Extensions.ObjectExt
{
	public static class ObjectExtensions
	{
		public static string SafeToString<T>(this T instance, string @default = null) where T : class
		{
			return instance == null ? @default : instance.ToString();
		}

		public static U Safe<T, U>(this T argument, Func<T, U> func)
			where T : class
			where U : class
		{
			U result = null;
			if (argument != null)
			{
				result = func(argument);
			}
			return result;
		}

		#region Db, old ADO checking

		public class DbExtensionPoint : ExtensionPoint<object>
		{
			public DbExtensionPoint(object value) : base(value) { }
		}

		public static DbExtensionPoint Db(this object subject)
		{
			return new DbExtensionPoint(subject);
		}

		/// <summary>
		/// Returns <c>null</c> if the object cannot be converted somehow to bool or if it is<see cref="DBNull.Value"/>.
		/// </summary>
		public static bool? ToBoolean(this DbExtensionPoint e)
		{
			bool? result = null;
			try
			{
				object o = e.ExtendedValue;
				if (o != DBNull.Value)
				{
					if (o is bool)
					{
						result = (bool)o;
					}
					else if (IsIntegral(o))
					{
						long l = Convert.ToInt64(o);
						if (l == 0) result = false;
						if (l == 1) result = true;
					}
					else if (o is char)
					{
						var c = (char) o;
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
			}
			// swallow any exception and return null
			catch { }
			return result;
		}

		public static T? Unbox<T>(this DbExtensionPoint e) where T: struct
		{
			T? result = null;
			try
			{
				if (e.ExtendedValue != null)
				{
					result = (T?)Convert.ChangeType(e.ExtendedValue, typeof(T));
				}
			}
			// there is no TryChangeType :-(
			catch (FormatException) { }
			catch (OverflowException) { }
			catch (InvalidCastException) { }
			return result;
		}

		#endregion

		public static bool IsIntegral(this object o)
		{
			return (o is byte) || (o is sbyte) ||
				(o is short) || (o is ushort) ||
				(o is int) || (o is uint) ||
				(o is long) || (o is ulong);
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
	}
}
