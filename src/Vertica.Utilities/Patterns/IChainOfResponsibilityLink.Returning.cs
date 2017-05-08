namespace Vertica.Utilities_v4.Patterns
{
	public interface IChainOfResponsibilityLink<in T, out TResult>
	{
		bool CanHandle(T context);
		TResult DoHandle(T context);
	}
}