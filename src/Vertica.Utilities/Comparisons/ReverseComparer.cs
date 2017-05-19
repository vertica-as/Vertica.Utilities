using System.Collections.Generic;

namespace Vertica.Utilities.Comparisons
{
	public class ReversedComparer<T> : ChainableComparer<T>
	{
		private readonly IComparer<T> _inner;

		public ReversedComparer(IComparer<T> inner, Direction direction = Direction.Ascending) : base(direction)
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