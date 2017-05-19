using System;
using System.Collections.Generic;
using NUnit.Framework;
using Vertica.Utilities.Tests.Comparisons.Support;
using Vertica.Utilities.Comparisons;

namespace Vertica.Utilities.Tests.Comparisons
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

		[Test]
		public void Clients_DoNotHaveToCareAboutNulls()
		{
			var notNull = new ComparisonSubject("a", 1, 1m);
			var chainable = new ReversedComparer<ComparisonSubject>(new Property2Comparer());

			Assert.That(chainable.Compare(notNull, null), Is.GreaterThan(0));
			Assert.That(chainable.Compare(null, notNull), Is.LessThan(0));
			Assert.That(chainable.Compare(null, null), Is.EqualTo(0));
		}
	}
}