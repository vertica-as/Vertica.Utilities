using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		[Test]
		public void Join_Null_Self()
		{
			Range<int> range = Range.New(1, 3);

			Range<int> union = range.Join(null);

			Assert.That(union, Is.SameAs(range));
		}

		[Test]
		public void Join_NotEmptyToEmpty_Self()
		{
			Range<int> notEmpty = Range.New(1, 3);

			var union = notEmpty.Join(Range.Empty<int>());
			Assert.That(union, Is.SameAs(notEmpty));
		}

		[Test]
		public void Join_EmptyToNotEmpty_NotEmpty()
		{
			Range<int> notEmpty = Range.New(1, 3);

			var union = Range.Empty<int>().Join(notEmpty);

			Assert.That(union, Is.SameAs(notEmpty));
		}

		[Test]
		public void Join_EmptyToNull_Empty()
		{
			var union = Range.Empty<int>().Join(null);

			Assert.That(union, Is.SameAs(Range.Empty<int>()));
		}

		[Test]
		public void Join_EmptyToEmpty_Empty()
		{
			var union = Range.Empty<byte>().Join(Range<byte>.Empty);

			Assert.That(union, Is.SameAs(Range.Empty<byte>()));
		}

		[Test]
		public void Join_Disjointed_BigRange()
		{
			Range<int> oneToThree = Range.New(1, 3),
				nineToTen = Range.New(9, 10);

			Range<int> oneToSeven = oneToThree.Join(nineToTen);

			Assert.That(oneToSeven.LowerBound, Is.EqualTo(1));
			Assert.That(oneToSeven.UpperBound, Is.EqualTo(10));
		}

		[Test]
		public void Join_Intersecting_BigRange()
		{
			Range<int> oneToThree = Range.New(1, 3),
				twoToFive = Range.New(2, 5);

			Range<int> oneToFive = oneToThree.Join(twoToFive);

			Assert.That(oneToFive.LowerBound, Is.EqualTo(1));
			Assert.That(oneToFive.UpperBound, Is.EqualTo(5));
		}

		[Test]
		public void Join_Same_SameRange()
		{
			Range<int> oneToThree = Range.New(1, 3),
				anotherOneToThree = Range.New(1, 3);

			Range<int> oneToFive = oneToThree.Join(anotherOneToThree);

			Assert.That(oneToFive.LowerBound, Is.EqualTo(1));
			Assert.That(oneToFive.UpperBound, Is.EqualTo(3));
		}

		[Test]
		public void Join_LowerBound_IsSameBoundTypeAsMin()
		{
			Range<int> oneToThree = Range.Closed(1, 3),
				twoToFive = Range.Open(2, 5);

			var oneToFive = oneToThree.Join(twoToFive);
			Assert.That(oneToFive.Contains(1), Is.True);
		}

		[Test]
		public void Join_UpperBound_IsSameBoundTypeAsMax()
		{
			Range<int> oneToThree = Range.Closed(1, 3),
				twoToFive = Range.Open(2, 5);

			var oneToFive = oneToThree.Join(twoToFive);
			Assert.That(oneToFive.Contains(5), Is.False);
		}

		[Test]
		public void Join_LowerBoundTypeMissmatch_Closed()
		{
			Range<int> oneToThree = Range.HalfOpen(1, 3),
				oneToFive = Range.HalfClosed(1, 5);

			var union = oneToThree.Join(oneToFive);

			Assert.That(union.Contains(1), Is.True);
		}

		[Test]
		public void Join_UpperBoundTypeMissmatch_Closed()
		{
			Range<int> oneToThree = Range.HalfOpen(1, 3),
				oneToFive = Range.HalfClosed(1, 5);

			var union = oneToThree.Join(oneToFive);

			Assert.That(union.Contains(5), Is.True);
		}
	}
}