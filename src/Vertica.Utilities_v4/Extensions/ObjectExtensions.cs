using System;

namespace Vertica.Utilities_v4.Extensions.ObjectExt
{
	public static class ObjectExtensions
	{
		public static string SafeToString<T>(this T instance, string @default = null) where T : class
		{
			return instance == null ? @default : instance.ToString();
		}

		public static T NullOrAction<T>(this T argument, Func<T> func) where T : class
		{
			T result = null;
			if (argument != null)
			{
				result = func();
			}
			return result;
		}
	}
}
