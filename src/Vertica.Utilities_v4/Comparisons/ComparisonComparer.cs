using System;

namespace Vertica.Utilities_v4.Comparisons
{
	public class ComparisonComparer<T> : ChainableComparer<T>
	{
		private readonly Comparison<T> _comparison;

		public ComparisonComparer(Comparison<T> comparison) :
			this(comparison, Direction.Ascending) { }

		public ComparisonComparer(Comparison<T> comparison, Direction sortDirection)
			: base(sortDirection)
		{
			_comparison = comparison;
		}

		protected override int DoCompare(T x, T y)
		{
			return _comparison(x, y);
		}

		public Comparison<T> Comparison { get { return Compare; } }
	}
}