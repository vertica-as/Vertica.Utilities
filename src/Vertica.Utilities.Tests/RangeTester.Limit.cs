using NUnit.Framework;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		#region LimitLower

		[Test]
		public void LimitLower_WellContained_SameValue(
			[ValueSource("oneToFives")]
			Range<int> oneToFive)
		{
			Assert.That(oneToFive.LimitLower(3), Is.EqualTo(3));
		}

		[Test]
		public void LimitLower_NotContained_LimitAppliedOnlyInLowerEnd(
			[ValueSource("oneToFives")]
			Range<int> oneToFive)
		{
			Assert.That(oneToFive.LimitLower(0), Is.EqualTo(1));
			Assert.That(oneToFive.LimitLower(6), Is.EqualTo(6));
		}

		[Test]
		public void LimitLower_Bounds_SameValueRegardlessOfBoundInclusion(
			[ValueSource("oneToFives")]
			Range<int> oneToFive)
		{
			Assert.That(oneToFive.LimitLower(1), Is.EqualTo(1));
			Assert.That(oneToFive.LimitLower(5), Is.EqualTo(5));
		}

		#endregion

		#region LimitUpper

		[Test]
		public void LimitUpper_WellContained_SameValue(
			[ValueSource("oneToFives")]
			Range<int> oneToFive)
		{
			Assert.That(oneToFive.LimitUpper(3), Is.EqualTo(3));
		}

		[Test]
		public void LimitUpper_NotContained_LimitAppliedOnlyInUpperEnd(
			[ValueSource("oneToFives")]
			Range<int> oneToFive)
		{
			Assert.That(oneToFive.LimitUpper(0), Is.EqualTo(0));
			Assert.That(oneToFive.LimitUpper(6), Is.EqualTo(5));
		}

		[Test]
		public void LimitUpper_Bounds_SameValueRegardlessOfBoundInclusion(
			[ValueSource("oneToFives")]
			Range<int> oneToFive)
		{
			Assert.That(oneToFive.LimitUpper(1), Is.EqualTo(1));
			Assert.That(oneToFive.LimitUpper(5), Is.EqualTo(5));
		}

		#endregion

		#region Limit

		[Test]
		public void Limit_Contained_SameValue(
			[ValueSource("oneToFives")]
			Range<int> oneToFive)
		{
			Assert.That(oneToFive.Limit(3), Is.EqualTo(3));
		}

		[Test]
		public void Limit_NotContained_LimitLowerOrHigher(
			[ValueSource("oneToFives")]
			Range<int> oneToFive)
		{
			Assert.That(oneToFive.Limit(0), Is.EqualTo(1));
			Assert.That(oneToFive.Limit(6), Is.EqualTo(5));
		}

		[Test]
		public void Limit_Bounds_SameValueRegardlessOfBoundInclusion(
			[ValueSource("oneToFives")]
			Range<int> oneToFive)
		{
			Assert.That(oneToFive.Limit(1), Is.EqualTo(1));
			Assert.That(oneToFive.Limit(5), Is.EqualTo(5));
		}

		#endregion
	}
}
