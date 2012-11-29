using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

using Desc = System.ComponentModel.DescriptionAttribute;

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

		enum MaxEnum : ulong { Max = ulong.MaxValue }

		enum Attributed
		{
			Zero = 0,
			[Desc("Sub-Zero")]
			SubZero = -1,
		}

		[Flags]
		public enum NoZeroFlags : byte
		{
			One = 1,
			Two = 2,
			Three = 4,
			Four = 8
		}

		[Flags]
		public enum ZeroFlags : byte
		{
			Zero = 0,
			One = 1,
			Two = 2,
			Three = 4,
			Four = 8
		}
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
			var undefined = (StringComparison)100;
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
			var undefined = (StringComparison)100;
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
			Assert.That(Enum.GetName(typeof(StringComparison), StringComparison.Ordinal), Is.EqualTo("Ordinal"));
		}

		[Test]
		public void GetName_UndefinedEnumValue_Exception()
		{
			StringComparison undefined = (StringComparison)100;
			Assert.That(Enum.GetName(typeof(StringComparison), undefined), Is.Null);
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
			Assert.That(() => Enumeration.GetUnderlyingType<int>(), Throws.ArgumentException);
		}

		#region values

		[Test]
		public void GetValues_GetsAllValuesAsTyped()
		{
			IEnumerable<StringSplitOptions> values = Enumeration.GetValues<StringSplitOptions>();
			Assert.That(values, Is.EquivalentTo(new[] { StringSplitOptions.None, StringSplitOptions.RemoveEmptyEntries }));
		}

		[Test]
		public void GetValues_NotAnEnum_Exception()
		{
			Assert.That(() => Enumeration.GetValues<int>(), Throws.ArgumentException);
		}

		[Test]
		public void GetValue_DefinedUnderlying_NumericValue()
		{
			byte bValue = Enumeration.GetValue<ByteEnum, byte>(ByteEnum.Two);
			Assert.That(bValue, Is.EqualTo(1));
			sbyte sbValue = Enumeration.GetValue<SByteEnum, sbyte>(SByteEnum.Two);
			Assert.That(sbValue, Is.EqualTo(1));
			short sValue = Enumeration.GetValue<ShortEnum, short>(ShortEnum.Two);
			Assert.That(sValue, Is.EqualTo(1));
			ushort usValue = Enumeration.GetValue<UShortEnum, ushort>(UShortEnum.Two);
			Assert.That(usValue, Is.EqualTo(1));
			int iValue = Enumeration.GetValue<IntEnum, int>(IntEnum.Two);
			Assert.That(iValue, Is.EqualTo(1));
			uint uiValue = Enumeration.GetValue<UIntEnum, uint>(UIntEnum.Two);
			Assert.That(uiValue, Is.EqualTo(1));
			long lValue = Enumeration.GetValue<LongEnum, long>(LongEnum.Two);
			Assert.That(lValue, Is.EqualTo(1));
			ulong ulValue = Enumeration.GetValue<ULongEnum, ulong>(ULongEnum.Two);
			Assert.That(ulValue, Is.EqualTo(1));
		}

		[Test]
		public void GetValue_FittingNotUnderlying_NumericValue()
		{
			decimal dValue = Enumeration.GetValue<LongEnum, decimal>(LongEnum.Two);
			Assert.That(dValue, Is.EqualTo(1));
		}

		[Test]
		public void GetValue_OverflowingNotUnderlying_Exception()
		{
			Assert.That(() => Enumeration.GetValue<MaxEnum, byte>(MaxEnum.Max), Throws.InstanceOf<OverflowException>());
		}

		[Test]
		public void GetValue_Undefined_Exception()
		{
			StringComparison undefined = (StringComparison)100;
			Assert.That(() => Enumeration.GetValue<StringComparison, int>(undefined), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void GetValue_NotAnEnum_Exception()
		{
			Assert.That(() => Enumeration.GetValue<int, byte>(2), Throws.ArgumentException);
		}

		[Test]
		public void TryGetValue_DefinedUnderlying_True()
		{
			byte? bValue;
			Assert.That(Enumeration.TryGetValue(ByteEnum.Two, out bValue), Is.True);
			Assert.That(bValue, Is.EqualTo(1));
			sbyte? sbValue;
			Assert.That(Enumeration.TryGetValue(SByteEnum.Two, out sbValue), Is.True);
			Assert.That(sbValue, Is.EqualTo(1));
			short? sValue;
			Assert.That(Enumeration.TryGetValue(ShortEnum.Two, out sValue), Is.True);
			Assert.That(sValue, Is.EqualTo(1));
			ushort? usValue;
			Assert.That(Enumeration.TryGetValue(UShortEnum.Two, out usValue), Is.True);
			Assert.That(usValue, Is.EqualTo(1));
			int? iValue;
			Assert.That(Enumeration.TryGetValue(IntEnum.Two, out iValue), Is.True);
			Assert.That(iValue, Is.EqualTo(1));
			uint? uiValue;
			Assert.That(Enumeration.TryGetValue(UIntEnum.Two, out uiValue), Is.True);
			Assert.That(uiValue, Is.EqualTo(1));
			long? lValue;
			Assert.That(Enumeration.TryGetValue(LongEnum.Two, out lValue), Is.True);
			Assert.That(lValue, Is.EqualTo(1));
			ulong? ulValue;
			Assert.That(Enumeration.TryGetValue(ULongEnum.Two, out ulValue), Is.True);
			Assert.That(ulValue, Is.EqualTo(1));
		}

		[Test]
		public void TryGetValue_FittingNotUnderlying_True()
		{
			decimal? dValue;
			Assert.That(Enumeration.TryGetValue(LongEnum.Two, out dValue), Is.True);
			Assert.That(dValue, Is.EqualTo(1));
		}

		[Test]
		public void TryGetValue_OverflowingNotUnderlying_False()
		{
			byte? bValue;
			Assert.That(Enumeration.TryGetValue(MaxEnum.Max, out bValue), Is.False);
		}

		[Test]
		public void TryGetValue_Undefined_False()
		{
			var undefined = (StringComparison)100;
			int? iValue;
			Assert.That(Enumeration.TryGetValue(undefined, out iValue), Is.False);
		}

		[Test]
		public void TryGetValue_NotAnEnum_Exception()
		{
			byte? bValue;
			Assert.That(() => Enumeration.TryGetValue(2, out bValue), Throws.ArgumentException);
		}

		#endregion

		#region cast

		[Test]
		public void Cast_DefinedUnderlyingValue_EnumValue()
		{
			Assert.That(Enumeration.Cast<ByteEnum>((byte)1), Is.EqualTo(ByteEnum.Two));
			Assert.That(Enumeration.Cast<SByteEnum>((sbyte)1), Is.EqualTo(SByteEnum.Two));
			Assert.That(Enumeration.Cast<ShortEnum>((short)1), Is.EqualTo(ShortEnum.Two));
			Assert.That(Enumeration.Cast<UShortEnum>((ushort)1), Is.EqualTo(UShortEnum.Two));
			Assert.That(Enumeration.Cast<IntEnum>(1), Is.EqualTo(IntEnum.Two));
			Assert.That(Enumeration.Cast<UIntEnum>(1u), Is.EqualTo(UIntEnum.Two));
			Assert.That(Enumeration.Cast<LongEnum>(1L), Is.EqualTo(LongEnum.Two));
			Assert.That(Enumeration.Cast<ULongEnum>(1UL), Is.EqualTo(ULongEnum.Two));
		}

		[Test]
		public void Cast_NotUnderLying_Exception()
		{
			Assert.That(() => Enumeration.Cast<ByteEnum>(1L), Throws.ArgumentException);
			Assert.That(() => Enumeration.Cast<ByteEnum>(100L), Throws.ArgumentException);
		}

		[Test]
		public void Cast_UndefinedValue_Exception()
		{
			Assert.That(() => Enumeration.Cast<StringComparison>(100), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void TryCast_DefinedUnderlyingValue_True()
		{
			ByteEnum? bValue;
			Assert.That(Enumeration.TryCast((byte)1, out bValue), Is.True);
			Assert.That(bValue, Is.EqualTo(ByteEnum.Two));
			SByteEnum? sbValue;
			Assert.That(Enumeration.TryCast((sbyte)1, out sbValue), Is.True);
			Assert.That(sbValue, Is.EqualTo(SByteEnum.Two));
			ShortEnum? sValue;
			Assert.That(Enumeration.TryCast((short)1, out sValue), Is.True);
			Assert.That(sValue, Is.EqualTo(ShortEnum.Two));
			UShortEnum? usValue;
			Assert.That(Enumeration.TryCast((ushort)1, out usValue), Is.True);
			Assert.That(usValue, Is.EqualTo(UShortEnum.Two));
			IntEnum? iValue;
			Assert.That(Enumeration.TryCast(1, out iValue), Is.True);
			Assert.That(iValue, Is.EqualTo(IntEnum.Two));
			UIntEnum? uiValue;
			Assert.That(Enumeration.TryCast(1u, out uiValue), Is.True);
			Assert.That(uiValue, Is.EqualTo(UIntEnum.Two));
			LongEnum? lValue;
			Assert.That(Enumeration.TryCast(1L, out lValue), Is.True);
			Assert.That(lValue, Is.EqualTo(LongEnum.Two));
			ULongEnum? ulValue;
			Assert.That(Enumeration.TryCast(1UL, out ulValue), Is.True);
			Assert.That(ulValue, Is.EqualTo(ULongEnum.Two));
		}

		[Test]
		public void TryCast_NotUnderLying_Exception()
		{
			ByteEnum? bValue;
			Assert.That(() => Enumeration.TryCast(1L, out bValue), Throws.ArgumentException);
			Assert.That(() => Enumeration.TryCast(100L, out bValue), Throws.ArgumentException);
		}

		[Test]
		public void TryCast_UndefinedValue_False()
		{
			StringComparison? value;
			Assert.That(Enumeration.TryCast(100, out value), Is.False);
		}

		[Test]
		public void Cast_Vs_ToObject()
		{
			Assert.That(Enumeration.Cast<StringComparison>(4), Is.EqualTo(StringComparison.Ordinal));
			Assert.That(Enum.ToObject(typeof(StringComparison), 4), Is.EqualTo(StringComparison.Ordinal));

			Assert.That(() => Enumeration.Cast<StringComparison>(100), Throws.InstanceOf<InvalidEnumArgumentException>());
			Assert.That(Enum.ToObject(typeof(StringComparison), 100), Is.EqualTo((StringComparison)100));

			Assert.That(() => Enumeration.Cast<StringComparison>(4L), Throws.ArgumentException);
			Assert.That(Enum.ToObject(typeof(StringComparison), 4L), Is.EqualTo(StringComparison.Ordinal));
		}

		#endregion

		#region parse

		[Test]
		public void Parse_DefinedValue_Value()
		{
			Assert.That(Enumeration.Parse<PlatformID>("Unix"), Is.EqualTo(PlatformID.Unix));
			Assert.That(Enumeration.Parse<PlatformID>("unIx", true), Is.EqualTo(PlatformID.Unix));
		}

		[Test]
		public void Parse_UndefinedValue_Exception()
		{
			Assert.That(() => Enumeration.Parse<PlatformID>("nonExisting"), Throws.InstanceOf<InvalidEnumArgumentException>());
			Assert.That(() => Enumeration.Parse<PlatformID>("UniX"), Throws.InstanceOf<InvalidEnumArgumentException>());
			Assert.That(() => Enumeration.Parse<PlatformID>("UNIx", false), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void Parse_DefinedNumericValue_Value()
		{
			Assert.That(Enumeration.Parse<StringComparison>("4"), Is.EqualTo(StringComparison.Ordinal));
		}

		[Test]
		public void Parse_Empty_Exception()
		{
			Assert.That(() => Enumeration.Parse<PlatformID>(string.Empty), Throws.InstanceOf<InvalidEnumArgumentException>());
			Assert.That(() => Enumeration.Parse<PlatformID>(null), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void Parse_UndefinedNumericValue_Exception()
		{
			Assert.That(() => Enumeration.Parse<PlatformID>("100"), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void TryParse_DefinedValue_True()
		{
			PlatformID? parsed;
			Assert.That(Enumeration.TryParse("Unix", out parsed), Is.True);
			Assert.That(parsed, Is.EqualTo(PlatformID.Unix));

			Assert.That(Enumeration.TryParse("unIx", true, out parsed), Is.True);
			Assert.That(parsed, Is.EqualTo(PlatformID.Unix));
		}

		[Test]
		public void TryParse_UndefinedValue_False()
		{
			PlatformID? parsed;
			Assert.That(() => Enumeration.TryParse("nonExisting", out parsed), Is.False);
			Assert.That(() => Enumeration.TryParse("UniX", out parsed), Is.False);
			Assert.That(() => Enumeration.TryParse("UNIx", false, out parsed), Is.False);
		}

		[Test]
		public void TryParse_DefinedNumericValue_True()
		{
			StringComparison? parsed;
			Assert.That(Enumeration.TryParse("4", out parsed), Is.True);
			Assert.That(parsed, Is.EqualTo(StringComparison.Ordinal));
		}

		[Test]
		public void TryParse_UndefinedNumericValue_False()
		{
			PlatformID? parsed;
			Assert.That(() => Enumeration.TryParse("100", out parsed), Is.False);
		}

		[Test]
		public void TryParse_Empty_False()
		{
			PlatformID? nonExisting;

			Assert.That(Enumeration.TryParse(string.Empty, out nonExisting), Is.False);
			Assert.That(Enumeration.TryParse(null, out nonExisting), Is.False);
		}

		#endregion

		#region Invert

		[Test]
		public void Invert_NullOrEmpty_OriginalValues()
		{
			var values = Enumeration.GetValues<StringSplitOptions>();
			Assert.That(Enumeration.Invert<StringSplitOptions>(), Is.EqualTo(values));
			Assert.That(Enumeration.Invert((IEnumerable<StringSplitOptions>)null), Is.EqualTo(values));
			Assert.That(Enumeration.Invert(Enumerable.Empty<StringSplitOptions>()), Is.EqualTo(values));
		}

		[Test]
		public void Invert_AllValues_Empty()
		{
			Assert.That(Enumeration.Invert(Enumeration.GetValues<StringSplitOptions>()), Is.Empty);
		}

		[Test]
		public void Invert_SomeValues_RemainingValues()
		{
			Assert.That(Enumeration.Invert(StringSplitOptions.None), Is.EquivalentTo(new[] { StringSplitOptions.RemoveEmptyEntries }));
			Assert.That(Enumeration.Invert(new[] { StringSplitOptions.None }), Is.EquivalentTo(new[] { StringSplitOptions.RemoveEmptyEntries }));
		}

		#endregion

		#region reflection

		#region GetField

		[Test]
		public void GetField_Defined_FieldInfo()
		{
			FieldInfo field = Enumeration.GetField(StringSplitOptions.RemoveEmptyEntries);
			Assert.That(field.Name, Is.EqualTo("RemoveEmptyEntries"));
		}

		[Test]
		public void GetField_Undefined_Null()
		{
			var undefined = (StringSplitOptions)100;
			Assert.That(Enumeration.GetField(undefined), Is.Null);
		}

		[Test]
		public void GetField_NotAnEnum_Exception()
		{
			Assert.That(() => Enumeration.GetField(2m), Throws.ArgumentException);
		}

		#endregion

		#region attributes

		[Test]
		public void HasAttribute_ExistingAttribute_True()
		{
			Assert.That(Enumeration.HasAttribute<Attributed, Desc>(Attributed.SubZero), Is.True);
		}

		[Test]
		public void HasAttributeGetAttribute_NonExistingAttribute_False()
		{
			Assert.That(Enumeration.HasAttribute<Attributed, TestAttribute>(Attributed.SubZero), Is.False);
		}

		[Test]
		public void HAsAttribute_NotAnEnum_Exception()
		{
			Assert.That(() => Enumeration.HasAttribute<decimal, TestAttribute>(1m), Throws.ArgumentException);
		}


		[Test]
		public void GetAttribute_ExistingAttribute_InstanceObtained()
		{
			var existing = Enumeration.GetAttribute<Attributed, Desc>(Attributed.SubZero);
			Assert.That(existing, Is.Not.Null);
			Assert.That(existing.Description, Is.EqualTo("Sub-Zero"));
		}

		[Test]
		public void GetAttribute_NonExistingAttribute_Null()
		{
			var nonExisting = Enumeration.GetAttribute<Attributed, TestAttribute>(Attributed.SubZero);
			Assert.That(nonExisting, Is.Null);
		}

		[Test]
		public void GetAttribute_NotAnEnum_Exception()
		{
			Assert.That(() => Enumeration.GetAttribute<decimal, TestAttribute>(1m), Throws.ArgumentException);
		}

		[Test]
		public void GetDescription_ExistingAttribute_Description()
		{
			Assert.That(Enumeration.GetDescription(Attributed.SubZero), Is.EqualTo("Sub-Zero"));
		}

		[Test]
		public void GetDescription_NonExistingAttribute_Null()
		{
			Assert.That(Enumeration.GetDescription(Attributed.Zero), Is.Null);
		}

		[Test]
		public void GetDescription_NotAnEnum_Exception()
		{
			Assert.That(() => Enumeration.GetDescription(1m), Throws.ArgumentException);
		}

		#endregion

		#endregion

		#region flags

		[Test]
		public void IsFlags_Flagged_True()
		{
			Assert.That(Enumeration.IsFlags<ZeroFlags>(), Is.True);
		}

		[Test]
		public void IsFlags_NotFlagged_False()
		{
			Assert.That(Enumeration.IsFlags<IntEnum>(), Is.False);
		}

		[Test]
		public void IsFlags_NotEnum_False()
		{
			Assert.That(Enumeration.IsFlags<decimal>(), Is.False);
		}

		[Test]
		public void AssertFlags_Flagged_NoException()
		{
			Assert.That(() => Enumeration.AssertFlags<ZeroFlags>(), Throws.Nothing);
		}

		[Test]
		public void AssertFlags_NotFlagged_Exception()
		{
			Assert.That(() => Enumeration.AssertFlags<IntEnum>(), Throws.ArgumentException
				.With.Message.StringContaining("IntEnum"));
		}

		[Test]
		public void AssertFlags_NotEnum_Exception()
		{
			Assert.That(() => Enumeration.AssertFlags<decimal>(), Throws.ArgumentException);
		}

		#region SetFlag

		[Test]
		public void SetFlag_NotSetValue_ValueSet()
		{
			var fourNotSet = NoZeroFlags.Three;

			Assert.That(fourNotSet.HasFlag(NoZeroFlags.Four), Is.False, "does not contain four initially");

			NoZeroFlags fourSet = fourNotSet.SetFlag(NoZeroFlags.Four);

			Assert.That(fourSet.HasFlag(NoZeroFlags.Four), Is.True, "later it does contain four");
		}

		[Test]
		public void SetFlag_AlreadySetValue_ValueLeftAsSet()
		{
			NoZeroFlags fourAlreadySet = NoZeroFlags.Three | NoZeroFlags.Four;

			Assert.That(fourAlreadySet.HasFlag(NoZeroFlags.Four), Is.True, "contains four initially");

			fourAlreadySet = fourAlreadySet.SetFlag(NoZeroFlags.Four);

			Assert.That(fourAlreadySet.HasFlag(NoZeroFlags.Four), Is.True, "still contains four");
		}

		[Test]
		public void SetFlag_DoesNotMutateArgument()
		{
			NoZeroFlags fourNotSet = NoZeroFlags.Three;

			Assert.That(fourNotSet.HasFlag(NoZeroFlags.Four), Is.False, "does not contain four initially");

			fourNotSet.SetFlag(NoZeroFlags.Four); // no assignation

			Assert.That(fourNotSet.HasFlag(NoZeroFlags.Four), Is.False, "no mutation");
		}

		[Test]
		public void SetFlag_Zero_NoChange()
		{
			ZeroFlags twoAndThree = ZeroFlags.Two | ZeroFlags.Three;

			Assert.That(twoAndThree.ToString(), Is.EqualTo("Two, Three"));
			var noneExtra = twoAndThree.SetFlag(ZeroFlags.Zero);
			Assert.That(noneExtra.ToString(), Is.EqualTo("Two, Three"));
		}

		[Test]
		public void SetFlag_NoFlags_Exception()
		{
			Assert.That(() => IntEnum.One.SetFlag(IntEnum.Two), Throws.ArgumentException);
		}

		[Test]
		public void SetFlag_NotEnum_Exception()
		{
			Assert.That(() => 2m.SetFlag(1m), Throws.ArgumentException);
		}

		#endregion

		#region UnsetFlag

		[Test]
		public void UnsetFlag_AlreadySetValue_RemovesItFromSetValues()
		{
			NoZeroFlags fourAlreadySet = NoZeroFlags.Three | NoZeroFlags.Four;

			Assert.That(fourAlreadySet.HasFlag(NoZeroFlags.Four), Is.True);

			var fourUnset = fourAlreadySet.UnsetFlag(NoZeroFlags.Four);

			Assert.That(fourUnset.HasFlag(NoZeroFlags.Four), Is.False);
			Assert.That(fourUnset.HasFlag(NoZeroFlags.Three), Is.True);
		}

		[Test]
		public void UnsetFlag_NotSetValue_LeavesItUnset()
		{
			NoZeroFlags fourNotSet = NoZeroFlags.Three;

			Assert.That(fourNotSet.HasFlag(NoZeroFlags.Four), Is.False);

			var fourUnset = fourNotSet.UnsetFlag(NoZeroFlags.Four);

			Assert.That(fourUnset.HasFlag(NoZeroFlags.Four), Is.False);
			Assert.That(fourUnset.HasFlag(NoZeroFlags.Three), Is.True);
		}

		[Test]
		public void UnsetFlag_DoesNotMutateArgument()
		{
			NoZeroFlags fourAlreadySet = NoZeroFlags.Three | NoZeroFlags.Four;

			Assert.That(fourAlreadySet.HasFlag(NoZeroFlags.Four), Is.True);

			fourAlreadySet.UnsetFlag(NoZeroFlags.Four);

			Assert.That(fourAlreadySet.HasFlag(NoZeroFlags.Four), Is.True);
		}

		[Test]
		public void UnsetFlag_Zero_NoChange()
		{
			ZeroFlags twoAndThree = ZeroFlags.Two | ZeroFlags.Three;

			Assert.That(twoAndThree.ToString(), Is.EqualTo("Two, Three"));

			var unsetNone = twoAndThree.UnsetFlag(ZeroFlags.Zero);
			Assert.That(unsetNone.ToString(), Is.EqualTo("Two, Three"));
		}

		[Test]
		public void UnsetFlag_NoFlags_Exception()
		{
			Assert.That(() => IntEnum.One.UnsetFlag(IntEnum.Two), Throws.ArgumentException);
		}

		[Test]
		public void UnsetFlag_NotEnum_Exception()
		{
			Assert.That(() => 2m.UnsetFlag(1m), Throws.ArgumentException);
		}

		#endregion

		#endregion

	}
}