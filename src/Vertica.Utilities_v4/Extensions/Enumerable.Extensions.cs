using System;
using System.Collections.Generic;
using System.Linq;
using Vertica.Utilities_v4.Collections;
using Vertica.Utilities_v4.Resources;

namespace Vertica.Utilities_v4.Extensions.EnumerableExt
{
	// ReSharper disable PossibleMultipleEnumeration
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

		public static IEnumerable<T> SkipNulls<T>(this IEnumerable<T> source) where T : class
		{
			return source.EmptyIfNull().Where(s => s != null);
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

		public static void For<T>(this IEnumerable<T> collection, Action<T, int> action)
		{
			int i = 0;

			foreach (var item in collection.EmptyIfNull())
			{
				action(item, i++);
			}
		}

		public static void For<T>(this IEnumerable<T> collection, Action<T, int> action, IEnumerable<int> indexes)
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

		#endregion

		public static IEnumerable<TBase> Convert<TDerived, TBase>(this IEnumerable<TDerived> source) where TDerived : TBase
		{
			return source.EmptyIfNull().Select(i => (TBase)i);
		}

		#region ToDelimited

		public static string ToDelimitedString<T>(this IEnumerable<T> source, string delimiter, Func<T, string> toString)
		{
			return string.Join(delimiter, source.EmptyIfNull().Select(toString));
		}

		public static string ToDelimitedString<T>(this IEnumerable<T> source, string delimiter)
		{
			return ToDelimitedString(source, delimiter, t => t.ToString());
		}

		public static string ToDelimitedString<T>(this IEnumerable<T> source, Func<T, string> toString)
		{
			return ToDelimitedString(source, ", ", toString);
		}

		public static string ToDelimitedString<T>(this IEnumerable<T> source)
		{
			return ToDelimitedString(source, ", ", t => t.ToString());
		}

		public static string ToCsv<T>(this IEnumerable<T> source)
		{
			return ToDelimitedString(source, ",");
		}

		public static string ToCsv<T>(this IEnumerable<T> source, Func<T, string> toString)
		{
			return ToDelimitedString(source, ",", toString);
		}

		#endregion

		#region enumerable generation

		/* based on http://www.codeproject.com/KB/collections/Enumerators.aspx */

		public static IEnumerable<T> ToCircular<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable.EmptyIfNull().Any())
			{
				while (true)
				{
					foreach (T item in enumerable)
					{
						yield return item;
					}
				}
			}
		}

		public static IEnumerable<T> ToStepped<T>(this IEnumerable<T> enumerable, uint step)
		{
			Guard.AgainstArgument<ArgumentOutOfRangeException>("step", step < 1, "Cannot be negative or zero.");

			using (IEnumerator<T> enumerator = enumerable.EmptyIfNull().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					yield return enumerator.Current;

					for (uint i = step; i > 1; i--)
						if (!enumerator.MoveNext()) break;
				}
			}
		}

		public static IEnumerable<Pair<T>> Merge<T>(this IEnumerable<T> firsts, IEnumerable<T> seconds)
		{
			using (var firstsEnumerator = firsts.EmptyIfNull().GetEnumerator())
			{
				using (var secondsEnumerator = seconds.EmptyIfNull().GetEnumerator())
				{
					bool nextFirst = firstsEnumerator.MoveNext();
					bool nextSecond = secondsEnumerator.MoveNext();

					while (nextFirst && nextSecond)
					{
						yield return new Pair<T>(firstsEnumerator.Current, secondsEnumerator.Current);

						nextFirst = firstsEnumerator.MoveNext();
						nextSecond = secondsEnumerator.MoveNext();
					}

					if (nextFirst || nextSecond)
					{
						throw new ArgumentException("firsts && seconds not same length", "seconds");
					}
				}
			}
		}

		public static IEnumerable<Tuple<T, K>> Merge<T, K>(this IEnumerable<T> firsts, IEnumerable<K> seconds)
		{
			using (var firstsEnumerator = firsts.EmptyIfNull().GetEnumerator())
			{
				using (var secondsEnumerator = seconds.EmptyIfNull().GetEnumerator())
				{
					bool nextFirst = firstsEnumerator.MoveNext();
					bool nextSecond = secondsEnumerator.MoveNext();

					while (nextFirst && nextSecond)
					{
						yield return Tuple.Create(firstsEnumerator.Current, secondsEnumerator.Current);

						nextFirst = firstsEnumerator.MoveNext();
						nextSecond = secondsEnumerator.MoveNext();
					}

					if (nextFirst || nextSecond)
					{
						throw new ArgumentException("firsts && seconds not same length", "seconds");
					}
				}
			}
		}

		public static IEnumerable<T> Interlace<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			using (IEnumerator<T> e1 = first.EmptyIfNull().GetEnumerator())
			{
				using (IEnumerator<T> e2 = second.EmptyIfNull().GetEnumerator())
					while (e1.MoveNext() && e2.MoveNext())
					{
						yield return e1.Current;
						yield return e2.Current;
					}
			}
		}

		public static IEnumerable<TResult> Zip<T1, T2, TResult>(this IEnumerable<T1> first, IEnumerable<T2> second, Func<T1, T2, int, TResult> selector)
		{
			return first.Zip(second, (a, b) => new { a, b }).Select((pair, i) => selector(pair.a, pair.b, i));
		}

		public static IEnumerable<Tuple<T1, T2>> Zip<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second)
		{
			var enumerator1 = first.EmptyIfNull().GetEnumerator();
			var enumerator2 = second.EmptyIfNull().GetEnumerator();

			bool moreOfFirst;
			do
			{
				moreOfFirst = enumerator1.MoveNext();
				bool moreOfSecond = enumerator2.MoveNext();

				if (moreOfFirst != moreOfSecond)
					throw new InvalidOperationException(Exceptions.EnumerableExtensions_Zip_SameLength);

				if (moreOfSecond)
					yield return Tuple.Create(enumerator1.Current, enumerator2.Current);

			} while (moreOfFirst);
		}

		#endregion

		#region concatenation helpers

		public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params T[] toTheEnd)
		{
			return source.Concat(toTheEnd.EmptyIfNull());
		}

		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, params T[] inTheBeginning)
		{
			return inTheBeginning.EmptyIfNull().Concat(source);
		}

		#endregion

		#region batching

		public static IEnumerable<IEnumerable<T>> InBatchesOf<T>(this IEnumerable<T> items, uint batchSize)
		{
			Guard.AgainstArgument<ArgumentOutOfRangeException>("batchSize", batchSize == 0, Exceptions.EnumerableExtensions_NonZeroBatch);

			int count = 0;
			T[] batch = new T[batchSize];
			foreach (T item in items)
			{
				batch[count++ % batchSize] = item;
				if (count % batchSize == 0) yield return batch;
			}
			if (count % batchSize > 0) yield return batch.Take(count % (int)batchSize);
		}

		#endregion

		#region Shuffle

		public static IEnumerable<T> Shuffle<T>(this IList<T> collection)
		{
			return Shuffle(collection, new FrameworkRandomizer());
		}

		public static IEnumerable<T> Shuffle<T>(this IList<T> collection, int itemsToTake)
		{
			return Shuffle(collection, new FrameworkRandomizer(), itemsToTake);
		}

		public static IEnumerable<T> Shuffle<T>(this IList<T> collection, IRandomizer provider)
		{
			IList<T> safeCollection = collection ?? new T[0];
			return Shuffle(collection, provider, safeCollection.Count);
		}

		public static IEnumerable<T> Shuffle<T>(this IList<T> collection, IRandomizer provider, int itemsToTake)
		{
			// make a copy of the argument
			IList<T> safeCollection = collection == null ? new List<T>(0) : collection.ToList();
			// take a copy of the current list
			int initialCount = Math.Min(safeCollection.Count, itemsToTake);
			if (safeCollection.Count > 0)
			{
				// iterate count times and "randomly" return one of the 
				// elements
				for (int i = 1; i <= initialCount; i++)
				{
					// maximum index actually buffer.Count -1 because 
					// Random.Next will only return values LESS than 
					// specified.
					int randomIndex = provider.Next(safeCollection.Count);
					yield return safeCollection[randomIndex];
					// remove the element to prevent further selection
					safeCollection.RemoveAt(randomIndex);
				}
			}
		}

		#endregion

		#region CompareBy

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
			Func<TSource, TKey> selector)
		{
			return source.MinBy(selector, Comparer<TKey>.Default);
		}

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
			   Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			return compareBy(source, selector, comparer, i => i < 0);
		}

		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
			Func<TSource, TKey> selector)
		{
			return source.MaxBy(selector, Comparer<TKey>.Default);
		}

		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
			   Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			return compareBy(source, selector, comparer, i => i > 0);
		}

		private static TSource compareBy<TSource, TKey>(IEnumerable<TSource> source,
			Func<TSource, TKey> selector, IComparer<TKey> comparer,
			Func<int, bool> candidateComparison)
		{
			Guard.AgainstNullArgument("source", source);

			using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
			{
				if (!sourceIterator.MoveNext())
				{
					throw new InvalidOperationException(Exceptions.EnumerableExtensions_EmptyCollection);
				}
				TSource current = sourceIterator.Current;
				TKey currentKey = selector(current);
				while (sourceIterator.MoveNext())
				{
					TSource candidate = sourceIterator.Current;
					TKey candidateProjected = selector(candidate);
					if (candidateComparison(comparer.Compare(candidateProjected, currentKey)))
					{
						current = candidate;
						currentKey = candidateProjected;
					}
				}
				return current;
			}
		}

		#endregion

		#region ToHashSet

		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
		{
			Guard.AgainstNullArgument(new { source });

			return new HashSet<T>(source);
		}
		#endregion

		// ReSharper restore PossibleMultipleEnumeration
	}
}
