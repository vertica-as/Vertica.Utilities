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

		#region Unbox

		[Test]
		public void Unbox_NoConversion_SameValue()
		{
			Assert.That(3.Unbox<int>(), Is.EqualTo(3));
		}

		[Test]
		public void Unbox_SafeConversion_ExpectedValue()
		{
			const short maxValue = short.MaxValue;
			Assert.That((maxValue + 10).Unbox<int>(), Is.EqualTo(maxValue + 10));
		}

		[Test]
		public void Unbox_NullArgument_Null()
		{
			object o = null;
			Assert.That(o.Unbox<short>(), Is.Null);
		}

		[Test]
		public void Unbox_UnSafeConversionNoOverflow_ExpectedValue()
		{
			const int maxValue = short.MaxValue - 10;
			Assert.That(maxValue.Unbox<short>(), Is.EqualTo(maxValue));
		}

		[Test]
		public void Unbox_UnSafeConversionOverFlow_Null()
		{
			const int maxValue = short.MaxValue + 10;
			Assert.That(maxValue.Unbox<short>(), Is.Null);
		}


		[TestCase(3.5, 4)]
		[TestCase(3.9, 4)]
		[TestCase(3.49, 3)]
		[TestCase(3.1, 3)]
		[TestCase(3.0000001, 3)]
		public void Unbox_DifferentTypesRound_RoundedUpOrLow(double rational, int roundedValue)
		{
			Assert.That(rational.Unbox<int>(), Is.EqualTo(roundedValue));
		}

		[Test]
		public void Unbox_NoConversionPossible_Null()
		{
			Assert.That("s".Unbox<short>(), Is.Null);
		}

		[Test]
		public void Unbox_NotIConvertible_Null()
		{
			Assert.That("whatever".Unbox<NotIConvertible>(), Is.Null);
		}

		#endregion

		#region UnboxBool

		[Test]
		public void UnboxBool_null_null()
		{
			Assert.That(DBNull.Value.UnboxBool(), Is.Null);
			object @null = null;
			Assert.That(@null.UnboxBool(), Is.Null);
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
		public void ToUnboxBool_SpecTest(object input, bool? expected)
		{
			Assert.That(input.UnboxBool(), Is.EqualTo(expected));
		}

		#endregion

		#region IsBoxedDefault

		[Test]
		public void IsBoxedDefault_NullReferenceTypes_True()
		{
			Exception ex = null;
			Assert.That(ex.IsBoxedDefault(), Is.True);
			object o = ex;
			Assert.That(o.IsBoxedDefault(), Is.True);
		}

		[Test]
		public void IsBoxedDefault_NotNullReferenceTypes_False()
		{
			var ex = new Exception();
			Assert.That(ex.IsBoxedDefault(), Is.False);
			object o = ex;
			Assert.That(o.IsBoxedDefault(), Is.False);
			o = new DescriptionAttribute(null);
			Assert.That(o.IsBoxedDefault(), Is.False);
		}

		[Test]
		public void IsBoxedDefault_NullNullableTypes_True()
		{
			int? i = null;
			Assert.That(i.IsBoxedDefault(), Is.True);
			object o = i;
			Assert.That(o.IsBoxedDefault(), Is.True);
		}

		[Test]
		public void IsBoxedDefault_NotNullNonDefaultNullableTypes_False()
		{
			int? i = 3;
			Assert.That(i.IsBoxedDefault(), Is.False);
			object o = i;
			Assert.That(o.IsBoxedDefault(), Is.False);
		}

		[Test]
		public void IsBoxedDefault_NotNullDefaultNullableTypes_True()
		{
			int? i = 0;
			Assert.That(i.IsBoxedDefault(), Is.True);
			object o = i;
			Assert.That(o.IsBoxedDefault(), Is.True);
		}

		[Test]
		public void IsBoxedDefault_NonDefaultValueTypes_False()
		{
			int i = 3;
			Assert.That(i.IsBoxedDefault(), Is.False);
			object o = i;
			Assert.That(o.IsBoxedDefault(), Is.False);
		}

		[Test]
		public void IsBoxedDefault_DefaultValueTypes_True()
		{
			int i = 0;
			Assert.That(i.IsBoxedDefault(), Is.True);
			object o = i;
			Assert.That(o.IsBoxedDefault(), Is.True);
		}

		[Test]
		public void IsBoxedDefault_NonDefaultEnums_False()
		{
			var e1 = NonZeroEnum.One;
			Assert.That(e1.IsBoxedDefault(), Is.False);
			object o = e1;
			Assert.That(o.IsBoxedDefault(), Is.False);

			var e2 = ZeroEnum.Two;
			Assert.That(e2.IsBoxedDefault(), Is.False);
			o = e2;
			Assert.That(o.IsBoxedDefault(), Is.False);
		}

		[Test]
		public void IsBoxedDefault_DefaultEnums_TrueWhenZeroBased()
		{
			var e1 = NonZeroEnum.Zero;
			Assert.That(e1.IsBoxedDefault(), Is.False);
			object o = e1;
			Assert.That(o.IsBoxedDefault(), Is.False);
			e1 = default(NonZeroEnum);
			Assert.That(e1.IsBoxedDefault(), Is.True);

			var e2 = ZeroEnum.Zero;
			Assert.That(e2.IsBoxedDefault(), Is.True);
			o = e2;
			Assert.That(o.IsBoxedDefault(), Is.True);
		}

		#endregion

		#region IsDefault/IsNotDefault

		[Test]
		public void IsDefault_ReferenceType_TrueWhenNull()
		{
			string s = null;
			Assert.That(s.IsDefault(), Is.True);
			Assert.That(s.IsNotDefault(), Is.False);
			Exception ex = null;
			Assert.That(ex.IsDefault(), Is.True);
			Assert.That(ex.IsNotDefault(), Is.False);

			Assert.That(string.Empty.IsDefault(), Is.False);
			Assert.That(string.Empty.IsNotDefault(), Is.True);
			ex = new Exception();
			Assert.That(ex.IsDefault(), Is.False);
			Assert.That(ex.IsNotDefault(), Is.True);
		}

		[Test]
		public void IsDefault_ValueType_TrueWhenDefault()
		{
			int i = default(int);
			Assert.That(i.IsDefault(), Is.True);
			Assert.That(i.IsNotDefault(), Is.False);

			Guid g = default(Guid);
			Assert.That(g.IsDefault(), Is.True);
			Assert.That(g.IsNotDefault(), Is.False);

			i = 3;
			Assert.That(i.IsDefault(), Is.False);
			Assert.That(i.IsNotDefault(), Is.True);
			g = new Guid("{E4878620-577E-46fe-97D8-BE0840B3C85D}");
			Assert.That(g.IsDefault(), Is.False);
			Assert.That(g.IsNotDefault(), Is.True);
		}

		[Test]
		public void IsDefault_NullableTypes_TrueWhenNull()
		{
			int? i = null;
			Assert.That(i.IsDefault(), Is.True);
			Assert.That(i.IsNotDefault(), Is.False);

			Guid? g = null;
			Assert.That(g.IsDefault(), Is.True);
			Assert.That(g.IsNotDefault(), Is.False);

			i = default(int);
			Assert.That(i.IsDefault(), Is.False);
			Assert.That(i.IsNotDefault(), Is.True);
			g = default(Guid);
			Assert.That(g.IsDefault(), Is.False);
			Assert.That(g.IsNotDefault(), Is.True);
		}

		[Test]
		public void IsDefault_Enumerations_TrueWhenDefault()
		{
			var a = default(AttributeTargets);
			Assert.That((int)a, Is.EqualTo(0));
			Assert.That(a.IsDefault(), Is.True);
			Assert.That(a.IsNotDefault(), Is.False);

			var zeroEnum = default(ZeroEnum);
			Assert.That((int)zeroEnum, Is.EqualTo(0));
			Assert.That(zeroEnum.IsDefault(), Is.True);
			Assert.That(zeroEnum.IsNotDefault(), Is.False);

			var nonZeroEnum = default(NonZeroEnum);
			Assert.That((int)nonZeroEnum, Is.EqualTo(0));
			Assert.That(nonZeroEnum.IsDefault(), Is.True);
			Assert.That(nonZeroEnum.IsNotDefault(), Is.False);

			a = AttributeTargets.Assembly;
			Assert.That(a.IsDefault(), Is.False);
			Assert.That(a.IsNotDefault(), Is.True);

			zeroEnum = ZeroEnum.Zero;
			Assert.That(zeroEnum.IsDefault(), Is.True);
			Assert.That(zeroEnum.IsNotDefault(), Is.False);

			zeroEnum = ZeroEnum.One;
			Assert.That(zeroEnum.IsDefault(), Is.False);
			Assert.That(zeroEnum.IsNotDefault(), Is.True);

			nonZeroEnum = NonZeroEnum.Zero;
			Assert.That(nonZeroEnum.IsDefault(), Is.False);
			Assert.That(nonZeroEnum.IsNotDefault(), Is.True);
		}

		[Test]
		public void IsDefault_NullableEnumerations_TrueWhenNull()
		{
			AttributeTargets? a = default(AttributeTargets?);
			Assert.That(a, Is.Null);
			Assert.That(a.IsDefault(), Is.True);
			Assert.That(a.IsNotDefault(), Is.False);

			ZeroEnum? zeroEnum = default(ZeroEnum?);
			Assert.That(zeroEnum, Is.Null);
			Assert.That(zeroEnum.IsDefault(), Is.True);
			Assert.That(zeroEnum.IsNotDefault(), Is.False);

			NonZeroEnum? nonZeroEnum = default(NonZeroEnum?);
			Assert.That(nonZeroEnum, Is.Null);
			Assert.That(nonZeroEnum.IsDefault(), Is.True);
			Assert.That(nonZeroEnum.IsNotDefault(), Is.False);

			a = 0;
			Assert.That(a.IsDefault(), Is.False);
			Assert.That(a.IsNotDefault(), Is.True);

			zeroEnum = 0;
			Assert.That(zeroEnum.IsDefault(), Is.False);
			Assert.That(zeroEnum.IsNotDefault(), Is.True);

			nonZeroEnum = NonZeroEnum.Zero;
			Assert.That(nonZeroEnum.IsDefault(), Is.False);
			Assert.That(nonZeroEnum.IsNotDefault(), Is.True);
		}

		#endregion
	}
}