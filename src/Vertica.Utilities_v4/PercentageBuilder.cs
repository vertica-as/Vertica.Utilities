namespace Vertica.Utilities_v4
{
	public static class PercentageBuilder
	{
		public static Percentage Percent(this double value)
		{
			return new Percentage(value);
		}

		public static Percentage AsPercentOf(this double given, double total)
		{
			return Percentage.FromAmounts(given, total);
		}
	}
}