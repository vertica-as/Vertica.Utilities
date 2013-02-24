using System;
using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities_v4.Extensions.EnumerableExt
{
	public static class EnumerableExtensions
	{
		#region nullability

		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
		{
			return source ?? Enumerable.Empty<T>();
		}

		public static IEnumerable<T> NullIfEmpty<T>(this IEnumerable<T> source)
		{
			return source == null || !source.Any() ? null : source;
		}

		#endregion

		#region count contraints

		public static bool HasOne<T>(this IEnumerable<T> source)
		{
			bool hasOne = false;
			if (source != null)
			{
				IEnumerator<T> enumerator = source.GetEnumerator();
				hasOne = enumerator.MoveNext() && !enumerator.MoveNext();
			}
			return hasOne;
		}

		public static bool HasAtLeast<T>(this IEnumerable<T> source, uint matchingCount)
		{
			bool result = false;
			if (source != null)
			{
				int i = 0;
				using (var enumerator = source.GetEnumerator())
				{
					while (i <= matchingCount && enumerator.MoveNext())
					{
						i++;
					}
				}
				result = i >= matchingCount;
			}
			return result;
		}

		#endregion

		#region iteration

		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (var item in source.EmptyIfNull())
			{
				action(item);
			}
		}

		public static void For<T>(this IEnumerable<T> collection, IEnumerable<int> indexes, Action<T, int> action)
		{
			var hashedIndexes = new HashSet<int>(indexes);

			int i = 0;
			foreach (var item in collection.EmptyIfNull())
			{
				if (hashedIndexes.Contains(i))
				{
					action(item, i);
				}
				i++;
			}
		}

		public static void For<T>(this IEnumerable<T> collection, Action<T, int> action, params int[] indexes)
		{
			For(collection, indexes.AsEnumerable(), action);
		}

		internal static void Iterate<T>(this IEnumerable<T> enumerable)
		{
#pragma warning disable 168
			foreach (var item in enumerable) { }
#pragma warning restore 168
		}

		#endregion

		
	}
}
