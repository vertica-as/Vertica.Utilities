using System;
using NUnit.Framework;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		[Test, Sequential]
		public void Equals_SameBounds_True(
			[ValueSource("oneToFives")]
			Range<int> oneToFive,
			[ValueSource("oneToFives")]
			Range<int> anotherOneToFive)
		{
			Assert.That(oneToFive.Equals(anotherOneToFive), Is.True);
			Assert.That(anotherOneToFive.Equals(oneToFive), Is.True);
		}

		[Test]
		public void Equals_SameBounds_True()
		{
			var s1 = new Range<DateTime>(DateTime.MinValue, DateTime.MaxValue);
			var s2 = new Range<DateTime>(DateTime.MinValue, DateTime.MaxValue);

			Assert.That(s1.Equals(s2), Is.True);
			Assert.That(s2.Equals(s1), Is.True);
		}

		[Test, Sequential]
		public void Equals_DifferentBounds_False(
			[ValueSource("oneToFives")]
			Range<int> oneToFive,
			[ValueSource("oneToThrees")]
			Range<int> oneToThree)
		{
			Assert.That(oneToFive.Equals(oneToThree), Is.False);
			Assert.That(oneToThree.Equals(oneToFive), Is.False);
		}

		[Test]
		public void Equals_DifferentTypeOfBounds_False()
		{
			Assert.That(Range.Closed(1, 5).Equals(Range.Open(1, 5)), Is.False);
			Assert.That(Range.Closed(1, 5).Equals(Range.HalfOpen(1, 5)), Is.False);
			Assert.That(Range.Closed(1, 5).Equals(Range.HalfClosed(1, 5)), Is.False);
		}

		[Test]
		public void Equals_DifferentBounds_False()
		{
			var s1 = new Range<DateTime>(DateTime.MinValue, DateTime.MaxValue);
			var s2 = new Range<DateTime>(DateTime.MinValue, DateTime.MaxValue.AddYears(-1));

			Assert.That(s1.Equals(s2), Is.False);
			Assert.That(s2.Equals(s1), Is.False);
		}

		[Test]
		public void Equals_Null_False(
			[ValueSource("oneToFives")]
			Range<int> oneToFive)
		{
			Range<int> @null = null;

			Assert.That(oneToFive.Equals(@null), Is.False);
		}

		[Test]
		public void Equals_SameBoundsButDifferentType_False(
			[ValueSource("oneToFives")]
			Range<int> oneToFive)
		{
			var anotherOnetoFive = new Range<long>(1, 5);

			Assert.That(oneToFive.Equals(anotherOnetoFive), Is.False);
		}
	}
}
