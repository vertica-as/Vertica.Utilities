using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
			return typeof (TEnum).IsEnum;
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
				Enum.IsDefined(typeof (TEnum), name) :
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
			throw new InvalidEnumArgumentException(string.Format(Exceptions.Enumeration_ValueNotDefinedTemplate,
				valueOrName, typeof (TEnum).Name));
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



		#endregion

	}
}