using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Vertica.Utilities.Extensions.Infrastructure;
using Vertica.Utilities.Extensions.ObjectExt;

namespace Vertica.Utilities.Extensions.StringExt
{
	public delegate bool TryParseDelegate<T>(string s, out T result);

	public static class StringExtensions
	{
		#region emptiness

		public static bool IsEmpty(this string str)
		{
			return string.IsNullOrWhiteSpace(str);
		}

		public static bool IsNotEmpty(this string str)
		{
			return !IsEmpty(str);
		}

		public static string NullIfEmpty(this string s)
		{
			return s.IsEmpty() ? null : s;
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
			return s.Safe(s1 =>
			{
				string result = s1;
				if (chars != null)
				{
					foreach (char c in chars)
					{
						result = result.Replace(c.ToString(), string.Empty);
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
			return s.Safe(s1 => s1.Replace(subString, string.Empty));
		}

		private static readonly string _strippingPattern = @"<(.|\n)*?>";
		private static readonly Regex _strippingRegEx = new Regex(_strippingPattern, RegexOptions.Compiled);
		public static string StripHtmlTags(this string input)
		{
			return input.Safe(s1 => _strippingRegEx.Replace(s1, string.Empty));
		}

		#endregion

		#region substrings

		public class SubstrExtensionPoint : ExtensionPoint<string>
		{
			public SubstrExtensionPoint(string value) : base(value) { }
		}

		public static SubstrExtensionPoint Substr(this string subject)
		{
			return new SubstrExtensionPoint(subject);
		}

		#region Right

		/// <summary>
		/// Returns the last few characters of the string with a length
		/// specified by the given parameter. If the string's length is less than the 
		/// given length the complete string is returned. If length is zero or 
		/// less an empty string is returned
		/// </summary>
		/// <param name="ep">the string to process</param>
		/// <param name="length">Number of characters to return</param>
		/// <returns></returns>
		public static string Right(this SubstrExtensionPoint ep, int length)
		{
			string e = ep.ExtendedValue;
			length = Math.Max(length, 0);
			return e.Safe(s => (s.Length > length) ? s.Substring(s.Length - length, length) : s);
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
		public static string RightFromFirst(this SubstrExtensionPoint sp, string substring, StringComparison comparison = StringComparison.Ordinal)
		{
			string e = sp.ExtendedValue;
			return e.Safe(s =>
			{
				substring = substring.EmptyIfNull();
				int indexOfSubstringEnd = s.IndexOf(substring, comparison) >= 0 ?
					s.IndexOf(substring, comparison) + substring.Length :
					-1;
				return indexOfSubstringEnd < 0 ? null : sp.Right(s.Length - indexOfSubstringEnd);
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
		public static string RightFromLast(this SubstrExtensionPoint sp, string substring, StringComparison comparison = StringComparison.Ordinal)
		{
			string e = sp.ExtendedValue;
			return e.Safe(s =>
			{
				substring = substring.EmptyIfNull();
				int indexOfSubstringEnd = -1;

				if (substring.IsEmpty())
					indexOfSubstringEnd = s.Length;
				else if (s.LastIndexOf(substring, comparison) >= 0)
					indexOfSubstringEnd = s.LastIndexOf(substring, comparison) + substring.Length;

				return indexOfSubstringEnd < 0 ? null : sp.Right(s.Length - indexOfSubstringEnd);
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
		public static string Left(this SubstrExtensionPoint sp, int length)
		{
			string e = sp.ExtendedValue;
			length = Math.Max(length, 0);
			return e.Safe(s => (s.Length > length) ? s.Substring(0, length) : s);
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
		public static string LeftFromFirst(this SubstrExtensionPoint sp, string substring, StringComparison comparison = StringComparison.Ordinal)
		{
			string e = sp.ExtendedValue;
			return e.Safe(s =>
			{
				substring = substring.EmptyIfNull();
				int indexOfSubstringStart = s.IndexOf(substring, comparison) >= 0 ?
					s.IndexOf(substring, comparison) : -1;

				return indexOfSubstringStart < 0 ? null : sp.Left(indexOfSubstringStart);
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

		public static string LeftFromLast(this SubstrExtensionPoint sp, string substring, StringComparison comparison = StringComparison.Ordinal)
		{
			string e = sp.ExtendedValue;
			return e.Safe(s =>
			{
				substring = substring.EmptyIfNull();
				int indexOfSubstringStart = -1;

				if (substring.IsEmpty())
					indexOfSubstringStart = 0;
				else if (s.LastIndexOf(substring, comparison) >= 0)
					indexOfSubstringStart = s.LastIndexOf(substring, comparison);

				return indexOfSubstringStart < 0 ? null : sp.Left(indexOfSubstringStart);
			});
		}

		#endregion

		#endregion

		#region conditional concatenation

		public class IfNotThereExtensionPoint : ExtensionPoint<string>
		{
			public IfNotThereExtensionPoint(string value) : base(value) { }
		}

		public static IfNotThereExtensionPoint IfNotThere(this string subject)
		{
			return new IfNotThereExtensionPoint(subject);
		}

		public static string Append(this IfNotThereExtensionPoint sp, string appendix, StringComparison comparison = StringComparison.Ordinal)
		{
			string str = sp.ExtendedValue;
			if (str == null && appendix == null) return null;
			return appendIfNotThere(str.EmptyIfNull(), appendix.EmptyIfNull(), comparison);
		}

		public static string Prepend(this IfNotThereExtensionPoint sp, string prefix, StringComparison comparison = StringComparison.Ordinal)
		{
			string str = sp.ExtendedValue;
			if (str == null && prefix == null) return null;
			return prependIfNotThere(str.EmptyIfNull(), prefix.EmptyIfNull(), comparison);
		}

		// we have preciously handled nulls, so that they are empties
		private static string appendIfNotThere(string str, string appendix, StringComparison comparison)
		{
			return str.EndsWith(appendix, comparison) ? str : string.Concat(str, appendix);
		}

		private static string prependIfNotThere(string str, string prefix, StringComparison comparison)
		{
			return str.StartsWith(prefix, comparison) ? str : string.Concat(prefix, str);
		}

		#endregion

		public static string FormatWith(this string s, params object[] additionalArgs)
		{
			return s.Safe(input =>
			{
				string result = additionalArgs == null || additionalArgs.Length == 0 ?
					input :
					string.Format(input, additionalArgs);
				return result;
			});
		}

		public static string Separate(this string ss, uint size, string separator)
		{
			Guard.AgainstArgument<ArgumentOutOfRangeException>("size", size == 0);

			return ss.Safe(input =>
			{
				// RegEx pattern: (.{1,4})
				string pattern = "(.{{1,{0}}})".FormatWith(size);
				return Regex.Replace(input, pattern, m =>
					m.Value + (m.NextMatch().Success ?
					separator :
					string.Empty));
			});
		}

		public static IEnumerable<string> Chunkify(this string text, uint chunkSize)
		{
			int offset = 0;
			while (offset < text.Length)
			{
				int size = Math.Min((int)chunkSize, text.Length - offset);
				yield return text.Substring(offset, size);
				offset += size;
			}
		}

		#region IO

		public class IOExtensionPoint<T> : ExtensionPoint<T>
		{
			public IOExtensionPoint(T value) : base(value) { }
		}

		public static IOExtensionPoint<MemoryStream> IO(this MemoryStream subject)
		{
			return new IOExtensionPoint<MemoryStream>(subject);
		}

		public static IOExtensionPoint<string> IO(this string subject)
		{
			return new IOExtensionPoint<string>(subject);
		}

		public static string GetString(this IOExtensionPoint<MemoryStream> m)
		{
			string result = null;

			if (m?.ExtendedValue != null && m.ExtendedValue.Length > 0)
			{
				m.ExtendedValue.Flush();
				m.ExtendedValue.Position = 0;
				var sr = new StreamReader(m.ExtendedValue);
				result = sr.ReadToEnd();
			}
			return result;
		}

		/// <summary>
		/// Default encoding set to <see cref="Encoding.UTF8"/>
		/// </summary>
		public static MemoryStream GetMemoryStream(this IOExtensionPoint<string> s)
		{
			return GetMemoryStream(s, Encoding.UTF8);
		}

		public static MemoryStream GetMemoryStream(this IOExtensionPoint<string> s, Encoding encoding)
		{
			MemoryStream result = null;
			if (s.ExtendedValue.IsNotEmpty())
			{
				byte[] bytes = encoding.GetBytes(s.ExtendedValue);
				result = new MemoryStream(bytes);
			}
			return result;
		}

		#endregion

		#region HttpUtility

		public class HttpExtensionPoint : ExtensionPoint<string>
		{
			public HttpExtensionPoint(string value) : base(value) { }
		}

		public static HttpExtensionPoint Http(this string subject)
		{
			return new HttpExtensionPoint(subject);
		}

		public static string UrlEncode(this HttpExtensionPoint str)
		{
			return WebUtility.UrlEncode(str.ExtendedValue);
		}

		public static string UrlDecode(this HttpExtensionPoint str)
		{
			return WebUtility.UrlDecode(str.ExtendedValue);
		}

		public static string HtmlEncode(this HttpExtensionPoint str)
		{
			return WebUtility.HtmlEncode(str.ExtendedValue);
		}

		public static string HtmlDecode(this HttpExtensionPoint str)
		{
			return WebUtility.HtmlDecode(str.ExtendedValue);
		}

		#endregion

		#region Parse

		public static T Parse<T>(this string s)
		{
			T result = default(T);
			if (s.IsNotEmpty())
			{
				TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
				result = (T)tc.ConvertFrom(s);
			}
			return result;
		}

		public static ExtensionPoint<string> Parse(this string subject)
		{
			return new ExtensionPoint<string>(subject);
		}

		/// <summary>
		/// Converts the string representation of a <typeparamref name="T"/> struct to its nullable equivalent.
		/// </summary>
		/// <remarks>If the string <paramref name="s"/> is null, null will be returned.</remarks>
		public static T? AsNullable<T>(this ExtensionPoint<string> s, Func<string, T> parser) where T : struct
		{
			T? result = null;
			if (s.ExtendedValue.IsNotEmpty())
			{
				result = parser(s.ExtendedValue);
			}
			return result;
		}

		public static bool TryAsNullable<T>(this ExtensionPoint<string> s, out T? result, TryParseDelegate<T> parser) where T : struct
		{
			bool success;
			if (s.ExtendedValue.IsEmpty())
			{
				result = null;
				success = false;
			}
			else
			{
				T temp;
				success = parser(s.ExtendedValue, out temp);
				if (success)
				{
					result = temp;
				}
				else
				{
					result = null;
				}
			}
			return success;
		}

		#endregion

		#region Compression

		/// <summary>
		/// Compresses the string.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static string Compress(this string text)
		{
			byte[] buffer = Encoding.UTF8.GetBytes(text);

			using (var memoryStream = new MemoryStream())
			{
				using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
				{
					gZipStream.Write(buffer, 0, buffer.Length);
				}

				memoryStream.Position = 0;

				var compressedData = new byte[memoryStream.Length];
				memoryStream.Read(compressedData, 0, compressedData.Length);

				var gZipBuffer = new byte[compressedData.Length + 4];
				Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
				Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);

				return Convert.ToBase64String(gZipBuffer);
			}
		}

		/// <summary>
		/// Decompresses the string.
		/// </summary>
		/// <param name="compressedText">The compressed text.</param>
		/// <returns></returns>
		public static string Decompress(this string compressedText)
		{
			byte[] gZipBuffer = Convert.FromBase64String(compressedText);
			using (var memoryStream = new MemoryStream())
			{
				int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
				memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

				var buffer = new byte[dataLength];

				memoryStream.Position = 0;
				using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					gZipStream.Read(buffer, 0, buffer.Length);
				}

				return Encoding.UTF8.GetString(buffer);
			}
		}

		#endregion

	}
}