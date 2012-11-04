namespace Vertica.Utilities_v4.Eventing
{
	public class ValueIndexChangingEventArgs<T> : ValueChangingEventArgs<T>, IIndexEventArgs
	{
		public ValueIndexChangingEventArgs(int index, T oldValue, T newValue) : base(oldValue, newValue)
		{
			_index = index;
		}

		private readonly int _index;
		public int Index { get { return _index; } }
	}
}