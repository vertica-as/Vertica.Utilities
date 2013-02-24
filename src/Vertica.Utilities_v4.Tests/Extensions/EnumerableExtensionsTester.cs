using System.Linq;
using NUnit.Framework;
using Vertica.Utilities_v4.Collections;
using Vertica.Utilities_v4.Extensions.EnumerableExt;

namespace Vertica.Utilities_v4.Tests.Extensions
{
	[TestFixture]
	public class EnumerableExtensionsTester
	{
		#region nullability

		#region EmptyIfNull

		[Test]
		public void EmptyIfNull_NonEmptySource_ReturnsSource()
		{
			var source = new[] { 1, 2, 3 };
			Assert.That(source.EmptyIfNull(), Is.SameAs(source));
		}

		[Test]
		public void EmptyIfNull_NullSource_ReturnsEmpty()
		{
			Assert.That(Chain.Null<int>().EmptyIfNull(), Is.Empty);
		}

		[Test]
		public void EmptyIfNull_EmptySource_ReturnsEmpty()
		{
			Assert.That(Chain.Empty<int>().EmptyIfNull(), Is.Empty);
		}

		#endregion

		#region NullIfEmpty

		[Test]
		public void NullIfEmpty_NonEmptySource_ReturnsSource()
		{
			var source = new[] { 1, 2, 3 };
			Assert.That(source.NullIfEmpty(), Is.SameAs(source));
		}

		[Test]
		public void NullIfEmpty_NullSource_ReturnsNull()
		{
			Assert.That(Chain.Null<int>().NullIfEmpty(), Is.Null);
		}

		[Test]
		public void NullIfEmpty_EmptySource_ReturnsNull()
		{
			Assert.That(Chain.Empty<int>().NullIfEmpty(), Is.Null);
		}

		#endregion

		#endregion
	}
}