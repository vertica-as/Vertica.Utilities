namespace Vertica.Utilities_v4.Collections
{
	/// <summary>
	/// Represents each entry returned within a collection,
	/// containing the value and whether it is the first and/or
	/// the last entry in the collection's. enumeration
	/// </summary>
	public struct SmartEntry<T>
	{
		internal SmartEntry(bool isFirst, bool isLast, T value, int index)
		{
			_isFirst = isFirst;
			_isLast = isLast;
			_value = value;
			_index = index;
		}

		private readonly T _value;
		public T Value { get { return _value; } }

		private readonly bool _isFirst;
		public bool IsFirst { get { return _isFirst; } }

		private readonly bool _isLast;
		public bool IsLast { get { return _isLast; } }

		private readonly int _index;
		/// <summary>
		/// The 0-based index of this entry (i.e. how many entries have been returned before this one)
		/// </summary>
		public int Index { get { return _index; } }
	}
}