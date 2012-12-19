using System.Collections.Generic;
using NUnit.Framework;
using Vertica.Utilities_v4.Comparisons;
using Vertica.Utilities_v4.Tests.Comparisons.Support;

namespace Vertica.Utilities_v4.Tests.Comparisons
{
	[TestFixture]
	public class ChainableEqualizerTester
	{
		private static readonly EqualitySubject _a = new EqualitySubject("A", 4, 7.60m);
		private static readonly EqualitySubject _b = new EqualitySubject("B", 1, 3.00m);


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
