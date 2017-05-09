using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities.Collections
{
	public class PaginatedCollection<T> : IPaginatedCollection<T>
	{
		private readonly IEnumerable<T> _collection;
		private readonly Pagination _pagination;

		#region ctor

		public PaginatedCollection(Pagination pagination, IEnumerable<T> collection)
		{
			_pagination = pagination;
			_collection = collection;
		}

		#endregion

		#region properties

		public uint CurrentPage { get { return _pagination.PageNumber; } }
		public uint Pagesize { get { return _pagination.PageSize; } }

		private uint? _numberOfPages;
		public uint NumberOfPages
		{
			get
			{
				_numberOfPages = _numberOfPages ?? initNumberOfPages();
				return _numberOfPages.Value;
			}
		}
		private IEnumerable<T> _paginatedCollection;
		public IEnumerable<T> Collection
		{
			get
			{
				_paginatedCollection = _paginatedCollection ?? initPagedResult();
				return _paginatedCollection;
			}
		}

		#endregion

		private IEnumerable<T> initPagedResult()
		{
			IEnumerable<T> result = null;
			if (_collection != null)
			{
				result = _collection.Select(t => t)
					.Skip((int)(_pagination.PageSize * (_pagination.PageNumber - 1)))
					.Take((int)_pagination.PageSize);
			}
			return result;
		}

		private uint initNumberOfPages()
		{
			uint numberOfPages = _collection != null ?
				_pagination.PageCount(Convert.ToUInt32(_collection.Count())) :
				0;
			return numberOfPages;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return Collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}