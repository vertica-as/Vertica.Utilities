using System;
using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests.Extensions.Support
{
	[Category("cat"), Description("desc")]
	internal class DecoratedWithCategoryAndDescription { }

	internal class ParentDecoratedWithCategoryAndDecription : DecoratedWithCategoryAndDescription { }

	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	internal sealed class MultiAttribute : Attribute
	{
		// This is a positional argument
		public MultiAttribute(string positional)
		{
			Positional = positional;
		}

		public string Positional { get; private set; }
	}

	[Multi("a"), Multi("c"), Multi("b")]
	internal class DecoratedMultipleTimes { }
}