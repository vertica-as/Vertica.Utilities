namespace Vertica.Utilities.Extensions.Infrastructure
{
	public class CastExtensionPoint<T> : ExtensionPoint<T>
	{
			public CastExtensionPoint(T value) : base(value) { }
	}
}