namespace Vertica.Utilities.Collections
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
			IsFirst = isFirst;
			IsLast = isLast;
			Value = value;
			Index = index;
		}

		public T Value { get; }

		public bool IsFirst { get; }

		public bool IsLast { get; }

		/// <summary>
		/// The 0-based index of this entry (i.e. how many entries have been returned before this one)
		/// </summary>
		public int Index { get; }
	}
}