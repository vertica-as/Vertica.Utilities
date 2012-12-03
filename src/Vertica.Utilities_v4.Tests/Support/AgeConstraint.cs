using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Vertica.Utilities_v4.Reflection;

namespace Vertica.Utilities_v4.Tests.Support
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

		private Constraint _inner;
		public override bool Matches(object current)
		{
			actual = current;
			bool matches = true;
			var enumerator = _constraints.GetEnumerator();
			while (enumerator.MoveNext())
			{
				_inner = enumerator.Current;
				matches = _inner.Matches(current);
				if (!matches) break;
			}
			return matches;
		}


		public override void WriteDescriptionTo(MessageWriter writer)
		{
			_inner.WriteDescriptionTo(writer);
		}

		public override void WriteActualValueTo(MessageWriter writer)
		{
			_inner.WriteActualValueTo(writer);
		}

		public override void WriteMessageTo(MessageWriter writer)
		{
			_inner.WriteMessageTo(writer);
		}
	}
}
