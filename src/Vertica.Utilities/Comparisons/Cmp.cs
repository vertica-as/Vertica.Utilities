using System;

namespace Vertica.Utilities_v4.Comparisons
{
	public static class Cmp<T>
	{
		public static ChainableComparer<T> By(Comparison<T> next, Direction sortDirection = Direction.Ascending)
		{
			return new ComparisonComparer<T>(next, sortDirection);
		}

		public static ChainableComparer<T> By<K>(Func<T, K> keySelector, Direction sortDirection = Direction.Ascending)
		{
			return new SelectorComparer<T, K>(keySelector, sortDirection);
		}

		public static ChainableComparer<T> FromOperators(Direction sortDirection = Direction.Ascending)
		{
			return new OperatorComparer<T>(sortDirection);
		}
	}
}
