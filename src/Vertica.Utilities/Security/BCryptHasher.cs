namespace Vertica.Utilities_v4.Security
{
	public class BCryptHasher : IPasswordHasher
	{
		public string HashPassword(string password)
		{
			string salt = BCrypt.GenerateSalt();
			return BCrypt.HashPassword(password, salt);
		}

		public bool CheckPassword(string password, string hashed)
		{
			return BCrypt.CheckPassword(password, hashed);
		}
	}
}