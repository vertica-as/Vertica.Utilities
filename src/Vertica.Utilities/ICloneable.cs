namespace Vertica.Utilities_v4
{
	public interface IShallowCloneable<out T>
	{
		T ShallowClone();
	}

	public interface IDeepCloneable<out T>
	{
		T DeepClone();
	}

	public interface ICloneable<out T> : IShallowCloneable<T>, IDeepCloneable<T> { }
}