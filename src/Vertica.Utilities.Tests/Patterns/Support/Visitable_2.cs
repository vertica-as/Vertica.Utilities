using Vertica.Utilities_v4.Patterns;

namespace Vertica.Utilities_v4.Tests.Patterns.Support
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