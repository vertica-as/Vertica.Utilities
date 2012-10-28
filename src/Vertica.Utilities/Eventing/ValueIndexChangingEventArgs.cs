namespace Vertica.Utilities.Eventing
{
	public class ValueIndexChangingEventArgs<T> : ValueChangingEventArgs<T>, IIndexEventArgs
	{
		public ValueIndexChangingEventArgs(T value, T newValue, int index) : base(value, newValue)
		{
			_index = index;
		}

		private readonly int _index;
		public int Index { get { return _index; } }
	}
}