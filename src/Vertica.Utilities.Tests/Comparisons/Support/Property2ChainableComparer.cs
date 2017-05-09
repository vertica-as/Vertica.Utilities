using Vertica.Utilities.Comparisons;

namespace Vertica.Utilities.Tests.Comparisons.Support
{
	internal class Property2ChainableComparer : ChainableComparer<ComparisonSubject>
	{
		public Property2ChainableComparer() { }
		public Property2ChainableComparer(Direction sortDirection) : base(sortDirection) { }

		protected override int DoCompare(ComparisonSubject x, ComparisonSubject y)
		{
			return x.Property2.CompareTo(y.Property2);
		}
	}
}