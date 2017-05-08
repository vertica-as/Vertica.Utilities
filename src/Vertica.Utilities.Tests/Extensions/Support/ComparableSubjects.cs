using System;

namespace Vertica.Utilities_v4.Tests.Extensions.Support
{
	internal class ComparableSubject : IComparable
	{
		private readonly IComparable _inner;

		public ComparableSubject(IComparable inner)
		{
			_inner = inner;
		}

		public int CompareTo(object obj)
		{
			return _inner.CompareTo(obj);
		}
	}

	internal class GenericComparableSubject<T> : IComparable<GenericComparableSubject<T>> where T : IComparable<T>
	{
		private readonly T _inner;

		public GenericComparableSubject(T inner)
		{
			_inner = inner;
		}

		public int CompareTo(GenericComparableSubject<T> other)
		{
			return _inner.CompareTo(other._inner);
		}
	}
}
