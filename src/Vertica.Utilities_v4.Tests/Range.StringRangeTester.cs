using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class StringRangeTester
	{
		[Test]
		public void Generate_Closed_StringGenerator_CollectionOfSuccesiveStrings()
		{
			Assert.That(Range.Closed("<<koala>>", "<<koale>>").Generate(Range.StringGenerator), Is.EqualTo(
				new[]
				{
					"<<koala>>",
					"<<koalb>>",
					"<<koalc>>",
					"<<koald>>",
					"<<koale>>"
				}));

			Assert.That(Range.Closed("1", "7").Generate(Range.StringGenerator), Is.EqualTo(
				new[] { "1", "2", "3", "4", "5", "6", "7" }));
		}

		[Test]
		public void Generate_Open_StringGenerator_ObbeysBoundInclusion()
		{
			Assert.That(Range.Open("1", "7").Generate(Range.StringGenerator), Is.EqualTo(
				new[] { "2", "3", "4", "5", "6" }));
		}
	}
}
