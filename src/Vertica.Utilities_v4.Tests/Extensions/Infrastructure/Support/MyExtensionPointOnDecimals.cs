using Vertica.Utilities_v4.Extensions.Infrastructure;

namespace Vertica.Utilities_v4.Tests.Extensions.Infrastructure.Support
{
	public class MyExtensionPointOnDecimals : ExtensionPoint<decimal>
	{
		public MyExtensionPointOnDecimals(decimal value) : base(value) { }
	}

	public static partial class EntryPoints
	{
		public static MyExtensionPointOnDecimals My(this decimal subject)
		{
			return new MyExtensionPointOnDecimals(subject);
		}
	}
}