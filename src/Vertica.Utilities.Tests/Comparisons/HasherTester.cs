using NUnit.Framework;
using Vertica.Utilities_v4.Comparisons;
using Vertica.Utilities_v4.Tests.Comparisons.Support;

namespace Vertica.Utilities_v4.Tests.Comparisons
{
	[TestFixture]
	public class HasherTester
	{
		[Test]
		public void DefaultHasher_NotNull_InvokedGetHashCodeOnObject()
		{
			var spy = new EqualitySpy();

			Hasher.Default(spy);

			Assert.That(spy.GetHashCodeCalled, Is.True);
		}

		[Test]
		public void DefaultHasher_Null_Zero()
		{
			var hash = Hasher.Default<string>(null);

			Assert.That(hash, Is.EqualTo(0));
		}

		[Test]
		public void ZeroHasher_ReturnsZero()
		{
			var spy = new EqualitySpy();

			Assert.That(Hasher.Zero(spy), Is.EqualTo(0));

			Assert.That(spy.GetHashCodeCalled, Is.False);
		}

		[Test]
		public void CanonicalHasherl_SameValue_AsResharperImplementation()
		{
			var baseline = new EqualitySubject { I = 243, D = 298.75m, S = "dGG" };

			int actual = Hasher.Canonical(baseline.I, baseline.D, baseline.S);

			Assert.That(actual, Is.EqualTo(baseline.GetHashCode()));
		}

		[Test]
		public void CanonicalHasher_NoValues_Zero()
		{
			Assert.That(Hasher.Canonical(), Is.EqualTo(0));
		}

		[Test]
		public void CanonicalHasher_OrderOfArguments_Matters()
		{
			int oneTwoThree = Hasher.Canonical(1, 2, 3),
				twoThreeOne = Hasher.Canonical(2, 3, 1);

			Assert.That(oneTwoThree, Is.Not.EqualTo(twoThreeOne));
		}
	}
}
