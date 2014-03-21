using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		[Test]
		public void Overlaps_NullOrEmpty_False()
		{
			var range = Range.New(1, 2);

			Assert.That(range.Overlaps(null), Is.False);
			Assert.That(range.Overlaps(Range.Empty<int>()), Is.False);
		}

		[Test]
		public void Overlaps_EmptyWithAnything_False()
		{
			var notEmpty = Range.New<byte>(1, 2);

			Assert.That(Range<byte>.Empty.Overlaps(null), Is.False);
			Assert.That(Range<byte>.Empty.Overlaps(Range.Empty<byte>()), Is.False);
			Assert.That(Range<byte>.Empty.Overlaps(notEmpty), Is.False);
		}
	}
}