using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests.Extensions.Support
{
	[Category("cat"), Description("desc")]
	internal class DecoratedWithCategoryAndDescription { }

	internal class ParentDecoratedWithCategoryAndDecription : DecoratedWithCategoryAndDescription { }
}