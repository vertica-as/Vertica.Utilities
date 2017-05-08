using NUnit.Framework;
using Vertica.Utilities_v4.Patterns;
using Vertica.Utilities_v4.Tests.Patterns.Support;

namespace Vertica.Utilities_v4.Tests.Patterns
{
	using Visit_1 = GenericVisitor<VisitableBase>.VisitDelegate<Visitable_1>;
	using Visit_2 = GenericVisitor<VisitableBase>.VisitDelegate<Visitable_2>;

	[TestFixture]
	public class GenericVisitorTester
	{
		[Test]
		public void Visit_AddSquareFunctionality_SquareCalculatedAndSet()
		{
			var visitor = new GenericVisitor<VisitableBase>();
			Visit_1 square1 = s => s.Property1 *= s.Property1;
			Visit_2 square2 = s => s.Property2 *= s.Property2;
			visitor
				.AddDelegate(square1).AddDelegate(square2);

			var s1 = new Visitable_1 { Property1 = 2 };
			var s2 = new Visitable_2 { Property2 = 3 };

			visitor.Visit(s1);
			Assert.That(s1.Property1, Is.EqualTo(4));

			visitor.Visit(s2);
			Assert.That(s2.Property2, Is.EqualTo(9));
		}

		[Test]
		public void Visit_ClassicVisitor_OperationPerformedOnlyOnRegisteredTypes()
		{
			int externalContext = -1;

			var visitor = new GenericVisitor<VisitableBase>();
			visitor.AddDelegate<Visitable_1>(s => externalContext = s.Property1);

			var subject1 = new Visitable_1 { Property1 = 5 };
			var subject2 = new Visitable_2 { Property2 = 10 };

			subject1.Accept(visitor);
			Assert.That(externalContext, Is.EqualTo(subject1.Property1), "must change the external context");

			subject2.Accept(visitor);
			Assert.That(externalContext, Is.Not.EqualTo(subject2.Property2), "must not change the external context as no delegate was registered");
		}
	}
}