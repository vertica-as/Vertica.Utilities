using System;

namespace Vertica.Utilities
{
	public struct Percentage : IFormattable
	{
		public double Value { get; private set; }
		public double Fraction { get; private set; }

		// structs have a default constructor, therefore, _formattable can be null
		private FormattablePercentage<double>? _formattable;
		private FormattablePercentage<double> Formattable
		{
			get
			{
				_formattable = _formattable ?? new FormattablePercentage<double>(Value);
				return _formattable.Value;
			}
		}

		#region construction

		public Percentage(double value)
			: this()
		{
			Value = value;
			Fraction = value / 100d;
			_formattable = new FormattablePercentage<double>(value);
		}

		public static Percentage FromFraction(double fraction)
		{
			return new Percentage(fraction * 100d);
		}

		public static Percentage FromAmounts(long given, long total)
		{
			Guard.AgainstArgument("total", total == 0L, FormattablePercentage<double>.DivideByZeroMessage);
			return new Percentage(given / (double)total * 100d);
		}

		public static Percentage FromAmounts(double given, double total)
		{
			Guard.AgainstArgument("total", total == 0d, FormattablePercentage<double>.DivideByZeroMessage);
			return new Percentage(given / total * 100d);
		}

		public static Percentage FromDifference(long total, long given)
		{
			Guard.AgainstArgument("total", total == 0L, FormattablePercentage<double>.DivideByZeroMessage);
			return new Percentage(((total - given) / (double)total) * 100d);
		}

		public static Percentage FromDifference(double total, double given)
		{
			Guard.AgainstArgument("total", total == 0d, FormattablePercentage<double>.DivideByZeroMessage);
			return new Percentage(((total - given) / total) * 100d);
		}

		#endregion

		public double Apply(long given)
		{
			return Fraction * given;
		}

		public double Apply(double given)
		{
			return Fraction * given;
		}

		public double DeductFrom(double amountIncludingPercentage)
		{
			return amountIncludingPercentage / (1 + Fraction);
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
