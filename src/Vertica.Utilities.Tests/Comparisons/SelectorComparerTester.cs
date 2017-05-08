using System;
using System.Collections.Generic;
using NUnit.Framework;
using Vertica.Utilities_v4.Comparisons;
using Vertica.Utilities_v4.Tests.Comparisons.Support;

namespace Vertica.Utilities_v4.Tests.Comparisons
{
	[TestFixture]
	public class SelectorComparerTester
	{
		[Test, Category("Exploratory")]
		public void Explore()
		{
			IComparer<ComparisonSubject> subject = new SelectorComparer<ComparisonSubject, int>(s => s.Property2, Direction.Descending);
			Assert.That(subject.Compare(ComparisonSubject.One, ComparisonSubject.Two), Is.GreaterThan(0));
			
			IComparer<ComparisonSubject> by3Then2Desc = new SelectorComparer<ComparisonSubject, decimal>(s => s.Property3)
				.Then(s => s.Property2, Direction.Descending);
			by3Then2Desc = Cmp<ComparisonSubject>.By(s => s.Property3)
				.Then(s => s.Property2, Direction.Descending);
		}

		[Test]
		public void Ctor_DefaultsToAscending()
		{
			var subject = new SelectorComparer<ComparisonSubject, int>(s => s.Property2);
			Assert.That(subject.SortDirection, Is.EqualTo(Direction.Ascending));
		}

		[Test]
		public void Ctor_SetsDirection()
		{
			var subject = new SelectorComparer<ComparisonSubject, int>(s => s.Property2, Direction.Descending);
			Assert.That(subject.SortDirection, Is.EqualTo(Direction.Descending));
		}

		[Test]
		public void Compare_ComparedTheSelectedProperty_HonoringDirection()
		{
			var subject = new SelectorComparer<ComparisonSubject, int>(s => s.Property2, Direction.Ascending);
			Assert.That(subject.Compare(ComparisonSubject.One, ComparisonSubject.Two), Is.LessThan(0));

			subject = new SelectorComparer<ComparisonSubject, int>(s => s.Property2, Direction.Descending);
			Assert.That(subject.Compare(ComparisonSubject.One, ComparisonSubject.Two), Is.GreaterThan(0));
		}

		[Test]
		public void Comparison_HonorsDirection()
		{
			Comparison<ComparisonSubject> comparison = new SelectorComparer<ComparisonSubject, int>(
				s => s.Property2, Direction.Ascending).Comparison;

			Assert.That(comparison(ComparisonSubject.One, ComparisonSubject.Two), Is.LessThan(0));

			comparison = new SelectorComparer<ComparisonSubject, int>(
				s => s.Property2, Direction.Descending).Comparison;

			Assert.That(comparison(ComparisonSubject.One, ComparisonSubject.Two), Is.GreaterThan(0));
		}

		[Test]
		public void Clients_DoNotHaveToCareAboutNulls()
		{
			var notNull = new ComparisonSubject("a", 1, 1m);
			var chainable = new SelectorComparer<ComparisonSubject, int>(s => s.Property2);

			Assert.That(chainable.Compare(notNull, null), Is.GreaterThan(0));
			Assert.That(chainable.Compare(null, notNull), Is.LessThan(0));
			Assert.That(chainable.Compare(null, null), Is.EqualTo(0));
		}
	}
}
