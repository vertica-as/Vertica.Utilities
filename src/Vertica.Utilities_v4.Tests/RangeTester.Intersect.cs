using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		[Test]
		public void Intersect_Null_Empty()
		{
			Range<int> notEmpty = Range.New(1, 10);

			var intersection = notEmpty.Intersect(null);
			Assert.That(intersection, Is.SameAs(Range.Empty<int>()));
		}

		[Test]
		public void Intersect_NotEmptyToEmpty_Empty()
		{
			Range<int> notEmpty = Range.New(1, 10);

			var intersection = notEmpty.Intersect(Range.Empty<int>());
			Assert.That(intersection, Is.SameAs(Range.Empty<int>()));
		}
	}
}