namespace Vertica.Utilities.Eventing
{
	public class ValueIndexEventArgs<T> : ValueEventArgs<T>
	{
		public ValueIndexEventArgs(T value, int index) : base(value)
		{
			_index = index;
		}

		private readonly int _index;
		public int Index { get { return _index; } }
	}
}