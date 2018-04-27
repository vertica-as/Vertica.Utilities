namespace Vertica.Utilities.Collections
{
	public struct Pagination
	{
		public Pagination(uint pageSize, uint pageNumber) : this()
		{
			PageSize = pageSize;
			PageNumber = pageNumber;
		}

		public uint PageNumber { get; }
		public uint PageSize { get; }

		public uint FirstRecord => PageNumber == 0 ? 1 : ((PageNumber - 1) * PageSize) + 1;

		public uint LastRecord => PageSize == 0 ? FirstRecord : FirstRecord + PageSize - 1;

		public uint PageCount(uint totalCount)
		{
			uint numberOfPages = 0u;

			if (PageSize != 0)
			{
				uint n = totalCount % PageSize;
				numberOfPages = n > 0 ? (totalCount / PageSize) + 1 : totalCount / PageSize;
			}

			return numberOfPages;
		}

		public Pagination Next(uint increment = 1u)
		{
			return new Pagination(PageSize, PageNumber + increment);
		}
	}
}