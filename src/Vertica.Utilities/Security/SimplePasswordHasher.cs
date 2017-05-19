using System;
using System.Security.Cryptography;
using System.Text;

namespace Vertica.Utilities.Security
{
	public class SimplePasswordHasher : IPasswordHasher
	{
		private readonly string _userName;

		public SimplePasswordHasher(string userName)
		{
			_userName = userName;
		}

		private string generateSalt()
		{
			var hasher = new Rfc2898DeriveBytes(_userName, Encoding.UTF8.GetBytes("thisisasalt"), 10000);
			return Convert.ToBase64String(hasher.GetBytes(25));
		}

		public string HashPassword(string password)
		{
			string salt = generateSalt();
			var hasher = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10000);
			return Convert.ToBase64String(hasher.GetBytes(25));
		}

		public bool CheckPassword(string password, string hashed)
		{
			return StringComparer.Ordinal.Equals(hashed, HashPassword(password));
		}
	}
}