using NUnit.Framework;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public partial class RangeTester
	{
		[Test]
		public void Empty_ContainsValue_False()
		{
			Range<int> empty = Range<int>.Empty;

			Assert.That(empty.Contains(0), Is.False);
			Assert.That(empty.Contains(-1), Is.False);
			Assert.That(empty.Contains(int.MinValue), Is.False);
			Assert.That(empty.Contains(int.MaxValue), Is.False);
			Assert.That(empty.Contains(1), Is.False);
		}

		[Test]
		public void Empty_Generate_Empty()
		{
			Assert.That(Range<int>.Empty.Generate(x => x + 1), Is.Empty);
			Assert.That(Range<int>.Empty.Generate(1), Is.Empty);
		}

		[Test]
		public void Empty_LimitLower_SameValue()
		{
			Range<int> empty = Range<int>.Empty;
			Assert.That(empty.LimitLower(0), Is.EqualTo(0));
			Assert.That(empty.LimitLower(-1), Is.EqualTo(-1));
			Assert.That(empty.LimitLower(2), Is.EqualTo(2));
		}

		[Test]
		public void Empty_LimitUpper_SameValue()
		{
			Range<int> empty = Range<int>.Empty;
			Assert.That(empty.LimitUpper(0), Is.EqualTo(0));
			Assert.That(empty.LimitUpper(-1), Is.EqualTo(-1));
			Assert.That(empty.LimitUpper(2), Is.EqualTo(2));
		}

		[Test]
		public void Empty_Limit_SameValue()
		{
			Range<int> empty = Range<int>.Empty;
			Assert.That(empty.Limit(0), Is.EqualTo(0));
			Assert.That(empty.Limit(-1), Is.EqualTo(-1));
			Assert.That(empty.Limit(2), Is.EqualTo(2));
		}

		[Test]
		public void Empty_IsSingleton()
		{
			Assert.That(Range<int>.Empty, Is.SameAs(Range.Empty<int>()));
		}
	}
}
