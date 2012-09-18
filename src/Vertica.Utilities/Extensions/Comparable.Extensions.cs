using System;
using System.Collections.Generic;

namespace Vertica.Utilities.Extensions.ComparableExt
{
	public static class ComparableExtensions
	{
		#region IComparable<>

		public static bool IsEqualTo<T>(this T first, T second) where T : IComparable<T>
		{
			return first.CompareTo(second) == 0;
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
			return first.CompareTo(second) <= 0;
		}

		public static bool IsAtMost<T>(this T first, T second, IComparer<T> comparer)
		{
			return comparer.Compare(first, second) <= 0;
		}

		public static bool IsAtLeast<T>(this T first, T second) where T : IComparable<T>
		{
			return first.CompareTo(second) >= 0;
		}

		public static bool IsAtLeast<T>(this T first, T second, IComparer<T> comparer)
		{
			return comparer.Compare(first, second) >= 0;
		}

		public static bool IsLessThan<T>(this T first, T second) where T : IComparable<T>
		{
			return first.CompareTo(second) < 0;
		}

		public static bool IsLessThan<T>(this T first, T second, IComparer<T> comparer)
		{
			return comparer.Compare(first, second) < 0;
		}

		public static bool IsMoreThan<T>(this T first, T second) where T : IComparable<T>
		{
			return first.CompareTo(second) > 0;
		}

		public static bool IsMoreThan<T>(this T first, T second, IComparer<T> comparer)
		{
			return comparer.Compare(first, second) > 0;
		}

		#endregion

		#region IComparable

		public static bool IsEqualTo(this IComparable first, object second)
		{
			return first.CompareTo(second) == 0;
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

		// TODO: reverse comparer
		/*public static IComparer<T> Reverse<T>(this IComparer<T> comparer)
		{
			Guard.AgainstNullArgument("comparer", comparer);
			return new ReversedComparer<T>(comparer);
		}*/
	}
}
