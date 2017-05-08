namespace Vertica.Utilities_v4.Security
{
	public interface IPasswordHasher
	{
		string HashPassword(string password);
		bool CheckPassword(string password, string hashed);
	}
}