using Vertica.Utilities_v4.Extensions.Infrastructure;

namespace Vertica.Utilities_v4.Extensions.AnonymousExt
{
	public static class AnonnymousExtensions
	{
		public static T ByExample<T>(this CastExtensionPoint<object> obj, T example)
		{
			return (T)obj.ExtendedValue;
		}
	}
}