﻿using System;
using System.Globalization;
using Vertica.Utilities_v4.Patterns;

namespace Vertica.Utilities_v4.Tests.Patterns.Support
{
	internal class IntToStringLink : ChainOfResponsibilityLink<int, string>
	{
		private readonly int _contextToHandle;

		public IntToStringLink(int contextToHandle)
		{
			_contextToHandle = contextToHandle;
		}

		public override bool CanHandle(int context)
		{
			return context == _contextToHandle;
		}

		protected override string DoHandle(int context)
		{
			return context.ToString(CultureInfo.InvariantCulture);
		}
	}
}