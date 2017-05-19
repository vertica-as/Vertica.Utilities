using NUnit.Framework;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		[Test]
		public void Overlaps_NullOrEmpty_False()
		{
			var range = Range.New(1, 2);

			Assert.That(range.Overlaps(null), Is.False);
			Assert.That(range.Overlaps(Range.Empty<int>()), Is.False);
		}

		[Test]
		public void Overlaps_EmptyWithAnything_False()
		{
			var notEmpty = Range.New<byte>(1, 2);

			Assert.That(Range<byte>.Empty.Overlaps(null), Is.False);
			Assert.That(Range<byte>.Empty.Overlaps(Range.Empty<byte>()), Is.False);
			Assert.That(Range<byte>.Empty.Overlaps(notEmpty), Is.False);
		}

		[Test]
		public void Overlaps_WellDisjoint_False()
		{
			Range<int> oneToTen = Range.New(1, 10),
				fiftytoHundred = Range.New(50, 100);

			var overlapping = oneToTen.Overlaps(fiftytoHundred);
			Assert.That(overlapping, Is.False);
		}

		[Test]
		public void Overlaps_LowerBoundNotTouchedByUpperBound_False()
		{
			Range<int> open = Range.Open(5, 10);
			Range<int> closed = Range.Closed(1, 5);

			var overlapping = open.Overlaps(closed);
			Assert.That(overlapping, Is.False);
		}

		[Test]
		public void Overlaps_UpperBoundNotTouchedByLowerBound_False()
		{
			Range<int> closed = Range.HalfOpen(5, 10);
			Range<int> halfClosed = Range.HalfClosed(10, 15);

			var overlapping = closed.Overlaps(halfClosed);
			Assert.That(overlapping, Is.False);
		}

		[Test]
		public void Overlaps_LowerBoundTouchingUpperBound_True()
		{
			Range<int> halfOpen = Range.HalfOpen(5, 10);
			Range<int> halfClosed = Range.HalfClosed(1, 5);

			var overlapping = halfOpen.Overlaps(halfClosed);

			Assert.That(overlapping, Is.True);
		}

		[Test]
		public void Overlaps_UpperBoundTouchingLowerBound_True()
		{
			Range<int> halfClosed = Range.HalfClosed(5, 10);
			Range<int> halfOpen = Range.HalfOpen(10, 15);

			var overlapping = halfClosed.Overlaps(halfOpen);

			Assert.That(overlapping, Is.True);
		}


		[Test]
		public void Overlaps_WellContained_True()
		{
			Range<int> container = Range.New(1, 10),
				contained = Range.New(3, 6);

			var overlapping = container.Overlaps(contained);

			Assert.That(overlapping, Is.True);
		}

		[Test]
		public void Overlaps_CleanIntersection_True()
		{
			Range<int> left = Range.Closed(1, 5),
				right = Range.Closed(3, 8);

			var overlapping = left.Overlaps(right);

			Assert.That(overlapping, Is.True);
		}
	}
}