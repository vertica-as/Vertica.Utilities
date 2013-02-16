using System.Collections.Generic;

namespace Vertica.Utilities_v4.Patterns
{
	public abstract class ChainOfResponsibilityLink<T, TResult>
	{
		public ChainOfResponsibilityLink<T, TResult> Next { get; private set; }

		public TResult Handle(T context)
		{
			TResult result = default(TResult);
			if (CanHandle(context))
			{
				result = DoHandle(context);
			}
			else
			{
				if (Next != null)
				{
					result = Next.Handle(context);
				}
			}
			return result;
		}

		public bool TryHandle(T context, out TResult result)
		{
			result = default(TResult);
			bool handled = false;
			if (CanHandle(context))
			{
				result = DoHandle(context);
				handled = true;
			}
			else
			{
				if (Next != null)
				{
					handled = Next.TryHandle(context, out result);
				}
			}
			return handled;
		}

		public abstract bool CanHandle(T context);
		protected abstract TResult DoHandle(T context);

		private ChainOfResponsibilityLink<T, TResult> _lastLink;
		public ChainOfResponsibilityLink<T, TResult> Chain(ChainOfResponsibilityLink<T, TResult> lastHandler)
		{

			if (Next == null)
			{
				Next = lastHandler;
			}
			else
			{
				_lastLink.Chain(lastHandler);
			}
			_lastLink = lastHandler;
			return this;
		}

		public ChainOfResponsibilityLink<T, TResult> Chain(params ChainOfResponsibilityLink<T, TResult>[] handlers)
		{
			return Chain((IEnumerable<ChainOfResponsibilityLink<T, TResult>>)handlers);
		}

		public ChainOfResponsibilityLink<T, TResult> Chain(IEnumerable<ChainOfResponsibilityLink<T, TResult>> handlers)
		{
			var first = default(ChainOfResponsibilityLink<T, TResult>);
			foreach (var link in handlers)
			{
				first = Chain(link);
			}
			return first;
		}
	}
}