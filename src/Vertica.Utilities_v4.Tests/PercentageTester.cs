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

		#endregion
	}
}
