using System;
using NUnit.Framework;
using Testing.Commons.Time;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		#region well contained item

		[Test]
		public void Contains_Closed_WellContained_True()
		{
			var subject = Range.Closed(1, 5);

			Assert.That(subject.Contains(3), Is.True);
		}

		[Test]
		public void Contains_Open_WellContained_True()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(3), Is.True);
		}

		[Test]
		public void Contains_HalfOpen_WellContained_True()
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(subject.Contains(3), Is.True);
		}

		[Test]
		public void Contains_HalfClosed_WellContained_True()
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(subject.Contains(3), Is.True);
		}

		#endregion

		#region lower bound item

		[Test]
		public void Contains_Closed_LowerBound_True()
		{
			var subject = Range.Closed(1, 5);

			Assert.That(subject.Contains(1), Is.True);
		}

		[Test]
		public void Contains_Open_LowerBound_False()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(1), Is.False);
		}

		[Test]
		public void Contains_HalfOpen_LowerBound_True()
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(subject.Contains(1), Is.True);
		}

		[Test]
		public void Contains_HalfClosed_LowerBound_False()
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(subject.Contains(1), Is.False);
		}

		#endregion

		#region upper bound item

		[Test]
		public void Contains_Closed_UpperBound_True()
		{
			var subject = Range.Closed(1, 5);

			Assert.That(subject.Contains(5), Is.True);
		}

		[Test]
		public void Contains_Open_UpperBound_False()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(5), Is.False);
		}

		[Test]
		public void Contains_HalfOpen_UpperBound_False()
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(subject.Contains(5), Is.False);
		}

		[Test]
		public void Contains_HalfClosed_UpperBound_True()
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(subject.Contains(5), Is.True);
		}

		#endregion

		#region not contained item

		[Test]
		public void Contains_Closed_NotContained_False()
		{
			var subject = Range.Closed(1, 5);

			Assert.That(subject.Contains(6), Is.False);
		}

		[Test]
		public void Contains_Open_NotContained_False()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(6), Is.False);
		}

		[Test]
		public void Contains_HalfOpen_NotContained_False()
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(subject.Contains(6), Is.False);
		}

		[Test]
		public void Contains_HalfClosed_NotContained_False()
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(subject.Contains(6), Is.False);
		}

		#endregion

		[Test]
		public void Contains_Dates_ContainedAndNotContained()
		{
			var ww2Period = Range.Closed(3.September(1939), 2.September(1945));
			Assert.That(ww2Period.Contains(1.January(1940)), Is.True);
			Assert.That(ww2Period.Contains(1.January(1980)), Is.False);
			Assert.That(ww2Period.Contains(2.September(1939).At(12, 59, 59, 999)), Is.False);
		}

		[TestCase(10, 20)]
		[TestCase(15, 16)]
		[TestCase(10, 10)]
		[TestCase(20, 20)]
		public void Contains_ContainedRanges_True(int lower, int upper)
		{
			Assert.Inconclusive("break in well-contained,...");

			var subject = new Range<int>(10, 20);

			Assert.That(subject.Contains(new Range<int>(lower, upper)));
		}

		[TestCase(9, 11)]
		[TestCase(19, 21)]
		[TestCase(9, 21)]
		public void Contains_NonContainedRanges_False(int lower, int upper)
		{
			Assert.Inconclusive("break in well-contained,...");
			var subject = new Range<int>(10, 20);

			Assert.That(subject.Contains(null), Is.False);
			Assert.That(subject.Contains(new Range<int>(lower, upper)), Is.False);
		}
	}
}
