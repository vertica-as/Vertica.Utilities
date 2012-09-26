using System;
using Vertica.Utilities.Extensions.ComparableExt;

namespace Vertica.Utilities.Extensions.RangeExt
{
	public static class RangeExtensions
	{
		/// <summary>
		/// Creates a closed range or an <see cref="Range{T}.Empty"/> if bounds not congruent.
		/// </summary>
		public static Range<T> To<T>(this T lowerBound, T upperBound) where T : IComparable<T>
		{
			Range<T> result = Range<T>.Empty;
			if (lowerBound.IsLessThan(upperBound))
			{
				result = new Range<T>(lowerBound, upperBound);
			}
			return result;
		}

		/// <summary>
		/// Checks containment
		/// </summary>
		public static bool Within<T>(this T value, Range<T> range) where T : IComparable<T>
		{
			return range.Contains(value);
		}

		public static T LimitLower<T>(this T value, Range<T> range) where T : IComparable<T>
		{
			return range.LimitLower(value);
		}

		public static T LimitUpper<T>(this T value, Range<T> range) where T : IComparable<T>
		{
			return range.LimitUpper(value);
		}

		public static T Limit<T>(this T item, Range<T> range) where T : IComparable<T>
		{
			return range.Limit(item);
		}
	}
}
