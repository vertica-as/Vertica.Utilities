using System;
using System.Collections.Generic;
using NUnit.Framework;
using Vertica.Utilities_v4.Comparisons;
using Vertica.Utilities_v4.Tests.Comparisons.Support;

namespace Vertica.Utilities_v4.Tests.Comparisons
{
	[TestFixture]
	public class ReversedComparerTester
	{
		#region documentation

		[Test, Category("Exploratory")]
		public void Explore()
		{
			string a = "a", b = "b";

			IComparer<string> normalComparison = StringComparer.OrdinalIgnoreCase;
			Assert.That(a, Is.LessThan(b).Using(normalComparison));

			IComparer<string> reversedComparer = new ReversedComparer<string>(normalComparison);
			Assert.That(a, Is.GreaterThan(b).Using(reversedComparer));
		}

		#endregion

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