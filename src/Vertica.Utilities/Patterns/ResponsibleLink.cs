namespace Vertica.Utilities.Patterns
{
	public class ResponsibleLink<T> : ChainOfResponsibilityLink<T>
	{
		private readonly IChainOfResponsibilityLink<T> _link;

		public ResponsibleLink(IChainOfResponsibilityLink<T> link)
		{
			_link = link;
		}

		public override bool CanHandle(T context)
		{
			return _link != null && _link.CanHandle(context);
		}

		protected override void DoHandle(T context)
		{
			if (_link != null) _link.DoHandle(context);
		}
	}
}