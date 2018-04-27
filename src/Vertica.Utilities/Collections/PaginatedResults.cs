using System;
using System.Linq;

namespace Vertica.Utilities.Collections
{
	public class PaginatedResults<T> : IPaginatedResults
	{
		public PaginatedResults()
		{
			PageOfResults = new T[0];
			RecordNumbers = Range.Empty<uint>();
		}

		public PaginatedResults(T[] pageOfResults, int totalResults, Pagination pagination)
		{
			Pagination = pagination;

			var totalCount = Convert.ToUInt32(totalResults);

			CurrentPage = pagination.PageNumber == 0 ? 1 : pagination.PageNumber;
			PageOfResults = pageOfResults;
			TotalResults = totalCount;
			TotalPages = pagination.PageCount(totalCount);

			uint lastRecord = 0;
			if (CurrentPage <= TotalPages)
			{
				lastRecord = PageNotComplete(pageOfResults, pagination) ?
				   Convert.ToUInt32(pagination.FirstRecord + pageOfResults.Length - 1) :
				   pagination.LastRecord;
			}

			RecordNumbers = lastRecord > 0 ? new Range<uint>(pagination.FirstRecord, lastRecord) : new Range<uint>(0, 0);
		}

		private static bool PageNotComplete(T[] pageOfResults, Pagination pagination)
		{
			return pageOfResults.Length < pagination.PageSize;
		}

		public T[] PageOfResults { get; }
		public uint CurrentPage { get; }
		public uint TotalResults { get; }
		public uint TotalPages { get; }
		public Range<uint> RecordNumbers { get; }

		public string PageNumber => "PageNumber";
		public Pagination Pagination { get; }

		/// <summary>
		/// Projects each element of this PaginatedResults into a new form.
		/// </summary>
		public PaginatedResults<TTo> Project<TTo>(Func<T, TTo> mapper)
		{
			if (mapper == null) throw new ArgumentNullException(nameof(mapper));

			return new PaginatedResults<TTo>(PageOfResults.Select(mapper).ToArray(), (int)TotalResults, Pagination);
		}
	}
}