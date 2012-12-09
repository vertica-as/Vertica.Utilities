using System;

namespace Vertica.Utilities_v4
{
	public struct DecimalPercentage
	{
		public decimal Value { get; private set; }
		public decimal Fraction { get; private set; }

		// structs have a default constructor, therefore, _formattable can be null
		private FormattablePercentage<decimal>? _formattable;
		private FormattablePercentage<decimal> Formattable
		{
			get
			{
				_formattable = _formattable ?? new FormattablePercentage<decimal>(Value);
				return _formattable.Value;
			}
		}

		#region construction

		public DecimalPercentage(decimal value) : this()
		{
			Value = value;
			Fraction = value / 100m;
		}

		public static DecimalPercentage FromFraction(decimal fraction)
		{
			return new DecimalPercentage(fraction * 100m);
		}

		public static DecimalPercentage FromAmounts(long given, long total)
		{
			Guard.AgainstArgument("total", total == 0L, FormattablePercentage<decimal>.DivideByZeroMessage);
			return new DecimalPercentage(given / (decimal)total * 100m);
		}

		public static DecimalPercentage FromAmounts(decimal given, decimal total)
		{
			Guard.AgainstArgument("total", total == 0m, FormattablePercentage<decimal>.DivideByZeroMessage);
			return new DecimalPercentage(given / total * 100m);
		}

		public static DecimalPercentage FromDifference(long total, long given)
		{
			Guard.AgainstArgument("total", total == 0L, FormattablePercentage<decimal>.DivideByZeroMessage);
			return new DecimalPercentage(((total - given) / (decimal)total) * 100m);
		}

		public static DecimalPercentage FromDifference(decimal total, decimal given)
		{
			Guard.AgainstArgument("total", total == 0m, FormattablePercentage<decimal>.DivideByZeroMessage);
			return new DecimalPercentage(((total - given) / total) * 100m);
		}

		#endregion

		public decimal Apply(long given)
		{
			return Fraction * given;
		}

		public decimal Apply(decimal given)
		{
			return Fraction * given;
		}

		public override string ToString()
		{
			return Formattable.ToString();
		}

		public string ToString(string numberFormat)
		{
			return Formattable.ToString(numberFormat);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return Formattable.ToString(format, formatProvider);
		}
	}
}