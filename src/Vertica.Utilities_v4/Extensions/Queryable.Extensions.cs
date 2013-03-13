using System;
using System.Linq;
using System.Linq.Expressions;
using Vertica.Utilities_v4.Collections;
using Vertica.Utilities_v4.Comparisons;

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

		public static IOrderedQueryable<TSource> SortBy<TSource>(this IQueryable<TSource> unordered)
		{
			Guard.AgainstNullArgument("unordered", unordered);
			return unordered.SortBy(e => e, Direction.Ascending);
		}

		public static IOrderedQueryable<TSource> SortBy<TSource>(this IQueryable<TSource> unordered, Direction? direction)
		{
			Guard.AgainstNullArgument("unordered", unordered);

			return unordered.SortBy(e => e, direction);
		}

		public static IOrderedQueryable<TSource> SortBy<TSource, TKey>(this IQueryable<TSource> unordered, Expression<Func<TSource, TKey>> selector, Direction? direction)
		{
			Guard.AgainstNullArgument("unordered", unordered);

			return direction.HasValue ?
				direction.Equals(Direction.Ascending) ? 
					unordered.OrderBy(selector) :
					unordered.OrderByDescending(selector) :
				(IOrderedQueryable<TSource>)unordered;
		}
	}
}