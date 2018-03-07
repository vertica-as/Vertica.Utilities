using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities.Collections
{
	public class KeyValuesCollection : KeyValuesCollection<string, string>
	{
		public KeyValuesCollection() { }

		public KeyValuesCollection(IEqualityComparer<string> keyComparer) : base(keyComparer) { }
	}
    public class KeyValuesCollection<TKey, TValue> : ILookup<TKey, TValue>
    {
	    private readonly Dictionary<TKey, MutableGrouping> _groups;

	    public KeyValuesCollection(IEqualityComparer<TKey> keyComparer = null)
	    {
		    _groups = new Dictionary<TKey, MutableGrouping>(keyComparer ?? EqualityComparer<TKey>.Default);
	    }

	    private static readonly IEnumerable<TValue> Empty = new TValue[0];

		public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
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

	    public IEnumerable<TKey> Keys => _groups.Keys;

	    public void TrimExcess()
	    {
		    foreach (var group in _groups.Values)
		    {
			    group.TrimExcess();
		    }
	    }

		#region Contains

		public bool Contains(TKey key)
	    {
		    MutableGrouping group;
		    return _groups.TryGetValue(key, out group) && group.Count > 0;
	    }

	    public bool Contains(TKey key, TValue value)
	    {
		    MutableGrouping group;
		    return _groups.TryGetValue(key, out group) && group.Contains(value);
	    }

	    #endregion

	    #region Add

	    public void Add(TKey key, TValue value)
	    {
		    MutableGrouping group;
		    if (!_groups.TryGetValue(key, out group))
		    {
			    group = new MutableGrouping(key);
			    _groups.Add(key, group);
		    }
		    group.Add(value);
	    }

	    public void AddRange(TKey key, params TValue[] values)
	    {
		    AddRange(key, values.AsEnumerable());
	    }

	    public void AddRange(TKey key, IEnumerable<TValue> values)
	    {
		    Guard.AgainstNullArgument(nameof(values), values);
		    MutableGrouping group;
		    if (!_groups.TryGetValue(key, out group))
		    {
			    group = new MutableGrouping(key);
			    _groups.Add(key, group);
		    }
		    foreach (TValue value in values)
		    {
			    group.Add(value);
		    }
		    if (group.Count == 0)
		    {
			    _groups.Remove(key);
		    }
	    }

	    public void AddRange(ILookup<TKey, TValue> lookup)
	    {
		    Guard.AgainstNullArgument(nameof(lookup), lookup);

		    foreach (IGrouping<TKey, TValue> group in lookup)
		    {
			    AddRange(group.Key, group);
		    }
	    }

		#endregion

	    #region Remove

	    public bool Remove(TKey key)
	    {
		    return _groups.Remove(key);
	    }

	    public bool Remove(TKey key, TValue value)
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

	    public void Clear()
	    {
			_groups.Clear();
	    }

	    public void Clear(TKey key)
	    {
		    if (_groups.TryGetValue(key, out MutableGrouping group))
		    {
				group.Clear();
		    }
	    }

		public int Count => _groups.Count;

	    public IEnumerable<TValue> GetValues(TKey key)
	    {
		    if (_groups.TryGetValue(key, out MutableGrouping group))
		    {
			    return group;
		    }
		    return Empty;
	    }

	    public IEnumerable<TValue> this[TKey key] => GetValues(key);

	    internal sealed class MutableGrouping : IGrouping<TKey, TValue>
	    {
		    private readonly List<TValue> _items = new List<TValue>();
		    public TKey Key { get; }

		    public MutableGrouping(TKey key)
		    {
			    Key = key;
		    }
		    public int Count => _items.Count;

		    public void Add(TValue item)
		    {
			    _items.Add(item);
		    }
		    public bool Contains(TValue item)
		    {
			    return _items.Contains(item);
		    }
		    public bool Remove(TValue item)
		    {
			    return _items.Remove(item);
		    }
		    public void TrimExcess()
		    {
			    _items.TrimExcess();
		    }

		    public IEnumerator<TValue> GetEnumerator()
		    {
			    return _items.GetEnumerator();
		    }

		    IEnumerator IEnumerable.GetEnumerator()
		    {
			    return GetEnumerator();
		    }

		    public void Clear()
		    {
				_items.Clear();
		    }
	    }
	}
}
