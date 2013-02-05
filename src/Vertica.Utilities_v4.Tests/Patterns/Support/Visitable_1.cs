using Vertica.Utilities_v4.Patterns;

namespace Vertica.Utilities_v4.Tests.Patterns.Support
{
	internal class Visitable_1 : VisitableBase
	{
		public int Property1 { get; set; }

		public override void Accept(GenericVisitor<VisitableBase> visitor)
		{
			visitor.Visit(this);
		}
	}
}