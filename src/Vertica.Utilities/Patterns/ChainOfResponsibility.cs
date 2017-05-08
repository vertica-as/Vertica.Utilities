namespace Vertica.Utilities_v4.Patterns
{
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

		public static ChainOfResponsibilityLink<T, TResult> Empty<T, TResult>()
		{
			return new EmptyLink<T, TResult>();
		}

		private class EmptyLink<T, TResult> : ChainOfResponsibilityLink<T, TResult>
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

	
}