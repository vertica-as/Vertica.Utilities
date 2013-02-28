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

		#region substring

		#region Right

		[TestCase("lazy lazy fox jumped", 3, "ped", Description = "last 3 chars")]
		[TestCase("lazy lazy fox jumped", 30, "lazy lazy fox jumped", Description = "length more than argument --> argument")]
		[TestCase("lazy lazy fox jumped", 0, "", Description = "zero length --> empty")]
		[TestCase("lazy lazy fox jumped", -1, "", Description = "negative length --> empty")]
		[TestCase(null, 30, null, Description = "null --> null")]
		[TestCase("", 30, "", Description = "empty --> empty")]
		public void Right_Specification(string input, int length, string expected)
		{
			Assert.That(input.Right(length), Is.EqualTo(expected));
		}

		[TestCase(null, "lazy lazy fox jumped", null, Description = "String not found")]
		[TestCase(null, "", null, Description = "*.RightFromFirst(null) --> *")]
		[TestCase(null, null, null, Description = "*.RightFromFirst(null) --> *")]
		[TestCase("", "lazy lazy fox jumped", null, Description = "String not found")]
		[TestCase("", "", "", Description = "*.RightFromFirst(string.Empty) --> *")]
		[TestCase("", null, "", Description = "*.RightFromFirst(null) --> *")]
		[TestCase("lazy lazy fox jumped", null, "lazy lazy fox jumped", Description = "*.RightFromFirst(null) --> *")]
		[TestCase("lazy lazy fox jumped", "", "lazy lazy fox jumped", Description = "*.RightFromFirst(string.Empty) --> *")]
		[TestCase("lazy lazy fox jumped", "l", "azy lazy fox jumped", Description = "right part from beginning")]
		[TestCase("lazy lazy fox jumped", "f", "ox jumped", Description = "right part from contained char")]
		[TestCase("lazy lazy fox jumped", "azy", " lazy fox jumped", Description = "right part from contained substring")]
		[TestCase("lazy lazy fox jumped", "d", "", Description = "Right from end of word")]
		[TestCase("lazy lazy fox jumped", "ed", "", Description = "Right from end of word")]
		[TestCase("lazy lazy fox jumped", "c", null, Description = "Char not found")]
		[TestCase("lazy lazy fox jumped", "lozy", null, Description = "String not found")]
		[TestCase("DOMAIN\\username", "\\", "username")]
		public void RightFromFirst_Specification(string input, string substring, string expected)
		{
			Assert.That(input.RightFromFirst(substring), Is.EqualTo(expected));
		}


		[TestCase(null, "lazy lazy fox jumped", null, Description = "String not found")]
		[TestCase(null, "", null, Description = "*.RightFromLast(null) --> *")]
		[TestCase(null, null, null, Description = "*.RightFromLast(null) --> *")]
		[TestCase("", "lazy lazy fox jumped", null, Description = "String not found")]
		[TestCase("", "", "", Description = "*.RightFromLast(string.Empty) --> *")]
		[TestCase("", null, "", Description = "*.RightFromLast(null) --> *")]
		[TestCase("lazy lazy fox jumped", null, "", Description = "*.RightFromLast(null) --> string.Empty")]
		[TestCase("lazy lazy fox jumped", "", "", Description = "*.RightFromLast(string.Empty) --> string.Empty")]
		[TestCase("lazy lazy fox jumped", "l", "azy fox jumped", Description = "right part from beginning")]
		[TestCase("lazy lazy fox jumped", "f", "ox jumped", Description = "right part from contained char")]
		[TestCase("lazy lazy fox jumped", "azy", " fox jumped", Description = "right part from contained substring")]
		[TestCase("lazy lazy fox jumped", "d", "", Description = "Right from end of word")]
		[TestCase("lazy lazy fox jumped", "ed", "", Description = "Right from end of word")]
		[TestCase("lazy lazy fox jumped", "c", null, Description = "Char not found")]
		[TestCase("lazy lazy fox jumped", "lozy", null, Description = "String not found")]
		[TestCase("DOMAIN\\username", "\\", "username")]
		public void RightFromLast_Specification(string input, string substring, string expected)
		{
			Assert.That(input.RightFromLast(substring), Is.EqualTo(expected));
		}

		#endregion

		#region Left

		[TestCase("lazy lazy fox jumped", 4, "lazy", Description = "first 4 chars")]
		[TestCase("lazy lazy fox jumped", 30, "lazy lazy fox jumped", Description = "length more than argument --> argument")]
		[TestCase("lazy lazy fox jumped", 0, "", Description = "zero length --> empty")]
		[TestCase("lazy lazy fox jumped", -1, "", Description = "negative length --> empty")]
		[TestCase(null, 30, null, Description = "null --> null")]
		[TestCase("", 30, "", Description = "empty --> empty")]
		public void Left_Specification(string input, int length, string expected)
		{
			Assert.That(input.Left(length), Is.EqualTo(expected));
		}


		[TestCase(null, "lazy lazy fox jumped", null, Description = "String not found")]
		[TestCase(null, "", null, Description = "null.LeftFromFirst(null) --> null")]
		[TestCase(null, null, null, Description = "null.LeftFromFirst(null) --> null")]
		[TestCase("", "lazy lazy fox jumped", null, Description = "String not found")]
		[TestCase("", "", "", Description = "*.LeftFromFirst(string.Empty) --> string.Empty")]
		[TestCase("", null, "", Description = "*.LeftFromFirst(null) --> string.Empty")]
		[TestCase("lazy lazy fox jumped", null, "", Description = "*.LeftFromFirst(null) --> string.Empty")]
		[TestCase("lazy lazy fox jumped", "", "", Description = "*.LeftFromFirst(string.Empty) --> string.Empty")]
		[TestCase("lazy lazy fox jumped", "l", "", Description = "Left from beginning")]
		[TestCase("lazy lazy fox jumped", "f", "lazy lazy ", Description = "Left from contained char")]
		[TestCase("lazy lazy fox jumped", "azy", "l", Description = "LEft from contained substring")]
		[TestCase("lazy lazy fox jumped", "d", "lazy lazy fox jumpe", Description = "Left from end of word")]
		[TestCase("lazy lazy fox jumped", "ed", "lazy lazy fox jump", Description = "Left from end of word")]
		[TestCase("lazy lazy fox jumped", "c", null, Description = "Char not found")]
		[TestCase("lazy lazy fox jumped", "lozy", null, Description = "String not found")]
		[TestCase("DOMAIN\\username", "\\", "DOMAIN")]
		public void LeftFromFirst_Specification(string input, string substring, string expected)
		{
			Assert.That(input.LeftFromFirst(substring), Is.EqualTo(expected));
		}


		[TestCase(null, "lazy lazy fox jumped", null, Description = "String not found")]
		[TestCase(null, "", null, Description = "*.LeftFromLast(null) --> *")]
		[TestCase(null, null, null, Description = "*.LeftFromLast(null) --> *")]
		[TestCase("", "lazy lazy fox jumped", null, Description = "String not found")]
		[TestCase("", "", "", Description = "*.LeftFromLast(string.Empty) --> *")]
		[TestCase("", null, "", Description = "*.LeftFromLast(null) --> *")]
		[TestCase("lazy lazy fox jumped", null, "", Description = "*.LeftFromLast(null) --> *")]
		[TestCase("lazy lazy fox jumped", "", "", Description = "*.LeftFromLast(string.Empty) --> *")]
		[TestCase("lazy lazy fox jumped", "l", "lazy ", Description = "Left from beginning")]
		[TestCase("lazy lazy fox jumped", "f", "lazy lazy ", Description = "Left from contained char")]
		[TestCase("lazy lazy fox jumped", "azy", "lazy l", Description = "LEft from contained substring")]
		[TestCase("lazy lazy fox jumped", "d", "lazy lazy fox jumpe", Description = "Left from end of word")]
		[TestCase("lazy lazy fox jumped", "ed", "lazy lazy fox jump", Description = "Left from end of word")]
		[TestCase("lazy lazy fox jumped", "c", null, Description = "Char not found")]
		[TestCase("lazy lazy fox jumped", "lozy", null, Description = "String not found")]
		[TestCase("DOMAIN\\username", "\\", "DOMAIN")]
		public void LeftFromLast_Specification(string input, string substring, string expected)
		{
			Assert.That(input.LeftFromLast(substring), Is.EqualTo(expected));
		}

		#endregion

		#endregion

		#region conditional concatenation

		[TestCase("abc", "c", "abc")]
		[TestCase("abc", "C", "abcC")]
		[TestCase("abc", "z", "abcz")]
		[TestCase("", "c", "c")]
		[TestCase(null, "c", "c")]
		[TestCase("c", "", "c")]
		[TestCase("c", null, "c")]
		[TestCase("", "", "")]
		[TestCase("", null, "")]
		[TestCase(null, "", "")]
		[TestCase(null, null, null)]
		public void AppendIfNotThere_Combinations(string input, string appendix, string expected)
		{
			Assert.That(input.AppendIfNotThere(appendix), Is.EqualTo(expected));
		}


		[TestCase("abc", "a", "abc")]
		[TestCase("abc", "A", "Aabc")]
		[TestCase("abc", "z", "zabc")]
		[TestCase("", "c", "c")]
		[TestCase(null, "c", "c")]
		[TestCase("c", "", "c")]
		[TestCase("c", null, "c")]
		[TestCase("", "", "")]
		[TestCase("", null, "")]
		[TestCase(null, "", "")]
		[TestCase(null, null, null)]
		public void PrependIfNotThere_Combinations(string input, string prefix, string expected)
		{
			Assert.That(input.PrependIfNotThere(prefix), Is.EqualTo(expected));
		}

		#endregion

		#region FormatWith

		[Test]
		public void FormatWith_OneArgument_FormattedString()
		{
			Assert.That("{0}".FormatWith(3), Is.EqualTo("3"));
		}

		[Test]
		public void FormatWith_OneArgumentNotEnoughFormatArguments_Exception()
		{
			Assert.That(() => "{0}{1}".FormatWith(3), Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void FormatWith_OneNullArgument_NoException()
		{
			NotFiniteNumberException arg = null;
			Assert.That(() => "{0}".FormatWith(arg), Throws.Nothing);
		}

		[Test]
		public void FormatWith_OneArgumentNoFormatArgument_OriginalString()
		{
			Assert.That("asd".FormatWith(2), Is.EqualTo("asd"));
		}

		[Test]
		public void FormatWith_OneArgumentStringEmpty_Empty()
		{
			Assert.That(string.Empty.FormatWith(2), Is.Empty);
		}

		[Test]
		public void FormatWith_OneArgumentNull_Null()
		{
			string s = null;
			Assert.That(s.FormatWith(2), Is.Null);
		}

		[Test]
		public void FormatWith_MultipleArguments_FormattedString()
		{
			Assert.That("{0}{1}".FormatWith(3, "3"), Is.EqualTo("33"));
		}

		[Test]
		public void FormatWith_MultipleArgumentsNotEnoughFormatArguments_Exception()
		{
			Assert.That(() => "{0}{1}{2}".FormatWith(3, "2"), Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void FormatWith_MultipleNullArguments_NoException()
		{
			NotFiniteNumberException arg = null;
			Assert.That(() => "{0}{1}".FormatWith(arg, arg),
				Throws.Nothing);
		}

		[Test]
		public void FormatWith_MultipleArgumentsNoFormatArgument_OriginalString()
		{
			string s = "asd";
			Assert.That(s.FormatWith(2, "2"), Is.EqualTo(s));
		}

		[Test]
		public void FormatWith_MultipleArgumentsStringEmpty_Empty()
		{
			string s = string.Empty;
			Assert.That(s.FormatWith(2, "3"), Is.Empty);
		}

		[Test]
		public void FormatWith_MultipleArgumentsNull_Null()
		{
			string s = null;
			Assert.That(s.FormatWith(2, "2"), Is.Null);
		}

		[Test]
		public void FormatWith_NoArgumentsNull_Null()
		{
			string s = null;
			Assert.That(s.FormatWith(), Is.Null);
		}

		[Test]
		public void FormatWith_NoArgumentsNotNull_Same()
		{
			string s = "asd";
			Assert.That(s.FormatWith(), Is.EqualTo(s));
		}

		[Test]
		public void IndirectFormat_MultipleArguments_FormattedString()
		{
			Assert.That(indirectFormat("{0}{1}", 3, "3"), Is.EqualTo("33"));
		}

		[Test]
		public void IndirectFormat_MultipleArgumentsNotEnoughFormatArguments_Exception()
		{
			Assert.Throws<FormatException>(() => indirectFormat("{0}{1}{2}", 3, "2"));
		}

		[Test]
		public void IndirectFormat_MultipleNullArguments_NoException()
		{
			NotFiniteNumberException arg = null;
			Assert.DoesNotThrow(() => indirectFormat("{0}{1}", arg, arg));
		}

		[Test]
		public void IndirectFormat_MultipleArgumentsNoFormatArgument_OriginalString()
		{
			string s = "asd";
			Assert.That(indirectFormat(s, 2, "2"), Is.EqualTo(s));
		}

		[Test]
		public void IndirectFormat_MultipleArgumentsStringEmpty_Empty()
		{
			string s = string.Empty;
			Assert.That(indirectFormat(s, 2, "3"), Is.Empty);
		}

		[Test]
		public void IndirectFormat_MultipleArgumentsNull_Null()
		{
			string s = null;
			Assert.That(indirectFormat(s, 2, "2"), Is.Null);
		}

		private static string indirectFormat(string s, params object[] parameters)
		{
			return s.FormatWith(parameters);
		}

		#endregion
	}
}