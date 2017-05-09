using System.Collections.Generic;
using System.Reflection;

namespace Vertica.Utilities.Comparisons
{
	public abstract class ChainableEqualizer<T> : IEqualityComparer<T>
	{
		protected abstract bool DoEquals(T x, T y);
		protected abstract int DoGetHashCode(T obj);

		public bool Equals(T x, T y)
		{
			bool? shortCircuit = handleNulls(x, y);
			if (shortCircuit.HasValue) return shortCircuit.Value;

			bool result = DoEquals(x, y);
			if (needsToEvaluateNext(result)) result = _nextEqualizer.Equals(x, y);

			return result;
		}

		private static bool? handleNulls(T x, T y)
		{
			bool? shortCircuit = null;
			if (!typeof(T).GetTypeInfo().IsValueType)
			{
				// ReSharper disable CompareNonConstrainedGenericWithNull
				if (x == null)
				{
					shortCircuit = (y == null);
				}
				else if (y == null)
				{
					shortCircuit = false;
				}
				// ReSharper restore CompareNonConstrainedGenericWithNull
			}
			return shortCircuit;
		}

		public int GetHashCode(T x)
		{
			int result = DoGetHashCode(x);
			return result;
		}

		private bool needsToEvaluateNext(bool ret)
		{
			return ret && _nextEqualizer != null;
		}

		private ChainableEqualizer<T> _nextEqualizer;
		private void chain(ChainableEqualizer<T> lastEqualizer)
		{
			if (_nextEqualizer == null)
			{
				_nextEqualizer = lastEqualizer;
			}
			else
			{
				_nextEqualizer.chain(lastEqualizer);
			}
		}

		private ChainableEqualizer<T> _lastEqualizer;
		public ChainableEqualizer<T> Then(ChainableEqualizer<T> equalizer)
		{
			if (_nextEqualizer == null)
			{
				_nextEqualizer = equalizer;
			}
			else
			{
				_lastEqualizer.chain(equalizer);
			}
			_lastEqualizer = equalizer;
			return this;
		}
	}
}
