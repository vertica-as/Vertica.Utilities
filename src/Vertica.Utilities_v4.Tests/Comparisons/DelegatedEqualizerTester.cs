using System;
using System.Collections.Generic;
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
		public void Equals_BothNull_True()
		{
			var subject = new DelegatedEqualizer<string>((x, y) => false);

			Assert.That(subject.Equals(null, null), Is.True);
		}

		[Test]
		public void Equals_BothNull_EqualsPredicateInvocationNotNeeded()
		{
			var spy = new EqualitySpy();
			Func<string, string, bool> notEqual = spy.GetEquals<string>(false);
			IEqualityComparer<string> subject = new DelegatedEqualizer<string>(notEqual);

			subject.Equals(null, null);

			Assert.That(spy.EqualsCalled, Is.False);
		}

		[TestCase(null, "notNull")]
		[TestCase("notNull", null)]
		public void Equals_OneNullArgument_False(string first, string second)
		{
			var subject = new DelegatedEqualizer<string>((x, y) => true);

			Assert.That(subject.Equals(first, second), Is.False);
		}

		[TestCase(null, "notNull")]
		[TestCase("notNull", null)]
		public void Equals_OneNullArgument_EqualsPredicateInvocationNotNeeded(string first, string second)
		{
			var spy = new EqualitySpy();
			Func<string, string, bool> equal = spy.GetEquals<string>(true);
			IEqualityComparer<string> subject = new DelegatedEqualizer<string>(equal);

			subject.Equals(first, second);

			Assert.That(spy.EqualsCalled, Is.False);
		}

		[TestCase("Daniel", "David", true)]
		[TestCase("Daniel", "Manolo", false)]
		public void Equals_NotNullArguments_EqualsPredicateInvoked(string first, string second, bool startWithSameLetter)
		{
			IEqualityComparer<string> subject = new DelegatedEqualizer<string>((x, y) => x[0].Equals(y[0]));

			Assert.That(subject.Equals(first, second), Is.EqualTo(startWithSameLetter));
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
