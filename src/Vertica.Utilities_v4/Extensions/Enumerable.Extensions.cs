using System;
using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities_v4.Extensions.EnumerableExt
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
		{
			return source ?? Enumerable.Empty<T>();
		}

		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (var item in source.EmptyIfNull())
			{
				action(item);
			}
		}

		internal static void Iterate<T>(this IEnumerable<T> enumerable)
		{
#pragma warning disable 168
			foreach (var item in enumerable) { }
#pragma warning restore 168
		}
	}
}
