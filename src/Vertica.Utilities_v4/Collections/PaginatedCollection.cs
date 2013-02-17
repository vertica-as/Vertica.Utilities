using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities_v4.Collections
{
	public interface IPaginatedCollection<out T> : IEnumerable<T>
	{
		uint CurrentPage { get; }
		uint Pagesize { get; }
		uint NumberOfPages { get; }
		IEnumerable<T> Collection { get; }
	}

	[Serializable]
	public struct Pagination
	{
		public Pagination(uint pageSize, uint pageNumber) : this()
		{
			PageSize = pageSize;
			PageNumber = pageNumber;
		}

		public uint PageNumber { get; private set; }
		public uint PageSize { get; private set; }

		public uint FirstRecord
		{
			get
			{
				return (PageNumber == 0) ?
					1 :
					((PageNumber - 1) * PageSize) + 1;
			}
		}

		public uint LastRecord
		{
			get
			{
				return (PageSize == 0) ?
					FirstRecord :
					FirstRecord + PageSize - 1;
			}
		}

		public uint PageCount(uint totalCount)
		{
			uint n = totalCount % PageSize;

			uint numberOfPages = n > 0 ?
				(totalCount / PageSize) + 1 :
				totalCount / PageSize;

			return numberOfPages;
		}
	}

	[Serializable]
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