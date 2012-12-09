﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Vertica.Utilities_v4.Resources;

namespace Vertica.Utilities_v4
{
	//* 
	//* Based upon http://rodenbaugh.net/archive/2006/11/01/Enum-Helper-Class-Using-Generics.aspx
	//* 
	public static class Enumeration
	{
		#region checking

		public static bool IsEnum<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return typeof(TEnum).IsEnum;
		}

		public static void AssertEnum<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!IsEnum<TEnum>())
			{
				ExceptionHelper.Throw<ArgumentException>(Exceptions.Enumeration_NotEnumTemplate, typeof(TEnum).ToString());
			}
		}

		#endregion

		#region definition

		/// <summary>
		/// Returns an indication whether a constant with a specified value exists in a specified enumeration.
		/// </summary>
		public static bool IsDefined<TEnum>(TEnum value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertEnum<TEnum>();
			return Enum.IsDefined(typeof(TEnum), value);
		}

		/// <summary>
		/// Returns an indication whether a constant with a specified value exists in a specified enumeration.
		/// </summary>
		public static bool IsDefined<TEnum>(byte value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return isDefined<TEnum, byte>(value);
		}

		/// <summary>
		/// Returns an indication whether a constant with a specified value exists in a specified enumeration.
		/// </summary>
		public static bool IsDefined<TEnum>(sbyte value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return isDefined<TEnum, sbyte>(value);
		}

		/// <summary>
		/// Returns an indication whether a constant with a specified value exists in a specified enumeration.
		/// </summary>
		public static bool IsDefined<TEnum>(short value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return isDefined<TEnum, short>(value);
		}

		/// <summary>
		/// Returns an indication whether a constant with a specified value exists in a specified enumeration.
		/// </summary>
		public static bool IsDefined<TEnum>(ushort value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return isDefined<TEnum, ushort>(value);
		}

		/// <summary>
		/// Returns an indication whether a constant with a specified value exists in a specified enumeration.
		/// </summary>
		public static bool IsDefined<TEnum>(int value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return isDefined<TEnum, int>(value);
		}

		/// <summary>
		/// Returns an indication whether a constant with a specified value exists in a specified enumeration.
		/// </summary>
		public static bool IsDefined<TEnum>(uint value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return isDefined<TEnum, uint>(value);
		}

		/// <summary>
		/// Returns an indication whether a constant with a specified value exists in a specified enumeration.
		/// </summary>
		public static bool IsDefined<TEnum>(long value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return isDefined<TEnum, long>(value);
		}

		/// <summary>
		/// Returns an indication whether a constant with a specified value exists in a specified enumeration.
		/// </summary>
		public static bool IsDefined<TEnum>(ulong value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return isDefined<TEnum, ulong>(value);
		}

		private static bool isDefined<TEnum, U>(U value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
			where U : struct, IComparable, IFormattable, IConvertible
		{
			AssertEnum<TEnum>();
			return Enum.IsDefined(typeof(TEnum), value);
		}

		/// <summary>
		/// Returns an indication whether a constant with a specified name exists in a specified enumeration.
		/// </summary>
		public static bool IsDefined<TEnum>(string name, bool ignoreCase = false) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertEnum<TEnum>();
			return !ignoreCase ?
				Enum.IsDefined(typeof(TEnum), name) :
				GetNames<TEnum>().Contains(name, StringComparer.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Throws an instance of <see cref="InvalidEnumArgumentException"/> when the specified value does not exist in a specified enumeration.
		/// </summary>
		public static void AssertDefined<TEnum>(TEnum value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!IsDefined(value)) throwNotDefined<TEnum, TEnum>(value);
		}

		/// <summary>
		/// Throws an instance of <see cref="InvalidEnumArgumentException"/> when the specified value does not exist in a specified enumeration.
		/// </summary>
		public static void AssertDefined<TEnum>(byte value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!IsDefined<TEnum>(value)) throwNotDefined<TEnum, byte>(value);
		}

		/// <summary>
		/// Throws an instance of <see cref="InvalidEnumArgumentException"/> when the specified value does not exist in a specified enumeration.
		/// </summary>
		public static void AssertDefined<TEnum>(sbyte value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!IsDefined<TEnum>(value)) throwNotDefined<TEnum, sbyte>(value);
		}

		/// <summary>
		/// Throws an instance of <see cref="InvalidEnumArgumentException"/> when the specified value does not exist in a specified enumeration.
		/// </summary>
		public static void AssertDefined<TEnum>(short value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!IsDefined<TEnum>(value)) throwNotDefined<TEnum, short>(value);
		}

		/// <summary>
		/// Throws an instance of <see cref="InvalidEnumArgumentException"/> when the specified value does not exist in a specified enumeration.
		/// </summary>
		public static void AssertDefined<TEnum>(ushort value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!IsDefined<TEnum>(value)) throwNotDefined<TEnum, ushort>(value);
		}

		/// <summary>
		/// Throws an instance of <see cref="InvalidEnumArgumentException"/> when the specified value does not exist in a specified enumeration.
		/// </summary>
		public static void AssertDefined<TEnum>(int value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!IsDefined<TEnum>(value)) throwNotDefined<TEnum, int>(value);
		}

		/// <summary>
		/// Throws an instance of <see cref="InvalidEnumArgumentException"/> when the specified value does not exist in a specified enumeration.
		/// </summary>
		public static void AssertDefined<TEnum>(uint value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!IsDefined<TEnum>(value)) throwNotDefined<TEnum, uint>(value);
		}

		/// <summary>
		/// Throws an instance of <see cref="InvalidEnumArgumentException"/> when the specified value does not exist in a specified enumeration.
		/// </summary>
		public static void AssertDefined<TEnum>(long value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!IsDefined<TEnum>(value)) throwNotDefined<TEnum, long>(value);
		}

		/// <summary>
		/// Throws an instance of <see cref="InvalidEnumArgumentException"/> when the specified value does not exist in a specified enumeration.
		/// </summary>
		public static void AssertDefined<TEnum>(ulong value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!IsDefined<TEnum>(value)) throwNotDefined<TEnum, ulong>(value);
		}

		/// <summary>
		/// Throws an instance of <see cref="InvalidEnumArgumentException"/> when the specified name does not exist in a specified enumeration.
		/// </summary>
		public static void AssertDefined<TEnum>(string name, bool ignoreCase = false) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!IsDefined<TEnum>(name, ignoreCase)) throwNotDefined<TEnum, string>(name);
		}

		private static void throwNotDefined<TEnum, U>(U valueOrName)
		{
			throw new InvalidEnumArgumentException(String.Format(Exceptions.Enumeration_ValueNotDefinedTemplate,
				valueOrName, typeof(TEnum).Name));
		}

		#endregion

		#region names

		/// <summary>
		/// Retrieves all the names in the specified enumeration.
		/// </summary>
		public static IEnumerable<string> GetNames<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertEnum<TEnum>();
			return Enum.GetNames(typeof(TEnum)).AsEnumerable();
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value.
		/// </summary>
		public static string GetName<TEnum>(TEnum value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertDefined(value);
			return getName<TEnum, TEnum>(value);
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value.
		/// </summary>
		public static string GetName<TEnum>(byte value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertDefined<TEnum>(value);
			return getName<TEnum, byte>(value);
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value.
		/// </summary>
		public static string GetName<TEnum>(sbyte value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertDefined<TEnum>(value);
			return getName<TEnum, sbyte>(value);
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value.
		/// </summary>
		public static string GetName<TEnum>(short value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertDefined<TEnum>(value);
			return getName<TEnum, short>(value);
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value.
		/// </summary>
		public static string GetName<TEnum>(ushort value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertDefined<TEnum>(value);
			return getName<TEnum, ushort>(value);
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value.
		/// </summary>
		public static string GetName<TEnum>(int value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertDefined<TEnum>(value);
			return getName<TEnum, int>(value);
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value.
		/// </summary>
		public static string GetName<TEnum>(uint value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertDefined<TEnum>(value);
			return getName<TEnum, uint>(value);
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value.
		/// </summary>
		public static string GetName<TEnum>(long value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertDefined<TEnum>(value);
			return getName<TEnum, long>(value);
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value.
		/// </summary>
		public static string GetName<TEnum>(ulong value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertDefined<TEnum>(value);
			return getName<TEnum, ulong>(value);
		}

		private static string getName<TEnum, U>(U value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
			where U : struct, IComparable, IFormattable, IConvertible
		{
			return Enum.GetName(typeof(TEnum), value);
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value. A return value indicates whether the conversion succeeded.
		/// </summary>
		public static bool TryGetName<TEnum>(TEnum value, out string name) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			name = getName<TEnum, TEnum>(value);
			return name != null;
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value. A return value indicates whether the conversion succeeded.
		/// </summary>
		public static bool TryGetName<TEnum>(byte value, out string name) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			name = getName<TEnum, byte>(value);
			return name != null;
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value. A return value indicates whether the conversion succeeded.
		/// </summary>
		public static bool TryGetName<TEnum>(sbyte value, out string name) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			name = getName<TEnum, sbyte>(value);
			return name != null;
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value. A return value indicates whether the conversion succeeded.
		/// </summary>
		public static bool TryGetName<TEnum>(short value, out string name) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			name = getName<TEnum, short>(value);
			return name != null;
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value. A return value indicates whether the conversion succeeded.
		/// </summary>
		public static bool TryGetName<TEnum>(ushort value, out string name) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			name = getName<TEnum, ushort>(value);
			return name != null;
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value. A return value indicates whether the conversion succeeded.
		/// </summary>
		public static bool TryGetName<TEnum>(int value, out string name) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			name = getName<TEnum, int>(value);
			return name != null;
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value. A return value indicates whether the conversion succeeded.
		/// </summary>
		public static bool TryGetName<TEnum>(uint value, out string name) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			name = getName<TEnum, uint>(value);
			return name != null;
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value. A return value indicates whether the conversion succeeded.
		/// </summary>
		public static bool TryGetName<TEnum>(long value, out string name) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			name = getName<TEnum, long>(value);
			return name != null;
		}

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value. A return value indicates whether the conversion succeeded.
		/// </summary>
		public static bool TryGetName<TEnum>(ulong value, out string name) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			name = getName<TEnum, ulong>(value);
			return name != null;
		}

		#endregion

		/// <summary>
		/// Returns the underlying type of the specified enumeration.
		/// </summary>
		public static Type GetUnderlyingType<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertEnum<TEnum>();
			return Enum.GetUnderlyingType(typeof(TEnum));
		}

		#region values

		/// <summary>
		/// Retrieves an enumerable of the values of the constants in a specified enumeration.
		/// </summary>
		public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertEnum<TEnum>();
			return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
		}

		/// <summary>
		///  Gets the numeric value of an enumeration
		/// </summary>
		public static TNumeric GetValue<TEnum, TNumeric>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
			where TNumeric : struct, IComparable, IFormattable, IConvertible, IComparable<TNumeric>, IEquatable<TNumeric>
		{
			AssertDefined(value);
			return (TNumeric)Convert.ChangeType(value, typeof(TNumeric));
		}

		/// <summary>
		/// Gets the numeric value of an enumeration. A return value indicates whether the conversion succeeded.
		/// </summary>
		public static bool TryGetValue<TEnum, TNumeric>(TEnum value, out TNumeric? numericValue)
			where TEnum : struct, IComparable, IFormattable, IConvertible
			where TNumeric : struct, IComparable, IFormattable, IConvertible, IComparable<TNumeric>, IEquatable<TNumeric>
		{
			bool result = false;
			numericValue = null;

			if (IsDefined(value))
			{
				try
				{
					numericValue = (TNumeric)Convert.ChangeType(value, typeof(TNumeric));
					result = true;
				}
				// there is no Convert.CanChangeType :-(
				catch (InvalidCastException) { }
				catch (OverflowException) { }
			}
			return result;
		}

		#endregion

		#region casting

		public static TEnum Cast<TEnum>(byte value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			TEnum? casted;
			bool result = TryCast(value, out casted);
			if (!result)
			{
				throwNotDefined<TEnum, byte>(value);
			}
			return casted.Value;
		}

		public static bool TryCast<TEnum>(byte value, out TEnum? casted) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			casted = null;
			bool success = IsDefined<TEnum>(value);
			if (success) casted = (TEnum)Enum.ToObject(typeof(TEnum), value);
			return success;
		}

		public static TEnum Cast<TEnum>(sbyte value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			TEnum? casted;
			bool result = TryCast(value, out casted);
			if (!result)
			{
				throwNotDefined<TEnum, sbyte>(value);
			}
			return casted.Value;
		}

		public static bool TryCast<TEnum>(sbyte value, out TEnum? casted) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			casted = null;
			bool success = IsDefined<TEnum>(value);
			if (success) casted = (TEnum)Enum.ToObject(typeof(TEnum), value);
			return success;
		}

		public static TEnum Cast<TEnum>(short value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			TEnum? casted;
			bool result = TryCast(value, out casted);
			if (!result)
			{
				throwNotDefined<TEnum, short>(value);
			}
			return casted.Value;
		}

		public static bool TryCast<TEnum>(short value, out TEnum? casted) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			casted = null;
			bool success = IsDefined<TEnum>(value);
			if (success) casted = (TEnum)Enum.ToObject(typeof(TEnum), value);
			return success;
		}

		public static TEnum Cast<TEnum>(ushort value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			TEnum? casted;
			bool result = TryCast(value, out casted);
			if (!result)
			{
				throwNotDefined<TEnum, ushort>(value);
			}
			return casted.Value;
		}

		public static bool TryCast<TEnum>(ushort value, out TEnum? casted) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			casted = null;
			bool success = IsDefined<TEnum>(value);
			if (success) casted = (TEnum)Enum.ToObject(typeof(TEnum), value);
			return success;
		}

		public static TEnum Cast<TEnum>(int value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			TEnum? casted;
			bool result = TryCast(value, out casted);
			if (!result)
			{
				throwNotDefined<TEnum, int>(value);
			}
			return casted.Value;
		}

		public static bool TryCast<TEnum>(int value, out TEnum? casted) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			casted = null;
			bool success = IsDefined<TEnum>(value);
			if (success) casted = (TEnum)Enum.ToObject(typeof(TEnum), value);
			return success;
		}

		public static TEnum Cast<TEnum>(uint value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			TEnum? casted;
			bool result = TryCast(value, out casted);
			if (!result)
			{
				throwNotDefined<TEnum, uint>(value);
			}
			return casted.Value;
		}

		public static bool TryCast<TEnum>(uint value, out TEnum? casted) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			casted = null;
			bool success = IsDefined<TEnum>(value);
			if (success) casted = (TEnum)Enum.ToObject(typeof(TEnum), value);
			return success;
		}

		public static TEnum Cast<TEnum>(long value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			TEnum? casted;
			bool result = TryCast(value, out casted);
			if (!result)
			{
				throwNotDefined<TEnum, long>(value);
			}
			return casted.Value;
		}

		public static bool TryCast<TEnum>(long value, out TEnum? casted) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			casted = null;
			bool success = IsDefined<TEnum>(value);
			if (success) casted = (TEnum)Enum.ToObject(typeof(TEnum), value);
			return success;
		}

		public static TEnum Cast<TEnum>(ulong value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			TEnum? casted;
			bool result = TryCast(value, out casted);
			if (!result)
			{
				throwNotDefined<TEnum, ulong>(value);
			}
			return casted.Value;
		}

		public static bool TryCast<TEnum>(ulong value, out TEnum? casted) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			casted = null;
			bool success = IsDefined<TEnum>(value);
			if (success) casted = (TEnum)Enum.ToObject(typeof(TEnum), value);
			return success;
		}

		#endregion

		#region parsing

		public static TEnum Parse<TEnum>(string value, bool ignoreCase = false) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			TEnum? casted;
			bool result = TryParse(value, ignoreCase, out casted);
			if (!result) throwNotDefined<TEnum, string>(value);
			return casted.Value;
		}

		public static bool TryParse<TEnum>(string value, out TEnum? parsed) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			parsed = null;
			Type enumType = typeof(TEnum);
			AssertEnum<TEnum>();
			bool success = false;
			TEnum p;
			if (Enum.TryParse(value, out p))
			{
				success = Enum.IsDefined(enumType, p);
				parsed = p;
			}

			return success;
		}

		public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum? parsed) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			parsed = null;
			Type enumType = typeof(TEnum);
			AssertEnum<TEnum>();
			bool success = false;
			TEnum p;
			if (Enum.TryParse(value, ignoreCase, out p))
			{
				success = Enum.IsDefined(enumType, p);
				parsed = p;
			}

			return success;
		}

		#endregion

		public static IEnumerable<TEnum> Invert<TEnum>(IEnumerable<TEnum> valuesToRemove) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertEnum<TEnum>();
			var values = GetValues<TEnum>();
			return valuesToRemove != null ? values.Except(valuesToRemove) : values;
		}

		public static IEnumerable<TEnum> Invert<TEnum>(params TEnum[] valuesToRemove) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return Invert((IEnumerable<TEnum>)valuesToRemove);
		}

		#region reflection

		public static FieldInfo GetField<TEnum>(TEnum value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertEnum<TEnum>();
			return typeof(TEnum).GetField(value.ToString(CultureInfo.InvariantCulture));
		}

		public static bool HasAttribute<TEnum, U>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
			where U : Attribute
		{
			return GetAttribute<TEnum, U>(value) != null;
		}

		public static U GetAttribute<TEnum, U>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
			where U : Attribute
		{
			AssertEnum<TEnum>();
			return (U)Attribute.GetCustomAttribute(GetField(value), typeof(U), false);
		}

		public static string GetDescription<TEnum>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			DescriptionAttribute description = GetAttribute<TEnum, DescriptionAttribute>(value);

			return description != null ? description.Description : null;
		}

		#endregion

		#region flags

		public static bool IsFlags<TFlags>() where TFlags : struct, IComparable, IFormattable, IConvertible
		{
			return IsEnum<TFlags>() && typeof(TFlags).IsDefined(typeof(FlagsAttribute), false);
		}

		public static void AssertFlags<TFlags>() where TFlags : struct, IComparable, IFormattable, IConvertible
		{
			AssertEnum<TFlags>();
			if (!IsFlags<TFlags>())
			{
				ExceptionHelper.Throw<ArgumentException>(Exceptions.Enumeration_NoFlagsTemplate, typeof(TFlags).Name);
			}
		}

		private static class Flags<TFlags> where TFlags : struct, IComparable, IFormattable, IConvertible
		{
			internal static readonly Func<TFlags, TFlags, TFlags> bitwiseOr, bitwiseAnd;
			internal static readonly Func<TFlags, TFlags> not;
			static Flags()
			{
				Type underlying = GetUnderlyingType<TFlags>();
				Type tFlags = typeof(TFlags);
				ParameterExpression param1 = Expression.Parameter(tFlags, "x");
				ParameterExpression param2 = Expression.Parameter(tFlags, "y");
				Expression convertedParam1 = Expression.Convert(param1, underlying);
				Expression convertedParam2 = Expression.Convert(param2, underlying);

				bitwiseOr = Expression.Lambda<Func<TFlags, TFlags, TFlags>>(
					Expression.Convert(Expression.Or(convertedParam1, convertedParam2), tFlags), param1, param2)
					.Compile();
				bitwiseAnd = Expression.Lambda<Func<TFlags, TFlags, TFlags>>(
					Expression.Convert(Expression.And(convertedParam1, convertedParam2), tFlags), param1, param2)
					.Compile();
				not = Expression.Lambda<Func<TFlags, TFlags>>(
					Expression.Convert(Expression.Not(convertedParam1), tFlags), param1)
					.Compile();
			}
		}

		public static TFlags SetFlag<TFlags>(this TFlags flags, TFlags flagToSet) where TFlags : struct, IComparable, IFormattable, IConvertible
		{
			AssertFlags<TFlags>();
			return Flags<TFlags>.bitwiseOr(flags, flagToSet);
		}

		public static TFlags UnsetFlag<TFlags>(this TFlags flags, TFlags flagToSet) where TFlags : struct, IComparable, IFormattable, IConvertible
		{
			AssertFlags<TFlags>();
			return Flags<TFlags>.bitwiseAnd(flags, Flags<TFlags>.not(flagToSet));
		}

		public static TFlags ToggleFlag<TFlags>(this TFlags flags, TFlags flagToToggle) where TFlags : struct, IComparable, IFormattable, IConvertible
		{
			AssertFlags<TFlags>();
			// since HasFlags incurs in boxing, I do not care one more boxing
			return asEnum(flags).HasFlag(asEnum(flagToToggle)) ?
				UnsetFlag(flags, flagToToggle) :
				SetFlag(flags, flagToToggle);
		}

		private static Enum asEnum<T>(T t)
		{
			return (Enum)((object)t);
		}

		public static IEnumerable<TFlags> GetFlags<TFlags>(this TFlags flag, bool ignoreZeroValue = false) where TFlags : struct, IComparable, IFormattable, IConvertible
		{
			AssertFlags<TFlags>();
			// flags always have the default as the option with value zero (regardless of whether is defined or not)
			// so ignoring the zero value means leaving out the default value altogether from the list of values
			return GetValues<TFlags>()
				// boxing, again
				.Where(t => !(ignoreZeroValue && Equals(t, default(TFlags))))
				.Where(t => asEnum(flag).HasFlag(asEnum(t)));
		}

		#endregion

		#region fast comparison

		/* based on: http://www.codeproject.com/KB/cs/EnumComparer.aspx */
		internal class FastEnumComparer<TEnum> : IEqualityComparer<TEnum> where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			public static readonly IEqualityComparer<TEnum> Instance;

			private static readonly Func<TEnum, TEnum, bool> equals;
			private static readonly Func<TEnum, int> getHashCode;

			static FastEnumComparer()
			{
				getHashCode = generateGetHashCode();
				equals = generateEquals();
				Instance = new FastEnumComparer<TEnum>();
			}
			/// <summary>
			/// A private constructor to prevent user instantiation.
			/// </summary>
			private FastEnumComparer()
			{
				assertUnderlyingTypeIsSupported();
			}

			public bool Equals(TEnum x, TEnum y)
			{
				// call the generated method
				return equals(x, y);
			}

			public int GetHashCode(TEnum obj)
			{
				// call the generated method
				return getHashCode(obj);
			}

			private static void assertUnderlyingTypeIsSupported()
			{
				var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
				ICollection<Type> supportedTypes = new[]
				{
					typeof (byte), typeof (sbyte),
					typeof (short), typeof (ushort),
					typeof (int), typeof (uint),
					typeof (long), typeof (ulong)
				};

				if (!supportedTypes.Contains(underlyingType))
				{
					string typeNames = string.Join(", ", supportedTypes.Select(t => t.Name));
					ExceptionHelper.Throw<NotSupportedException>(
						Exceptions.Enumeration_NotSupportedUnderlyingTypeTemplate,
						typeof(TEnum).Name,
						underlyingType.Name,
						typeNames);
				}
			}

			private static Func<TEnum, TEnum, bool> generateEquals()
			{
				var xParam = Expression.Parameter(typeof(TEnum), "x");
				var yParam = Expression.Parameter(typeof(TEnum), "y");
				var equalExpression = Expression.Equal(xParam, yParam);
				return Expression.Lambda<Func<TEnum, TEnum, bool>>(equalExpression, new[] { xParam, yParam }).Compile();
			}

			private static Func<TEnum, int> generateGetHashCode()
			{
				var objParam = Expression.Parameter(typeof(TEnum), "obj");
				var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
				var convertExpression = Expression.Convert(objParam, underlyingType);
				var getHashCodeMethod = underlyingType.GetMethod("GetHashCode");
				var getHashCodeExpression = Expression.Call(convertExpression, getHashCodeMethod);
				return Expression.Lambda<Func<TEnum, int>>(getHashCodeExpression, new[] { objParam }).Compile();
			}
		}

		public static IEqualityComparer<TEnum> GetComparer<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			AssertEnum<TEnum>();
			return FastEnumComparer<TEnum>.Instance;
		}

		#endregion
	}
}