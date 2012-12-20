﻿using System.Collections.Generic;

namespace Vertica.Utilities_v4.Comparisons
{
	public abstract class ChainableComparer<T> : IComparer<T>
	{
		private readonly Direction _direction;
		public Direction SortDirection { get { return _direction; } }
		protected abstract int DoCompare(T x, T y);

		protected ChainableComparer(Direction sortDirection = Direction.Ascending)
		{
			_direction = sortDirection;
		}

		public int Compare(T x, T y)
		{
			int? shortCircuit = handleNulls(x, y);
			if (shortCircuit.HasValue) return shortCircuit.Value;

			int result = DoCompare(x, y);
			if (needsToEvaluateNext(result))
			{
				result = _nextComparer.Compare(x, y);
			}

			if (_direction == Direction.Descending) invert(ref result);

			return result;
		}

		private static int? handleNulls(T x, T y)
		{
			int? shortCircuit = null;
			if (!typeof(T).IsValueType)
			{
				// ReSharper disable CompareNonConstrainedGenericWithNull
				if (x == null)
				{
					shortCircuit = y == null ? 0 : -1;
				}
				else if (y == null)
				{
					shortCircuit = 1;
				}
				// ReSharper restore CompareNonConstrainedGenericWithNull
			}
			return shortCircuit;
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
	}
}