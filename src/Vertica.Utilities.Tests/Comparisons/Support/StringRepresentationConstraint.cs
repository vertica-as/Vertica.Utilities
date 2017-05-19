using System.Collections;
using System.Linq;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;

namespace Vertica.Utilities.Tests.Comparisons.Support
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

		public override string Description
		{
			get
			{
				MessageWriter writer = new TextMessageWriter();
				representedAs(writer);
				writer.WriteValue(_representation);
				return writer.ToString();
			}
		}

		private static void representedAs(MessageWriter writer)
		{
			writer.Write("Something representable as ");
		}

		public override ConstraintResult ApplyTo<TActual>(TActual actual)
		{
			string representation = represent((IEnumerable)actual);
			var result = _inner.ApplyTo(representation);
			return new RepresentationResult(_inner, result);
		}

		class RepresentationResult : ConstraintResult
		{
			private readonly ConstraintResult _result;

			public RepresentationResult(IConstraint constraint, ConstraintResult result) : base(constraint, result.ActualValue, result.IsSuccess)
			{
				_result = result;
			}

			public override void WriteActualValueTo(MessageWriter writer)
			{
				representedAs(writer);
				_result.WriteActualValueTo(writer);
			}
		}
	}
}