namespace Vertica.Utilities.Security
{
	public interface IPasswordHasher
	{
		string HashPassword(string password);
		bool CheckPassword(string password, string hashed);
	}
}