using System;
using System.Collections.Generic;

namespace Vertica.Utilities.Comparisons
{
	public class SelectorEqualizer<T, K> : ChainableEqualizer<T>
	{
		private readonly Func<T, K> _selector;
		public SelectorEqualizer(Func<T, K> selector)
		{
			_selector = selector;
		}

		private K keyFor(T obj)
		{
			return _selector(obj);
		}

		protected override bool DoEquals(T x, T y)
		{
			K xKey = keyFor(x), yKey = keyFor(y);

			return EqualityComparer<K>.Default.Equals(xKey, yKey);
		}

		protected override int DoGetHashCode(T obj)
		{
			K key = keyFor(obj);
			return EqualityComparer<K>.Default.GetHashCode(key);
		}
	}

	public static partial class ChainableExtensions
	{
		public static ChainableEqualizer<T> Then<T, U>(this ChainableEqualizer<T> chainable, Func<T, U> keySelector)
		{
			return chainable.Then(new SelectorEqualizer<T, U>(keySelector));
		}
	}
}
