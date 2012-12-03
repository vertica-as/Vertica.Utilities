using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vertica.Utilities_v4
{
	public struct Percentage
	{
		public double Value { get; private set; }
		public double Fraction { get; private set; }

		#region construction

		public Percentage(double value)
			: this()
		{
			Value = value;
			Fraction = value / 100d;
		}

		public static Percentage FromFraction(double fraction)
		{
			return new Percentage(fraction * 100d);
		}

		internal static readonly string _divideByZeroMessage = new DivideByZeroException().Message;

		public static Percentage FromAmounts(long given, long total)
		{
			Guard.AgainstArgument("total", total == 0, _divideByZeroMessage);
			return new Percentage(given / (double)total * 100d);
		}

		public static Percentage FromAmounts(double given, double total)
		{
			Guard.AgainstArgument("total", total == 0d, _divideByZeroMessage);
			return new Percentage(given / total * 100d);
		}

		public static Percentage FromDifference(long total, long given)
		{
			Guard.AgainstArgument("total", total == 0L, _divideByZeroMessage);
			return new Percentage(((total - given) / (double)total) * 100d);
		}

		public static Percentage FromDifference(double total, double given)
		{
			Guard.AgainstArgument("total", total == 0d, _divideByZeroMessage);
			return new Percentage(((total - given) / total) * 100d);
		}

		#endregion


	}
}
