using NUnit.Framework;
using Vertica.Utilities_v4.Tests.Extensions.Infrastructure.Support;

namespace Vertica.Utilities_v4.Tests.Extensions.Infrastructure
{
	[TestFixture, Category("Exploratory")]
	public class ExtensionPointsTester
	{
		[Test]
		public void MyExtensions_AreScoped_AndCanBeUsedOnAnyType()
		{
			string echoed = "echo?".My().Echo();
			Assert.That(echoed, Is.EqualTo("echo?echo?"));
		}

		[Test]
		public void MyExtensions_AreScoped_AndCanBeUsedOnTheConstrainedType()
		{
			decimal doubledUp = 3m.My().DoubleUp();
			Assert.That(doubledUp, Is.EqualTo(6m));
		}
	}
}