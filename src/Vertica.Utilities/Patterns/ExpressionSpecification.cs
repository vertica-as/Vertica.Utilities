using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Vertica.Utilities.Patterns
{
	public class ExpressionSpecification<T> : Specification<T>
	{
		private readonly Expression<Func<T, bool>> _predicateExpression;
		public ExpressionSpecification(Expression<Func<T, bool>> expression)
		{
			_predicateExpression = expression;
		}

		public Expression<Func<T, bool>> Expression { get { return _predicateExpression; } }

		private Func<T, bool> _predicate;
		public Func<T, bool> Function { get { return _predicate = _predicate ?? _predicateExpression.Compile(); } }

		public override bool IsSatisfiedBy(T entity)
		{
			return Function(entity);
		}

		#region operators

		#region conversion

		public static implicit operator Func<T, bool>(ExpressionSpecification<T> specification)
		{
			return specification.Function;
		}

		public static implicit operator Expression<Func<T, bool>>(ExpressionSpecification<T> specification)
		{
			return specification.Expression;
		}

		public static implicit operator Predicate<T>(ExpressionSpecification<T> spec)
		{
			return t => spec.Function(t);
		}

		#endregion

		public static ExpressionSpecification<T> operator !(ExpressionSpecification<T> spec)
		{
			var newExpression = System.Linq.Expressions.Expression.MakeUnary(ExpressionType.Not, spec.Expression.Body, typeof(bool));

			return new ExpressionSpecification<T>(toLambda(newExpression, spec.Expression.Parameters));
		}

		public static ExpressionSpecification<T> operator &(ExpressionSpecification<T> leftSide, ExpressionSpecification<T> rightSide)
		{
			var newExpression = mergeIntoBinary(rightSide.Expression, leftSide.Expression,
				ExpressionType.AndAlso);

			return new ExpressionSpecification<T>(toLambda(newExpression, leftSide.Expression.Parameters));
		}

		public static ExpressionSpecification<T> operator |(ExpressionSpecification<T> leftSide, ExpressionSpecification<T> rightSide)
		{
			var newExpression = mergeIntoBinary(rightSide.Expression, leftSide.Expression,
				ExpressionType.OrElse);

			return new ExpressionSpecification<T>(toLambda(newExpression, leftSide.Expression.Parameters));
		}

		public static bool operator true(ExpressionSpecification<T> specification) { return false; }

		public static bool operator false(ExpressionSpecification<T> specification) { return false; }

		#endregion

		#region operator-based methods

		public new ExpressionSpecification<T> Not()
		{
			return !(this);
		}

		public ExpressionSpecification<T> And(ExpressionSpecification<T> right)
		{
			return this && right;
		}

		public ExpressionSpecification<T> Or(ExpressionSpecification<T> right)
		{
			return this || right;
		}

		#endregion

		private static Expression<Func<T, bool>> toLambda(Expression expression, IEnumerable<ParameterExpression> parameters)
		{
			return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(expression, parameters);
		}

		private static BinaryExpression mergeIntoBinary(Expression<Func<T, bool>> right, Expression<Func<T, bool>> left, ExpressionType type)
		{
			InvocationExpression rightInvoke = System.Linq.Expressions.Expression.Invoke(right, left.Parameters);

			BinaryExpression mergedExpression = System.Linq.Expressions.Expression.MakeBinary(type, left.Body, rightInvoke);

			return mergedExpression;
		}
	}
}