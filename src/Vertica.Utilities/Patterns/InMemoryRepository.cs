using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vertica.Utilities.Collections;
using Vertica.Utilities.Extensions.ObjectExt;

namespace Vertica.Utilities.Patterns
{
	public class InMemoryRepository<T, K> : IRepository<T, K> where T : IIdentifiable<K>
	{
		private readonly List<T> _inner;
		public InMemoryRepository(IEnumerable<T> initialData)
		{
			_inner = new List<T>();
			
			if (initialData != null)
			{
				_inner.AddRange(initialData);
			}
		}

		public InMemoryRepository(params T[] initialData) : this(initialData.AsEnumerable()) { }

		public int Count()
		{
			return _inner.Count;
		}

		public int Count(Expression<Func<T, bool>> expression)
		{
			return _inner.Count(expression.Compile());
		}

		public void Add(T entity)
		{
			_inner.Add(entity);
		}

		public void Remove(T entity)
		{
			_inner.RemoveAll(e => e.Id.Equals(entity.Id));
		}

		public void Save(T entity)
		{
			// no need to persist changes in memory, as they are already there
		}

		public T FindOne(K id)
		{
			return _inner.Single(e => e.Id.Equals(id));
		}

		public T FindOne(Expression<Func<T, bool>> expression)
		{
			return _inner.Single(expression.Compile());
		}

		public bool TryFindOne(Expression<Func<T, bool>> expression, out T entity)
		{
			try
			{
				entity = _inner.SingleOrDefault(expression.Compile());
			}
				// when more than one instance is found, null is returned
			catch (InvalidOperationException)
			{
				entity = default(T);
			}
			return entity.IsNotDefault();
		}

		public Collections.IReadOnlyList<T> FindAll()
		{
			return new ReadOnlyListAdapter<T>(_inner);
		}

		public Collections.IReadOnlyList<T> FindAll(Expression<Func<T, bool>> expression)
		{

			return new ReadOnlyListAdapter<T>(_inner.Where(expression.Compile()).ToList());
		}

		public IQueryable<T> Find()
		{
			return _inner.AsQueryable();
		}

		public IQueryable<T> Find(K id)
		{
			return _inner.FindAll(e => e.Id.Equals(id)).AsQueryable();
		}

		public IQueryable<T> Find(Expression<Func<T, bool>> expression)
		{
			return _inner.Where(expression.Compile()).AsQueryable();
		}
	}
}