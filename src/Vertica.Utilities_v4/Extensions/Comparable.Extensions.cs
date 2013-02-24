using System;
using System.Collections.Generic;
using Vertica.Utilities_v4.Comparisons;

namespace Vertica.Utilities_v4.Extensions.ComparableExt
{
	public static class ComparableExtensions
	{
		#region IComparable<>

		public static bool IsEqualTo<T>(this T first, T second) where T : IComparable<T>
		{
			return IsEqualTo(first, second, Comparer<T>.Default);
		}

		public static bool IsEqualTo<T>(this T first, T second, IComparer<T> comparer)
		{
			return comparer.Compare(first, second) == 0;
		}

		public static bool IsDifferentFrom<T>(this T first, T second) where T : IComparable<T>
		{
			return !IsEqualTo(first, second);
		}

		public static bool IsDifferentFrom<T>(this T first, T second, IComparer<T> comparer)
		{
			return !IsEqualTo(first, second, comparer);
		}

		public static bool IsAtMost<T>(this T first, T second) where T : IComparable<T>
		{
			return IsAtMost(first, second, Comparer<T>.Default);
		}

		public static bool IsAtMost<T>(this T first, T second, IComparer<T> comparer)
		{
			return comparer.Compare(first, second) <= 0;
		}

		public static bool IsAtLeast<T>(this T first, T second) where T : IComparable<T>
		{
			return IsAtLeast(first, second, Comparer<T>.Default);
		}

		public static bool IsAtLeast<T>(this T first, T second, IComparer<T> comparer)
		{
			return comparer.Compare(first, second) >= 0;
		}

		public static bool IsLessThan<T>(this T first, T second) where T : IComparable<T>
		{
			return IsLessThan(first, second, Comparer<T>.Default);
		}

		public static bool IsLessThan<T>(this T first, T second, IComparer<T> comparer)
		{
			return comparer.Compare(first, second) < 0;
		}

		public static bool IsMoreThan<T>(this T first, T second) where T : IComparable<T>
		{
			return IsMoreThan(first, second, Comparer<T>.Default);
		}

		public static bool IsMoreThan<T>(this T first, T second, IComparer<T> comparer)
		{
			return comparer.Compare(first, second) > 0;
		}

		#endregion

		#region IComparable

		public static bool IsEqualTo(this IComparable first, object second)
		{
			return Comparer<object>.Default.Compare(first, second) == 0;
		}

		public static bool IsDifferentFrom(this IComparable first, object second)
		{
			return !IsEqualTo(first, second);
		}

		public static bool IsAtMost(this IComparable first, object second)
		{
			return first.CompareTo(second) <= 0;
		}

		public static bool IsAtLeast(this IComparable first, object second)
		{
			return first.CompareTo(second) >= 0;
		}

		public static bool IsLessThan(this IComparable first, object second)
		{
			return first.CompareTo(second) < 0;
		}

		public static bool IsMoreThan(this IComparable first, object second)
		{
			return first.CompareTo(second) > 0;
		}

		#endregion

		public static IComparer<T> Reverse<T>(this IComparer<T> comparer)
		{
			return new ReversedComparer<T>(comparer);
		}
	}
}
