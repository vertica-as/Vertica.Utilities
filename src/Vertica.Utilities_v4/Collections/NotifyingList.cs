using System;
using System.Collections;
using System.Collections.Generic;
using Vertica.Utilities_v4.Eventing;

namespace Vertica.Utilities_v4.Collections
{
	public class NotifyingList<T> : IList<T>
	{
		 private readonly List<T> _list = new List<T>();

		#region Events

		public event EventHandler<ValueIndexCancelEventArgs<T>, NotifyingList<T>> Inserting;
		public event EventHandler<ValueIndexEventArgs<T>, NotifyingList<T>> Inserted;
		public event EventHandler<ValueIndexChangingEventArgs<T>, NotifyingList<T>> Setting;
		public event EventHandler<ValueIndexChangedEventArgs<T>, NotifyingList<T>> Set;
		public event EventHandler<ValueIndexCancelEventArgs<T>, NotifyingList<T>> Removing;
		public event EventHandler<ValueIndexEventArgs<T>, NotifyingList<T>> Removed;
		public event EventHandler<CancelEventArgs, NotifyingList<T>> Clearing;
		public event EventHandler<EventArgs, NotifyingList<T>> Cleared;

		#endregion

		#region IList<T> Members

		public int IndexOf(T item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			if (OnInserting(item, index))
			{
				_list.Insert(index, item);
				OnInserted(item, index);
			}
		}

		public void RemoveAt(int index)
		{
			T item = _list[index];

			if (OnRemoving(item, index))
			{
				_list.RemoveAt(index);
				OnRemoved(item, index);
			}
		}

		public T this[int index]
		{
			get { return _list[index]; }
			set
			{
				T oldValue = _list[index];

				if (OnSetting(_list[index], index, value))
				{
					_list[index] = value;
					OnSet(value, index, oldValue);
				}
			}
		}

		#endregion

		#region ICollection<T> Members

		public void Add(T item)
		{
			Insert(Count, item);
		}

		public void Clear()
		{
			if (OnClearing())
			{
				_list.Clear();
				OnCleared();
			}
		}

		public bool Contains(T item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(T item)
		{
			bool result = false;
			int index = _list.IndexOf(item);
			if (index != -1)
			{
				if (OnRemoving(item, index))
				{
					_list.RemoveAt(index);
					result = true;
					OnRemoved(item, index);
				}
			}
			return result;
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion

		protected bool OnInserting(T item, int index)
		{
			if (Inserting == null) return true;

			var evArgs = new ValueIndexCancelEventArgs<T>(index, item);
			Inserting(this, evArgs);
			return !evArgs.IsCancelled;
		}

		protected void OnInserted(T item, int index)
		{
			if (Inserted != null)
			{
				var eventArgs = new ValueIndexEventArgs<T>(index, item);
				Inserted(this, eventArgs);
			}
		}

		protected bool OnSetting(T item, int index, T newValue)
		{
			if (Setting == null) return true;

			var eventArgs = new ValueIndexChangingEventArgs<T>(index, item, newValue);
			Setting(this, eventArgs);
			return !eventArgs.IsCancelled;
		}

		protected void OnSet(T item, int index, T oldValue)
		{
			if (Set != null)
			{
				var eventArgs = new ValueIndexChangedEventArgs<T>(index, oldValue, item);
				Set(this, eventArgs);
			}
		}

		protected bool OnRemoving(T item, int index)
		{
			if (Removing == null) return true;

			var eventArgs = new ValueIndexCancelEventArgs<T>(index, item);
			Removing(this, eventArgs);
			return !eventArgs.IsCancelled;
		}

		protected void OnRemoved(T item, int index)
		{
			if (Removed != null)
			{
				var eventArgs = new ValueIndexEventArgs<T>(index, item);
				Removed(this, eventArgs);
			}
		}

		protected bool OnClearing()
		{
			if (Clearing == null) return true;
			
			var evArgs = new CancelEventArgs();
			Clearing(this, evArgs);
			return !evArgs.IsCancelled;
		}

		protected void OnCleared()
		{
			if (Cleared != null) Cleared(this, EventArgs.Empty);
		}
	}
}