namespace Vertica.Utilities_v4.Eventing
{
	public class ValueIndexEventArgs<T> : ValueEventArgs<T>, IIndexEventArgs
	{
		public ValueIndexEventArgs(int index, T value) : base(value)
		{
			_index = index;
		}

		private readonly int _index;
		public int Index { get { return _index; } }
	}
}