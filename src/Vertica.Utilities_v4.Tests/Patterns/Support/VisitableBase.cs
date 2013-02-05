using Vertica.Utilities_v4.Patterns;

namespace Vertica.Utilities_v4.Tests.Patterns.Support
{
	public abstract class VisitableBase
	{
		public int BaseProperty { get; set; }
		public abstract void Accept(GenericVisitor<VisitableBase> visitor);
		 
	}
}