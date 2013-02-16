using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;
using Vertica.Utilities_v4.Patterns;

namespace Vertica.Utilities_v4.Tests.Patterns.Support
{
	public class SpecificationConstraint<T> : DelegatingConstraint<ISpecification<T>>
	{
		private readonly List<T> _values;

		public SpecificationConstraint(T value, bool satisfied)
		{
			_values = new List<T> {value};
			Delegate = new EqualConstraint(satisfied);
		}

		private T _failingValue;
		protected override bool matches(ISpecification<T> current)
		{
			bool result = false;
			foreach (var value in _values)
			{
				result = Delegate.Matches(current.IsSatisfiedBy(value));
				if (!result)
				{
					_failingValue = value;
					break;
				}
			}
			return result;
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

		public override void WriteMessageTo(MessageWriter writer)
		{
			writer.Write("Value ");
			writer.WriteValue(_failingValue);
			writer.WriteLine();
			base.WriteMessageTo(writer);
		}
	}
}