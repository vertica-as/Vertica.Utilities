using System;
using System.Collections.Generic;

namespace Vertica.Utilities
{
	public sealed class Option<T> : IEquatable<Option<T>>
	{
		public static Option<T> None { get { return _none; } }

		public static Option<T> NoneWithDefault(T defaultValue)
		{
			return new Option<T> { @default = defaultValue };
		}

		public static Option<T> Some(T value)
		{
			return new Option<T>(value);
		}

		// either the "classic" default
		public bool IsNone
		{
			get { return this == _none || _defaultChanged; }
		}
		public bool IsSome
		{
			get { return !IsNone; }
		}

		public T Value
		{
			get
			{
				if (IsSome) return _value;

				throw new InvalidOperationException();
			}
		}

		public T ValueOrDefault { get { return IsSome ? _value : @default; } }

		public T GetValueOrDefault(T defaultValue)
		{
			return IsSome ? Value : defaultValue;
		}

		private readonly T _value;
		private bool _defaultChanged;
		private T _default;
		private T @default
		{
			get { return _default; }
			set
			{
				_default = value;
				_defaultChanged = true;
			}
		}
		private static readonly Option<T> _none = new Option<T>();

		private Option() { }
		private Option(T value)
		{
			_value = value;
		}

		public override bool Equals(object obj)
		{
			if (obj is Option<T>) return Equals((Option<T>)obj);

			return false;
		}
		public bool Equals(Option<T> other)
		{
			if (other == null) return false;

			if (IsNone)
			{
				return other.IsNone && EqualityComparer<T>.Default.Equals(_default, other._default);
			}
			return other.IsSome && EqualityComparer<T>.Default.Equals(_value, other._value);
		}
		public override int GetHashCode()
		{
			if (IsNone) return 0;

			return EqualityComparer<T>.Default.GetHashCode(_value);
		}
	}
}
