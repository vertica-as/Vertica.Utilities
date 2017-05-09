using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities.Collections
{
	public class Tree<TModel, TKey> : Tree<TModel, TModel, TKey>
	{
		protected internal Tree(IEnumerable<TModel> items, Func<TModel, TKey> key, Func<TModel, Parent, Parent.Key> parentKey, IEqualityComparer<TKey> comparer = null)
			: base(items, key, parentKey, x => x, comparer)
		{
		}
	}

	public class Tree<TItem, TModel, TKey> : IEnumerable<TreeNode<TModel>>
	{
	    private readonly Dictionary<TKey, Node> _tree;
		private readonly List<TKey> _root;
		private readonly HashSet<TKey> _orphans;

		protected internal Tree(IEnumerable<TItem> items, Func<TItem, TKey> key, Func<TItem, Parent, Parent.Key> parentKey, Func<TItem, TModel> projection, IEqualityComparer<TKey> comparer = null)
		{
			if (items == null) throw new ArgumentNullException("items");
			if (key == null) throw new ArgumentNullException("key");
			if (parentKey == null) throw new ArgumentNullException("parentKey");
			if (projection == null) throw new ArgumentNullException("projection");

			// Optimize by setting the initial count to the number of items (if item is List, Collection, Array)
            _tree = new Dictionary<TKey, Node>(TreeCapacityOr(items as ICollection<TModel>, 0), comparer);

            _root = new List<TKey>();
			_orphans = new HashSet<TKey>(comparer);

			var parentHelper = new Parent();

			// Initialize Tree with all items
			foreach (TItem item in items)
			{
				TKey itemKey = key(item);

			    Node itemNode;
			    if (!_tree.TryGetValue(itemKey, out itemNode))
			        _tree[itemKey] = itemNode = new Node(itemKey, projection(item));

                Parent.Key itemParentKey = parentKey(item, parentHelper);

                if (itemParentKey != null)
			        itemNode.Parents.Add(itemParentKey);
			}

			// Relate children to their parents
            foreach (Node item in _tree.Values)
			{
				// Test if we have a parent
				if (item.Parents.Count > 0)
				{
				    bool isOrphan = true;

				    foreach (var itemParentKey in item.Parents)
				    {
                        Node parent;
                        if (_tree.TryGetValue(itemParentKey.Value, out parent))
                        {
                            parent.Children.Add(item.Key);
                            isOrphan = false;
                        }
                    }

                    if (isOrphan)
                        _orphans.Add(item.Key);
                }
				else
				{
					_root.Add(item.Key);
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

			Node item;
			if (!_orphans.Contains(key) && _tree.TryGetValue(key, out item))
			{
				node = new TreeNode<TModel>(
					_tree[key].Model, 
					GetEnumerator(item.Children),
 					index => Get(item.Children[index]),
					item.Parents.Select(x => Get(x.Value)));
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
			return _orphans.Select(x => _tree[x].Model);
		}

		public int Count
		{
			get { return _tree.Count - _orphans.Count; }
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

	    private class Node
	    {
	        public Node(TKey key, TModel model)
	        {
	            Key = key;
	            Model = model;

                // No way to optimize/guess the initial capacity of the lists
                Parents = new List<Parent.Key>();
                Children = new List<TKey>();
            }

	        public TKey Key { get; private set; }
	        public TModel Model { get; private set; }

	        public List<Parent.Key> Parents { get; private set; }
            public List<TKey> Children { get; private set; }
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
		private readonly Func<int, TreeNode<TModel>> _childNodeAt;
		private readonly IEnumerable<TreeNode<TModel>> _parents;

		internal TreeNode(TModel model, IEnumerator<TreeNode<TModel>> children, Func<int, TreeNode<TModel>> childNodeAt, IEnumerable<TreeNode<TModel>> parents)
		{
			Model = model;
			_children = children;
			_childNodeAt = childNodeAt;
			_parents = parents;
		}

		public TreeNode<TModel> this[int index]
		{
			get { return _childNodeAt(index); }
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<TreeNode<TModel>> GetEnumerator()
		{
			return _children;
		}

	    public IEnumerable<TreeNode<TModel>> Parents
	    {
	        get { return _parents; }
	    }

	    public TreeNode<TModel> Parent
		{
			get { return Parents.FirstOrDefault(); }
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