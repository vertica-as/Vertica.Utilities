using Vertica.Utilities.Extensions.Infrastructure;

namespace Vertica.Utilities.Tests.Extensions.Infrastructure.Support
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