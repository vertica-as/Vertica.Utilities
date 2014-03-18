using System;
using System.Collections.Generic;
using Vertica.Utilities_v4.Extensions.ComparableExt;

namespace Vertica.Utilities_v4
{
	public static class Bound
	{
		public static IBound<T> Open<T>(T value) where T : IComparable<T>
		{
			return new Open<T>(value);
		}

		public static IBound<T> Closed<T>(T value) where T : IComparable<T>
		{
			return new Closed<T>(value);
		}
	}

	public interface IBound<T> : IEquatable<IBound<T>> where T : IComparable<T>
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

		T Generate(Func<T, T> nextGenerator);

		#region union implementation

		bool IsClosed { get; }
		IBound<T> New(T value);

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

		public bool MoreThan(T other)
		{
			return _value.IsAtLeast(other);
		}

		public T Generate(Func<T, T> nextGenerator)
		{
			return _value;
		}

		public string ToAssertion()
		{
			return _value + " (inclusive)";
		}
		public bool IsClosed { get { return true; } }

		public IBound<T> New(T value)
		{
			return new Closed<T>(value);
		}

		#region value equality (to increase performance)

		public bool Equals(IBound<T> other)
		{
			return other is Closed<T> && EqualityComparer<T>.Default.Equals(Value, other.Value);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Closed<T> && Equals((Closed<T>) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (EqualityComparer<T>.Default.GetHashCode(_value) * 397) ^ 1;
			}
		}

		#endregion
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

		public bool MoreThan(T other)
		{
			return _value.IsMoreThan(other);
		}

		public T Generate(Func<T, T> nextGenerator)
		{
			return nextGenerator(_value);
		}

		public string ToAssertion()
		{
			return _value + " (not inclusive)";
		}

		public bool IsClosed { get { return false; } }

		public IBound<T> New(T value)
		{
			return new Open<T>(value);
		}

		#region value equality (to increase performance)

		public bool Equals(IBound<T> other)
		{
			return other is Open<T> && EqualityComparer<T>.Default.Equals(Value, other.Value);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Open<T> && Equals((Open<T>)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (EqualityComparer<T>.Default.GetHashCode(_value) * 397) ^ 2;	
			}
		}

		#endregion
	}
}
