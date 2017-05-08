using NUnit.Framework;
using Vertica.Utilities_v4.Extensions.DomainExt;

namespace Vertica.Utilities_v4.Tests.Extensions
{
	[TestFixture]
	public class DomainOfValuesExtensionsTester
	{
		[Test]
		public void CheckAgainst_ExistingValueWithinDomain_True()
		{
			Assert.That(3.CheckAgainst(2, 3, 4), Is.True);
		}

		[Test]
		public void CheckAgainst_MissingValueWithinDomain_False()
		{
			Assert.That(5.CheckAgainst(2, 3, 4), Is.False);
		}

		[Test]
		public void AssertAgainst_ExistingValueWithinDomain_NoException()
		{
			Assert.That(() => 3.AssertAgainst(2, 3, 4), Throws.Nothing);
		}

		[Test]
		public void AssertAgainst_MissingValueWithinDomain_False()
		{
			Assert.That(() => 5.AssertAgainst(2, 3, 4), Throws.InstanceOf<InvalidDomainException<int>>());
		}
	}
}