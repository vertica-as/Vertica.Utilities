using System;
using Vertica.Utilities_v4.Extensions.ObjectExt;

namespace Vertica.Utilities_v4.Extensions.StringExt
{
	public static class StringExtensions
	{
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
		 /// <param name="s"></param>
		 /// <param name="substring"></param>
		 /// <returns></returns>
		 public static string RightFromFirst(this string s, string substring)
		 {
			 return s.NullOrAction(() =>
			 {
				 substring = substring.EmptyIfNull();
				 int indexOfSubstringEnd = s.IndexOf(substring, StringComparison.Ordinal) >= 0 ?
					 s.IndexOf(substring, StringComparison.Ordinal) + substring.Length :
					 -1;
				 return indexOfSubstringEnd < 0 ? null : s.Right(s.Length - indexOfSubstringEnd);
			 });
		 }
	}
}