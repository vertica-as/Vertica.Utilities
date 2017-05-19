using System;

namespace Vertica.Utilities.Comparisons
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

	public static partial class ChainableExtensions
	{
		public static ChainableComparer<T> Then<T>(this ChainableComparer<T> chainable, Comparison<T> next)
		{
			return chainable.Then(new ComparisonComparer<T>(next));
		}

		public static ChainableComparer<T> Then<T>(this ChainableComparer<T> chainable, Comparison<T> next, Direction sortDirection)
		{
			return chainable.Then(new ComparisonComparer<T>(next, sortDirection));
		}
	}
}