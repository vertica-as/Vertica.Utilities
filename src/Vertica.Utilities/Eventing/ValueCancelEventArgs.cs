namespace Vertica.Utilities_v4.Eventing
{
	public class ValueCancelEventArgs<T> : CancelEventArgs, IValueEventArgs<T>
	{
		public ValueCancelEventArgs(T value)
		{
			Value = value;
		}

		public T Value { get; private set; }
	}
}