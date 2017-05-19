namespace Vertica.Utilities.Patterns
{
	public interface IChainOfResponsibilityLink<in T>
	{
		bool CanHandle(T context);
		void DoHandle(T context);
	}
}