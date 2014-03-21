using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		[Test]
		public void Intersect_Null_Empty()
		{
			Range<int> notEmpty = Range.New(1, 10);

			var intersection = notEmpty.Intersect(null);
			Assert.That(intersection, Is.SameAs(Range.Empty<int>()));
		}

		[Test]
		public void Intersect_NotEmptyToEmpty_Empty()
		{
			Range<int> notEmpty = Range.New(1, 10);

			var intersection = notEmpty.Intersect(Range.Empty<int>());
			Assert.That(intersection, Is.SameAs(Range.Empty<int>()));
		}

		[Test]
		public void Intersect_EmptyToNull_Empty()
		{
			var intersection = Range<byte>.Empty.Intersect(null);
			Assert.That(intersection, Is.SameAs(Range.Empty<byte>()));
		}

		[Test]
		public void Intersect_EmptyToNotEmpty_Empty()
		{
			Range<int> notEmpty = Range.New(1, 10);
			var intersection = Range<int>.Empty.Intersect(notEmpty);
			Assert.That(intersection, Is.SameAs(Range.Empty<int>()));
		}

		[Test]
		public void Intersect_EmptyToEmpty_Empty()
		{
			var intersection = Range<byte>.Empty.Intersect(Range.Empty<byte>());
			Assert.That(intersection, Is.SameAs(Range.Empty<byte>()));
		}

		[Test]
		public void Intersect_WellDisjoint_Empty()
		{
			Range<int> oneToTen = Range.New(1, 10),
				fiftytoHundred = Range.New(50, 100);

			var intersection = oneToTen.Intersect(fiftytoHundred);
			Assert.That(intersection, Is.SameAs(Range.Empty<int>()));
		}

		[Test]
		public void Intersect_LowerBoundNotTouchedByUpperBound_Empty()
		{
			Range<int> open = Range.Open(5, 10);
			Range<int> closed = Range.Closed(1, 5);

			var intersection = open.Intersect(closed);
			Assert.That(intersection, Is.SameAs(Range.Empty<int>()));
		}

		[Test]
		public void Intersect_UpperBoundNotTouchedByLowerBound_Empty()
		{
			Range<int> closed = Range.HalfOpen(5, 10);
			Range<int> halfClosed = Range.HalfClosed(10, 15);

			var intersection = closed.Intersect(halfClosed);
			Assert.That(intersection, Is.SameAs(Range.Empty<int>()));
		}

		[Test]
		public void Intersect_LowerBoundTouchingUpperBound_ClosedSingleLower()
		{
			Range<int> halfOpen = Range.HalfOpen(5, 10);
			Range<int> halfClosed = Range.HalfClosed(1, 5);

			var intersection = halfOpen.Intersect(halfClosed);

			Assert.That(intersection.LowerBound, Is.EqualTo(5));
			Assert.That(intersection.UpperBound, Is.EqualTo(5));
			Assert.That(intersection.Contains(5), Is.True);
		}

		[Test]
		public void Intersect_UpperBoundTouchingLowerBound_ClosedSingleUpper()
		{
			Range<int> halfClosed = Range.HalfClosed(5, 10);
			Range<int> halfOpen = Range.HalfOpen(10, 15);

			var intersection = halfClosed.Intersect(halfOpen);

			Assert.That(intersection.LowerBound, Is.EqualTo(10));
			Assert.That(intersection.UpperBound, Is.EqualTo(10));
			Assert.That(intersection.Contains(10), Is.True);
		}
	}
}