using System.Collections;
using System.Collections.Generic;

namespace Vertica.Utilities.Collections
{
	public interface IReadOnlyList<T> : IEnumerable<T>
	{
		int IndexOf(T item);
		bool Contains(T item);
		void CopyTo(T[] array, int arrayIndex);

		int Count { get; }
		T this[int index] { get; }
	}

	public class ReadOnlyListAdapter<T> : IReadOnlyList<T>
	{
		private readonly IList<T> _adptee;

		public ReadOnlyListAdapter(IList<T> adaptee)
		{
			_adptee = adaptee;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _adptee.GetEnumerator();
		}

		public bool Contains(T item)
		{
			return _adptee.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_adptee.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _adptee.Count; }
		}

		public int IndexOf(T item)
		{
			return _adptee.IndexOf(item);
		}

		public T this[int index]
		{
			get { return _adptee[index]; }
			set { _adptee[index] = value; }
		}
	}
}
