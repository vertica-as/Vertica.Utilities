using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using Vertica.Utilities_v4.Extensions.ObjectExt;

namespace Vertica.Utilities_v4.Extensions.StringExt
{
	public static class StringExtensions
	{
		#region emptiness

		public static bool IsEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		public static bool IsNotEmpty(this string str)
		{
			return !IsEmpty(str);
		}

		public static string NullIfEmpty(this string s)
		{
			return (s == string.Empty) ? null : s;
		}

		public static string EmptyIfNull(this string s)
		{
			return s ?? string.Empty;
		}

		#endregion

		#region stripping

		/// <summary>
		/// Strip a string of the specified characters.
		/// </summary>
		/// <param name="s">the string to process</param>
		/// <param name="chars">list of characters to remove from the string</param>
		/// <example>
		/// string s = "abcde";
		/// 
		/// s = s.Strip('a', 'd');  //s becomes 'bce;
		/// </example>
		public static string Strip(this string s, params char[] chars)
		{
			return s.NullOrAction(() =>
			{
				string result = s;
				if (chars != null)
				{
					foreach (char c in chars)
					{
						result = result.Replace(c.ToString(CultureInfo.CurrentCulture), string.Empty);
					}
				}
				return result;
			});
		}
		/// <summary>
		/// Strip a string of the specified substring.
		/// </summary>
		/// <param name="s">the string to process</param>
		/// <param name="subString">substring to remove</param>
		/// <example>
		/// string s = "abcde";
		/// 
		/// s = s.Strip("bcd");  //s becomes 'ae;
		/// </example>
		/// <returns></returns>
		public static string Strip(this string s, string subString)
		{
			return s.NullOrAction(() => s.Replace(subString, string.Empty));
		}

		private static readonly string _strippingPattern = @"<(.|\n)*?>";
		private static readonly Regex _strippingRegEx = new Regex(_strippingPattern, RegexOptions.Compiled);
		public static string StripHtmlTags(this string input)
		{
			return input.NullOrAction(() => _strippingRegEx.Replace(input, string.Empty));
		}

		#endregion

		#region substrings

		#region Right

		/// <summary>
		/// Returns the last few characters of the string with a length
		/// specified by the given parameter. If the string's length is less than the 
		/// given length the complete string is returned. If length is zero or 
		/// less an empty string is returned
		/// </summary>
		/// <param name="s">the string to process</param>
		/// <param name="length">Number of characters to return</param>
		/// <returns></returns>
		public static string Right(this string s, int length)
		{
			length = Math.Max(length, 0);
			return s.NullOrAction(() => (s.Length > length) ? s.Substring(s.Length - length, length) : s);
		}

		/// <summary>
		/// Returns the right part from the first ocurrence of the given substring (without the substring).
		/// </summary>
		/// <remarks>
		/// Features driven by this set of rules:
		/// <list type="bullet">
		/// <item><description>null.RightFromFirst(*) --> null</description></item>
		/// <item><description>null and string.Empty are always substrings of a not null string</description></item>
		/// /// <item><description>string.Empty only contains itself and null</description></item>
		/// <item><description>*.RightFromFirst(notFound) --> null</description></item>
		/// </list>
		/// </remarks>
		public static string RightFromFirst(this string s, string substring, StringComparison comparison = StringComparison.Ordinal)
		{
			return s.NullOrAction(() =>
			{
				substring = substring.EmptyIfNull();
				int indexOfSubstringEnd = s.IndexOf(substring, comparison) >= 0 ?
					s.IndexOf(substring, comparison) + substring.Length :
					-1;
				return indexOfSubstringEnd < 0 ? null : s.Right(s.Length - indexOfSubstringEnd);
			});
		}

		/// <summary>
		/// Returns the right part from the last ocurrence of the given substring (without the substring).
		/// </summary>
		/// <remarks>
		/// Features driven by this set of rules:
		/// <list type="bullet">
		/// <item><description>null.RightFromLast(*) --> null</description></item>
		/// <item><description>null and string.Empty are always substrings of a not null string</description></item>
		/// /// <item><description>string.Empty only contains itself and null</description></item>
		/// <item><description>*.RightFromLast(notFound) --> null</description></item>
		/// </list>
		/// </remarks>
		public static string RightFromLast(this string s, string substring, StringComparison comparison = StringComparison.Ordinal)
		{
			return s.NullOrAction(() =>
			{
				substring = substring.EmptyIfNull();
				int indexOfSubstringEnd = -1;

				if (substring.IsEmpty())
					indexOfSubstringEnd = s.Length;
				else if (s.LastIndexOf(substring, comparison) >= 0)
					indexOfSubstringEnd = s.LastIndexOf(substring, comparison) + substring.Length;

				return indexOfSubstringEnd < 0 ? null : s.Right(s.Length - indexOfSubstringEnd);
			});
		}

		#endregion

		#region Left

		/// <summary>
		/// Returns the first few characters of the string with a length
		/// specified by the given parameter. If the string's length is less than the 
		/// given length the complete string is returned. If length is zero or 
		/// less an empty string is returned
		/// </summary>
		/// <param name="s">the string to process</param>
		/// <param name="length">Number of characters to return</param>
		/// <returns></returns>
		public static string Left(this string s, int length)
		{
			length = Math.Max(length, 0);
			return s.NullOrAction(() => (s.Length > length) ? s.Substring(0, length) : s);
		}


		/// <summary>
		/// Returns the left part from the first ocurrence of the given substring (without the substring).
		/// </summary>
		/// <remarks>
		/// Features driven by this set of rules:
		/// <list type="bullet">
		/// <item><description>null.LeftFromFirst(*) --> null</description></item>
		/// <item><description>null and string.Empty are always substrings of a not null string</description></item>
		/// /// <item><description>string.Empty only contains itself and null</description></item>
		/// <item><description>*.LeftFromFirst(notFound) --> null</description></item>
		/// </list>
		/// </remarks>
		public static string LeftFromFirst(this string s, string substring, StringComparison comparison = StringComparison.Ordinal)
		{
			return s.NullOrAction(() =>
			{
				substring = substring.EmptyIfNull();
				int indexOfSubstringStart = s.IndexOf(substring, comparison) >= 0 ?
					s.IndexOf(substring, comparison) : -1;

				return indexOfSubstringStart < 0 ? null : s.Left(indexOfSubstringStart);
			});
		}

		/// <summary>
		/// Returns the left part from the last ocurrence of the given substring (without the substring).
		/// </summary>
		/// <remarks>
		/// Features driven by this set of rules:
		/// <list type="bullet">
		/// <item><description>null.LeftFromLast(*) --> null</description></item>
		/// <item><description>null and string.Empty are always substrings of a not null string</description></item>
		/// /// <item><description>string.Empty only contains itself and null</description></item>
		/// <item><description>*.LeftFromLast(notFound) --> null</description></item>
		/// </list>
		/// </remarks>

		public static string LeftFromLast(this string s, string substring, StringComparison comparison = StringComparison.Ordinal)
		{
			return s.NullOrAction(() =>
			{
				substring = substring.EmptyIfNull();
				int indexOfSubstringStart = -1;

				if (substring.IsEmpty())
					indexOfSubstringStart = 0;
				else if (s.LastIndexOf(substring, comparison) >= 0)
					indexOfSubstringStart = s.LastIndexOf(substring, comparison);

				return indexOfSubstringStart < 0 ? null : s.Left(indexOfSubstringStart);
			});
		}

		#endregion

		#endregion

		#region conditional concatenation

		public static string AppendIfNotThere(this string str, string appendix)
		{
			if (str == null && appendix == null) return null;
			return appendIfNotThere(str.EmptyIfNull(), appendix.EmptyIfNull());
		}

		public static string PrependIfNotThere(this string str, string prefix)
		{
			if (str == null && prefix == null) return null;
			return prependIfNotThere(str.EmptyIfNull(), prefix.EmptyIfNull());
		}

		// we have preciously handled nulls, so that they are empties
		private static string appendIfNotThere(string str, string appendix)
		{
			return str.EndsWith(appendix) ? str : string.Concat(str, appendix);
		}

		private static string prependIfNotThere(string str, string prefix)
		{
			return str.StartsWith(prefix) ? str : string.Concat(prefix, str);
		}

		#endregion

		public static string FormatWith(this string s, params object[] additionalArgs)
		{
			return s.NullOrAction(() =>
			{
				string result = additionalArgs == null || additionalArgs.Length == 0 ?
					s :
					string.Format(s, additionalArgs);
				return result;
			});
		}

		public static T Parse<T>(this string s)
		{
			T result = default(T);
			if (!String.IsNullOrEmpty(s))
			{
				TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
				result = (T)tc.ConvertFrom(s);
			}
			return result;
		}
	}
}