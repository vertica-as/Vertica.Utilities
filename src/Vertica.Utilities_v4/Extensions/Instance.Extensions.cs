namespace Vertica.Utilities_v4.Extensions.InstanceExt
{
	public static class InstanceExtensions
	{
		public static bool IsIntegral<T>(this T o)
		{
			return (o is byte) || (o is sbyte) ||
				(o is short) || (o is ushort) ||
				(o is int) || (o is uint) ||
				(o is long) || (o is ulong);
		}
	}
}
