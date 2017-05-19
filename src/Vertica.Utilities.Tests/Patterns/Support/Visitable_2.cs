using Vertica.Utilities.Patterns;

namespace Vertica.Utilities.Tests.Patterns.Support
{
	internal class Visitable_2 : VisitableBase, IVisitable<VisitableBase>
	{
		public int Property2 { get; set; }
		
		public void Accept(IVisitor<VisitableBase> visitor)
		{
			visitor.Visit(this);
		}
	}
}