namespace Vertica.Utilities_v4.Patterns
{
	public interface IChainOfResponsibilityLink<in T>
	{
		bool CanHandle(T context);
		void DoHandle(T context);
	}
}