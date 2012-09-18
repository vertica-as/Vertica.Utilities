using System;
using Vertica.Utilities.Extensions.ComparableExt;

namespace Vertica.Utilities.Extensions.RangeExt
{
	public static class RangeExtensions
	{
		public static Range<T> To<T>(this T item, T upperBound) where T : IComparable<T>
		{
			Range<T> result = Range<T>.Empty;
			if (item.IsLessThan(upperBound))
			{
				result = new Range<T>(item, upperBound);
			}
			return result;
		}

		public static bool Between<T>(this T item, Range<T> range) where T : IComparable<T>
		{
			return range.Contains(item);
		}

		public static bool Between<T>(this T item, T lowerBound, T upperBound) where T : IComparable<T>
		{
			return item.Between(new Range<T>(lowerBound, upperBound));
		}

		public static T LimitLower<T>(this T item, T lowerBound) where T : IComparable<T>
		{
			return Range<T>.LimitLower(item, lowerBound);
		}

		public static T LimitLower<T>(this T item, Range<T> range) where T : IComparable<T>
		{
			return Range<T>.LimitLower(item, range.LowerBound);
		}

		public static T LimitUpper<T>(this T item, T upperBound) where T : IComparable<T>
		{
			return Range<T>.LimitUpper(item, upperBound);
		}

		public static T LimitUpper<T>(this T item, Range<T> range) where T : IComparable<T>
		{
			return Range<T>.LimitUpper(item, range.UpperBound);
		}

		public static T Limit<T>(this T item, T loweBound, T upperBound) where T : IComparable<T>
		{
			return Range<T>.Limit(item, loweBound, upperBound);
		}

		public static T Limit<T>(this T item, Range<T> range) where T : IComparable<T>
		{
			return Range<T>.Limit(item, range.LowerBound, range.UpperBound);
		}
	}
}
