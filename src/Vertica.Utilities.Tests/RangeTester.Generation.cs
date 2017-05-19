using System;
using System.Linq;
using NUnit.Framework;
using Testing.Commons.Time;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		#region Generate(next)

		[Test]
		public void Generate_ClosedPlusOne_YieldedAllItemsInRange()
		{
			Func<int, int> plusOne = i => i + 1;

			Assert.That(Range.Closed(1, 5).Generate(plusOne), Is.EqualTo(new[] { 1, 2, 3, 4, 5 }));
			Assert.That(Range.Closed(-5, -3).Generate(plusOne), Is.EqualTo(new[] { -5, -4, -3 }));
			Assert.That(Range.Closed(-2, 2).Generate(plusOne), Is.EqualTo(new[] { -2, -1, 0, 1, 2 }));
		}

		[Test]
		public void Generate_OpenPlusTwo_BoundsNotYielded()
		{
			Assert.That(Range.Open(1, 5).Generate(i => i + 2), Is.EqualTo(new[] { 3 }));

			Assert.That(Range.Open(4, 10).Generate(x => x + 2), Is.EqualTo(new[] { 6, 8 }));
		}

		[Test]
		public void Generate_HalfClosedPlusOneMonth_LowerBoundNotYielded()
		{
			DateTime begin = 3.September(1939), threeMonthsLater = begin.AddMonths(3);

			Func<DateTime, DateTime> oneMonthIncrement = d => d.AddMonths(1);

			Assert.That(Range.HalfClosed(begin, threeMonthsLater).Generate(oneMonthIncrement),
				Is.EqualTo(new[]
				{
					3.October(1939),
					3.November(1939),
					3.December(1939)
				}));
		}

		[Test]
		public void Generate_HalfOpenPlusTen_UpperBoundNotYielded()
		{
			Assert.That(Range.HalfOpen(0m, 50m).Generate(d => d + 10), Is.EqualTo(new[] { 0m, 10m, 20m, 30m, 40m }));
		}

		[Test]
		public void Generate_BackwardsGenerator_Exception()
		{
			Func<int, int> minusOne = i => i - 1;
			Assert.That(()=> Range.Closed(-10, -1).Generate(minusOne).ToArray(), Throws.ArgumentException);
		}

		[Test]
		public void Generate_NoOpGenerator_Exception()
		{
			Func<int, int> noOp = i => i;
			Assert.That(() => Range.Closed(-10, -1).Generate(noOp).ToArray(), Throws.ArgumentException);
		}

		#endregion

		#region Generate(value)

		[Test]
		public void Generate_Value_TypeWithSumOperator_ValuesYielded()
		{
			Assert.That(Range.Closed(1, 5).Generate(1), Is.EqualTo(new[] { 1, 2, 3, 4, 5 }));

			Assert.That(Range.HalfOpen(0m, 50m).Generate(10m), Is.EqualTo(new[] { 0m, 10m, 20m, 30m, 40m }));
		}

		[Test]
		public void Generate_Value_TypeWithoutSumOperator_Exception()
		{
			DateTime begin = 3.September(1939), threeMonthsLater = begin.AddMonths(3);
			
			Assert.That(() => Range.HalfClosed(begin, threeMonthsLater).Generate(threeMonthsLater),
				Throws.InstanceOf<InvalidOperationException>()
				.With.Message.Contain("operator Add is not defined"));
		}

		#endregion
	}
}
