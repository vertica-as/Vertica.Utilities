using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities_v4.Collections
{
	public class Tree<TModel, TKey> : Tree<TModel, TModel, TKey>
	{
		internal protected Tree(IEnumerable<TModel> items, Func<TModel, TKey> key, Func<TModel, Parent, Parent.Key> parentKey, IEqualityComparer<TKey> comparer = null)
			: base(items, key, parentKey, x => x, comparer)
		{
		}
	}

	public class Tree<TItem, TModel, TKey> : IEnumerable<TreeNode<TModel>>
	{
		private readonly Dictionary<TKey, Tuple<TKey, Parent.Key, TModel, List<TKey>>> _tree;
		private readonly List<TKey> _root;
		private readonly HashSet<TKey> _orphans;

		internal protected Tree(IEnumerable<TItem> items, Func<TItem, TKey> key, Func<TItem, Parent, Parent.Key> parentKey, Func<TItem, TModel> projection, IEqualityComparer<TKey> comparer = null)
		{
			if (items == null) throw new ArgumentNullException("items");
			if (key == null) throw new ArgumentNullException("key");
			if (parentKey == null) throw new ArgumentNullException("parentKey");
			if (projection == null) throw new ArgumentNullException("projection");

			// Optimize by setting the initial count to the number of items (if item is List, Collection, Array)
			_tree = new Dictionary<TKey, Tuple<TKey, Parent.Key, TModel, List<TKey>>>(TreeCapacityOr(items as ICollection<TModel>, 0), comparer);

			_root = new List<TKey>();
			_orphans = new HashSet<TKey>(comparer);

			var parentHelper = new Parent();

			// Initialize Tree with all items
			foreach (TItem item in items)
			{
				TKey itemKey = key(item);
				Parent.Key itemParentKey = parentKey(item, parentHelper);

				// No way to optimize/guess the initial capacity of the list
				_tree[itemKey] = Tuple.Create(itemKey, itemParentKey, projection(item), new List<TKey>());
			}

			// Relate children to their parents
			foreach (Tuple<TKey, Parent.Key, TModel, List<TKey>> item in _tree.Values)
			{
				// Test if we have a parent
				if (item.Item2 != null)
				{
					Tuple<TKey, Parent.Key, TModel, List<TKey>> parent;
					if (_tree.TryGetValue(item.Item2.Value, out parent))
					{
						parent.Item4.Add(item.Item1);
					}
					else
					{
						_orphans.Add(item.Item1);
					}
				}
				else
				{
					_root.Add(item.Item1);
				}
			}
		}

		private static int TreeCapacityOr(ICollection<TModel> collection, int defaultCapacity)
		{
			return collection != null ? collection.Count : defaultCapacity;
		}

		public TreeNode<TModel> this[int index]
		{
			get { return Get(_root[index]); }
		}

		public TreeNode<TModel> Get(TKey key)
		{
			TreeNode<TModel> node;
			if (!TryGet(key, out node))
				throw new KeyNotFoundException(String.Format(@"Node with key {0} was not found.", key));

			return node;
		}

		public bool TryGet(TKey key, out TreeNode<TModel> node)
		{
			node = null;

			Tuple<TKey, Parent.Key, TModel, List<TKey>> item;
			if (!_orphans.Contains(key) && _tree.TryGetValue(key, out item))
			{
				node = new TreeNode<TModel>(
					_tree[key].Item3, 
					GetEnumerator(item.Item4), 
					() => item.Item2 != null ? Get(item.Item2.Value) : null);
			}

			return node != null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<TreeNode<TModel>> GetEnumerator()
		{
			return GetEnumerator(_root);
		}

		public IEnumerable<TModel> Orphans()
		{
			return _orphans.Select(x => _tree[x].Item3);
		}

		private IEnumerator<TreeNode<TModel>> GetEnumerator(List<TKey> nodes)
		{
			if (nodes == null) throw new ArgumentNullException("nodes");

			return nodes
				.Select(x =>
				{
					TreeNode<TModel> node;
					TryGet(x, out node);
					return node;
				})
				.Where(x => x != null)
				.GetEnumerator();
		}

		public class Parent
		{
			public Key Value(TKey key)
			{
				return new Key(key);
			}

			public Key None { get { return null; } }

			public class Key
			{
				internal Key(TKey key)
				{
					Value = key;
				}

				internal TKey Value { get; private set; }
			}
		}
	}

	public class TreeNode<TModel> : IEnumerable<TreeNode<TModel>>
	{
		private readonly IEnumerator<TreeNode<TModel>> _children;
		private readonly Func<TreeNode<TModel>> _parent;

		internal TreeNode(TModel model, IEnumerator<TreeNode<TModel>> children, Func<TreeNode<TModel>> parent)
		{
			Model = model;
			_children = children;
			_parent = parent;
		}

		public TreeNode<TModel> this[int index]
		{
			get { return this.ElementAt(index); }
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<TreeNode<TModel>> GetEnumerator()
		{
			return _children;
		}

		public TreeNode<TModel> Parent
		{
			get { return _parent(); }
		}

		public TModel Model { get; private set; }

		public TModel[] Breadcrumb()
		{
			var stack = new Stack<TModel>();

			TreeNode<TModel> current = this;

			while (current != null)
			{
				stack.Push(current.Model);
				current = current.Parent;
			}

			return stack.ToArray(); 
		}
	}
}