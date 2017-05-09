using System.Collections.Generic;
using NUnit.Framework;
using Vertica.Utilities.Tests.Comparisons.Support;
using Vertica.Utilities.Comparisons;

namespace Vertica.Utilities.Tests.Comparisons
{
	[TestFixture]
	public class ChainableEqualizerTester
	{
		private static readonly EqualitySubject _a = new EqualitySubject("A", 4, 7.60m);
		private static readonly EqualitySubject _b = new EqualitySubject("B", 1, 3.00m);
		
		[Test, Category("Exploratory")]
		public void Explore()
		{
			PropertySEqualizer inheritor = new PropertySEqualizer();
			Assert.That(inheritor, Is.InstanceOf<IEqualityComparer<EqualitySubject>>(), "is a proper comparer");

			EqualitySubject one = new EqualitySubject("1", 1, 1m), two = new EqualitySubject("1", 2, 2m);
			Assert.That(inheritor.Equals(one, two), Is.True, "x.S == y.S");
			Assert.That(inheritor.GetHashCode(one), Is.EqualTo(one.S.GetHashCode()));

			ChainableEqualizer<EqualitySubject> chained = inheritor.Then(new PropertyIEqualizer());
			Assert.That(one, Is.Not.EqualTo(two).Using(chained), "x.I != y.I");
		}


		[Test]
		public void SChainedComparer_CompareS()
		{
			var subject = new PropertySEqualizer();
			Assert.That(subject.Equals(_a, new EqualitySubject { S = "A" }), Is.True);
			Assert.That(subject.Equals(_a, _b), Is.False);
		}

		[Test]
		public void Inheritors_DoNotHaveToCareAboutNulls()
		{
			var notNull = new EqualitySubject {S = "A"};

			var chainable = new PropertySEqualizer();
			Assert.That(chainable.Equals(notNull, null), Is.False);
			Assert.That(chainable.Equals(null, notNull), Is.False);
			Assert.That(chainable.Equals(null, null), Is.True);
		}
	}
}
