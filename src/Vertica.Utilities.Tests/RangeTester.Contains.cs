using NUnit.Framework;
using Testing.Commons.Time;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		[Test]
		public void Contains_WellContained_True([ValueSource("oneToFives")] Range<int> oneToFive)
		{
			Assert.That(oneToFive.Contains(3), Is.True);
		}

		[Test]
		public void Contains_NotContained_False([ValueSource("oneToFives")] Range<int> oneToFive)
		{
			Assert.That(oneToFive.Contains(6), Is.False);
		}

		[Test]
		public void Contains_CanBeUsedWithDates()
		{
			var ww2Period = Range.Closed(3.September(1939), 2.September(1945));
			Assert.That(ww2Period.Contains(1.January(1940)), Is.True);
			Assert.That(ww2Period.Contains(1.January(1980)), Is.False);
			Assert.That(ww2Period.Contains(2.September(1939).At(12, 59, 59, 999)), Is.False);
		}

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
	}
}
