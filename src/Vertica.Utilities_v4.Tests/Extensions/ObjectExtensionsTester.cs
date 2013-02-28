using System;
using NUnit.Framework;
using Vertica.Utilities_v4.Extensions.ObjectExt;
using Vertica.Utilities_v4.Tests.Extensions.Support;

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

		#region Unbox

		#region Unbox

		[Test]
		public void Unbox_NoConversion_SameValue()
		{
			Assert.That(3.Db().Unbox<int>(), Is.EqualTo(3));
		}

		[Test]
		public void Unbox_SafeConversion_ExpectedValue()
		{
			const short maxValue = short.MaxValue;
			Assert.That((maxValue + 10).Db().Unbox<int>(), Is.EqualTo(maxValue + 10));
		}

		[Test]
		public void Unbox_NullArgument_Null()
		{
			object o = null;
			Assert.That(o.Db().Unbox<short>(), Is.Null);
		}

		[Test]
		public void Unbox_UnSafeConversionNoOverflow_ExpectedValue()
		{
			const int maxValue = short.MaxValue - 10;
			Assert.That(maxValue.Db().Unbox<short>(), Is.EqualTo(maxValue));
		}

		[Test]
		public void Unbox_UnSafeConversionOverFlow_Null()
		{
			const int maxValue = short.MaxValue + 10;
			Assert.That(maxValue.Db().Unbox<short>(), Is.Null);
		}


		[TestCase(3.5, 4)]
		[TestCase(3.9, 4)]
		[TestCase(3.49, 3)]
		[TestCase(3.1, 3)]
		[TestCase(3.0000001, 3)]
		public void Unbox_DifferentTypesRound_RoundedUpOrLow(double rational, int roundedValue)
		{
			Assert.That(rational.Db().Unbox<int>(), Is.EqualTo(roundedValue));
		}

		[Test]
		public void Unbox_NoConversionPossible_Null()
		{
			Assert.That("s".Db().Unbox<short>(), Is.Null);
		}

		[Test]
		public void Unbox_NotIConvertible_Null()
		{
			Assert.That("whatever".Db().Unbox<NotIConvertible>(), Is.Null);
		}

		#endregion

		#endregion

		#endregion
	}
}