using Vertica.Utilities.Extensions.Infrastructure;

namespace Vertica.Utilities.Tests.Extensions.Infrastructure.Support
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