using System;
using System.Globalization;
using Vertica.Utilities.Patterns;

namespace Vertica.Utilities.Tests.Patterns.Support
{
	internal class MultiLink : IChainOfResponsibilityLink<int, string>, IChainOfResponsibilityLink<string, int>, IChainOfResponsibilityLink<Exception>
	{
		public MultiLink() { }

		private readonly int _intToHandle;
		public MultiLink(int contextToHandle)
		{
			_intToHandle = contextToHandle;
		}

		public bool CanHandle(int context)
		{
			return context.Equals(_intToHandle);
		}

		public string DoHandle(int context)
		{
			return context.ToString(CultureInfo.InvariantCulture);
		}

		private int _handled;
		public bool CanHandle(string context)
		{
			return int.TryParse(context, NumberStyles.Integer, CultureInfo.InvariantCulture, out _handled);
		}

		public int DoHandle(string context)
		{
			return _handled;
		}

		public bool CanHandle(Exception context)
		{
			return context != null;
		}

		public void DoHandle(Exception context)
		{
			context.Data.Add("handled", Time.UtcNow);
		}
	}
}