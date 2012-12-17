using System;
using System.Collections.Generic;
using NUnit.Framework;
using Testing.Commons;
using Vertica.Utilities_v4.Comparisons;
using Vertica.Utilities_v4.Tests.Comparisons.Support;

namespace Vertica.Utilities_v4.Tests.Comparisons
{
	[TestFixture]
	public class ComparisonComparerTester
	{
		[Test]
		public void Ctor_DefaultsToAscending()
		{
			var subject = new ComparisonComparer<ComparisonSubject>(
				(x, y) => x.Property2.CompareTo(y.Property2));

			Assert.That(subject.SortDirection, Is.EqualTo(Direction.Ascending));
		}

		[Test]
		public void Ctor_SetsDirection()
		{
			var subject = new ComparisonComparer<ComparisonSubject>(
				(x, y) => x.Property2.CompareTo(y.Property2), Direction.Descending);
			Assert.That(subject.SortDirection, Is.EqualTo(Direction.Descending));
		}

		[Test]
		public void Compare_ComparedTheSelectedProperty_HonoringDirection()
		{
			Comparison<ComparisonSubject> compare2 = (x, y) => x.Property2.CompareTo(y.Property2);

			var subject = new ComparisonComparer<ComparisonSubject>(compare2, Direction.Ascending);
			Assert.That(subject.Compare(ComparisonSubject.One, ComparisonSubject.Two), Is.LessThan(0));

			subject = new ComparisonComparer<ComparisonSubject>(compare2, Direction.Descending);
			Assert.That(subject.Compare(ComparisonSubject.One, ComparisonSubject.Two), Is.GreaterThan(0));
		}

		[Test]
		public void Comparison_HonorsDirection()
		{
			Comparison<ComparisonSubject> subject = new ComparisonComparer<ComparisonSubject>(
				(x, y) => x.Property2.CompareTo(y.Property2), Direction.Ascending)
				.Comparison;
			Assert.That(subject(ComparisonSubject.One, ComparisonSubject.Two), Is.EqualTo(-1));

			subject = new ComparisonComparer<ComparisonSubject>(
				(x, y) => x.Property2.CompareTo(y.Property2), Direction.Descending)
				.Comparison;
			Assert.That(subject(ComparisonSubject.One, ComparisonSubject.Two), Is.EqualTo(1));
		}

		private static readonly ComparisonSubject _a = new ComparisonSubject("A", 4, 7.60m);
		private static readonly ComparisonSubject _b = new ComparisonSubject("B", 1, 3.00m);
		private static readonly ComparisonSubject _c = new ComparisonSubject("C", 3, 7.60m);

		[Test]
		public void CanBeChained()
		{
			IComparer<ComparisonSubject> by3Then2Desc = new ComparisonComparer<ComparisonSubject>((x, y) => x.Property3.CompareTo(y.Property3))
				.Then(new ComparisonComparer<ComparisonSubject>((x, y) => x.Property2.CompareTo(y.Property2), Direction.Descending));

			var list = new List<ComparisonSubject> { _b, _c, _a };
			list.Sort(by3Then2Desc);

			Assert.That(list, Must.Be.RepresentableAs("B, A, C"));
		}

		[Test]
		public void Composes_ComparisonDelegates()
		{
			var comparer = new ComparisonComparer<ComparisonSubject>((x, y) => x.Property3.CompareTo(y.Property3));
			comparer.Then(new ComparisonComparer<ComparisonSubject>((x, y) => x.Property2.CompareTo(y.Property2), Direction.Descending));

			var list = new List<ComparisonSubject> { _b, _c, _a };
			list.Sort(comparer.Comparison);

			Assert.That(list, Must.Be.RepresentableAs("B, A, C"));
		}

		[Test]
		public void Adapts_ComparisonDelegates_ToIComparable()
		{
			Comparison<ComparisonSubject> comparison = (x, y) => x.Property2.CompareTo(y.Property2);
			IComparer<ComparisonSubject> comparer = new ComparisonComparer<ComparisonSubject>(comparison);

			Assert.That(comparer.Compare(ComparisonSubject.One, ComparisonSubject.Two), Is.LessThan(0));
			Assert.That(comparison(ComparisonSubject.One, ComparisonSubject.Two), Is.LessThan(0));
		}
	}
}