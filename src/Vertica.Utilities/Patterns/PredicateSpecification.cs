using System;

namespace Vertica.Utilities.Patterns
{
	public class PredicateSpecification<T> : Specification<T>
	{
		private readonly Predicate<T> _predicate;
		public PredicateSpecification(Predicate<T> predicate)
		{
			_predicate = predicate;
		}

		public override bool IsSatisfiedBy(T item)
		{
			return _predicate(item);
		}

		public Predicate<T> Predicate { get { return _predicate; } }

		public Func<T, bool> Function { get { return new Func<T, bool>(t => _predicate(t)); } }

		#region operators

		public static PredicateSpecification<T> operator &(PredicateSpecification<T> left, PredicateSpecification<T> right)
		{
			return new PredicateSpecification<T>(t => left.IsSatisfiedBy(t) && right.IsSatisfiedBy(t));
		}

		public static PredicateSpecification<T> operator |(PredicateSpecification<T> left, PredicateSpecification<T> right)
		{
			return new PredicateSpecification<T>(t => left.IsSatisfiedBy(t) || right.IsSatisfiedBy(t));
		}

		public static PredicateSpecification<T> operator !(PredicateSpecification<T> specification)
		{
			return new PredicateSpecification<T>(t => !specification.IsSatisfiedBy(t));
		}

		public static bool operator true(PredicateSpecification<T> specification) { return false; }

		public static bool operator false(PredicateSpecification<T> specification) { return false; }

		public static implicit operator Predicate<T>(PredicateSpecification<T> spec)
		{
			return spec.Predicate;
		}

		public static implicit operator Func<T, bool>(PredicateSpecification<T> spec)
		{
			return spec.Function;
		}

		#endregion
	}
}