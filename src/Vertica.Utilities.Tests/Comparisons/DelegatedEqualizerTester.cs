using System;
using System.Collections.Generic;
using NUnit.Framework;
using Vertica.Utilities.Tests.Comparisons.Support;
using Vertica.Utilities.Comparisons;

namespace Vertica.Utilities.Tests.Comparisons
{
	[TestFixture]
	public class DelegatedEqualizerTester
	{
		[Test, Category("Exploratory")]
		public void Explore()
		{
			Func<EqualitySubject, EqualitySubject, bool> compareI = (x, y) => x.I.Equals(y.I);
			var subject = new DelegatedEqualizer<EqualitySubject>(compareI, s => s.I.GetHashCode());
			var x1 = new EqualitySubject("x", 1, 1m);
			Assert.That(subject.Equals(x1, new EqualitySubject("y", 1, 2m)), Is.True);
			
			var custom = new DelegatedEqualizer<EqualitySubject>((x, y) => x.I.Equals(y.I), s => s.I.GetHashCode());
			Assert.That(custom.GetHashCode(x1), Is.EqualTo(x1.I.GetHashCode()));
			custom = new DelegatedEqualizer<EqualitySubject>((x, y) => x.I.Equals(y.I), s => Hasher.Default(s.I));
			Assert.That(custom.GetHashCode(x1), Is.EqualTo(x1.I.GetHashCode()));
			var @default = new DelegatedEqualizer<EqualitySubject>((x, y) => x.I.Equals(y.I), Hasher.Default);
			Assert.That(@default.GetHashCode(x1), Is.EqualTo(x1.GetHashCode()));
			var zero = new DelegatedEqualizer<EqualitySubject>((x, y) => x.I.Equals(y.I), Hasher.Zero);
			Assert.That(zero.GetHashCode(x1), Is.EqualTo(0));


			IEqualityComparer<EqualitySubject> bySAndI = new DelegatedEqualizer<EqualitySubject>((x, y) => x.S.Equals(y.S), Hasher.Zero)
				.Then(new DelegatedEqualizer<EqualitySubject>((x, y) => x.I.Equals(y.I), Hasher.Default));
			bySAndI = new DelegatedEqualizer<EqualitySubject>((x, y) => x.S.Equals(y.S), Hasher.Zero)
				.Then((x, y) => x.I == y.I, Hasher.Default);
			bySAndI = Eq<EqualitySubject>.By((x, y) => x.S.Equals(y.S), Hasher.Zero)
				.Then((x, y) => x == y, Hasher.Default);

			IComparer<EqualitySubject> comparer = Cmp<EqualitySubject>.By(x => x.I);
			IEqualityComparer<EqualitySubject> eq = new DelegatedEqualizer<EqualitySubject>(comparer, Hasher.Zero);
			Comparison<EqualitySubject> comparison = (x, y) => x.D.CompareTo(y.D);
			eq = new DelegatedEqualizer<EqualitySubject>(comparison, Hasher.Zero);
		}


		[Test]
		public void Ctor()
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
		public void Ctor_Comparison()
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
		public void Ctor_Comparer()
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
		public void Equals_BothNull_True()
		{
			var subject = new DelegatedEqualizer<string>((x, y) => false, Hasher.Zero);

			Assert.That(subject.Equals(null, null), Is.True);
		}

		[Test]
		public void Equals_BothNull_EqualsPredicateInvocationNotNeeded()
		{
			var spy = new EqualitySpy();
			Func<string, string, bool> notEqual = spy.GetEquals<string>(false);
			IEqualityComparer<string> subject = new DelegatedEqualizer<string>(notEqual, Hasher.Zero);

			subject.Equals(null, null);

			Assert.That(spy.EqualsCalled, Is.False);
		}

		[TestCase(null, "notNull")]
		[TestCase("notNull", null)]
		public void Equals_OneNullArgument_False(string first, string second)
		{
			var subject = new DelegatedEqualizer<string>((x, y) => true, Hasher.Zero);

			Assert.That(subject.Equals(first, second), Is.False);
		}

		[TestCase(null, "notNull")]
		[TestCase("notNull", null)]
		public void Equals_OneNullArgument_EqualsPredicateInvocationNotNeeded(string first, string second)
		{
			var spy = new EqualitySpy();
			Func<string, string, bool> equal = spy.GetEquals<string>(true);
			IEqualityComparer<string> subject = new DelegatedEqualizer<string>(equal, Hasher.Zero);

			subject.Equals(first, second);

			Assert.That(spy.EqualsCalled, Is.False);
		}

		[TestCase("Daniel", "David", true)]
		[TestCase("Daniel", "Manolo", false)]
		public void Equals_NotNullArguments_EqualsPredicateInvoked(string first, string second, bool startWithSameLetter)
		{
			IEqualityComparer<string> subject = new DelegatedEqualizer<string>((x, y) => x[0].Equals(y[0]), Hasher.Zero);

			Assert.That(subject.Equals(first, second), Is.EqualTo(startWithSameLetter));
		}

		[Test]
		public void Chaining_IsDestructive()
		{
			EqualitySubject x1 = new EqualitySubject("x", 1, 1m), x2 = new EqualitySubject("x", 1, 2m);

			ChainableEqualizer<EqualitySubject> sAndI = new DelegatedEqualizer<EqualitySubject>((x, y) => x.S.Equals(y.S), Hasher.Zero)
				.Then((x, y) => x.I.Equals(y.I), Hasher.Zero);

			Assert.That(sAndI.Equals(x1, x2), Is.True);

			var allProp = sAndI.Then(Eq<EqualitySubject>.By((x, y) => x.D.Equals(y.D), x => x.D.GetHashCode()));
			Assert.That(allProp.Equals(x1, x2), Is.False);
			Assert.That(sAndI.Equals(x1, x2), Is.False);
		}

		[Test]
		public void Clients_DoNotHaveToCareAboutNulls()
		{
			var notNull = new EqualitySubject("a", 1, 1m);
			var chainable = new DelegatedEqualizer<EqualitySubject>((x, y) => x.I.Equals(x.I), x => x.I.GetHashCode());

			Assert.That(chainable.Equals(notNull, null), Is.False);
			Assert.That(chainable.Equals(null, notNull), Is.False);
			Assert.That(chainable.Equals(null, null), Is.True);
		}
	}
}
