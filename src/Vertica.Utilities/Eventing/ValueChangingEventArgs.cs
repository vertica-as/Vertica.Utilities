namespace Vertica.Utilities_v4.Eventing
{
	public class ValueChangingEventArgs<T> : ValueCancelEventArgs<T>, INewValueEventArgs<T>
	{
		public ValueChangingEventArgs(T value, T newValue) : base(value)
		{
			NewValue = newValue;
		}

		public T NewValue { get; private set; }
	}
}