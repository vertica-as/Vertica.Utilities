using System;
using NUnit.Framework;
using Testing.Commons.Time;
using Vertica.Utilities_v4.Extensions.RangeExt;

namespace Vertica.Utilities_v4.Tests.Extensions
{
	[TestFixture]
	public class RangeExtensionsTester
	{
		#region To

		[Test]
		public void To_CongruentBounds_CreatesClosedRange()
		{
			Range<int> congruent = 3.To(5);
			Assert.That(congruent, Is.EqualTo(Range.Closed(3, 5)));
		}

		[Test]
		public void To_IncongruentBounds_CreatesEmptyRange()
		{
			Range<string> incongruent = "s".To("a");
			Assert.That(incongruent, Is.SameAs(Range.Empty<string>()));
		}

		[Test, Category("Exploratory")]
		public void To_ReadsWellWithDates()
		{
			Assert.That(3.September(1939).To(2.September(1945)).Generate(d => d.AddYears(1)),
				Is.EquivalentTo(new[]
				{
					3.September(1939),
					3.September(1940),
					3.September(1941),
					3.September(1942),
					3.September(1943),
					3.September(1944)
				}));
		}

		#endregion

		#region Limit

		[Test]
		public void LimitLower_ConstrainsValueToRangesLowerBound([ValueSource(typeof (RangeTester), "oneToFives")] Range<int> oneToFive)
		{
			Assert.That(3.LimitLower(oneToFive), Is.EqualTo(3));
			Assert.That(0.LimitLower(oneToFive), Is.EqualTo(1));
			Assert.That(6.LimitLower(oneToFive), Is.EqualTo(6));
		}

		[Test]
		public void LimitUpper_ConstrainsValueToRangesUpperBound([ValueSource(typeof (RangeTester), "oneToFives")] Range<int> oneToFive)
		{
			Assert.That(3.LimitUpper(oneToFive), Is.EqualTo(3));
			Assert.That(0.LimitUpper(oneToFive), Is.EqualTo(0));
			Assert.That(6.LimitUpper(oneToFive), Is.EqualTo(5));
		}

		[Test]
		public void Limit_ConstrainsValueWithinRangesBounds([ValueSource(typeof (RangeTester), "oneToFives")] Range<int> oneToFive)
		{
			Assert.That(3.Limit(oneToFive), Is.EqualTo(3));
			Assert.That(0.Limit(oneToFive), Is.EqualTo(1));
			Assert.That(6.Limit(oneToFive), Is.EqualTo(5));
		}

		#endregion

		[Test]
		public void Between_DateRange_ContainedAndNotContained()
		{
			var ww2Period = new Range<DateTime>(3.September(1939), 2.September(1945));

			Assert.That(new DateTime(1940, 1, 1).Within(ww2Period), Is.True);
			Assert.That(new DateTime(1980, 1, 1).Within(ww2Period), Is.False);
			Assert.That(new DateTime(1939, 9, 2).Within(ww2Period), Is.False);
		}
	}
}
