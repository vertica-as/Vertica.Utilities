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
			var sixtyPercent = Percentage.FromFraction(.6d);

			Assert.That(sixtyPercent.Value, Is.EqualTo(60d));
			Assert.That(sixtyPercent.Fraction, Is.EqualTo(.6d));
		}

		#endregion
	}
}
