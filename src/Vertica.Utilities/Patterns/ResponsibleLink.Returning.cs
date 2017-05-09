namespace Vertica.Utilities.Patterns
{
	public class ResponsibleLink<T, TResult> : ChainOfResponsibilityLink<T, TResult>
	{
		private readonly IChainOfResponsibilityLink<T, TResult> _link;

		public ResponsibleLink(IChainOfResponsibilityLink<T, TResult> link)
		{
			_link = link;
		}

		public override bool CanHandle(T context)
		{
			return _link != null && _link.CanHandle(context);
		}

		protected override TResult DoHandle(T context)
		{
			return _link != null ? _link.DoHandle(context) : default(TResult);
		}
	}
}