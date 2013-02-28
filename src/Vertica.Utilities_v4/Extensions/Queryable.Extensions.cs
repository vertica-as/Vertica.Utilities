using System.Linq;
using Vertica.Utilities_v4.Collections;

namespace Vertica.Utilities_v4.Extensions.QueryableExt
{
	public static class QueryableExtensions
	{
		public static IQueryable<T> Paginate<T>(this IQueryable<T> nonPaginated, Pagination page)
		{
			Guard.AgainstNullArgument("nonPaginated", nonPaginated);

			return nonPaginated
					.Skip((int)page.FirstRecord - 1)
					.Take((int)page.PageSize);
		}
	}
}