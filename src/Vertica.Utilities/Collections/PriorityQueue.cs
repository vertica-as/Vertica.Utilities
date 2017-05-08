using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Vertica.Utilities_v4.Collections
{
	public enum Priority
	{
		Low, Normal, High
	}

	public class PriorityQueue<TPriorityEnum, TValue> : IEnumerable<TValue>
		where TPriorityEnum : struct, IComparable, IFormattable, IConvertible
	{
		#region priority comparer class
		private class PriorityComparer : IComparer<TPriorityEnum>
		{
			#region IComparer<Priority> Members
			public int Compare(TPriorityEnum x, TPriorityEnum y)
			{
				return y.CompareTo(x);
			}
			#endregion
		}
		#endregion

		private readonly SortedList<TPriorityEnum, Queue<TValue>> _list;
		/// <summary>
		/// Creates a new <see>PriorityQueue</see>.
		/// </summary>
		public PriorityQueue()
		{
			_list = new SortedList<TPriorityEnum, Queue<TValue>>(new PriorityComparer());
			IEnumerable<TPriorityEnum> values = Enumeration.GetValues<TPriorityEnum>();
			foreach (var priority in values)
			{
				_list.Add(priority, new Queue<TValue>());
			}
		}

		/// <summary>
		/// Enqueues an item at the specified priority.
		/// </summary>
		/// <param name="priority">The <see>Priority</see> at which to enqueue the item.</param>
		/// <param name="item">The item to enqueue.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if <paramref name="priority"/> is not a valid enumeration value.</exception>
		public void Enqueue(TPriorityEnum priority, TValue item)
		{
			Enumeration.AssertDefined(priority);

			lock (_list)
			{
				_list[priority].Enqueue(item);
				Interlocked.Increment(ref _count);
			}
		}

		/// <summary>
		/// Gets an item from the queue.
		/// </summary>
		/// <returns>The next highest-priority item in the queue.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the queue is empty.</exception>
		public TValue Dequeue()
		{
			Guard.Against(Count == 0, "This queue is empty.");

			TValue item = default(TValue);
			lock (_list)
			{
				foreach (Queue<TValue> queue in _list.Values)
				{
					if (queue.Count > 0)
					{
						item = queue.Dequeue();
						Interlocked.Decrement(ref _count);
						break;
					}
				}
			}

			return item;
		}

		public TValue Peek()
		{
			TValue item = default(TValue);
			lock (_list)
			{
				foreach (Queue<TValue> queue in _list.Values)
				{
					if (queue.Count > 0)
					{
						item = queue.Peek();
						break;
					}
				}
			}
			return item;
		}

		/// <summary>
		/// Gets the current number of items in the queue.
		/// </summary>
		private int _count;
		public int Count { get { return _count; } }

		public IEnumerator<TValue> GetEnumerator()
		{
			IEnumerable<TValue> mergedEnumerator = _list.Values
				.Aggregate(Enumerable.Empty<TValue>(),
					(current, queue) => current.Concat(queue));

			return mergedEnumerator.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}