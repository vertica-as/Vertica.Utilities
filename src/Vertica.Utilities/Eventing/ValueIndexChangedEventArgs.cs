namespace Vertica.Utilities.Eventing
{
	public class ValueIndexChangedEventArgs<T> : ValueChangedEventArgs<T>, IIndexEventArgs
	{
		public ValueIndexChangedEventArgs(T value, T oldValue, int index) : base(value, oldValue)
		{
			Index = index;
		}

		public int Index { get; private set; }
	}
}