using NUnit.Framework;
using Vertica.Utilities_v4.Extensions.StringExt;

namespace Vertica.Utilities_v4.Tests.Extensions
{
	[TestFixture]
	public class StringExtensionsTester
	{
		#region emptiness

		[TestCase(null, true)]
		[TestCase("", true)]
		[TestCase("abc", false)]
		public void IsEmpty_Combinations(string input, bool isEmpty)
		{
			Assert.That(input.IsEmpty(), Is.EqualTo(isEmpty));
		}

		[TestCase(null, false)]
		[TestCase("", false)]
		[TestCase("abc", true)]
		public void IsNotEmpty_Combinations(string input, bool isNotEmpty)
		{
			Assert.That(input.IsNotEmpty(), Is.EqualTo(isNotEmpty));
		}

		[Test]
		public void NullIfEmpty_Empty_Null()
		{
			Assert.That(string.Empty.NullIfEmpty(), Is.Null);
		}

		[Test]
		public void NullIfEmpty_Null_Null()
		{
			Assert.That(((string)null).NullIfEmpty(), Is.Null);
		}

		[Test]
		public void NullIfEmpty_NotEmptyNull_Inout()
		{
			Assert.That("not empty".NullIfEmpty(), Is.EqualTo("not empty"));
		}

		#endregion
	}
}