using System;

namespace Vertica.Utilities_v4
{
	//* 
	//* Based upon http://rodenbaugh.net/archive/2006/11/01/Enum-Helper-Class-Using-Generics.aspx
	//* 
	public static class Enumeration
	{
		public static bool IsEnum<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return IsEnum(typeof(TEnum));
		}

		public static bool IsEnum(Type type)
		{
			return type.IsEnum;
		}
	}
}