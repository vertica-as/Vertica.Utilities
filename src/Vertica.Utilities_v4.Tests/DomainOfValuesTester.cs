using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class DomainOfValuesTester
	{
		#region creation

		[Test]
		public void Ctor_Params_BuildsEnumerableWrapper()
		{
			var twoThreeFour = new DomainOfValues<int>(2, 3, 4);
			Assert.That(twoThreeFour, Is.EqualTo(new[] { 2, 3, 4 }));
		}

		[Test]
		public void Ctor_EmptyParams_BuildsEmptyWrapper()
		{
			var twoThreeFour = new DomainOfValues<int>();
			Assert.That(twoThreeFour, Is.Empty);
		}

		[Test]
		public void Ctor_Enumerable_BuildsEnumerableWrapper()
		{
			var twoThreeFour = new DomainOfValues<int>(new[] { 2, 3, 4 });
			Assert.That(twoThreeFour, Is.EqualTo(new[] { 2, 3, 4 }));
		}

		[Test]
		public void Ctor_EmptyEnumerable_BuildsEmptyWrapper()
		{
			var twoThreeFour = new DomainOfValues<int>(new int[0]);
			Assert.That(twoThreeFour, Is.Empty);
		}

		[Test]
		public void Ctor_NullEnumerable_BuildsEmptyWrapper()
		{
			var twoThreeFour = new DomainOfValues<int>(null);
			Assert.That(twoThreeFour, Is.Empty);
		}

		[Test]
		public void TypeInferenceHelper_Params_BuildsEnumerableWrapper()
		{
			DomainOfValues<int> twoThreeFour = DomainOf.Values(2, 3, 4);
			Assert.That(twoThreeFour, Is.EqualTo(new[] { 2, 3, 4 }));
		}

		[Test]
		public void TypeInferenceHelper_EmptyParams_BuildsEmptyWrapper()
		{
			DomainOfValues<int> twoThreeFour = DomainOf.Values<int>();
			Assert.That(twoThreeFour, Is.Empty);
		}

		[Test]
		public void TypeInferenceHelper_Enumerable_BuildsEnumerableWrapper()
		{
			DomainOfValues<int> twoThreeFour = DomainOf.Values(new[] { 2, 3, 4 });
			Assert.That(twoThreeFour, Is.EqualTo(new[] { 2, 3, 4 }));
		}

		[Test]
		public void TypeInferenceHelper_EmptyEnumerable_BuildsEmptyWrapper()
		{
			DomainOfValues<int> twoThreeFour = DomainOf.Values(new int[0]);
			Assert.That(twoThreeFour, Is.Empty);
		}

		[Test]
		public void TypeInferenceHelper_NullEnumerable_BuildsEmptyWrapper()
		{
			DomainOfValues<int> twoThreeFour = DomainOf.Values<int>(null);
			Assert.That(twoThreeFour, Is.Empty);
		}

		#endregion

		#region Check

		[Test]
		public void CheckContains_ContainedValueType_True()
		{
			var twoThreeFour = DomainOf.Values(2, 3, 4);
			Assert.That(twoThreeFour.CheckContains(2), Is.True);
		}

		[Test]
		public void CheckContains_ContainedReferenceType_True()
		{
			var twoThreeFour = DomainOf.Values("2", "3", "4");
			Assert.That(twoThreeFour.CheckContains("2"), Is.True);
		}

		[Test]
		public void CheckContains_NotContainedValueType_False()
		{
			var twoThreeFour = DomainOf.Values(2, 3, 4);
			Assert.That(twoThreeFour.CheckContains(5), Is.False);
		}

		[Test]
		public void CheckContains_NotContainedReferenceType_False()
		{
			var twoThreeFour = DomainOf.Values("2", "3", "4");
			Assert.That(twoThreeFour.CheckContains("5"), Is.False);
		}

		#endregion

		#region Assert

		[Test]
		public void AssertContains_ContainedValueType_NoException()
		{
			var twoThreeFour = DomainOf.Values(2, 3, 4);
			Assert.That(() => twoThreeFour.AssertContains(2), Throws.Nothing);
		}

		[Test]
		public void AssertContains_ContainedReferenceType_True()
		{
			var twoThreeFour = DomainOf.Values("2", "3", "4");
			Assert.That(() => twoThreeFour.AssertContains("2"), Throws.Nothing);
		}

		[Test]
		public void AssertContains_NotContainedValueType_Exception()
		{
			var twoThreeFour = DomainOf.Values(2, 3, 4);
			Assert.That(() => twoThreeFour.AssertContains(5), Throws.InstanceOf<InvalidDomainException<int>>());
		}

		[Test]
		public void AssertContains_NotContainedReferenceType_False()
		{
			var twoThreeFour = DomainOf.Values("2", "3", "4");
			Assert.That(()=>twoThreeFour.AssertContains("5"), Throws.InstanceOf<InvalidDomainException<string>>());
		}

		[Test]
		public void AssertContains_NotContained_ExceptionShowsCurrentValueAndValuesInDomain()
		{
			var twoThreeFour = DomainOf.Values(2, 3, 4);
			Assert.That(() => twoThreeFour.AssertContains(5),
				Throws.InstanceOf<InvalidDomainException<int>>()
					.With.Message.StringContaining("5").And
					.Message.StringContaining("[2, 3, 4]"));
		}

		#endregion

	}
}