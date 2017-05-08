using System.Collections;
using System.Linq;
using NUnit.Framework.Constraints;

namespace Vertica.Utilities_v4.Tests.Comparisons.Support
{
	internal class StringRepresentationConstraint : Constraint
	{
		private readonly string _representation;

		private readonly EqualConstraint _inner;
		public StringRepresentationConstraint(string representation)
		{
			_representation = representation;
			_inner = new EqualConstraint(representation);
		}

		private string represent(IEnumerable collection)
		{
			return string.Join(", ", collection.Cast<object>().Select(o => o.ToString()));
		}

		public override bool Matches(object current)
		{
			actual = represent((IEnumerable)current);
			return _inner.Matches(actual);
		}

		public override void WriteDescriptionTo(MessageWriter writer)
		{
			representedAs(writer);
			writer.WriteValue(_representation);
		}

		private void representedAs(MessageWriter writer)
		{
			writer.WritePredicate("Something representable as");
		}

		public override void WriteActualValueTo(MessageWriter writer)
		{
			representedAs(writer);
			_inner.WriteActualValueTo(writer);
		}
	}
}