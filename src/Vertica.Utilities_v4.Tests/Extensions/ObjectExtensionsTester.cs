using System;
using NUnit.Framework;
using Vertica.Utilities_v4.Extensions.ObjectExt;

namespace Vertica.Utilities_v4.Tests.Extensions
{
	[TestFixture]
	public class ObjectExtensionsTester
	{
		[Test]
		public void Safe_NullInstance_Null()
		{
			Exception @null = null;
			Assert.That(@null.Safe(o => o.Message), Is.Null);
		}

		[Test]
		public void Safe_NotNullInstance_FunctionExecuted()
		{
			Exception notNull = new Exception("msg");
			Assert.That(notNull.Safe(o => o.Message), Is.EqualTo("msg"));
		}
	}
}