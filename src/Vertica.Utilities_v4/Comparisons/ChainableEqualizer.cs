using System.Collections.Generic;

namespace Vertica.Utilities_v4.Comparisons
{
	public abstract class ChainableEqualizer<T> : IEqualityComparer<T>
	{
		public abstract bool DoEquals(T x, T y);
		public abstract int DoGetHashCode(T obj);

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
			if (!typeof(T).IsValueType)
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

		/*public ChainableEqualizer<T> Then(Func<T, T, bool> equals)
		{
			return Then(new DelegatedEqualizer<T>(equals));
		}

		public ChainableEqualizer<T> Then(Comparison<T> equals)
		{
			return Then(new DelegatedEqualizer<T>(equals));
		}

		public ChainableEqualizer<T> Then(IComparer<T> equals)
		{
			return Then(new DelegatedEqualizer<T>(equals));
		}

		public ChainableEqualizer<T> Then<TResult>(Func<T, TResult> selector)
		{
			return Then(new DelegatedEqualizer<T, TResult>(selector));
		}*/

		public static ChainableEqualizer<T> New()
		{
			return new NewEqualizer();
		}

		private class NewEqualizer : ChainableEqualizer<T>
		{
			public override bool DoEquals(T x, T y)
			{
				return true;
			}

			public override int DoGetHashCode(T obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
