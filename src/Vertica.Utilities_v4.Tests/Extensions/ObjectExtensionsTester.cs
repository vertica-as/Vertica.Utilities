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
			var notNull = new Exception("msg");
			Assert.That(notNull.Safe(o => o.Message), Is.EqualTo("msg"));
		}

		#region Db

		#region ToBoolean

		[Test]
		public void ToBoolean_DBNull_null()
		{
			object input = DBNull.Value;
			Assert.That(input.Db().ToBoolean(), Is.Null);
		}

		[TestCase(true, true)]
		[TestCase(false, false)]
		[TestCase("true", true)]
		[TestCase("FALSE", false)]
		[TestCase("t", true)]
		[TestCase("F", false)]
		[TestCase("0", false)]
		[TestCase("1", true)]
		[TestCase("nonBoolean", null)]
		[TestCase('t', true)]
		[TestCase('F', false)]
		[TestCase('0', false)]
		[TestCase('1', true)]
		[TestCase('X', null)]
		[TestCase(1, true)]
		[TestCase(0, false)]
		[TestCase(300, null)]
		[TestCase(3.0, null)]
		public void ToBoolean_SpecTest(object input, bool? expected)
		{
			Assert.That(input.Db().ToBoolean(), Is.EqualTo(expected));
		}

		#endregion

		#endregion
	}
}