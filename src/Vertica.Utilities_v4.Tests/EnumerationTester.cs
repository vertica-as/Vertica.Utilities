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

		[Test]
		public void IsEnum_NotEnumType_False()
		{
			Assert.That(Enumeration.IsEnum<int>(), Is.False);
			Assert.That(Enumeration.IsEnum(typeof(Exception)), Is.False);
		}

		[Test]
		public void AssertEnum_EnumType_NoException()
		{
			Assert.That(() => Enumeration.AssertEnum<PlatformID>(), Throws.Nothing);
			Assert.That(() => Enumeration.AssertEnum(typeof(PlatformID)), Throws.Nothing);
		}

		[Test]
		public void AssertEnum_NotEnumType_Exception()
		{
			Assert.That(() => Enumeration.AssertEnum<int>(), Throws.InstanceOf<ArgumentException>()
				.With.Message.StringContaining("Int32"));
			Assert.That(() => Enumeration.AssertEnum(typeof(Exception)), Throws.InstanceOf<ArgumentException>()
				.With.Message.StringContaining("Exception"));
		}
	}
}