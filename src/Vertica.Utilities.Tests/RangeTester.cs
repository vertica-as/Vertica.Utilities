using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons.Time;

namespace Vertica.Utilities.Tests
{
	// ReSharper disable PossibleNullReferenceException
	[TestFixture]
	public partial class RangeTester
	{
		#region value sources

		internal Range<int>[] oneToFives = new[]
		{
			Range.Open(1, 5),
			Range.Closed(1, 5),
			Range.HalfOpen(1, 5),
			Range.HalfClosed(1, 5)
		};

		internal Range<int>[] twoToThrees = new[]
		{
			Range.Closed(2, 3),
			Range.Open(2, 3), 
			Range.HalfClosed(2, 3), 
			Range.HalfOpen(2, 3)
		};

		internal Range<int>[] minusTwoToMinusOnes = new[]
		{
			Range.Open(-2, -1),
			Range.Closed(-2, -1),
			Range.HalfOpen(-2, -1),
			Range.HalfClosed(-2, -1)
		};

		internal Range<int>[] sevenToEights = new[]
		{
			Range.Open(7, 8),
			Range.Closed(7, 8),
			Range.HalfOpen(7, 8),
			Range.HalfClosed(7, 8)
		};

		internal Range<int>[] minusTwoToTwos = new[]
		{
			Range.Open(-2, 2),
			Range.Closed(-2, 2),
			Range.HalfOpen(-2, 2),
			Range.HalfClosed(-2, 2)
		};

		internal Range<int>[] threeToNines = new[]
		{
			Range.Open(3, 9),
			Range.Closed(3, 9),
			Range.HalfOpen(3, 9),
			Range.HalfClosed(3, 9)
		};

		internal Range<int>[] oneToFours = new[]
		{
			Range.Open(1, 4),
			Range.Closed(1, 4),
			Range.HalfOpen(1, 4),
			Range.HalfClosed(1, 4)
		};

		internal Range<int>[] twoToFives = new[]
		{
			Range.Open(2, 5),
			Range.Closed(2, 5),
			Range.HalfOpen(2, 5),
			Range.HalfClosed(2, 5)
		};

		internal Range<int>[] minusTwoToSevens = new[]
		{
			Range.Open(-2, 7),
			Range.Closed(-2, 7),
			Range.HalfOpen(-2, 7),
			Range.HalfClosed(-2, 7)
		};

		#endregion

		#region construction

		[Test]
		public void IsSerializable()
		{
			Assert.That(typeof (Range<>), Has.Attribute<SerializableAttribute>());
			Assert.That(new Range<int>(3, 4), Is.BinarySerializable);
			Assert.That(new Range<char>('3', '4'), Is.XmlSerializable);
		}

		[Test]
		public void Ctor_SetsProperties()
		{
			var subject = new Range<char>('a', 'z');
			Assert.That(subject.LowerBound, Is.EqualTo('a'));
			Assert.That(subject.UpperBound, Is.EqualTo('z'));
		}

		[Test, SetCulture("da-DK")]
		public void Ctor_PoorlyConstructed_Exception()
		{
			Assert.That(() => new Range<int>(5, 1), throwsBoundException(1, "1"));

			Assert.That(() => new Range<int>(-1, -5), throwsBoundException(-5, "-5"));

			Assert.That(() => new Range<TimeSpan>(3.Seconds(), 2.Seconds()), throwsBoundException(2.Seconds(), "00:00:02"));

			Assert.That(() => new Range<DateTime>(11.March(1977), 31.October(1952)), throwsBoundException(31.October(1952), "31-10-1952 00:00:00"));
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
				.Message.StringContaining(upperBoundRepresentation);
		}
	}
	// ReSharper restore PossibleNullReferenceException	
}
