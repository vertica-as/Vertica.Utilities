using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Testing.Commons;
using Vertica.Utilities_v4.Comparisons;
using Vertica.Utilities_v4.Tests.Comparisons.Support;

namespace Vertica.Utilities_v4.Tests.Comparisons
{
	[TestFixture]
	public class ChainableComparerTester
	{
		#region documentation

		[Test, Category("Exploratory")]
		public void Explore()
		{
			Property2ChainableComparer inheritor = new Property2ChainableComparer();
			Assert.That(inheritor, Is.InstanceOf<IComparer<ComparisonSubject>>(), "is a proper comparer");

			ComparisonSubject x = new ComparisonSubject("x", 1, 1m), y = new ComparisonSubject("y", 1, 2m);
			Assert.That(inheritor.Compare(x, y), Is.EqualTo(0), "x.Property2 == y.Property2");

			ChainableComparer<ComparisonSubject> asc = inheritor.Then(new Property3ChainableComparer());
			Assert.That(x, Is.LessThan(y).Using(asc), "x.Properyty3 < y.Property2");

			ChainableComparer<ComparisonSubject> desc = new Property2ChainableComparer()
				.Then(new Property3ChainableComparer(Direction.Descending));
			Assert.That(x, Is.GreaterThan(y).Using(desc), "x.Properyty3 > y.Property2");

			var chainable = new Property2ChainableComparer();
			Assert.That(chainable.Compare(x, null), Is.GreaterThan(0));
			Assert.That(chainable.Compare(null, y), Is.LessThan(0));
			Assert.That(chainable.Compare(null, null), Is.EqualTo(0));
		}

		#endregion

		#region subjects

		private static readonly ComparisonSubject _a = new ComparisonSubject("A", 4, 7.60m);
		private static readonly ComparisonSubject _b = new ComparisonSubject("B", 1, 3.00m);
		private static readonly ComparisonSubject _c = new ComparisonSubject("C", 3, 7.60m);
		private static readonly ComparisonSubject _d = new ComparisonSubject("D", 2, 4.20m);
		private static readonly ComparisonSubject _e = new ComparisonSubject("E", 5, 6.40m);
		private List<ComparisonSubject> _subjects;

		[SetUp]
		public void initSubjects()
		{
			_subjects = new List<ComparisonSubject> { _e, _d, _b, _c, _a };
		}

		#endregion

		[Test(Description = "Exploratory")]
		public void Exploring_SortWithLinq()
		{
			Assert.That(_subjects
				.OrderBy(s => s.Property3)
				.ThenBy(s => s.Property2),
				Must.Be.RepresentableAs("B, D, E, C, A"));
		}

		[Test(Description = "Exploratory")]
		public void DefaultListSort_ByNameAsc()
		{
			_subjects.Sort();
			Assert.That(_subjects, Must.Be.RepresentableAs("A, B, C, D, E"));
		}

		[Test(Description = "Exploratory")]
		public void Property2Comparer_ByProperty2Asc()
		{
			_subjects.Sort(new Property2Comparer());
			Assert.That(_subjects, Must.Be.RepresentableAs("B, D, C, A, E"));
		}

		[Test]
		public void Property2ChainedComparer_ByProperty2Asc()
		{
			_subjects.Sort(new Property2ChainableComparer());
			Assert.That(_subjects, Must.Be.RepresentableAs("B, D, C, A, E"));

			_subjects.Sort(new Property2ChainableComparer(Direction.Ascending));
			Assert.That(_subjects, Must.Be.RepresentableAs("B, D, C, A, E"));
		}

		[Test]
		public void Property3ChainedComparer_ByProperty3()
		{
			_subjects.Sort(new Property3ChainableComparer());
			Assert.That(_subjects, Must.Be.RepresentableAs("B, D, E, A, C"));
		}

		[Test]
		public void Property3ChainedComparer_ByProperty3Asc()
		{
			_subjects.Sort(new Property3ChainableComparer(Direction.Ascending));
			Assert.That(_subjects, Must.Be.RepresentableAs("B, D, E, A, C"));
		}

		[Test]
		public void Property3ChainedComparer_ByProperty3Desc()
		{
			_subjects.Sort(new Property3ChainableComparer(Direction.Descending));
			Assert.That(_subjects, Must.Be.RepresentableAs("C, A, E, D, B"));
		}

		[Test]
		public void ChainableComparer_By3Then2_Instances()
		{
			var by3Then2Asc = new Property3ChainableComparer()
				.Then(new Property2ChainableComparer(Direction.Ascending));
			_subjects.Sort(by3Then2Asc);
			Assert.That(_subjects, Must.Be.RepresentableAs("B, D, E, C, A"));

			var by3Then2Desc = new Property3ChainableComparer()
				.Then(new Property2ChainableComparer(Direction.Descending));
			_subjects.Sort(by3Then2Desc);
			Assert.That(_subjects, Must.Be.RepresentableAs("B, D, E, A, C"));
		}

		[Test]
		public void ChainableComparer_By3Then2_FactoredWithSelectors()
		{
			_subjects.Sort(Cmp<ComparisonSubject>.By(c => c.Property3)
				.Then(c => c.Property2, Direction.Ascending));
			Assert.That(_subjects, Must.Be.RepresentableAs("B, D, E, C, A"));

			_subjects.Sort(Cmp<ComparisonSubject>.By(c => c.Property3)
				.Then(c => c.Property2, Direction.Descending));
			Assert.That(_subjects, Must.Be.RepresentableAs("B, D, E, A, C"));
		}

		[Test]
		public void ChaineableComparer_CanBeUsedWithLinq()
		{
			Assert.That(_subjects.OrderBy(s => s, Cmp<ComparisonSubject>
					.By(s => s.Property3)
					.Then(s => s.Property2)),
				Must.Be.RepresentableAs("B, D, E, C, A"));

			Assert.That(_subjects.OrderBy(s => s, Cmp<ComparisonSubject>
					.By(s => s.Property3)
					.Then(s => s.Property2, Direction.Descending)),
				Must.Be.RepresentableAs("B, D, E, A, C"));
		}

		[Test]
		public void Inheritors_DoNotHaveToCareAboutNulls()
		{
			var notNull = new ComparisonSubject("a", 1, 1m);
			var comparer = new Property2Comparer();
			Assert.That(()=>comparer.Compare(notNull, null), Throws.InstanceOf<NullReferenceException>());

			var chainable = new Property2ChainableComparer();
			Assert.That(chainable.Compare(notNull, null), Is.GreaterThan(0));
			Assert.That(chainable.Compare(null, notNull), Is.LessThan(0));
			Assert.That(chainable.Compare(null, null), Is.EqualTo(0));
		}
	}
}
