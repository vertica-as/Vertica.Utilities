using System;
using System.Globalization;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons.Globalization;
using Testing.Commons.Time;
using Vertica.Utilities.Extensions.RangeExt;

namespace Vertica.Utilities.Tests
{
	// ReSharper disable PossibleNullReferenceException
	[TestFixture]
	public partial class RangeTester
	{
		#region value sources

		private static readonly Range<int>[] oneToFives = new[]
		{
			Range.Open(1, 5),
			Range.Closed(1, 5),
			Range.HalfOpen(1, 5),
			Range.HalfClosed(1, 5)
		};

		private static readonly Range<int>[] oneToThrees = new[]
		{
			Range.Open(1, 3),
			Range.Closed(1, 3),
			Range.HalfOpen(1, 3),
			Range.HalfClosed(1, 3)
		};

		#endregion

		#region construction

		[Test]
		public void Ctor_SetsProperties()
		{
			var subject = new Range<char>('a', 'z');
			Assert.That(subject.LowerBound, Is.EqualTo('a'));
			Assert.That(subject.UpperBound, Is.EqualTo('z'));
		}

		[Test]
		public void Ctor_PoorlyConstructed_Exception()
		{
			var platformAgnostic = new CultureInfo("dgg") { DateTimeFormat = { ShortDatePattern = "dd-MM-yyyy" } };
			
			using (CultureReseter.Set(platformAgnostic))
			{
				Assert.That(() => new Range<int>(5, 1), throwsBoundException(1, "1"));

				Assert.That(() => new Range<int>(-1, -5), throwsBoundException(-5, "-5"));

				Assert.That(() => new Range<TimeSpan>(3.Seconds(), 2.Seconds()), throwsBoundException(2.Seconds(), "00:00:02"));

				Assert.That(() => new Range<DateTime>(11.March(1977), 31.October(1952)),
					throwsBoundException(31.October(1952), "31-10-1952 00:00:00"));
			}
		}

		[Test]
		public void Ctor_NicelyConstructed_NoException()
		{
			Assert.That(() => new Range<int>(1, 5), Throws.Nothing);

			Assert.That(() => new Range<int>(-5, -1), Throws.Nothing);

			Assert.That(() => new Range<TimeSpan>(2.Seconds(), 3.Seconds()), Throws.Nothing);

			Assert.That(() => new Range<DateTime>(31.October(1952), 11.March(1977)), Throws.Nothing);
		}

		[Test]
		public void Inference_NicerSyntax()
		{
			Assert.That(() => Range.New(1, 5), Throws.Nothing);

			Assert.That(() => Range.New(-5, -1), Throws.Nothing);

			Assert.That(() => Range.New(2.Seconds(), 3.Seconds()), Throws.Nothing);

			Assert.That(() => Range.New(31.October(1952), 11.March(1977)), Throws.Nothing);
		}

		[Test]
		public void Degenerate_SameBounds()
		{
			Range<TimeSpan> degenerate = Range.Degenerate(2.Seconds());

			Assert.That(degenerate.LowerBound, Is.EqualTo(2.Seconds()));
			Assert.That(degenerate.UpperBound, Is.EqualTo(2.Seconds()));
		}

		#endregion

		#region representation

		[Test]
		public void ToString_Closed_BetweenBrackets()
		{
			var subject = new Range<int>(1.Close(), 5.Close());
			Assert.That(subject.ToString(), Is.EqualTo("[1..5]"));

			subject = Range.Closed(1, 5);
			Assert.That(subject.ToString(), Is.EqualTo("[1..5]"));
		}

		[Test]
		public void ToString_Open_BetweenParenthesis()
		{
			var subject = new Range<int>(1.Open(), 5.Open());
			Assert.That(subject.ToString(), Is.EqualTo("(1..5)"));

			subject = Range.Open(1, 5);
			Assert.That(subject.ToString(), Is.EqualTo("(1..5)"));
		}

		[Test]
		public void ToString_HalfOpen_ParenthesisAtTheEnd()
		{
			var subject = new Range<int>(1.Close(), 5.Open());
			Assert.That(subject.ToString(), Is.EqualTo("[1..5)"));

			subject = Range.HalfOpen(1, 5);
			Assert.That(subject.ToString(), Is.EqualTo("[1..5)"));
		}

		[Test]
		public void ToString_HalfClosed_ParenthesisAtTheBeginning()
		{
			var subject = new Range<int>(1.Open(), 5.Close());
			Assert.That(subject.ToString(), Is.EqualTo("(1..5]"));

			subject = Range.HalfClosed(1, 5);
			Assert.That(subject.ToString(), Is.EqualTo("(1..5]"));
		}

		[Test]
		public void ToString_Default_IsClosed()
		{
			var subject = new Range<int>(1, 5);
			Assert.That(subject.ToString(), Is.EqualTo("[1..5]"));

			subject = Range.New(1, 5);
			Assert.That(subject.ToString(), Is.EqualTo("[1..5]"));

			subject = Range.Closed(1, 5);
			Assert.That(subject.ToString(), Is.EqualTo("[1..5]"));
		}

		#endregion

		private static Constraint throwsBoundException<T>(T upperBound, string upperBoundRepresentation)
		{
			return Throws.InstanceOf<ArgumentOutOfRangeException>().With
				.Property("ActualValue").EqualTo(upperBound).And
				.Message.Contain(upperBoundRepresentation);
		}
	}
	// ReSharper restore PossibleNullReferenceException	
}
