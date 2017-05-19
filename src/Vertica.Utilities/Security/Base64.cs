using System;
using System.Collections.Generic;
using System.Text;

namespace Vertica.Utilities.Security
{
	internal class Base64
	{
		// Table for Base64 decoding.
		private static readonly int[] _indexes =
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, 0, 1, 54, 55,
			56, 57, 58, 59, 60, 61, 62, 63, -1, -1,
			-1, -1, -1, -1, -1, 2, 3, 4, 5, 6,
			7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
			17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27,
			-1, -1, -1, -1, -1, -1, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
			41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
			51, 52, 53, -1, -1, -1, -1, -1
		};

		// Table for Base64 encoding.
		private static readonly char[] _codes =
		{
			'.', '/', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
			'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
			'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
			'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
			'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5',
			'6', '7', '8', '9'
		};

		public static char Code(int index)
		{
			return _codes[index];
		}

		/// <summary>Look up the 3 bits base64-encoded by the specified
		/// character, range-checking against the conversion
		/// table.</summary>
		/// <param name="c">The Base64-encoded value</param>
		/// <returns>The decoded value of <c>x</c></returns>
		public static int Char(char c)
		{
			int i = c;
			return (i < 0 || i > _indexes.Length) ? -1 : _indexes[i];
		}

		/// <summary>Encode a byte array using bcrypt's slightly-modified
		/// Base64 encoding scheme. Note that this is _not_ compatible
		/// with the standard MIME-Base64 encoding.</summary>
		/// <param name="d">The byte array to encode</param>
		/// <param name="length">The number of bytes to encode</param>
		/// <returns>A Base64-encoded string</returns>
		public static string Encode(byte[] d, int length)
		{
			if (length <= 0 || length > d.Length)
			{
				throw new ArgumentOutOfRangeException("length", length, null);
			}

			var rs = new StringBuilder(length * 2);

			for (int offset = 0; offset < length; )
			{
				int c1 = d[offset++] & 0xff;
				rs.Append(Code((c1 >> 2) & 0x3f));
				c1 = (c1 & 0x03) << 4;
				if (offset >= length)
				{
					rs.Append(Code(c1 & 0x3f));
					break;
				}
				int c2 = d[offset++] & 0xff;
				c1 |= (c2 >> 4) & 0x0f;
				rs.Append(Code(c1 & 0x3f));
				c1 = (c2 & 0x0f) << 2;
				if (offset >= length)
				{
					rs.Append(Code(c1 & 0x3f));
					break;
				}
				c2 = d[offset++] & 0xff;
				c1 |= (c2 >> 6) & 0x03;
				rs.Append(Code(c1 & 0x3f));
				rs.Append(Code(c2 & 0x3f));
			}

			return rs.ToString();
		}

		/// <summary>Decode a string encoded using BCrypt's Base64 scheme to a
		/// byte array. Note that this is _not_ compatible with the standard
		/// MIME-Base64 encoding.</summary>
		/// <param name="s">The string to decode</param>
		/// <param name="maximumLength">The maximum number of bytes to decode</param>
		/// <returns>An array containing the decoded bytes</returns>
		public static byte[] Decode(string s, int maximumLength)
		{
			var bytes = new List<byte>(Math.Min(maximumLength, s.Length));

			if (maximumLength <= 0)
			{
				throw new ArgumentOutOfRangeException("maximumLength", maximumLength, null);
			}

			for (int offset = 0, slen = s.Length, length = 0; offset < slen - 1 && length < maximumLength; )
			{
				int c1 = Char(s[offset++]);
				int c2 = Char(s[offset++]);
				if (c1 == -1 || c2 == -1)
				{
					break;
				}

				bytes.Add((byte)((c1 << 2) | ((c2 & 0x30) >> 4)));
				if (++length >= maximumLength || offset >= s.Length)
				{
					break;
				}

				int c3 = Char(s[offset++]);
				if (c3 == -1)
				{
					break;
				}

				bytes.Add((byte)(((c2 & 0x0f) << 4) | ((c3 & 0x3c) >> 2)));
				if (++length >= maximumLength || offset >= s.Length)
				{
					break;
				}

				int c4 = Char(s[offset++]);
				bytes.Add((byte)(((c3 & 0x03) << 6) | c4));

				++length;
			}

			return bytes.ToArray();
		}
	}
}