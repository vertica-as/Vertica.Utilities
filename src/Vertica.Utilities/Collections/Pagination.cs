using System;

namespace Vertica.Utilities_v4.Collections
{
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
}