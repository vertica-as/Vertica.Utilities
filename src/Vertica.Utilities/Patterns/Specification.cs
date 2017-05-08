namespace Vertica.Utilities_v4.Patterns
{
	public abstract class Specification<T> : ISpecification<T>
	{
		public abstract bool IsSatisfiedBy(T item);

		public virtual ISpecification<T> And(ISpecification<T> other)
		{
			return new AndSpecification(this, other);
		}

		public virtual ISpecification<T> Not()
		{
			return new NotSpecification(this);
		}

		public virtual ISpecification<T> Or(ISpecification<T> other)
		{
			return new OrSpecification(this, other);
		}

		private class AndSpecification : Specification<T>
		{
			private readonly ISpecification<T> _leftSide;
			private readonly ISpecification<T> _rightSide;

			public AndSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
			{
				_leftSide = leftSide;
				_rightSide = rightSide;
			}

			public override bool IsSatisfiedBy(T item)
			{
				return _leftSide.IsSatisfiedBy(item) && _rightSide.IsSatisfiedBy(item);
			}
		}

		private class NotSpecification : Specification<T>
		{
			private readonly ISpecification<T> _specification;

			public NotSpecification(ISpecification<T> specification)
			{
				_specification = specification;
			}

			public override bool IsSatisfiedBy(T item)
			{
				return !_specification.IsSatisfiedBy(item);
			}
		}

		private class OrSpecification : Specification<T>
		{
			private readonly ISpecification<T> _leftSide;
			private readonly ISpecification<T> _rightSide;

			public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
			{
				_leftSide = leftSide;
				_rightSide = rightSide;
			}

			public override bool IsSatisfiedBy(T item)
			{
				return _leftSide.IsSatisfiedBy(item) || _rightSide.IsSatisfiedBy(item);
			}
		}
	}
}