using System.Collections.Generic;

namespace Vertica.Utilities_v4.Comparisons
{
	public class ReversedComparer<T> : ChainableComparer<T>
	{
		private readonly IComparer<T> _inner;

		public ReversedComparer(IComparer<T> inner) : this(inner, Direction.Ascending) { }

		public ReversedComparer(IComparer<T> inner, Direction direction) : base(direction)
		{
			Guard.AgainstNullArgument("inner", inner);
			_inner = inner;
		}

		protected override int DoCompare(T x, T y)
		{
			return _inner.Compare(y, x);
		}
	}
}