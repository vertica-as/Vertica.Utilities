namespace Vertica.Utilities.Tests.Extensions.Infrastructure.Support
{
	public static class MyExtensions
	{
		public static string Echo<T>(this MyGenericExtensionPoint<T> point)
		{
			var str = point.ExtendedValue.ToString();
			return str + str;
		}

		public static decimal DoubleUp(this MyExtensionPointOnDecimals point)
		{
			return point.ExtendedValue * 2m;
		}
	}
}