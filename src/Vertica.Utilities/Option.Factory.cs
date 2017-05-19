using System;
using System.Collections.Generic;
using System.Linq;
using Vertica.Utilities.Extensions.StringExt;

namespace Vertica.Utilities
{
	public static class Option
	{
		public static Option<T> Some<T>(T value)
		{
			return Option<T>.Some(value);
		}

		public static Option<T> None<T>(T defaultValue)
		{
			return Option<T>.NoneWithDefault(defaultValue);
		}

		public static Option<T> Maybe<T>(T value)  where T : class
		{
			return value == null ? Option<T>.None : Some(value);
		}

		public static Option<T> Maybe<T>(T value, Func<T> defaultValue) where T : class
		{
			return value == null ? None(defaultValue()) : Some(value);
		}

		public static Option<T> Maybe<T>(T? value) where T: struct
		{
			return value.HasValue ? Some(value.Value) : None(value.GetValueOrDefault());
		}

		public static Option<string> Maybe(string value)
		{
			return value.IsEmpty() ? None(string.Empty) : Some(value);
		}

		public static Option<IEnumerable<T>> Maybe<T>(IEnumerable<T> value)
		{
			return (value == null || !value.Any()) ? None(Enumerable.Empty<T>()) : Some(value);
		}

		public static Option<IEnumerable<T>> Maybe<T>(T[] value)
		{
			return value == null || !value.Any() ? None(Enumerable.Empty<T>()) : Some(value.AsEnumerable());
		}

		public static Option<IEnumerable<T>> Maybe<T>(IList<T> value)
		{
			return value == null || !value.Any() ? None(Enumerable.Empty<T>()) : Some(value.AsEnumerable());
		}

		public static Option<IEnumerable<T>> Maybe<T>(ICollection<T> value)
		{
			return value == null || !value.Any() ? None(Enumerable.Empty<T>()) : Some(value.AsEnumerable());
		}
	}
}
