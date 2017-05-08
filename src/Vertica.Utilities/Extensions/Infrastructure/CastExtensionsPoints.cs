namespace Vertica.Utilities_v4.Extensions.Infrastructure
{
	public class CastExtensionPoint<T> : ExtensionPoint<T>
	{
			public CastExtensionPoint(T value) : base(value) { }
	}
}