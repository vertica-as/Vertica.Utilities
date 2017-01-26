using System;

namespace Vertica.Utilities_v4.Patterns
{
	public class GenericCommand<TReceiver> : ICommand
	{
		private readonly TReceiver _receiver;
		private readonly Action<TReceiver> _commandToExecute;
		public GenericCommand(TReceiver receiver, Action<TReceiver> commandToExecute)
		{
			_receiver = receiver;
			_commandToExecute = commandToExecute;
		}

		public void Execute()
		{
			_commandToExecute(_receiver);
		}
	}

	public class GenericCommand<TReceiver, TReturn> : ICommand<TReturn>
	{
		private readonly TReceiver _receiver;
		private readonly Func<TReceiver, TReturn> _commandToExecute;

		public GenericCommand(TReceiver receiver, Func<TReceiver, TReturn> commandToExecute)
		{
			_receiver = receiver;
			_commandToExecute = commandToExecute;
		}

		public TReturn Execute()
		{
			return _commandToExecute(_receiver);
		}
	}

	public class GenericCommand<TReceiver, TInput, TReturn> : ICommand<TInput, TReturn>
	{
		private readonly TReceiver _receiver;
		private readonly Func<TReceiver, TInput, TReturn> _commandToExecute;

		public GenericCommand(TReceiver receiver, Func<TReceiver, TInput, TReturn> commandToExecute)
		{
			_receiver = receiver;
			_commandToExecute = commandToExecute;
		}

		public TReturn Execute(TInput input)
		{
			return _commandToExecute(_receiver, input);
		}
	}
}