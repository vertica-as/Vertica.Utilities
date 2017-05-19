using System;
using System.Globalization;
using NUnit.Framework;
using Testing.Commons.Globalization;
using Testing.Commons.Time;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		#region Check

		[Test]
		public void CheckBounds_CorrectlyOrderedBounds_True()
		{
			Assert.That(Range<char>.CheckBounds('a', 'z'), Is.True);
			Assert.That(Range<int>.CheckBounds(-1, 1), Is.True);
			Assert.That(Range<DateTime>.CheckBounds(11.March(1977), 9.September(2010)), Is.True);
			Assert.That(Range<TimeSpan>.CheckBounds(3.Seconds(), 1.Hours()), Is.True);
		}

		[Test]
		public void CheckBounds_SameValueForBothBounds_True()
		{
			Assert.That(Range<char>.CheckBounds('a', 'a'), Is.True);
			Assert.That(Range<int>.CheckBounds(-1, -1), Is.True);
			Assert.That(Range<DateTime>.CheckBounds(11.March(1977), 11.March(1977)), Is.True);
			Assert.That(Range<TimeSpan>.CheckBounds(3.Seconds(), 3.Seconds()), Is.True);
		}

		[Test]
		public void CheckBounds_IncorrectlyOrderedBounds_False()
		{
			Assert.That(Range<char>.CheckBounds('z', 'a'), Is.False);
			Assert.That(Range<int>.CheckBounds(1, -1), Is.False);
			Assert.That(Range<DateTime>.CheckBounds(9.September(2010), 11.March(1977)), Is.False);
			Assert.That(Range<TimeSpan>.CheckBounds(1.Hours(), 3.Seconds()), Is.False);
		}

		[Test]
		public void Checkbounds_Inference_NicerSyntax()
		{
			Assert.That(Range.CheckBounds('z', 'a'), Is.False);
			Assert.That(Range.CheckBounds(11.March(1977), 11.March(1977)), Is.True);
		}

		#endregion

		#region Assert

		[Test]
		public void AssertBounds_CorrectlyOrderedBounds_NoException()
		{
			Assert.That(() => Range<char>.AssertBounds('a', 'z'), Throws.Nothing);
			Assert.That(() => Range<int>.AssertBounds(-1, 1), Throws.Nothing);
			Assert.That(() => Range<DateTime>.AssertBounds(11.March(1977), 9.September(2010)), Throws.Nothing);
			Assert.That(() => Range<TimeSpan>.AssertBounds(3.Seconds(), 1.Hours()), Throws.Nothing);
		}

		[Test]
		public void AssertBound_SameValueForBothBounds_NoException()
		{
			Assert.That(() => Range<char>.AssertBounds('a', 'a'), Throws.Nothing);
			Assert.That(() => Range<int>.AssertBounds(-1, -1), Throws.Nothing);
			Assert.That(() => Range<DateTime>.AssertBounds(11.March(1977), 11.March(1977)), Throws.Nothing);
			Assert.That(() => Range<TimeSpan>.AssertBounds(3.Seconds(), 3.Seconds()), Throws.Nothing);
		}

		[Test]
		public void AssertBounds_IncorrectlyOrderedBounds_Exception()
		{
			using (CultureReseter.Set(_platformAgnostic))
			{
				Assert.That(() => Range<char>.AssertBounds('z', 'a'), throwsBoundException('a', "a"));
				Assert.That(() => Range<int>.AssertBounds(1, -1), throwsBoundException(-1, "-1"));
				Assert.That(() => Range<TimeSpan>.AssertBounds(1.Hours(), 3.Seconds()),
					throwsBoundException(3.Seconds(), "00:00:03"));
				Assert.That(() => Range<DateTime>.AssertBounds(9.September(2010), 11.March(1977)),
					throwsBoundException(11.March(1977), "11-03-1977"));
			}
		}

		[Test]
		public void AssertBounds_Inference_NicerSyntax()
		{
			Assert.That(() => Range.AssertBounds('z', 'a'), throwsBoundException('a', "a"));
			Assert.That(() => Range.AssertBounds(11.March(1977), 11.March(1977)), Throws.Nothing);
		}

		#endregion

		#region argument assertion

		[Test]
		public void AssertArgument_Closed_NotContained_Exception()
		{
			var subject = Range.Closed(1, 5);

			Assert.That(() => subject.AssertArgument("arg", 6),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contain("[1..5]").And
				.With.Message.Contain("1 (inclusive)").And
				.With.Message.Contain("5 (inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6));
		}

		[Test]
		public void AssertArgument_Closed_Contained_NoException()
		{
			var subject = Range.Closed(1, 5);

			Assert.That(() => subject.AssertArgument("arg", 4), Throws.Nothing);
		}

		[Test]
		public void AssertArgument_Open_NotContained_Exception()
		{
			var subject = Range.Open(1, 5);

			Assert.That(() => subject.AssertArgument("arg", 6),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contain("(1..5)").And
				.With.Message.Contain("1 (not inclusive)").And
				.With.Message.Contain("5 (not inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6));
		}

		[Test]
		public void AssertArgument_Open_Contained_NoException()
		{
			var subject = Range.Open(1, 5);

			Assert.That(() => subject.AssertArgument("arg", 3), Throws.Nothing);
		}

		[Test]
		public void AssertArgument_HalfOpen_NotContained_Exception()
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(() => subject.AssertArgument("arg", 6),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contain("[1..5)").And
				.With.Message.Contain("1 (inclusive)").And
				.With.Message.Contain("5 (not inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6));
		}

		[Test]
		public void AssertArgument_HalfOpen_Contained_NoException()
		{
			var subject = Range.HalfOpen(1, 5);

			Assert.That(() => subject.AssertArgument("arg", 4), Throws.Nothing);
		}

		[Test]
		public void AssertArgument_HalfClosed_NotContained_Exception()
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(() => subject.AssertArgument("arg", 6),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contain("(1..5]").And
				.With.Message.Contain("1 (not inclusive)").And
				.With.Message.Contain("5 (inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6));
		}

		[Test]
		public void AssertArgument_HalfClosed_Contained_NoException()
		{
			var subject = Range.HalfClosed(1, 5);

			Assert.That(() => subject.AssertArgument("arg", 4), Throws.Nothing);
		}

		[Test]
		public void AssertArgument_NullCollection_Exception()
		{
			Assert.That(() => Range.Closed(1, 5).AssertArgument("arg", null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void AssertArgument_AllContained_NoException()
		{
			Assert.That(() => Range.Closed(1, 5).AssertArgument("arg", new[] { 2, 3, 4 }),
				Throws.Nothing);
		}

		[Test]
		public void AssertArgument_SomeNotContained_ExceptionWithOffendingMember()
		{
			Assert.That(() => Range.Closed(1, 5).AssertArgument("arg", new[] { 2, 6, 4 }),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contain("[1..5]").And
				.With.Message.Contain("1 (inclusive)").And
				.With.Message.Contain("5 (inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6)
				);
		}

		#endregion
	}
}
