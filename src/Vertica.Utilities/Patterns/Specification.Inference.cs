using System;
using System.Linq.Expressions;

namespace Vertica.Utilities.Patterns
{
	public static class PredicateSpecification
	{
		public static PredicateSpecification<T> For<T>(Predicate<T> predicate)
		{
			return new PredicateSpecification<T>(predicate);
		}
	}

	public static class ExpressionSpecification
	{
		public static ExpressionSpecification<T> For<T>(Expression<Func<T, bool>> expression)
		{
			return new ExpressionSpecification<T>(expression);
		}
	}
}