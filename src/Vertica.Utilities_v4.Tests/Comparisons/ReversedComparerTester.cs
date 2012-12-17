using System.Collections.Generic;
using NUnit.Framework;
using Vertica.Utilities_v4.Comparisons;
using Vertica.Utilities_v4.Tests.Comparisons.Support;

namespace Vertica.Utilities_v4.Tests.Comparisons
{
	[TestFixture]
	public class ReversedComparerTester
	{
		private static readonly IComparer<ComparisonSubject> _toBeReversed =
			   new ComparisonComparer<ComparisonSubject>((x, y) => x.Property2.CompareTo(y.Property2));

		[Test]
		public void Ctor_DefaultsToAscending()
		{
			var subject = new ReversedComparer<ComparisonSubject>(_toBeReversed);
			Assert.That(subject.SortDirection, Is.EqualTo(Direction.Ascending));
		}

		[Test]
		public void Ctor_SetsDirection()
		{
			var subject = new ReversedComparer<ComparisonSubject>(_toBeReversed, Direction.Descending);
			Assert.That(subject.SortDirection, Is.EqualTo(Direction.Descending));
		}

		[Test]
		public void Compare_ComparedTheSelectedProperty_HonoringDirection()
		{
			var subject = new ReversedComparer<ComparisonSubject>(
				_toBeReversed, Direction.Ascending);
			Assert.That(subject.Compare(ComparisonSubject.One, ComparisonSubject.Two), Is.GreaterThan(0));

			subject = new ReversedComparer<ComparisonSubject>(_toBeReversed, Direction.Descending);
			Assert.That(subject.Compare(ComparisonSubject.One, ComparisonSubject.Two), Is.LessThan(0));
		}
	}
}