using System;
using System.Globalization;

namespace Vertica.Utilities_v4
{
	/// <summary>
	/// commons parts of percentages, thanks you lack of generic numeric super-type
	/// </summary>
	internal struct FormattablePercentage<TNumeric> where TNumeric : IFormattable, IComparable, IConvertible, IComparable<TNumeric>, IEquatable<TNumeric>
	{
		internal static readonly string DivideByZeroMessage = new DivideByZeroException().Message;

		public TNumeric Value { get; private set; }
		public FormattablePercentage(TNumeric value) : this()
		{
			Value = value;
		}
		
		public override string ToString()
		{
			return ToString("{0}", CultureInfo.InvariantCulture);
		}

		public string ToString(string numberFormat)
		{
			return doFormat(Value, numberFormat + " %", CultureInfo.InvariantCulture);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return doFormat(Value, format + " %", formatProvider);
		}

		private string doFormat(TNumeric percentage, string numberFormat, IFormatProvider provider)
		{
			return string.Format(provider, numberFormat, percentage);
		}
	}
}