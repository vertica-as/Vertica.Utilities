namespace Vertica.Utilities_v4.Eventing
{
	public class ValueIndexChangedEventArgs<T> : ValueChangedEventArgs<T>, IIndexEventArgs
	{
		public ValueIndexChangedEventArgs(int index, T oldValue, T newValue) : base(newValue, oldValue)
		{
			Index = index;
		}

		public int Index { get; private set; }
	}
}