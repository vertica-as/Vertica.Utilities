using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;

namespace Vertica.Utilities_v4.DirectoryServices
{
	[Obsolete(".NET Standard")]
	public class DirectoryEntryCollection : IDisposable, IEnumerable<DirectoryEntry>
	{
		private readonly List<DirectoryEntry> _inner;
		public DirectoryEntryCollection(int capacity)
		{
			_inner = new List<DirectoryEntry>(capacity);
		}

		public void Add(DirectoryEntry entry)
		{
			_inner.Add(entry);
		}

		public void Close()
		{
			_inner.ForEach(e => e.Close());
		}

		public void Dispose()
		{
			_inner.ForEach(e => e.Dispose());
		}

		public IEnumerator<DirectoryEntry> GetEnumerator()
		{
			return _inner.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count { get { return _inner.Count; } }
	}
}