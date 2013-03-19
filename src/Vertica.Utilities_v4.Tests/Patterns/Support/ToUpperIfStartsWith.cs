using Vertica.Utilities_v4.Patterns;

namespace Vertica.Utilities_v4.Tests.Patterns.Support
{
	internal class ToUpperIfStartsWith : ChainOfResponsibilityLink<Context>
	{
		private readonly string _substring;

		public ToUpperIfStartsWith(string substring)
		{
			_substring = substring;
		}

		public override bool CanHandle(Context context)
		{
			return context.S.StartsWith(_substring);
		}

		protected override void DoHandle(Context context)
		{
			context.S = context.S.ToUpperInvariant();
		}
	}

	internal class IToUpperIfStartsWith : IChainOfResponsibilityLink<Context>
	{
		private readonly string _substring;

		public IToUpperIfStartsWith(string substring)
		{
			_substring = substring;
		}

		public bool CanHandle(Context context)
		{
			return context.S.StartsWith(_substring);
		}

		public void DoHandle(Context context)
		{
			context.S = context.S.ToUpperInvariant();
		}
	}
}
