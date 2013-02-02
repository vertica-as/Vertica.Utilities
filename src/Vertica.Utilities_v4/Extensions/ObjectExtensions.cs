namespace Vertica.Utilities_v4.Extensions.ObjectExt
{
	public static class ObjectExtensions
	{
		public static string SafeToString<T>(this T instance, string @default = null) where T : class
		{
			return instance == null ? @default : instance.ToString();
		}
	}
}
