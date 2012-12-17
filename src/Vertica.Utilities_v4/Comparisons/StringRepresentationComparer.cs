using System;
using System.Collections.Generic;

namespace Vertica.Utilities_v4.Comparisons
{
	public class StringRepresentationComparer<T> : ChainableComparer<string>
	{
		private readonly Func<string, T> _converter;
		private readonly Comparer<T> _comparer;
		public StringRepresentationComparer(Func<string, T> converter) : this(converter, Direction.Ascending) { }

		public StringRepresentationComparer(Func<string, T> converter, Direction direction)
			: base(direction)
		{
			_converter = converter;
			_comparer = Comparer<T>.Default;
		}

		protected override int DoCompare(string x, string y)
		{
			return _comparer.Compare(_converter(x), _converter(y));
		}
	}
}