using NUnit.Framework;
using Testing.Commons.Time;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		#region Contains(item)

		[Test, Category("Contains(item)")]
		public void Contains_WellContained_True([ValueSource("oneToFives")] Range<int> oneToFive)
		{
			Assert.That(oneToFive.Contains(3), Is.True);
		}

		[Test, Category("Contains(item)")]
		public void Contains_NotContained_False([ValueSource("oneToFives")] Range<int> oneToFive)
		{
			Assert.That(oneToFive.Contains(6), Is.False);
		}

		[Test, Category("Contains(item)")]
		public void Contains_CanBeUsedWithDates()
		{
			var ww2Period = Range.Closed(3.September(1939), 2.September(1945));
			Assert.That(ww2Period.Contains(1.January(1940)), Is.True);
			Assert.That(ww2Period.Contains(1.January(1980)), Is.False);
			Assert.That(ww2Period.Contains(2.September(1939).At(12, 59, 59, 999)), Is.False);
		}

		#region lower bound item

		[Test, Category("Contains(item)")]
		public void Contains_Closed_LowerBound_True()
		{
			var subject = Range.Closed(1, 5);

			Assert.That(subject.Contains(1), Is.True);
		}

		[Test, Category("Contains(item)")]
		public void Contains_Open_LowerBound_False()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(1), Is.False);
		}

		[Test, Category("Contains(item)")]
		public void Contains_HalfOpen_LowerBound_True()
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(subject.Contains(1), Is.True);
		}

		[Test, Category("Contains(item)")]
		public void Contains_HalfClosed_LowerBound_False()
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(subject.Contains(1), Is.False);
		}

		#endregion

		#region upper bound item

		[Test, Category("Contains(item)")]
		public void Contains_Closed_UpperBound_True()
		{
			var subject = Range.Closed(1, 5);

			Assert.That(subject.Contains(5), Is.True);
		}

		[Test, Category("Contains(item)")]
		public void Contains_Open_UpperBound_False()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(5), Is.False);
		}

		[Test, Category("Contains(item)")]
		public void Contains_HalfOpen_UpperBound_False()
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(subject.Contains(5), Is.False);
		}

		[Test, Category("Contains(item)")]
		public void Contains_HalfClosed_UpperBound_True()
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(subject.Contains(5), Is.True);
		}

		#endregion

		#endregion

		#region Contains(range)

		[Test, Category("Contains(range)"), Combinatorial]
		public void Contains_WellContainedRange_True(
			[ValueSource("oneToFives")]
			Range<int> oneToFive,
			[ValueSource("twoToThrees")]
			Range<int> twoToThree)
		{
			Assert.That(oneToFive.Contains(twoToThree), Is.True);
		}

		#region lower-touching

		[Test, Category("Contains(range)")]
		public void Contains_Closed_LowerTouchingRange_True(
			[ValueSource("oneToFours")]
			Range<int> oneToFour)
		{
			var subject = Range.Closed(1, 5);

			Assert.That(subject.Contains(oneToFour), Is.True, @"
{1, 2, 3, 4, 5} ⊂ {1, 2, 3, 4}
{1, 2, 3, 4, 5} ⊂ {2, 3}
{1, 2, 3, 4, 5} ⊂ {1, 2, 3}
{1, 2, 3, 4, 5} ⊂ {2, 3, 4}
");
		}

		#region open subject

		[Test, Category("Contains(range)")]
		public void Contains_Open_ClosedLowerTouchingRange_False()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(Range.Closed(1, 4)), Is.False, "{2, 3, 4} ⊄ {1, 2, 3, 4}");
		}

		[Test, Category("Contains(range)")]
		public void Contains_Open_OpenLowerTouchingRange_True()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(Range.Open(1, 4)), Is.True, "{2, 3, 4} ⊂ {2, 3}");
		}

		[Test, Category("Contains(range)")]
		public void Contains_Open_HalfOpenLowerTouchingRange_False()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(Range.HalfOpen(1, 4)), Is.False, "{2, 3, 4} ⊄ {1, 2, 3}");
		}

		[Test, Category("Contains(range)")]
		public void Contains_Open_HalfClosedLowerTouchingRange_True()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(Range.HalfClosed(1, 4)), Is.True, "{2, 3, 4} ⊂ {2, 3, 4}");
		}

		#endregion

		[Test, Category("Contains(range)")]
		public void Contains_HalfOpen_LowerTouchingRange_True(
			[ValueSource("oneToFours")]
			Range<int> oneToFour
			)
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(subject.Contains(oneToFour), Is.True, @"
{1, 2, 3, 4} ⊂ {1, 2, 3, 4}
{1, 2, 3, 4} ⊂ {2, 3}
{1, 2, 3, 4} ⊂ {1, 2, 3}
{1, 2, 3, 4} ⊂ {2, 3, 4}
");
		}

		#region half-closed subject

		[Test, Category("Contains(range)")]
		public void Contains_HalfClosed_ClosedLowerTouchingRange_False()
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(subject.Contains(Range.Closed(1, 4)), Is.False, "{2, 3, 4, 5} ⊄ {1, 2, 3, 4}");
		}

		[Test, Category("Contains(range)")]
		public void Contains_HalfClosed_OpenLowerTouchingRange_True()
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(subject.Contains(Range.Open(1, 4)), Is.True, "{2, 3, 4, 5} ⊂ {2, 3}");
		}

		[Test, Category("Contains(range)")]
		public void Contains_HalfClosed_HalfOpenLowerTouchingRange_False()
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(subject.Contains(Range.HalfOpen(1, 4)), Is.False, "{2, 3, 4, 5} ⊄ {1, 2, 3}");
		}

		[Test, Category("Contains(range)")]
		public void Contains_HalfClosed_HalfClosedLowerTouchingRange_True()
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(subject.Contains(Range.HalfClosed(1, 4)), Is.True, "{2, 3, 4, 5} ⊂ {2, 3, 4}");
		}

		#endregion
		
		#endregion

		#region upper-touching

		[Test, Category("Contains(range)")]
		public void Contains_Closed_UpperTouchingRange_True(
			[ValueSource("twoToFives")]
			Range<int> twoToFive)
		{
			var subject = Range.Closed(1, 5);

			Assert.That(subject.Contains(twoToFive), Is.True, @"
{1, 2, 3, 4, 5} ⊂ {2, 3, 4, 5}
{1, 2, 3, 4, 5} ⊂ {3, 4}
{1, 2, 3, 4, 5} ⊂ {2, 3, 4}
{1, 2, 3, 4, 5} ⊂ {3, 4, 5}
");
		}

		#region open subject

		[Test, Category("Contains(range)")]
		public void Contains_Open_ClosedUpperTouchingRange_False()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(Range.Closed(2, 5)), Is.False, "{2, 3, 4} ⊄ {2, 3, 4, 5}");
		}

		[Test, Category("Contains(range)")]
		public void Contains_Open_OpenUpperTouchingRange_True()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(Range.Open(2, 5)), Is.True, "{2, 3, 4} ⊂ {3, 4}");
		}

		[Test, Category("Contains(range)")]
		public void Contains_Open_HalfOpenUpperTouchingRange_True()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(Range.HalfOpen(2, 5)), Is.True, "{2, 3, 4} ⊂ {2, 3, 4}");
		}

		[Test, Category("Contains(range)")]
		public void Contains_Open_HalfClosedUpperTouchingRange_False()
		{
			var subject = Range.Open(1, 5);

			Assert.That(subject.Contains(Range.HalfClosed(2, 5)), Is.False, "{2, 3, 4} ⊄ {3, 4, 5}");
		}

		#endregion

		#region half-open subject

		[Test, Category("Contains(range)")]
		public void Contains_HalfOpen_ClosedUpperTouchingRange_False()
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(subject.Contains(Range.Closed(2, 5)), Is.False, "{1, 2, 3, 4} ⊄ {2, 3, 4, 5}");
		}

		[Test, Category("Contains(range)")]
		public void Contains_HalfOpen_OpenUpperTouchingRange_True()
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(subject.Contains(Range.Open(2, 5)), Is.True, "{1, 2, 3, 4} ⊂ {3, 4}");
		}

		[Test, Category("Contains(range)")]
		public void Contains_HalfOpen_HalfOpenUpperTouchingRange_True()
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(subject.Contains(Range.HalfOpen(2, 5)), Is.True, "{1, 2, 3, 4} ⊂ {2, 3, 4}");
		}

		[Test, Category("Contains(range)")]
		public void Contains_HalfOpen_HalfClosedUpperTouchingRange_False()
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(subject.Contains(Range.HalfClosed(2, 5)), Is.False, "{1, 2, 3, 4} ⊄ {3, 4, 5}");
		}

		#endregion

		[Test, Category("Contains(range)")]
		public void Contains_HalfClosed_UpperTouchingRange_True(
			[ValueSource("twoToFives")]
			Range<int> twoToFive
			)
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(subject.Contains(twoToFive), Is.True, @"
{2, 3, 4, 5} ⊂ {2, 3, 4, 5}
{2, 3, 4, 5} ⊂ {3, 4}
{2, 3, 4, 5} ⊂ {2, 3, 4}
{2, 3, 4, 5} ⊂ {3, 4, 5}
");
		}

		#endregion

		#region not contained

		[Test, Category("Contains(range)"), Combinatorial]
		public void Contains_Disjoint_False(
			[ValueSource("oneToFives")]
			Range<int> oneToFive,
			[ValueSource("minusTwoToMinusOnes"), ValueSource("sevenToEights")]
			Range<int> disjoint)
		{
			Assert.That(oneToFive.Contains(disjoint), Is.False);
		}

		[Test, Category("Contains(range)"), Combinatorial]
		public void Contains_Intersecting_False(
			[ValueSource("oneToFives")]
			Range<int> oneToFive,
			[ValueSource("minusTwoToTwos"), ValueSource("threeToNines")]
			Range<int> intersecting)
		{
			Assert.That(oneToFive.Contains(intersecting), Is.False);
		}

		#endregion

		#endregion
	}
}
