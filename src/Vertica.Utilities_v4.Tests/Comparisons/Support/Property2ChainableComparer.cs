using Vertica.Utilities_v4.Comparisons;

namespace Vertica.Utilities_v4.Tests.Comparisons.Support
{
	internal class Property2ChainableComparer : ChainableComparer<ComparisonSubject>
	{
		public Property2ChainableComparer() { }
		public Property2ChainableComparer(Direction sortDirection) : base(sortDirection) { }

		public override int DoCompare(ComparisonSubject x, ComparisonSubject y)
		{
			return x.Property2.CompareTo(y.Property2);
		}
	}
}