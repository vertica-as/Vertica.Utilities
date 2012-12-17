using System;
using System.Collections.Generic;

namespace Vertica.Utilities_v4.Comparisons
{
	public class SelectorComparer<T, K> : ChainableComparer<T>
	{
		private readonly Func<T, K> _keySelector;

		public SelectorComparer(Func<T, K> keySelector) : this(keySelector, Direction.Ascending) { }

		public SelectorComparer(Func<T, K> keySelector, Direction sortDirection)
			: base(sortDirection)
		{
			_keySelector = keySelector;
		}

		protected override int DoCompare(T x, T y)
		{
			Comparer<K> comparer = Comparer<K>.Default;
			return comparer.Compare(_keySelector.Invoke(x), _keySelector.Invoke(y));
		}

		public Comparison<T> Comparison { get { return Compare; } }
	}


	public static partial class ChainableExtensions
	{
		public static ChainableComparer<T> Then<T, U>(this ChainableComparer<T> chainable, Func<T, U> keySelector)
		{
			return chainable.Then(new SelectorComparer<T, U>(keySelector));
		}

		public static ChainableComparer<T> Then<T, U>(this ChainableComparer<T> chainable, Func<T, U> keySelector, Direction sortDirection)
		{
			return chainable.Then(new SelectorComparer<T, U>(keySelector, sortDirection));
		}
	}
}
