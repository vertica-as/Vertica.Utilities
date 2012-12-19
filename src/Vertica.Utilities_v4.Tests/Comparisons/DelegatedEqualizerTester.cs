using NUnit.Framework;
using Vertica.Utilities_v4.Comparisons;
using Vertica.Utilities_v4.Tests.Comparisons.Support;

namespace Vertica.Utilities_v4.Tests.Comparisons
{
	[TestFixture]
	public class DelegatedEqualizerTester
	{
		[Test]
		public void Ctor_Equals_FuncAndDefaultUsed()
		{
			var spy = new EqualitySpy();
			var subject = new DelegatedEqualizer<int>(spy.GetEquals<int>(false));

			Assert.That(subject.Equals(1, 1), Is.False);
			Assert.That(spy.EqualsCalled, Is.True);
			Assert.That(subject.GetHashCode(1), Is.EqualTo(1.GetHashCode()));
		}

		[Test]
		public void Ctor_Both_BothUsed()
		{
			var spy = new EqualitySpy();
			var subject = new DelegatedEqualizer<int>(
				spy.GetEquals<int>(false),
				spy.GetHashCode<int>(42));

			Assert.That(subject.Equals(1, 1), Is.False);
			Assert.That(spy.EqualsCalled, Is.True);
			Assert.That(subject.GetHashCode(1), Is.EqualTo(42));
			Assert.That(spy.GetHashCodeCalled, Is.True);
		}

		[Test]
		public void Ctor_Comparison_DelegateAndDefaultUsed()
		{
			var spy = new EqualitySpy();
			var subject = new DelegatedEqualizer<int>(spy.GetComparison<int>(1977));

			Assert.That(subject.Equals(1, 1), Is.False);
			Assert.That(spy.EqualsCalled, Is.True);
			Assert.That(subject.GetHashCode(1), Is.EqualTo(1.GetHashCode()));
		}

		[Test]
		public void Ctor_ComparisonBoth_BothUsed()
		{
			var spy = new EqualitySpy();
			var subject = new DelegatedEqualizer<int>(
				spy.GetComparison<int>(1977),
				spy.GetHashCode<int>(42));

			Assert.That(subject.Equals(1, 1), Is.False);
			Assert.That(spy.EqualsCalled, Is.True);
			Assert.That(subject.GetHashCode(1), Is.EqualTo(42));
			Assert.That(spy.GetHashCodeCalled, Is.True);
		}

		[Test]
		public void Ctor_SelectorComparer_DelegateAndDefaultUsed()
		{
			var spy = new EqualitySpy();
			var subject = new DelegatedEqualizer<int>(spy.GetComparer<int>(1));

			Assert.That(subject.Equals(1, 1), Is.False);
			Assert.That(spy.EqualsCalled, Is.True);
			Assert.That(subject.GetHashCode(1), Is.EqualTo(1.GetHashCode()));
		}

		[Test]
		public void Ctor_ComparerBoth_BothUsed()
		{
			var spy = new EqualitySpy();
			var subject = new DelegatedEqualizer<int>(
				spy.GetComparer<int>(-1),
				spy.GetHashCode<int>(42));

			Assert.That(subject.Equals(1, 1), Is.False);
			Assert.That(spy.EqualsCalled, Is.True);
			Assert.That(subject.GetHashCode(1), Is.EqualTo(42));
			Assert.That(spy.GetHashCodeCalled, Is.True);
		}

		[Test]
		public void DefaultHasher_InvokedGetHashCodeOnObject()
		{
			var spy = new EqualitySpy();

			DelegatedEqualizer<EqualitySpy>.DefaultHasher(spy);
			
			Assert.That(spy.GetHashCodeCalled, Is.True);
		}

		[Test]
		public void ZeroHasher_ReturnsZero()
		{
			var spy = new EqualitySpy();

			Assert.That(DelegatedEqualizer<EqualitySpy>.ZeroHasher(spy), Is.EqualTo(0));

			Assert.That(spy.GetHashCodeCalled, Is.False);
		}

		[Test]
		public void Chaining_IsDestructive()
		{
			EqualitySubject x1 = new EqualitySubject("x", 1, 1m), x2 = new EqualitySubject("x", 1, 2m);

			ChainableEqualizer<EqualitySubject> sAndI = new DelegatedEqualizer<EqualitySubject>((x, y) => x.S.Equals(y.S))
				.Then((x, y) => x.I.Equals(y.I));

			Assert.That(sAndI.Equals(x1, x2), Is.True);

			var allProp = sAndI.Then(Eq<EqualitySubject>.By((x, y) => x.D.Equals(y.D)));
			Assert.That(allProp.Equals(x1, x2), Is.False);
			Assert.That(sAndI.Equals(x1, x2), Is.False);
		}

		[Test]
		public void Clients_DoNotHaveToCareAboutNulls()
		{
			var notNull = new EqualitySubject("a", 1, 1m);
			var chainable = new DelegatedEqualizer<EqualitySubject>((x, y) => x.I.Equals(x.I));

			Assert.That(chainable.Equals(notNull, null), Is.False);
			Assert.That(chainable.Equals(null, notNull), Is.False);
			Assert.That(chainable.Equals(null, null), Is.True);
		}
	}
}
