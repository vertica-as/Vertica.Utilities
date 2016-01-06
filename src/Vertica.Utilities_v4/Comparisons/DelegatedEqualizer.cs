using System;
using System.Collections.Generic;

namespace Vertica.Utilities_v4.Comparisons
{
	public class DelegatedEqualizer<T> : ChainableEqualizer<T>
	{
		private readonly Func<T, T, bool> _equals;
		private readonly Func<T, int> _hasher;

		public DelegatedEqualizer(Func<T, T, bool> equals, Func<T, int> hasher)
		{
			_equals = equals;
			_hasher = hasher;
		}

		public DelegatedEqualizer(Comparison<T> comparison, Func<T, int> hasher)
			: this(new ComparisonComparer<T>(comparison), hasher) { }

		public DelegatedEqualizer(IComparer<T> comparer, Func<T, int> hasher)
			: this((x, y) => comparer.Compare(x, y) == 0, hasher) { }

		protected override bool DoEquals(T x, T y)
		{
			return _equals(x, y);
		}

		protected override int DoGetHashCode(T obj)
		{
			return _hasher(obj);
		}
	}

	public static partial class ChainableExtensions
	{
		public static ChainableEqualizer<T> Then<T>(this ChainableEqualizer<T> chainable, Func<T, T, bool> equals, Func<T, int> hasher)
		{
			return chainable.Then(new DelegatedEqualizer<T>(equals, hasher));
		}
	}
}
