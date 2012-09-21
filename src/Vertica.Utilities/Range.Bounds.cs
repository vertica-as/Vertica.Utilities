using System;
using Vertica.Utilities.Extensions.ComparableExt;

namespace Vertica.Utilities
{
	public static class Bound
	{
		public static IBound<T> Close<T>(this T value) where T : IComparable<T>
		{
			return new Closed<T>(value);
		}

		public static IBound<T> Open<T>(this T value) where T : IComparable<T>
		{
			return new Open<T>(value);
		}
	}

	public interface IBound<T> where T : IComparable<T>
	{
		T Value { get; }

		#region representation

		string Lower();
		string Upper();

		#endregion

		#region value checking

		bool LessThan(T other);
		bool CanContainLower(IBound<T> other);
		bool MoreThan(T other);
		bool CanContainUpper(IBound<T> other);

		#endregion

		#region argument assertion

		string ToAssertion();

		#endregion

	}

	[Serializable]
	internal struct Closed<T> : IBound<T> where T : IComparable<T>
	{
		private readonly T _value;
		public T Value { get { return _value; } }

		public Closed(T value)
		{
			_value = value;
		}

		public string Lower()
		{
			return "[" + _value;
		}

		public string Upper()
		{
			return _value + "]";
		}

		public bool LessThan(T other)
		{
			return _value.IsAtMost(other);
		}

		public bool CanContainLower(IBound<T> other)
		{
			return _value.IsAtMost(other.Value);
		}

		public bool MoreThan(T other)
		{
			return _value.IsAtLeast(other);
		}

		public bool CanContainUpper(IBound<T> other)
		{
			return _value.IsAtLeast(other.Value);
		}

		public string ToAssertion()
		{
			return _value + " (inclusive)";
		}
	}

	[Serializable]
	internal struct Open<T> : IBound<T> where T : IComparable<T>
	{
		private readonly T _value;
		public T Value { get { return _value; } }

		public Open(T value)
		{
			_value = value;
		}

		public string Lower()
		{
			return "(" + _value;
		}

		public string Upper()
		{
			return _value + ")";
		}

		public bool LessThan(T other)
		{
			return _value.IsLessThan(other);
		}

		public bool CanContainLower(IBound<T> other)
		{
			return other is Closed<T> ? _value.IsLessThan(other.Value) : _value.IsAtMost(other.Value);
		}

		public bool MoreThan(T other)
		{
			return _value.IsMoreThan(other);
		}

		public bool CanContainUpper(IBound<T> other)
		{
			return other is Closed<T> ? _value.IsMoreThan(other.Value) : _value.IsAtLeast(other.Value);
		}

		public string ToAssertion()
		{
			return _value + " (not inclusive)";
		}
	}
}
