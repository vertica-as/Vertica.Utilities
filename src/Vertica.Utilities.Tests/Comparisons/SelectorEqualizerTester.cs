using System;
using System.Collections.Generic;
using NUnit.Framework;
using Vertica.Utilities.Comparisons;

namespace Vertica.Utilities.Tests.Comparisons.Support
{
	[TestFixture]
	public class SelectorEqualizerTester
	{
		[Test, Category("Exploratory")]
		public void Explore()
		{
			IEqualityComparer<EqualitySubject> subject = new SelectorEqualizer<EqualitySubject, int>(s => s.I);
			Assert.That(subject.Equals(new EqualitySubject { I = 1 }, new EqualitySubject { I = 2 }), Is.False);
			Assert.That(subject.GetHashCode(new EqualitySubject { I = 1 }), Is.EqualTo(1.GetHashCode()));

			IEqualityComparer<EqualitySubject> byDAndI = new SelectorEqualizer<EqualitySubject, decimal>(s => s.D)
				.Then(s => s.I);
			byDAndI = Eq<EqualitySubject>.By(s => s.D)
				.Then(s => s.I);
		}


		#region GetHashCode

		[Test]
		public void GetHashCode_NullKey_Zero()
		{
			IEqualityComparer<string> subject = new SelectorEqualizer<string, string>(s => null);

			Assert.That(subject.GetHashCode("whatever"), Is.EqualTo(0));
		}

		[Test]
		public void GetHashCode_NullKey_GetHashCodeInvocationNotNeeded()
		{
			var spy = new EqualitySpy();
			IEqualityComparer<EqualitySpy> subject = new SelectorEqualizer<EqualitySpy, string>(s => s.FakeASelector<string>(null));

			subject.GetHashCode(spy);

			Assert.That(spy.GetHashCodeCalled, Is.False);
		}

		[Test]
		public void GetHashCode_InvokedOverKey()
		{
			IEqualityComparer<string> subject = new SelectorEqualizer<string, string>(s => s.ToLower());

			Assert.That(subject.GetHashCode("DANIEL"), Is.EqualTo("daniel".GetHashCode()));
			Assert.That(subject.GetHashCode("DANIEL"), Is.Not.EqualTo("someoneElse".GetHashCode()));
		}

		#endregion

		#region Equals

		[Test]
		public void Equals_BothKeysNull_True()
		{
			IEqualityComparer<string> subject = new SelectorEqualizer<string, string>(s => null);

			Assert.That(subject.Equals("x", "x"), Is.True);
			Assert.That(subject.Equals("x", "y"), Is.True);
		}

		[TestCase("StartsWithCapital", "startsWithLowerCase")]
		[TestCase("startsWithLowerCase", "StartsWithCapital")]
		public void Equals_OneKeyNull_False(string first, string second)
		{
			Func<string, string> notNullIfStartsWithCapital = x => char.IsUpper(x[0]) ? x : null;

			IEqualityComparer<string> subject = new SelectorEqualizer<string, string>(notNullIfStartsWithCapital);

			Assert.That(subject.Equals(first, second), Is.False);
		}

		[Test]
		public void Equals_OneKeyNull_SelectedEqualsNotCalled()
		{
			EqualitySpy spyForX = new EqualitySpy(), spyForY = new EqualitySpy();
			Func<string, EqualitySpy> spyXForX = s => s.Equals("x") ? spyForX : null;
			Func<string, EqualitySpy> spyYForY = s => s.Equals("y") ? spyForY : null;

			IEqualityComparer<string> subject = new SelectorEqualizer<string, EqualitySpy>(spyXForX);
			subject.Equals("x", "y");
			Assert.That(spyForX.EqualsCalled, Is.False);

			subject = new SelectorEqualizer<string, EqualitySpy>(spyYForY);
			subject.Equals("x", "y");
			Assert.That(spyForY.EqualsCalled, Is.False);
		}

		[Test]
		public void Equals_NoKeyIsNull_KeysCompared()
		{
			IEqualityComparer<string> subject = new SelectorEqualizer<string, string>(s => s.ToLower());

			Assert.That(subject.Equals("AB", "ab"), Is.True);
			Assert.That(subject.Equals("AB", "CD"), Is.False);
		}

		[Test]
		public void Equals_NoKeyIsNull_EqualsInvoked()
		{
			EqualitySpy spyForX = new EqualitySpy(), spyForY = new EqualitySpy();
			Func<string, EqualitySpy> eachSpyToEachLetter = s => s.Equals("x") ? spyForX : spyForY;

			IEqualityComparer<string> subject = new SelectorEqualizer<string, EqualitySpy>(eachSpyToEachLetter);

			subject.Equals("x", "y");
			Assert.That(spyForX.EqualsCalled, Is.True);
			Assert.That(spyForY.EqualsCalled, Is.True);
		}

		#endregion

		[Test]
		public void Chaining_IsDestructive()
		{
			EqualitySubject x1 = new EqualitySubject("x", 1, 1m), x2 = new EqualitySubject("x", 1, 2m);
			ChainableEqualizer<EqualitySubject> sAndI = new SelectorEqualizer<EqualitySubject, string>(s => s.S)
				.Then(s => s.I);

			Assert.That(sAndI.Equals(x1, x2), Is.True);

			var allProp = sAndI.Then(Eq<EqualitySubject>.By(s => s.D));
			Assert.That(allProp.Equals(x1, x2), Is.False);
			Assert.That(sAndI.Equals(x1, x2), Is.False);
		}

		[Test]
		public void Clients_DoNotHaveToCareAboutNulls()
		{
			var notNull = new EqualitySubject("a", 1, 1m);
			var chainable = new SelectorEqualizer<EqualitySubject, int>(s => s.I);

			Assert.That(chainable.Equals(notNull, null), Is.False);
			Assert.That(chainable.Equals(null, notNull), Is.False);
			Assert.That(chainable.Equals(null, null), Is.True);
		}
	}
}
