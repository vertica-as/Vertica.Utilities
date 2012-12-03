using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class PercentageTester
	{
		#region construction

		[Test]
		public void Ctor_SetsValueAndFraction()
		{
			var sixtyPercent = new Percentage(60d);

			Assert.That(sixtyPercent.Value, Is.EqualTo(60d));
			Assert.That(sixtyPercent.Fraction, Is.EqualTo(.6d));
		}

		[Test]
		public void FromFraction_SetsValueAndFraction()
		{
			Percentage sixtyPercent = Percentage.FromFraction(.6d);

			Assert.That(sixtyPercent.Value, Is.EqualTo(60d));
			Assert.That(sixtyPercent.Fraction, Is.EqualTo(.6d));
		}

		[Test]
		public void FromAmounts_CalculatesPercentage()
		{
			Percentage eightyPercent = Percentage.FromAmounts(60L, 75L);

			Assert.That(eightyPercent.Value, Is.EqualTo(80d));
			Assert.That(eightyPercent.Fraction, Is.EqualTo(0.8d));

			Percentage tenPercent = Percentage.FromAmounts(10d, 100d);
			Assert.That(tenPercent.Value, Is.EqualTo(10d));
			Assert.That(tenPercent.Fraction, Is.EqualTo(0.1d));

			Percentage thousandPercent = Percentage.FromAmounts(100d, 10d);
			Assert.That(thousandPercent.Value, Is.EqualTo(1000d));
			Assert.That(thousandPercent.Fraction, Is.EqualTo(10d));
		}

		[Test]
		public void FromAmount_ZeroTotal_Exception()
		{
			Assert.That(()=>Percentage.FromAmounts(10, 0), Throws.ArgumentException);
		}

		[Test]
		public void FromDifference_TotalBigger_PositivePercentage()
		{
			Percentage fiftyPercentBigger = Percentage.FromDifference(20L, 10L);
			Assert.That(fiftyPercentBigger.Value, Is.EqualTo(50d));

			fiftyPercentBigger = Percentage.FromDifference(20d, 10d);
			Assert.That(fiftyPercentBigger.Value, Is.EqualTo(50d));
		}

		[Test]
		public void FromDifference_TotalSmaller_NegativePercentage()
		{
			Percentage twiceAsSmall = Percentage.FromDifference(10L, 20L);
			Assert.That(twiceAsSmall.Value, Is.EqualTo(-100d));

			twiceAsSmall = Percentage.FromDifference(10d, 20d);
			Assert.That(twiceAsSmall.Value, Is.EqualTo(-100d));
		}

		[Test]
		public void FromDifference_ZeroTotal_HundredPercent()
		{
			Percentage hundredPercentMore = Percentage.FromDifference(15, 0);
			Assert.That(hundredPercentMore.Value, Is.EqualTo(100d));
			Assert.That(hundredPercentMore.Fraction, Is.EqualTo(1d));

			hundredPercentMore = Percentage.FromDifference(long.MaxValue, 0);
			Assert.That(hundredPercentMore.Value, Is.EqualTo(100d));
			Assert.That(hundredPercentMore.Fraction, Is.EqualTo(1d));
		}

		[Test]
		public void ExtensionMethods_LikeConstructionCounterParts()
		{
			Percentage sixtyPercent = 60d.Percent();
			Assert.That(sixtyPercent.Value, Is.EqualTo(60d));
			Assert.That(sixtyPercent.Fraction, Is.EqualTo(.6d));

			Percentage tenPercent = 10d.AsPercentOf(100d);
			Assert.That(tenPercent.Value, Is.EqualTo(10d));
			Assert.That(tenPercent.Fraction, Is.EqualTo(0.1d));
		}

		#endregion

		[Test]
		public void Apply_AppliesThePercentageToTheAmountGiven()
		{
			var fiftyPercent = new Percentage(50);
			Assert.That(fiftyPercent.Apply(100L), Is.EqualTo(50d));
			Assert.That(fiftyPercent.Apply(100d), Is.EqualTo(50d));
		}
	}
}
