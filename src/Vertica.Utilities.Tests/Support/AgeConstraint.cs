using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Vertica.Utilities.Reflection;

namespace Vertica.Utilities.Tests.Support
{
	internal class AgeConstraint : Constraint
	{
		private readonly List<Constraint> _constraints = new List<Constraint>(7); 
		public AgeConstraint WithBounds(DateTime advent, DateTime terminus)
		{
			_constraints.Add(new PropertyConstraint(Name.Of<Age, DateTime>((a) => a.Advent), new EqualConstraint(advent)));
			_constraints.Add(new PropertyConstraint(Name.Of<Age, DateTime>((a) => a.Terminus), new EqualConstraint(terminus)));
			return this;
		}

		public AgeConstraint Elapsed(TimeSpan elapsed)
		{
			_constraints.Add(new PropertyConstraint(Name.Of<Age, TimeSpan>(a => a.Elapsed), new EqualConstraint(elapsed)));
			return this;
		}

		public AgeConstraint WithComponents(int days, int weeks, int months, int years)
		{
			_constraints.Add(new PropertyConstraint(Name.Of<Age, int>(a => a.Days), new EqualConstraint(days)));
			_constraints.Add(new PropertyConstraint(Name.Of<Age, int>(a => a.Weeks), new EqualConstraint(weeks)));
			_constraints.Add(new PropertyConstraint(Name.Of<Age, int>(a => a.Months), new EqualConstraint(months)));
			_constraints.Add(new PropertyConstraint(Name.Of<Age, int>(a => a.Years), new EqualConstraint(years)));
			return this;
		}

		public override string Description => _inner.Description;

		private Constraint _inner;
		public override ConstraintResult ApplyTo<TActual>(TActual actual)
		{
			ConstraintResult matches = new ConstraintResult(this, null, true);
			foreach (var constraint in _constraints)
			{
				_inner = constraint;
				matches = new AgeResult(constraint, constraint.ApplyTo(actual));
				if (!matches.IsSuccess) break;
			}
			return matches;
		}

		class AgeResult : ConstraintResult
		{
			private readonly ConstraintResult _result;

			public AgeResult(IConstraint constraint, ConstraintResult result) : base(constraint, result.ActualValue, result.IsSuccess)
			{
				_result = result;
			}

			public override void WriteActualValueTo(MessageWriter writer)
			{
				_result.WriteActualValueTo(writer);
			}

			public override void WriteMessageTo(MessageWriter writer)
			{
				_result.WriteMessageTo(writer);
			}
		}
	}
}
