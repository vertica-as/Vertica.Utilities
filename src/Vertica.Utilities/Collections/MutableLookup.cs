using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities.Collections
{
    public class MutableLookup<T, U> : ILookup<T, U>
    {
	    private readonly Dictionary<T, MutableGrouping> _groups;

	    public MutableLookup(IEqualityComparer<T> keyComparer = null)
	    {
		    _groups = new Dictionary<T, MutableGrouping>(keyComparer ?? EqualityComparer<T>.Default);
	    }

	    private static readonly IEnumerable<U> Empty = new U[0];

		public IEnumerator<IGrouping<T, U>> GetEnumerator()
	    {
			foreach (var group in _groups.Values)
			{
				yield return group;
			}
		}

	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return GetEnumerator();
	    }

	    public IEnumerable<T> Keys => _groups.Keys;

	    public void TrimExcess()
	    {
		    foreach (var group in _groups.Values)
		    {
			    group.TrimExcess();
		    }
	    }

		#region Contains

		public bool Contains(T key)
	    {
		    MutableGrouping group;
		    return _groups.TryGetValue(key, out group) && group.Count > 0;
	    }

	    public bool Contains(T key, U value)
	    {
		    MutableGrouping group;
		    return _groups.TryGetValue(key, out group) && group.Contains(value);
	    }

	    #endregion

	    #region Add

	    public void Add(T key, U value)
	    {
		    MutableGrouping group;
		    if (!_groups.TryGetValue(key, out group))
		    {
			    group = new MutableGrouping(key);
			    _groups.Add(key, group);
		    }
		    group.Add(value);
	    }

	    public void AddRange(T key, params U[] values)
	    {
		    AddRange(key, values.AsEnumerable());
	    }

	    public void AddRange(T key, IEnumerable<U> values)
	    {
		    Guard.AgainstNullArgument(nameof(values), values);
		    MutableGrouping group;
		    if (!_groups.TryGetValue(key, out group))
		    {
			    group = new MutableGrouping(key);
			    _groups.Add(key, group);
		    }
		    foreach (U value in values)
		    {
			    group.Add(value);
		    }
		    if (group.Count == 0)
		    {
			    _groups.Remove(key);
		    }
	    }

	    public void AddRange(ILookup<T, U> lookup)
	    {
		    Guard.AgainstNullArgument(nameof(lookup), lookup);

		    foreach (IGrouping<T, U> group in lookup)
		    {
			    AddRange(group.Key, group);
		    }
	    }

		#endregion

	    #region Remove

	    public bool Remove(T key)
	    {
		    return _groups.Remove(key);
	    }

	    public bool Remove(T key, U value)
	    {
		    MutableGrouping group;
		    bool removed = false;
		    if (_groups.TryGetValue(key, out group))
		    {
			    removed = group.Remove(value);
			    if (removed && group.Count == 0)
			    {
				    _groups.Remove(key);
			    }
		    }
		    return removed;
	    }

		#endregion

		public int Count => _groups.Count;

	    public IEnumerable<U> this[T key]
	    {
			get
			{
				MutableGrouping group;
				if (_groups.TryGetValue(key, out group))
				{
					return group;
				}
				return Empty;
			}
		}

	    internal sealed class MutableGrouping : IGrouping<T, U>
	    {
		    private readonly List<U> _items = new List<U>();
		    public T Key { get; }

		    public MutableGrouping(T key)
		    {
			    Key = key;
		    }
		    public int Count => _items.Count;

		    public void Add(U item)
		    {
			    _items.Add(item);
		    }
		    public bool Contains(U item)
		    {
			    return _items.Contains(item);
		    }
		    public bool Remove(U item)
		    {
			    return _items.Remove(item);
		    }
		    public void TrimExcess()
		    {
			    _items.TrimExcess();
		    }

		    public IEnumerator<U> GetEnumerator()
		    {
			    return _items.GetEnumerator();
		    }

		    IEnumerator IEnumerable.GetEnumerator()
		    {
			    return GetEnumerator();
		    }
	    }
	}
}
