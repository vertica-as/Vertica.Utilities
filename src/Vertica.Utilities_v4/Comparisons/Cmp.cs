using System;

namespace Vertica.Utilities_v4.Comparisons
{
	public static class Cmp<T>
	{
		public static ChainableComparer<T> By(Comparison<T> next)
		{
			return new ComparisonComparer<T>(next);
		}

		public static ChainableComparer<T> By(Comparison<T> next, Direction sortDirection)
		{
			return new ComparisonComparer<T>(next, sortDirection);
		}

		public static ChainableComparer<T> By<K>(Func<T, K> keySelector)
		{
			return new SelectorComparer<T, K>(keySelector);
		}

		public static ChainableComparer<T> By<K>(Func<T, K> keySelector, Direction sortDirection)
		{
			return new SelectorComparer<T, K>(keySelector, sortDirection);
		}
	}
}
