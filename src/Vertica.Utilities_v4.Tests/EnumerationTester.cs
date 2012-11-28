using System;
using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class EnumerationTester
	{
		[Test]
		public void IsEnum_EnumType_True()
		{
			Assert.That(Enumeration.IsEnum<PlatformID>(), Is.True);
			Assert.That(Enumeration.IsEnum(typeof(PlatformID)), Is.True);
		}
	}
}