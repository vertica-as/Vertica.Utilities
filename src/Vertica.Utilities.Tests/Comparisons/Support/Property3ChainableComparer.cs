using Vertica.Utilities.Comparisons;

namespace Vertica.Utilities.Tests.Comparisons.Support
{
	internal class Property3ChainableComparer : ChainableComparer<ComparisonSubject>
	{
		public Property3ChainableComparer() { }

		public Property3ChainableComparer(Direction sortDirection) : base(sortDirection) { }

		protected override int DoCompare(ComparisonSubject x, ComparisonSubject y)
		{
			return x.Property3.CompareTo(y.Property3);
		}
	}
}