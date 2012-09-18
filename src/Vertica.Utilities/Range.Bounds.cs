using System;

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
		bool MoreThan(T other);

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
			return _value.CompareTo(other) <= 0;
		}

		public bool MoreThan(T other)
		{
			return _value.CompareTo(other) >= 0;
		}

		public string ToAssertion()
		{
			return Value + " (inclusive)";
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
			return _value.CompareTo(other) < 0;
		}

		public bool MoreThan(T other)
		{
			return _value.CompareTo(other) > 0;
		}

		public string ToAssertion()
		{
			return Value + " (not inclusive)";
		}
	}
}
