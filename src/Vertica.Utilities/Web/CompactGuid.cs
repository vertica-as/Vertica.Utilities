using System;

namespace Vertica.Utilities.Web
{
	public struct CompactGuid : IEquatable<CompactGuid>
	{
		#region construction

		/// <summary>
		/// Initialises a new instance of the CompactGuid class
		/// </summary>
		/// <returns></returns>
		public static CompactGuid NewGuid()
		{
			return new CompactGuid(Guid.NewGuid());
		}

		/// <summary>
		/// Creates a CompactGuid from a base64 encoded string
		/// </summary>
		/// <param name="value">The encoded guid as a base64 string</param>
		public CompactGuid(string value) : this()
		{
			Value = value;
			Guid = Decode(value);
		}

		/// <summary>
		/// Creates a CompactGuid from a Guid
		/// </summary>
		/// <param name="guid">The Guid to encode</param>
		public CompactGuid(Guid guid) : this()
		{
			Value = Encode(guid);
			Guid = guid;
		}

		#endregion


		public Guid Guid { get; }
		public string Value { get; }

		public override string ToString()
		{
			return Value;
		}

		#region codification

		/// <summary>
		/// Creates a new instance of a Guid using the string value, 
		/// then returns the base64 encoded version of the Guid.
		/// </summary>
		/// <param name="value">An actual Guid string (i.e. not a CompactGuid)</param>
		/// <returns></returns>
		public static string Encode(string value)
		{
			var guid = new Guid(value);
			return Encode(guid);
		}

		/// <summary>
		/// Encodes the given Guid as a base64 string that is 22 
		/// characters long.
		/// </summary>
		/// <param name="guid">The Guid to encode</param>
		/// <returns></returns>
		public static string Encode(Guid guid)
		{
			string encoded = Convert.ToBase64String(guid.ToByteArray());
			encoded = encoded.Replace("/", "_").Replace("+", "-");
			return encoded.Substring(0, 22);
		}

		/// <summary>
		/// Decodes the given base64 string
		/// </summary>
		/// <param name="value">The base64 encoded string of a Guid</param>
		/// <returns>A new Guid</returns>
		public static Guid Decode(string value)
		{
			value = value.Replace("_", "/").Replace("-", "+");
			byte[] buffer = Convert.FromBase64String(value + "==");
			return new Guid(buffer);
		}

		#endregion

		#region equality

		public bool Equals(CompactGuid other)
		{
			return Guid.Equals(other.Guid);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is CompactGuid && Equals((CompactGuid) obj);
		}

		public override int GetHashCode()
		{
			return Guid.GetHashCode();
		}

		public static bool operator ==(CompactGuid left, CompactGuid right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(CompactGuid left, CompactGuid right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}