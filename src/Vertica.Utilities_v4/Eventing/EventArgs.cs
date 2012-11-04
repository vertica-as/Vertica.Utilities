namespace Vertica.Utilities_v4.Eventing
{
	public interface IValueEventArgs<out T> { T Value { get; } }

	public interface ICancelEventArgs
	{
		bool IsCancelled { get; }
		void Cancel();
	}

	public interface INewValueEventArgs<out T> { T NewValue { get; } }

	public interface IOldValueEventArgs<out T> { T OldValue { get; } }

	public interface IIndexEventArgs { int Index { get; } }

	public interface IMutableValueEventArgs<T> { T Value { get; set; } }

	/// <summary>
	/// Creation methods that leverage type inference
	/// </summary>
	public static class Args
	{
		public static MultiEventArgs<T, U> Value<T, U>(T value1, U value2)
		{
			return new MultiEventArgs<T, U>(value1, value2);
		}

		public static ValueEventArgs<T> Value<T>(T value)
		{
			return new ValueEventArgs<T>(value);
		}

		public static MutableValueEventArgs<T> Mutable<T>(T value)
		{
			return new MutableValueEventArgs<T> { Value = value };
		}

		public static PropertyValueChangedEventArgs<T> Changed<T>(string propertyName, T oldValue, T newValue)
		{
			return new PropertyValueChangedEventArgs<T>(propertyName, oldValue, newValue);
		}

		public static ValueIndexChangedEventArgs<T> Changed<T>(int index, T oldValue, T newValue)
		{
			return new ValueIndexChangedEventArgs<T>(index, oldValue, newValue);
		}

		public static PropertyValueChangingEventArgs<T> Changing<T>(string propertyName, T oldValue, T newValue)
		{
			return new PropertyValueChangingEventArgs<T>(propertyName, oldValue, newValue);
		}

		public static ValueIndexChangingEventArgs<T> Changing<T>(int index, T oldValue, T newValue)
		{
			return new ValueIndexChangingEventArgs<T>(index, newValue, oldValue);
		}

		public static ValueCancelEventArgs<T> Cancel<T>(T value)
		{
			return new ValueCancelEventArgs<T>(value);
		}

		public static ValueIndexCancelEventArgs<T> Cancel<T>(int index, T value)
		{
			return new ValueIndexCancelEventArgs<T>(index, value);
		}

		public static ValueIndexEventArgs<T> Index<T>(int index, T value)
		{
			return new ValueIndexEventArgs<T>(index, value);
		}
	}
}
