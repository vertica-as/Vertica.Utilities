using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vertica.Utilities_v4
{
	public struct Percentage
	{
		public Percentage(double value) : this()
		{
			Value = value;
			Fraction = value / 100d;
		}

		public double Value { get; private set; }
		public double Fraction { get; private set; }

		public static Percentage FromFraction(double fraction)
		{
			return new Percentage(fraction * 100d);
		}

	}
}
