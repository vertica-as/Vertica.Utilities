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

		private ChainOfResponsibilityLink<T> _lastLink;
		public ChainOfResponsibilityLink<T> Chain(ChainOfResponsibilityLink<T> lastHandler)
		{
			if (_nextLink == null)
			{
				_nextLink = lastHandler;
			}
			else
			{
				_lastLink.Chain(lastHandler);
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
	}

	public abstract class ChainOfResponsibilityLink<T, TResult>
	{
		private ChainOfResponsibilityLink<T, TResult> _nextLink;
		public ChainOfResponsibilityLink<T, TResult> Next
		{
			get { return _nextLink; }
		}

		public TResult Handle(T context)
		{
			TResult result = default(TResult);
			if (CanHandle(context))
			{
				result = DoHandle(context);
			}
			else
			{
				if (_nextLink != null)
				{
					result = _nextLink.Handle(context);
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
				if (_nextLink != null)
				{
					handled = _nextLink.TryHandle(context, out result);
				}
			}
			return handled;
		}

		public abstract bool CanHandle(T context);
		protected abstract TResult DoHandle(T context);

		private ChainOfResponsibilityLink<T, TResult> _lastLink;
		public ChainOfResponsibilityLink<T, TResult> Chain(ChainOfResponsibilityLink<T, TResult> lastHandler)
		{

			if (_nextLink == null)
			{
				_nextLink = lastHandler;
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

		public static ChainOfResponsibilityLink<T, TResult> Empty()
		{
			return new EmptyLink();
		}

		private class EmptyLink : ChainOfResponsibilityLink<T, TResult>
		{
			public override bool CanHandle(T context)
			{
				return false;
			}

			protected override TResult DoHandle(T context)
			{
				return default(TResult);
			}
		}
	}

	public static class ChainOfResponsibility
	{
		private class EmptyLink<T> : ChainOfResponsibilityLink<T>
		{
			public override bool CanHandle(T context)
			{
				return false;
			}

			protected override void DoHandle(T context) { }
		}

		public static ChainOfResponsibilityLink<T> Empty<T>()
		{
			return new EmptyLink<T>();	
		} 
	}
}