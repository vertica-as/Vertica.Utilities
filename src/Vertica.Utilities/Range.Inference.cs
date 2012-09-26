using System;

namespace Vertica.Utilities
{
	/// <summary>
	/// Leverages type inference for <see cref="Range{T}"/>.
	/// </summary>
	public static class Range
	{
		#region creation

		/// <summary>
		/// Creates a closed range: both bounds contained in the range.
		/// </summary>
		public static Range<T> New<T>(T lowerBound, T upperBound) where T : IComparable<T>
		{
			return Closed(lowerBound, upperBound);
		}

		/// <summary>
		/// Creates a closed range: both bounds contained in the range.
		/// </summary>
		public static Range<T> Closed<T>(T lowerBound, T upperbound) where T : IComparable<T>
		{
			return new Range<T>(lowerBound.Close(), upperbound.Close());
		}

		/// <summary>
		/// Creates an open range: neither bounds contained in the range.
		/// </summary>
		public static Range<T> Open<T>(T lowerBound, T upperbound) where T : IComparable<T>
		{
			return new Range<T>(lowerBound.Open(), upperbound.Open());
		}

		/// <summary>
		/// Creates a half-open range: only lower bound is contained in the range.
		/// </summary>
		/// <remarks>Closed at the beginning, open at the end.</remarks>
		public static Range<T> HalfOpen<T>(T lowerBound, T upperbound) where T : IComparable<T>
		{
			return new Range<T>(lowerBound.Close(), upperbound.Open());
		}

		/// <summary>
		/// Creates a half-closed range: only the upper bound is contained in the range.
		/// </summary>
		/// <remarks>Open at the begining, closed at the end.</remarks>
		public static Range<T> HalfClosed<T>(T lowerBound, T upperbound) where T : IComparable<T>
		{
			return new Range<T>(lowerBound.Open(), upperbound.Close());
		}

		public static Range<T> Empty<T>() where T : IComparable<T>
		{
			return Range<T>.Empty;
		}

		#endregion


		#region bound checking

		public static bool CheckBounds<T>(T lowerBound, T upperBound) where T : IComparable<T>
		{
			return Range<T>.CheckBounds(lowerBound, upperBound);
		}

		public static void AssertBounds<T>(T lowerBound, T upperBound) where T : IComparable<T>
		{
			Range<T>.AssertBounds(lowerBound, upperBound);
		}

		#endregion

	}
}
