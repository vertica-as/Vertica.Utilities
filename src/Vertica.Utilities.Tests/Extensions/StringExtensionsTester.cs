using System;
using System.Globalization;
using System.IO;
using System.Net;
using NUnit.Framework;
using Testing.Commons.Time;
using Vertica.Utilities.Extensions.StringExt;

namespace Vertica.Utilities.Tests.Extensions
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

		private static readonly string[] _onlySpaces = { " ", "  " };

		[Test]
		[TestCaseSource(nameof(_onlySpaces))]
		public void IsEmpty_OnlySpaces_True(string onlySpaces)
		{
			Assert.That(onlySpaces.IsEmpty(), Is.True);
		}

		[TestCase(null, false)]
		[TestCase("", false)]
		[TestCase("abc", true)]
		public void IsNotEmpty_Combinations(string input, bool isNotEmpty)
		{
			Assert.That(input.IsNotEmpty(), Is.EqualTo(isNotEmpty));
		}

		[Test]
		[TestCaseSource(nameof(_onlySpaces))]
		public void IsNoEmpty_OnlySpaces_False(string onlySpaces)
		{
			Assert.That(onlySpaces.IsNotEmpty(), Is.False);
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
		[TestCaseSource(nameof(_onlySpaces))]
		public void NullIfEmpty_OnlySpaces_Null(string onlySpaces)
		{
			Assert.That(onlySpaces.NullIfEmpty(), Is.Null);
		}

		[Test]
		public void NullIfEmpty_NotEmptyNull_Inout()
		{
			Assert.That("not empty".NullIfEmpty(), Is.EqualTo("not empty"));
		}

		[Test]
		public void EmptyIfNull_Null_Empty()
		{
			string @null = null;
			Assert.That(@null.EmptyIfNull(), Is.Not.Null.And.Empty);
			
		}

		[Test]
		public void EmptyIfNull_NotNull_Inout()
		{
			string notNull = "notNull";
			Assert.That(notNull.EmptyIfNull(), Is.EqualTo(notNull));
		}

		[Test]
		[TestCaseSource(nameof(_onlySpaces))]
		public void EmptyIfNull_OnlySpaces_Inout(string onlySpaces)
		{
			Assert.That(onlySpaces.EmptyIfNull(), Is.EqualTo(onlySpaces));
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
			Assert.That(input.Substr().Right(length), Is.EqualTo(expected));
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
			Assert.That(input.Substr().RightFromFirst(substring), Is.EqualTo(expected));
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
			Assert.That(input.Substr().RightFromLast(substring), Is.EqualTo(expected));
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
			Assert.That(input.Substr().Left(length), Is.EqualTo(expected));
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
			Assert.That(input.Substr().LeftFromFirst(substring), Is.EqualTo(expected));
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
			Assert.That(input.Substr().LeftFromLast(substring), Is.EqualTo(expected));
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
			Assert.That(input.IfNotThere().Append(appendix), Is.EqualTo(expected));
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
			Assert.That(input.IfNotThere().Prepend(prefix), Is.EqualTo(expected));
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
			DivideByZeroException arg = null;
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
			DivideByZeroException arg = null;
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
			DivideByZeroException arg = null;
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

		#region Separate

		[Test]
		public void Separate_NullInput_ReturnNull()
		{
			string s = null;
			Assert.That(s.Separate(1, null), Is.Null);
		}

		[Test]
		public void Separate_ZeroSplitCount_ThrowsArgumentOutOfRangeException()
		{
			Assert.That(() => "string".Separate(0, ""), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void Separate_NullSplitValue_ReturnsSameString()
		{
			string s = "input";
			Assert.That(s.Separate(1, null), Is.EqualTo("input"));
		}

		[Test]
		public void Separate_EmptySplitValue_ReturnsSameString()
		{
			string s = "input";
			Assert.That(s.Separate(1, String.Empty), Is.EqualTo("input"));
		}

		[Test]
		public void Separate_SplitCountEqualsInputLength_ReturnsSameString()
		{
			string s = "input";
			Assert.That(s.Separate((uint)s.Length, "."), Is.EqualTo("input"));
		}

		[Test]
		public void Separate_SplitCountOverflowInputLength_ReturnsSameString()
		{
			string s = "input";
			Assert.That(s.Separate((uint)s.Length + 1, "."), Is.EqualTo("input"));
		}

		[Test]
		public void Separate_ByEachChar_SeparatorEveryOtherChar()
		{
			string s = "input";
			Assert.That(s.Separate(1, "-"), Is.EqualTo("i-n-p-u-t"));
		}

		[Test]
		public void Separate_RoomforOnlyOneSeparator_OnlyOneSeparator()
		{
			Assert.That("input".Separate(4, "-"), Is.EqualTo("inpu-t"));
		}

		[Test]
		public void Separate_RoomforTwoSeparators_TwoSeparators()
		{
			Assert.That("input".Separate(2, "-"), Is.EqualTo("in-pu-t"));
		}

		[Test]
		public void Separate_SeparatorWouldBeLastChar_NoLastSeparator()
		{
			string s = "splitter";
			Assert.That(s.Separate(4, "-"), Is.EqualTo("spli-tter"));
		}

		#endregion

		#region Chunkify

		[Test]
		public void Chunkify_LengthMoreThanChunk_ManyStringsOfChunkLength()
		{
			Assert.That("1234567890".Chunkify(3), Is.EqualTo(new[] { "123", "456", "789", "0" }));
		}

		[Test]
		public void Chunkify_LengthLessThanChunk_OneOriginalString()
		{
			Assert.That("123".Chunkify(5), Is.EqualTo(new[] { "123" }));
		}

		#endregion

		#region IO

		[Test]
		public void GetMemoryStream_RoundTrip_SameString()
		{
			string s1 = "Hello World!";
			MemoryStream m = s1.IO().GetMemoryStream();

			Assert.AreEqual(12, m.Length);

			string s2 = m.IO().GetString();

			Assert.That(s1, Is.EqualTo(s2));
		}

		#endregion

		#region HttpUtility

		[Test]
		public void UrlEncode_SameAsExtended()
		{
			string s = "/æ";
			Assert.That(s.Http().UrlEncode(), Is.EqualTo(WebUtility.UrlEncode(s)));
		}

		[Test]
		public void UrlDecode_SameAsExtended()
		{
			string s = "q%20&æ";
			Assert.That(s.Http().UrlEncode(), Is.EqualTo(WebUtility.UrlEncode(s)));
		}

		[Test]
		public void HtmlEncode_SameAsExtended()
		{
			string s = "&<>";
			Assert.That(s.Http().HtmlEncode(), Is.EqualTo(WebUtility.HtmlEncode(s)));
		}

		[Test]
		public void HtmlDecode_SameAsExtended()
		{
			string s = "&lt;";
			Assert.That(s.Http().HtmlDecode(), Is.EqualTo(WebUtility.HtmlDecode(s)));
		}


		#endregion

		#region Parse()

		[Test]
		public void Parse_IntStringToInt_Correct()
		{
			Assert.That("1".Parse<int>(), Is.EqualTo(1));
		}

		[Test]
		public void Parse_IntStringToNullableInt_Correct()
		{
			Assert.That("1".Parse<int?>(), Is.EqualTo(1));
		}

		[Test]
		public void Parse_NotIntStringToInt_Exception()
		{
			Assert.Throws<Exception>(() => "asd".Parse<int>());
		}

		[Test]
		public void Parse_NotIntStringToNullableInt_Exception()
		{
			Assert.Throws<Exception>(() => "asd".Parse<int?>());
		}

		[Test]
		public void Parse_EmptyStringToInt_0()
		{
			Assert.That(string.Empty.Parse<int>(), Is.EqualTo(0));
		}

		[Test]
		public void Parse_EmptyStringToNullableInt_Null()
		{
			Assert.That(string.Empty.Parse<int?>(), Is.Null);
		}

		[Test]
		public void Parse_NullStringToInt_0()
		{
			string nullStr = null;
			Assert.That(nullStr.Parse<int>(), Is.EqualTo(0));
			// ReSharper disable AssignNullToNotNullAttribute
			Assert.Throws<ArgumentNullException>(() => int.Parse(nullStr));
			// ReSharper restore AssignNullToNotNullAttribute
		}

		[Test]
		public void Parse_NullStringToNullableInt_Null()
		{
			string nullStr = null;
			Assert.That(nullStr.Parse<int?>(), Is.Null);
		}

		[Test]
		public void Parse_StringToCharArray_Exception()
		{
			Assert.Throws<NotSupportedException>(() => "asd".Parse<char[]>());
		}

		#region AsNullable

		readonly Func<string, int> _intParse = s => int.Parse(s);

		private readonly Func<string, DateTime> _ddmmyyParseExact =
			s => DateTime.ParseExact(s, @"dd/MM/yy", CultureInfo.InvariantCulture);

		[TestCase(null, null)]
		[TestCase("", null)]
		[TestCase("3", 3)]
		[TestCase("-1", -1)]
		public void AsNullable_NullableInt(string s, int? expected)
		{
			Assert.That(s.Parse().AsNullable(_intParse), Is.EqualTo(expected));
		}

		[Test]
		public void AsNullable_NotAnInteger_Exception()
		{
			Assert.That(()=> "notAnInteger".Parse().AsNullable(_intParse), Throws.InstanceOf<FormatException>());
		}

		private void parse_NullableDateTime(string s, DateTime? expected)
		{
			Assert.That(s.Parse().AsNullable(_ddmmyyParseExact), Is.EqualTo(expected));
		}

		[Test]
		public void AsNullable_Null_Null()
		{
			parse_NullableDateTime(null, null);
		}
		[Test]
		public void AsNullable_Empty_Null()
		{
			parse_NullableDateTime(string.Empty, null);
		}
		[Test]
		public void AsNullable_CorrectBirthDay_Birthday()
		{
			parse_NullableDateTime("11/03/77", 11.March(1977));
		}
		[Test]
		public void AsNullable_IncorrectFormat_Exception()
		{
			Assert.Throws<FormatException>(() => parse_NullableDateTime("11/03/1977", null));
		}

		[Test]
		public void AsNullable_NotADate_Exception()
		{
			Assert.Throws<FormatException>(() => parse_NullableDateTime("notADate", null));
		}

		#endregion

		// ReSharper disable AccessToModifiedClosure
		[Test]
		public void Parse_Vs_AsNullable_WithStrings()
		{
			string input = "1";

			Assert.DoesNotThrow(() => input.Parse<int>());
			Assert.DoesNotThrow(() => input.Parse().AsNullable(int.Parse));

			input = null;
			Assert.DoesNotThrow(() => input.Parse<int>());
			Assert.DoesNotThrow(() => input.Parse().AsNullable(int.Parse));
		}
		// ReSharper restore AccessToModifiedClosure

		#region TryAsNullable

		readonly TryParseDelegate<int> _intTryParse =
			(string s, out int result) => int.TryParse(s, out result);

		private readonly TryParseDelegate<DateTime> _ddmmyyTryParseExact =
			(string s, out DateTime result) => DateTime.TryParseExact(s, @"dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);


		[TestCase(null, false, null)]
		[TestCase("", false, null)]
		[TestCase("3", true, 3)]
		[TestCase("-1", true, -1)]
		[TestCase("notAnInteger", false, null)]
		public void TryAsNullable_NullableInt(string s, bool result, int? expected)
		{
			int? actual;
			Assert.That(s.Parse().TryAsNullable(out actual, _intTryParse), Is.EqualTo(result));
			Assert.That(expected, Is.EqualTo(actual));
		}

		private void tryParse_NullableDateTime(string s, bool result, DateTime? expected)
		{
			DateTime? actual;
			Assert.That(s.Parse().TryAsNullable(out actual, _ddmmyyTryParseExact), Is.EqualTo(result));
			Assert.That(expected, Is.EqualTo(actual));
		}

		[Test]
		public void TryAsNullable_Null_FalseAndNull()
		{
			tryParse_NullableDateTime(null, false, null);
		}
		[Test]
		public void TryAsNullable_Empty_FalseAndNull()
		{
			tryParse_NullableDateTime(string.Empty, false, null);
		}
		[Test]
		public void TryAsNullable_CorrectBirthDay_TrueAndBirthday()
		{
			tryParse_NullableDateTime("11/03/77", true, 11.March(1977));
		}
		[Test]
		public void TryAsNullable_IncorrectFormat_FalseAndNull()
		{
			tryParse_NullableDateTime("11/03/1977", false, null);
		}

		[Test]
		public void TryAsNullable_NotADate_FalseAndNull()
		{
			tryParse_NullableDateTime("notADate", false, null);
		}

		#endregion

		#endregion

		#region Compress

		[Test]
		public void Compress_LongString_SmallerFootprint()
		{
			string longString = new string('a', 120);

			Assert.That(longString, Has.Length.EqualTo(120));

			Assert.That(longString.Compress(), Has.Length.LessThan(120));
		}

		[Test]
		public void Decompress_GetsOriginalString()
		{
			string original = new string('a', 120),
				compressed = original.Compress();
			
			Assert.That(compressed.Decompress(), Is.EqualTo(original));
		}

		#endregion
	}
}