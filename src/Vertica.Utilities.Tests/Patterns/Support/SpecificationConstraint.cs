using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;
using Vertica.Utilities.Patterns;

namespace Vertica.Utilities.Tests.Patterns.Support
{
	public class SpecificationConstraint<T> : DelegatingConstraint
	{
		private readonly List<T> _values;

		public SpecificationConstraint(T value, bool satisfied)
		{
			_values = new List<T> {value};
			Delegate = new EqualConstraint(satisfied);
		}

		public new SpecificationConstraint<T> Or(T value)
		{
			_values.Add(value);
			return this;
		}

		public new SpecificationConstraint<T> And(T value)
		{
			_values.Add(value);
			return this;
		}

		protected override ConstraintResult matches(object current)
		{
			ConstraintResult result = new ConstraintResult(this, current, true);

			ISpecification<T> spec = (ISpecification<T>)current;
			foreach (var value in _values)
			{
				result = new SpecificationResult(Delegate, Delegate.ApplyTo(spec.IsSatisfiedBy(value)));
				if (!result.IsSuccess)
				{
					break;
				}
			}
			return result;
		}

		class SpecificationResult : ConstraintResult
		{
			private readonly ConstraintResult _result;

			public SpecificationResult(IConstraint constraint, ConstraintResult result) : base(constraint, result.ActualValue, result.IsSuccess)
			{
				_result = result;
			}

			public override void WriteMessageTo(MessageWriter writer)
			{
				writer.Write("Value ");
				writer.WriteValue(_result.ActualValue);
				writer.WriteLine();
				base.WriteMessageTo(writer);
			}
		}
	}
}