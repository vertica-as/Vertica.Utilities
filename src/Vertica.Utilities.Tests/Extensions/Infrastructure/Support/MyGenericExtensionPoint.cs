using Vertica.Utilities_v4.Extensions.Infrastructure;

namespace Vertica.Utilities_v4.Tests.Extensions.Infrastructure.Support
{
	public class MyGenericExtensionPoint<T> : ExtensionPoint<T>
	{
		public MyGenericExtensionPoint(T value) : base(value) { }
	}

	public static partial class EntryPoints
	{
		public static MyGenericExtensionPoint<T> My<T>(this T subject)
		{
			return new MyGenericExtensionPoint<T>(subject);
		}
	}
}