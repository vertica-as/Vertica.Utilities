using System;
using System.ComponentModel;
using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class EnumerationTester
	{
		#region subjects

		// ReSharper disable UnusedMember.Local
		enum ByteEnum : byte { One, Two }
		enum SByteEnum : sbyte { One, Two }
		enum ShortEnum : short { One, Two }
		enum UShortEnum : ushort { One, Two }
		enum IntEnum : int { One, Two }
		enum UIntEnum : uint { One, Two }
		enum LongEnum : long { One, Two }
		enum ULongEnum : ulong { One, Two }
		// ReSharper restore UnusedMember.Local

		#endregion

		#region checking

		[Test]
		public void IsEnum_EnumType_True()
		{
			Assert.That(Enumeration.IsEnum<PlatformID>(), Is.True);
		}

		[Test]
		public void IsEnum_NotEnumType_False()
		{
			Assert.That(Enumeration.IsEnum<int>(), Is.False);
		}

		[Test]
		public void AssertEnum_EnumType_NoException()
		{
			Assert.That(() => Enumeration.AssertEnum<PlatformID>(), Throws.Nothing);
		}

		[Test]
		public void AssertEnum_NotEnumType_Exception()
		{
			Assert.That(() => Enumeration.AssertEnum<int>(), Throws.InstanceOf<ArgumentException>().With.Message.StringContaining("Int32"));
		}

		#endregion

		#region definition

		#region IsDefined

		[Test]
		public void IsDefined_DefinedEnumValue_True()
		{
			var defined = StringComparison.Ordinal;
			Assert.That(Enumeration.IsDefined(defined), Is.True);
		}

		[Test]
		public void IsDefined_UndefinedEnumValue_False()
		{
			var undefined = (StringComparison) 100;
			Assert.That(Enumeration.IsDefined(undefined), Is.False);
		}

		[Test]
		public void IsDefined_DefinedNumericValue_True()
		{
			byte bValue = 1;
			Assert.That(Enumeration.IsDefined<ByteEnum>(bValue), Is.True);
			sbyte sbValue = 1;
			Assert.That(Enumeration.IsDefined<SByteEnum>(sbValue), Is.True);
			short sValue = 1;
			Assert.That(Enumeration.IsDefined<ShortEnum>(sValue), Is.True);
			ushort usValue = 1;
			Assert.That(Enumeration.IsDefined<UShortEnum>(usValue), Is.True);
			int iValue = 1;
			Assert.That(Enumeration.IsDefined<IntEnum>(iValue), Is.True);
			uint uiValue = 1;
			Assert.That(Enumeration.IsDefined<UIntEnum>(uiValue), Is.True);
			long lValue = 1;
			Assert.That(Enumeration.IsDefined<LongEnum>(lValue), Is.True);
			ulong ulValue = 1;
			Assert.That(Enumeration.IsDefined<ULongEnum>(ulValue), Is.True);
		}

		[Test]
		public void IsDefined_UndefinedNumericValue_False()
		{
			byte bValue = 100;
			Assert.That(Enumeration.IsDefined<ByteEnum>(bValue), Is.False);
			sbyte sbValue = 100;
			Assert.That(Enumeration.IsDefined<SByteEnum>(sbValue), Is.False);
			short sValue = 100;
			Assert.That(Enumeration.IsDefined<ShortEnum>(sValue), Is.False);
			ushort usValue = 100;
			Assert.That(Enumeration.IsDefined<UShortEnum>(usValue), Is.False);
			int iValue = 100;
			Assert.That(Enumeration.IsDefined<IntEnum>(iValue), Is.False);
			uint uiValue = 100;
			Assert.That(Enumeration.IsDefined<UIntEnum>(uiValue), Is.False);
			long lValue = 100;
			Assert.That(Enumeration.IsDefined<LongEnum>(lValue), Is.False);
			ulong ulValue = 100;
			Assert.That(Enumeration.IsDefined<ULongEnum>(ulValue), Is.False);
		}

		[Test]
		public void IsDefined_UnderlyingValueMissmatch_Exception()
		{
			long lValue = 100;
			Assert.That(() => Enumeration.IsDefined<ByteEnum>(lValue), Throws.ArgumentException);
		}

		[Test]
		public void IsDefined_DefinedName_True()
		{
			string defined = "Ordinal";
			Assert.That(Enumeration.IsDefined<StringComparison>(defined), Is.True);
		}

		[Test]
		public void IsDefined_UndefinedName_False()
		{
			string undefined = "undefined";
			Assert.That(Enumeration.IsDefined<StringComparison>(undefined), Is.False);
		}

		[Test]
		public void IsDefined_DefinedWrongCasingName_False()
		{
			string wrongCasing = "ordinal";
			Assert.That(Enumeration.IsDefined<StringComparison>(wrongCasing), Is.False);
			Assert.That(Enumeration.IsDefined<StringComparison>(wrongCasing, false), Is.False);
		}

		[Test]
		public void IsDefined_DefinedWrongCasingNameWhenIgnoredCasing_True()
		{
			string wrongCasing = "ordinal";
			Assert.That(Enumeration.IsDefined<StringComparison>(wrongCasing, true), Is.True);
		}

		[Test]
		public void IsDefined_NotEnumType_Exception()
		{
			Assert.That(() => Enumeration.IsDefined<int>(1), Throws.InstanceOf<ArgumentException>());
		}

		#endregion
		
		#region AssertDefined

		[Test]
		public void AssertDefined_DefinedEnumValue_NoException()
		{
			var defined = StringComparison.Ordinal;
			Assert.That(() => Enumeration.AssertDefined(defined), Throws.Nothing);
		}

		[Test]
		public void AssertDefined_UndefinedEnumValue_Exception()
		{
			var undefined = (StringComparison) 100;
			Assert.That(() => Enumeration.AssertDefined(undefined), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("100").And.With.Message.StringContaining("StringComparison"));
		}

		[Test]
		public void AssertDefined_DefinedNumericValue_NoException()
		{
			byte bValue = 1;
			Assert.That(() => Enumeration.AssertDefined<ByteEnum>(bValue), Throws.Nothing);
			sbyte sbValue = 1;
			Assert.That(() => Enumeration.AssertDefined<SByteEnum>(sbValue), Throws.Nothing);
			short sValue = 1;
			Assert.That(() => Enumeration.AssertDefined<ShortEnum>(sValue), Throws.Nothing);
			ushort usValue = 1;
			Assert.That(() => Enumeration.AssertDefined<UShortEnum>(usValue), Throws.Nothing);
			int iValue = 1;
			Assert.That(() => Enumeration.AssertDefined<IntEnum>(iValue), Throws.Nothing);
			uint uiValue = 1;
			Assert.That(() => Enumeration.AssertDefined<UIntEnum>(uiValue), Throws.Nothing);
			long lValue = 1;
			Assert.That(() => Enumeration.AssertDefined<LongEnum>(lValue), Throws.Nothing);
			ulong ulValue = 1;
			Assert.That(() => Enumeration.AssertDefined<ULongEnum>(ulValue), Throws.Nothing);
		}

		[Test]
		public void AssertDefined_UndefinedNumericValue_Exception()
		{
			byte bValue = 100;
			Assert.That(() => Enumeration.AssertDefined<ByteEnum>(bValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			sbyte sbValue = 100;
			Assert.That(() => Enumeration.AssertDefined<SByteEnum>(sbValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			short sValue = 100;
			Assert.That(() => Enumeration.AssertDefined<ShortEnum>(sValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			ushort usValue = 100;
			Assert.That(() => Enumeration.AssertDefined<UShortEnum>(usValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			int iValue = 100;
			Assert.That(() => Enumeration.AssertDefined<IntEnum>(iValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			uint uiValue = 100;
			Assert.That(() => Enumeration.AssertDefined<UIntEnum>(uiValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			long lValue = 100;
			Assert.That(() => Enumeration.AssertDefined<LongEnum>(lValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			ulong ulValue = 100;
			Assert.That(() => Enumeration.AssertDefined<ULongEnum>(ulValue), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void AssertDefined_UnderlyingValueMissmatch_Exception()
		{
			long lValue = 100;
			Assert.That(() => Enumeration.AssertDefined<ByteEnum>(lValue), Throws.ArgumentException);
		}

		[Test]
		public void AssertDefined_DefinedName_NoException()
		{
			string defined = "Ordinal";
			Assert.That(() => Enumeration.AssertDefined<StringComparison>(defined), Throws.Nothing);
		}

		[Test]
		public void AssertDefined_UndefinedName_Exception()
		{
			string undefined = "undefined";
			Assert.That(() => Enumeration.AssertDefined<StringComparison>(undefined), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void AssertDefined_DefinedWrongCasingName_Exception()
		{
			string wrongCasing = "ordinal";
			Assert.That(() => Enumeration.AssertDefined<StringComparison>(wrongCasing), Throws.InstanceOf<InvalidEnumArgumentException>());
			Assert.That(() => Enumeration.AssertDefined<StringComparison>(wrongCasing, false), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void AssertDefined_DefinedWrongCasingNameWhenIgnoredCasing_NoException()
		{
			string wrongCasing = "ordinal";
			Assert.That(() => Enumeration.AssertDefined<StringComparison>(wrongCasing, true), Throws.Nothing);
		}

		[Test]
		public void AsseryDefined_NotEnumType_Exception()
		{
			Assert.That(() => Enumeration.AssertDefined<int>(1), Throws.InstanceOf<ArgumentException>());
		}

		#endregion

		#endregion

		#region names

		[Test]
		public void GetNames_EnumType_EnumerationOfStrings()
		{
			Assert.That(Enumeration.GetNames<StringComparison>(), Is.EqualTo(Enum.GetNames(typeof(StringComparison))));
		}

		[Test]
		public void GetNames_NotEnumType_Exception()
		{
			Assert.That(() => Enumeration.GetNames<int>(), Throws.InstanceOf<ArgumentException>());
		}

		#region GetName

		[Test]
		public void GetName_DefinedEnumValue_Name()
		{
			Assert.That(Enumeration.GetName(StringComparison.Ordinal), Is.EqualTo("Ordinal"));
			Assert.That(Enum.GetName(typeof (StringComparison), StringComparison.Ordinal), Is.EqualTo("Ordinal"));
		}

		[Test]
		public void GetName_UndefinedEnumValue_Exception()
		{
			StringComparison undefined = (StringComparison) 100;
			Assert.That(Enum.GetName(typeof (StringComparison), undefined), Is.Null);
			Assert.That(() => Enumeration.GetName(undefined), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void GetName_DefinedNumericValue_Name()
		{
			byte bValue = 1;
			Assert.That(Enumeration.GetName<ByteEnum>(bValue), Is.EqualTo("Two"));
			sbyte sbValue = 1;
			Assert.That(Enumeration.GetName<SByteEnum>(sbValue), Is.EqualTo("Two"));
			short sValue = 1;
			Assert.That(Enumeration.GetName<ShortEnum>(sValue), Is.EqualTo("Two"));
			ushort usValue = 1;
			Assert.That(Enumeration.GetName<UShortEnum>(usValue), Is.EqualTo("Two"));
			int iValue = 1;
			Assert.That(Enumeration.GetName<IntEnum>(iValue), Is.EqualTo("Two"));
			uint uiValue = 1;
			Assert.That(Enumeration.GetName<UIntEnum>(uiValue), Is.EqualTo("Two"));
			long lValue = 1;
			Assert.That(Enumeration.GetName<LongEnum>(lValue), Is.EqualTo("Two"));
			ulong ulValue = 1;
			Assert.That(Enumeration.GetName<ULongEnum>(ulValue), Is.EqualTo("Two"));
		}

		[Test]
		public void GetName_UndefinedNumericValue_Exception()
		{
			byte bValue = 100;
			Assert.That(() => Enumeration.GetName<ByteEnum>(bValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			sbyte sbValue = 100;
			Assert.That(() => Enumeration.GetName<SByteEnum>(sbValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			short sValue = 100;
			Assert.That(() => Enumeration.GetName<ShortEnum>(sValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			ushort usValue = 100;
			Assert.That(() => Enumeration.GetName<UShortEnum>(usValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			int iValue = 100;
			Assert.That(() => Enumeration.GetName<IntEnum>(iValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			uint uiValue = 100;
			Assert.That(() => Enumeration.GetName<UIntEnum>(uiValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			long lValue = 100;
			Assert.That(() => Enumeration.GetName<LongEnum>(lValue), Throws.InstanceOf<InvalidEnumArgumentException>());
			ulong ulValue = 100;
			Assert.That(() => Enumeration.GetName<ULongEnum>(ulValue), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void GetName_NotEnumType_Exception()
		{
			Assert.That(() => Enumeration.GetName<int>(1), Throws.InstanceOf<ArgumentException>());
		}

		#endregion

		#region TryGetName

		[Test]
		public void TryGetName_DefinedEnumValue_Name()
		{
			string name;
			Assert.That(Enumeration.TryGetName(StringComparison.Ordinal, out name), Is.True);
			Assert.That(name, Is.EqualTo("Ordinal"));
		}

		[Test]
		public void TryGetName_UndefinedEnumValue_False()
		{
			string name;
			StringComparison undefined = (StringComparison)100;
			Assert.That(() => Enumeration.TryGetName(undefined, out name), Is.False);
		}

		[Test]
		public void TryGetName_DefinedNumericValue_True()
		{
			string name = null;

			byte bValue = 1;
			Assert.That(Enumeration.TryGetName<ByteEnum>(bValue, out name), Is.True);
			Assert.That(name, Is.EqualTo("Two"));
			sbyte sbValue = 1;
			Assert.That(Enumeration.TryGetName<SByteEnum>(sbValue, out name), Is.True);
			Assert.That(name, Is.EqualTo("Two"));
			short sValue = 1;
			Assert.That(Enumeration.TryGetName<ShortEnum>(sValue, out name), Is.True);
			Assert.That(name, Is.EqualTo("Two"));
			ushort usValue = 1;
			Assert.That(Enumeration.TryGetName<UShortEnum>(usValue, out name), Is.True);
			Assert.That(name, Is.EqualTo("Two"));
			int iValue = 1;
			Assert.That(Enumeration.TryGetName<IntEnum>(iValue, out name), Is.True);
			Assert.That(name, Is.EqualTo("Two"));
			uint uiValue = 1;
			Assert.That(Enumeration.TryGetName<UIntEnum>(uiValue, out name), Is.True);
			Assert.That(name, Is.EqualTo("Two"));
			long lValue = 1;
			Assert.That(Enumeration.TryGetName<LongEnum>(lValue, out name), Is.True);
			Assert.That(name, Is.EqualTo("Two"));
			ulong ulValue = 1;
			Assert.That(Enumeration.TryGetName<ULongEnum>(ulValue, out name), Is.True);
			Assert.That(name, Is.EqualTo("Two"));
		}

		[Test]
		public void TryGetName_UndefinedNumericValue_False()
		{
			string name;

			byte bValue = 100;
			Assert.That(() => Enumeration.TryGetName<ByteEnum>(bValue, out name), Is.False);
			sbyte sbValue = 100;
			Assert.That(() => Enumeration.TryGetName<SByteEnum>(sbValue, out name), Is.False);
			short sValue = 100;
			Assert.That(() => Enumeration.TryGetName<ShortEnum>(sValue, out name), Is.False);
			ushort usValue = 100;
			Assert.That(() => Enumeration.TryGetName<UShortEnum>(usValue, out name), Is.False);
			int iValue = 100;
			Assert.That(() => Enumeration.TryGetName<IntEnum>(iValue, out name), Is.False);
			uint uiValue = 100;
			Assert.That(() => Enumeration.TryGetName<UIntEnum>(uiValue, out name), Is.False);
			long lValue = 100;
			Assert.That(() => Enumeration.TryGetName<LongEnum>(lValue, out name), Is.False);
			ulong ulValue = 100;
			Assert.That(() => Enumeration.TryGetName<ULongEnum>(ulValue, out name), Is.False);
		}

		[Test]
		public void TryGetName_NotEnumType_Exception()
		{
			string name;

			Assert.That(() => Enumeration.TryGetName<int>(1, out name), Throws.InstanceOf<ArgumentException>());
		}

		#endregion

		#endregion

		[Test]
		public void GetUnderlyingType_CorrectType()
		{
			Assert.That(Enumeration.GetUnderlyingType<ByteEnum>(), Is.EqualTo(typeof(byte)));
			Assert.That(Enumeration.GetUnderlyingType<SByteEnum>(), Is.EqualTo(typeof(sbyte)));
			Assert.That(Enumeration.GetUnderlyingType<ShortEnum>(), Is.EqualTo(typeof(short)));
			Assert.That(Enumeration.GetUnderlyingType<UShortEnum>(), Is.EqualTo(typeof(ushort)));
			Assert.That(Enumeration.GetUnderlyingType<IntEnum>(), Is.EqualTo(typeof(int)));
			Assert.That(Enumeration.GetUnderlyingType<UIntEnum>(), Is.EqualTo(typeof(uint)));
			Assert.That(Enumeration.GetUnderlyingType<LongEnum>(), Is.EqualTo(typeof(long)));
			Assert.That(Enumeration.GetUnderlyingType<ULongEnum>(), Is.EqualTo(typeof(ulong)));
		}

		[Test]
		public void GetUnderlyingType_NotAnEnum_Exception()
		{
			Assert.That(()=>Enumeration.GetUnderlyingType<int>(), Throws.ArgumentException);			
		}

	}
}