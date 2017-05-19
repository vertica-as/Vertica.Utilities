namespace Vertica.Utilities.Patterns
{
	public interface ICommand
	{
		void Execute();
	}

	public interface ICommand<out TReturn>
	{
		TReturn Execute();
	}

	public interface ICommand<in TInput, out TReturn>
	{
		TReturn Execute(TInput input);
	}
}
