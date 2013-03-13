using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities_v4.Patterns
{
	public abstract class ChainOfResponsibilityLink<T>
	{
		public ChainOfResponsibilityLink<T> Next { get; private set; }

		public void Handle(T context)
		{
			if (CanHandle(context))
			{
				DoHandle(context);
			}
			else
			{
				if (Next != null)
				{
					Next.Handle(context);
				}
			}
		}

		public bool TryHandle(T context)
		{
			bool handled = false;
			if (CanHandle(context))
			{
				DoHandle(context);
				handled = true;
			}
			else
			{
				if (Next != null)
				{
					handled = Next.TryHandle(context);
				}
			}
			return handled;
		}

		public abstract bool CanHandle(T context);
		protected abstract void DoHandle(T context);

		public ChainOfResponsibilityLink<T> Chain(IChainOfResponsibilityLink<T> lastHandler)
		{
			return Chain(new ResponsibleLink<T>(lastHandler));
		}

		private ChainOfResponsibilityLink<T> _lastLink;
		public ChainOfResponsibilityLink<T> Chain(ChainOfResponsibilityLink<T> lastHandler)
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

		public ChainOfResponsibilityLink<T> Chain(params IChainOfResponsibilityLink<T>[] handlers)
		{
			return Chain(handlers.AsEnumerable());
		}

		public ChainOfResponsibilityLink<T> Chain(params ChainOfResponsibilityLink<T>[] handlers)
		{
			return Chain(handlers.AsEnumerable());
		}

		public ChainOfResponsibilityLink<T> Chain(IEnumerable<IChainOfResponsibilityLink<T>> handlers)
		{
			return Chain(handlers.Select(h => new ResponsibleLink<T>(h)));
		}

		public ChainOfResponsibilityLink<T> Chain(IEnumerable<ChainOfResponsibilityLink<T>> handlers)
		{
			var first = default(ChainOfResponsibilityLink<T>);
			foreach (var link in handlers)
			{
				first = Chain(link);
			}
			return first;
		}
	}
}