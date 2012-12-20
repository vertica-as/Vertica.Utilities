using NUnit.Framework;
using Vertica.Utilities_v4.Comparisons;
using Vertica.Utilities_v4.Tests.Comparisons.Support;

namespace Vertica.Utilities_v4.Tests.Comparisons
{
	[TestFixture]
	public class HasherTester
	{
		[Test]
		public void DefaultHasher_InvokedGetHashCodeOnObject()
		{
			var spy = new EqualitySpy();

			Hasher.Default(spy);

			Assert.That(spy.GetHashCodeCalled, Is.True);
		}

		[Test]
		public void ZeroHasher_ReturnsZero()
		{
			var spy = new EqualitySpy();

			Assert.That(Hasher.Zero(spy), Is.EqualTo(0));

			Assert.That(spy.GetHashCodeCalled, Is.False);
		}
	}
}
