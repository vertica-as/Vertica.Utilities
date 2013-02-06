using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities_v4.Patterns
{
	public abstract class ChainOfResponsibilityLink<T>
	{
		private ChainOfResponsibilityLink<T> _nextLink;
		public ChainOfResponsibilityLink<T> Next
		{
			get { return _nextLink; }
		}

		public void Handle(T context)
		{
			if (CanHandle(context))
			{
				DoHandle(context);
			}
			else
			{
				if (_nextLink != null)
				{
					_nextLink.Handle(context);
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
				if (_nextLink != null)
				{
					handled = _nextLink.TryHandle(context);
				}
			}
			return handled;
		}

		public abstract bool CanHandle(T context);
		protected abstract void DoHandle(T context);
		private void chain(ChainOfResponsibilityLink<T> lastHandler)
		{
			if (_nextLink == null)
			{
				_nextLink = lastHandler;
			}
			else
			{
				_nextLink.chain(lastHandler);
			}
			//return lastHandler;
		}

		private ChainOfResponsibilityLink<T> _lastLink;
		public ChainOfResponsibilityLink<T> Chain(ChainOfResponsibilityLink<T> lastHandler)
		{

			if (_nextLink == null)
			{
				_nextLink = lastHandler;
			}
			else
			{
				_lastLink.chain(lastHandler);
			}
			_lastLink = lastHandler;
			return this;
		}

		public ChainOfResponsibilityLink<T> Chain(params ChainOfResponsibilityLink<T>[] handlers)
		{
			return Chain(handlers.AsEnumerable());
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

		public static ChainOfResponsibilityLink<T> Empty()
		{
			return new EmptyLink();
		}

		private class EmptyLink : ChainOfResponsibilityLink<T>
		{
			public override bool CanHandle(T context)
			{
				return false;
			}

			protected override void DoHandle(T context) { }
		}
	}
}