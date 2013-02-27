using System;
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

		#region stripping
		
		#region Strip(chars)

		[Test]
		public void Strip_SingleExistingChar_StringWithoutChar()
		{
			Assert.That("qwe".Strip('w'), Is.EqualTo("qe"));
		}

		[Test]
		public void Strip_SingleNonExistingChar_OriginalString()
		{
			string s = "qwe";
			Assert.That(s.Strip('a'), Is.EqualTo(s));
		}

		[Test]
		public void Strip_SingleCharOnStringEmpty_StringEmpty()
		{
			Assert.That(string.Empty.Strip('w'), Is.Empty);
		}

		[Test]
		public void Strip_SingleCharOnNull_Null()
		{
			string s = null;
			Assert.That(s.Strip('x'), Is.Null);
		}

		[Test]
		public void Strip_ExistingChars_StringWithoutChars()
		{
			Assert.That("qwe".Strip('w', 'e'), Is.EqualTo("q"));
		}

		[Test]
		public void Strip_NonExistingChar_OriginalString()
		{
			string s = "qwe";
			Assert.That(s.Strip('a', 'b'), Is.EqualTo(s));
		}

		[Test]
		public void Strip_CharOnStringEmpty_StringEmpty()
		{
			Assert.That(string.Empty.Strip('w', 'e'), Is.Empty);
		}

		[Test]
		public void Strip_CharOnNull_Null()
		{
			string s = null;
			Assert.That(s.Strip('x', 'y'), Is.Null);
		}

		[Test]
		public void Strip_ArrayOfChars_CharsRemoved()
		{
			string s = "qwe";
			var chars = new[] { 'w', 'e' };
			Assert.That(s.Strip(chars), Is.EqualTo("q"));
		}

		[Test]
		public void Strip_NullArrayOfChars_Original()
		{
			string s = "qwe";
			char[] chars = null;
			Assert.That(s.Strip(chars), Is.EqualTo(s));
		}

		#endregion

		#region Strip(string)

		[Test]
		public void Strip_ExistingSubstring_StringWithoutSubstring()
		{
			Assert.That("qwe".Strip("qw"), Is.EqualTo("e"));
		}

		[Test]
		public void Strip_NonExistingSubstring_OriginalString()
		{
			string s = "qwe";
			Assert.That(s.Strip("qe"), Is.EqualTo(s));
		}

		[Test]
		public void Strip_StringOnItslef_StringEmpty()
		{
			string s = "qwe";
			Assert.That(s.Strip(s), Is.Empty);
		}

		[Test]
		public void Strip_StringOnStringEmpty_StringEmpty()
		{
			Assert.That(string.Empty.Strip("w"), Is.Empty);
		}

		[Test]
		public void Strip_StringEmptyOnStringEmpty_Exception()
		{
			Assert.That(() => string.Empty.Strip(string.Empty), Throws.ArgumentException);
		}

		[Test]
		public void Strip_StringOnNull_Null()
		{
			string s = null;
			Assert.That(s.Strip("x"), Is.Null);
		}

		[Test]
		public void Strip_NullStringOnNull_Null()
		{
			string s = null;
			Assert.That(s.Strip(s), Is.Null);
		}

		[Test]
		public void Strip_NullStringOnString_Exception()
		{
			string s = "qwe";
			Assert.That(() => s.Strip((string)null), Throws.InstanceOf<ArgumentNullException>());
		}

		#endregion

		#region StripHtmlTags

		[Test]
		public void StripHtmlTags_Null_Null()
		{
			string s = null;
			Assert.That(s.StripHtmlTags(), Is.Null);
		}
		[Test]
		public void StripHtmlTags_Empty_Empty()
		{
			string s = string.Empty;
			Assert.That(s.StripHtmlTags(), Is.Empty);
		}

		[TestCase("asd", "asd")]
		[TestCase("aSd", "aSd")]
		[TestCase("a&åd", "a&åd")]
		[TestCase("2>3", "2>3")]
		[TestCase("2<3", "2<3")]
		public void StripHtmlTags_NoTags_Input(string input, string expected)
		{
			Assert.That(input.StripHtmlTags(), Is.EqualTo(expected));
		}

		[TestCase("<b>asd</b>", "asd")]
		[TestCase("<i>aSd</i>", "aSd")]
		[TestCase("<SPAN>a&åd</SPAN>", "a&åd")]
		[TestCase("<i>asd</i> qwe", "asd qwe")]
		[TestCase("<div>qwe</DIV>", "qwe")]
		[TestCase("<div>qwe", "qwe")]
		public void StripHtmlTags_Tags_TagsRemoved(string input, string expected)
		{
			Assert.That(input.StripHtmlTags(), Is.EqualTo(expected));
		}

		#endregion

		#endregion
	}
}