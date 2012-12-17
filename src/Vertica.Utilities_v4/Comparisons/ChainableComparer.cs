using System.Collections.Generic;

namespace Vertica.Utilities_v4.Comparisons
{
	public abstract class ChainableComparer<T> : IComparer<T>
	{
		private readonly Direction _direction;
		public Direction SortDirection { get { return _direction; } }
		protected abstract int DoCompare(T x, T y);

		#region Factory Methods

		/*public static ChainableComparer<T> By(Comparison<T> next)
		{
			return new ComparisonComparer<T>(next);
		}

		public static ChainableComparer<T> By(Comparison<T> next, Direction sortDirection)
		{
			return new ComparisonComparer<T>(next, sortDirection);
		}

		public static ChainableComparer<T> By<K>(Func<T, K> keySelector)
		{
			return new DynamicComparer<T, K>(keySelector);
		}

		public static ChainableComparer<T> By<K>(Func<T, K> keySelector, Direction sortDirection)
		{
			return new DynamicComparer<T, K>(keySelector, sortDirection);
		}*/

		#endregion

		protected ChainableComparer() : this(Direction.Ascending) { }

		protected ChainableComparer(Direction sortDirection)
		{
			_direction = sortDirection;
		}

		public int Compare(T x, T y)
		{
			int result = DoCompare(x, y);
			if (needsToEvaluateNext(result)) result = _nextComparer.Compare(x, y);

			if (_direction == Direction.Descending) invert(ref result);

			return result;
		}

		private bool needsToEvaluateNext(int ret)
		{
			return ret == 0 && _nextComparer != null;
		}

		private static void invert(ref int result)
		{
			result *= -1;
		}

		private ChainableComparer<T> _nextComparer;
		public void chain(ChainableComparer<T> lastComparer)
		{
			if (_nextComparer == null)
			{
				_nextComparer = lastComparer;
			}
			else
			{
				_nextComparer.chain(lastComparer);
			}
		}

		private ChainableComparer<T> _lastComparer;
		public ChainableComparer<T> Then(ChainableComparer<T> comparer)
		{

			if (_nextComparer == null)
			{
				_nextComparer = comparer;
			}
			else
			{
				_lastComparer.chain(comparer);
			}
			_lastComparer = comparer;
			return this;
		}

		/*public ChainableComparer<T> Then(Comparison<T> next)
		{
			return Then(new ComparisonComparer<T>(next));
		}

		public ChainableComparer<T> Then(Comparison<T> next, Direction sortDirection)
		{
			return Then(new ComparisonComparer<T>(next, sortDirection));
		}

		public ChainableComparer<T> Then<U>(Func<T, U> keySelector)
		{
			return Then(new DynamicComparer<T, U>(keySelector));
		}

		public ChainableComparer<T> Then<U>(Func<T, U> keySelector, Direction sortDirection)
		{
			return Then(new DynamicComparer<T, U>(keySelector, sortDirection));
		}*/
	}
}