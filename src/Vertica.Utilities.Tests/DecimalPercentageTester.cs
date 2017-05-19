using System.Globalization;
using NUnit.Framework;
using Testing.Commons.Globalization;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public class DecimalPercentageTester
	{
		#region construction

		[Test]
		public void Ctor_SetsValueAndFraction()
		{
			var sixtyPercent = new DecimalPercentage(60m);

			Assert.That(sixtyPercent.Value, Is.EqualTo(60m));
			Assert.That(sixtyPercent.Fraction, Is.EqualTo(.6m));
		}

		[Test]
		public void FromFraction_SetsValueAndFraction()
		{
			DecimalPercentage sixtyPercent = DecimalPercentage.FromFraction(.6m);

			Assert.That(sixtyPercent.Value, Is.EqualTo(60m));
			Assert.That(sixtyPercent.Fraction, Is.EqualTo(.6m));
		}

		[Test]
		public void FromAmounts_CalculatesPercentage()
		{
			DecimalPercentage eightyPercent = DecimalPercentage.FromAmounts(60L, 75L);

			Assert.That(eightyPercent.Value, Is.EqualTo(80m));
			Assert.That(eightyPercent.Fraction, Is.EqualTo(0.8m));

			DecimalPercentage tenPercent = DecimalPercentage.FromAmounts(10m, 100m);
			Assert.That(tenPercent.Value, Is.EqualTo(10m));
			Assert.That(tenPercent.Fraction, Is.EqualTo(0.1m));

			DecimalPercentage thousandPercent = DecimalPercentage.FromAmounts(100m, 10m);
			Assert.That(thousandPercent.Value, Is.EqualTo(1000m));
			Assert.That(thousandPercent.Fraction, Is.EqualTo(10m));
		}

		[Test]
		public void FromAmount_ZeroTotal_Exception()
		{
			Assert.That(() => DecimalPercentage.FromAmounts(10, 0), Throws.ArgumentException);
		}

		[Test]
		public void FromDifference_TotalBigger_PositivePercentage()
		{
			DecimalPercentage fiftyPercentBigger = DecimalPercentage.FromDifference(20L, 10L);
			Assert.That(fiftyPercentBigger.Value, Is.EqualTo(50m));

			fiftyPercentBigger = DecimalPercentage.FromDifference(20m, 10m);
			Assert.That(fiftyPercentBigger.Value, Is.EqualTo(50d));
		}

		[Test]
		public void FromDifference_TotalSmaller_NegativePercentage()
		{
			DecimalPercentage twiceAsSmall = DecimalPercentage.FromDifference(10L, 20L);
			Assert.That(twiceAsSmall.Value, Is.EqualTo(-100m));

			twiceAsSmall = DecimalPercentage.FromDifference(10m, 20m);
			Assert.That(twiceAsSmall.Value, Is.EqualTo(-100d));
		}

		[Test]
		public void FromDifference_ZeroTotal_HundredPercent()
		{
			DecimalPercentage hundredPercentMore = DecimalPercentage.FromDifference(15, 0);
			Assert.That(hundredPercentMore.Value, Is.EqualTo(100m));
			Assert.That(hundredPercentMore.Fraction, Is.EqualTo(1m));

			hundredPercentMore = DecimalPercentage.FromDifference(long.MaxValue, 0);
			Assert.That(hundredPercentMore.Value, Is.EqualTo(100d));
			Assert.That(hundredPercentMore.Fraction, Is.EqualTo(1d));
		}

		[Test]
		public void ExtensionMethods_LikeConstructionCounterParts()
		{
			DecimalPercentage sixtyPercent = 60m.Percent();
			Assert.That(sixtyPercent.Value, Is.EqualTo(60m));
			Assert.That(sixtyPercent.Fraction, Is.EqualTo(.6m));

			DecimalPercentage tenPercent = 10m.AsPercentOf(100m);
			Assert.That(tenPercent.Value, Is.EqualTo(10m));
			Assert.That(tenPercent.Fraction, Is.EqualTo(0.1m));
		}

		#endregion

		[Test]
		public void Apply_AppliesThePercentageToTheAmountGiven()
		{
			var fiftyPercent = new DecimalPercentage(50);
			Assert.That(fiftyPercent.Apply(100L), Is.EqualTo(50m));
			Assert.That(fiftyPercent.Apply(100m), Is.EqualTo(50m));
		}

		[Test]
		public void DeductFrom_CalculatesAnAmountWithoutThePercentage()
		{
			Assert.That(25m.Percent().DeductFrom(100m), Is.EqualTo(80m));
		}

		#region representation

		[Test]
		public void ToString_CultureIndependent()
		{
			using (CultureReseter.Set("es-ES"))
			{
				Assert.That(new DecimalPercentage(33.343m).ToString(), Is.EqualTo("33.343 %"));
			}

			using (CultureReseter.Set("en-US"))
			{
				Assert.That(new DecimalPercentage(33.3112m).ToString(), Is.EqualTo("33.3112 %"));
			}
		}

		[Test]
		public void ToString_FormattedNumber_HonorsFormatBeingCultureIndependent()
		{
			using (CultureReseter.Set("es-ES"))
			{
				Assert.That(new DecimalPercentage(33.3m).ToString("{0:000.0000}"), Is.EqualTo("033.3000 %"));
			}

			using (CultureReseter.Set("en-US"))
			{
				Assert.That(new DecimalPercentage(33.3m).ToString("{0:000.0000}"), Is.EqualTo("033.3000 %"));
			}
		}

		[Test]
		public void ToString_Formattable_UsesProvider()
		{
			var info = new NumberFormatInfo { NumberDecimalSeparator = "¤", PercentSymbol = "ignored" };
			Assert.That(new DecimalPercentage(33.3m).ToString("{0:000.0000}", info), Is.EqualTo("033¤3000 %"));
		}

		#endregion
	}
}