using NUnit.Framework;
using Vertica.Utilities.Security;

namespace Vertica.Utilities.Tests.Security
{
	[TestFixture]
	public class BCryptTester
	{
		[Test]
		public void HashPassword_DefaultSalt_GeneratesSaltedPassword()
		{
			string password = "password";
			string hashed = BCrypt.HashPassword(password, BCrypt.GenerateSalt());
			Assert.That(hashed, Is.Not.EqualTo(password));
		}

		[Test]
		public void HashPassword_CustomSalt_GeneratedSaltedPassword()
		{
			string password = "password";
			string hashed = BCrypt.HashPassword(password, BCrypt.GenerateSalt(6));
			Assert.That(hashed, Is.Not.EqualTo(password));
		}

		[Test]
		public void HashPassword_SaltMatters()
		{
			string password = "password";
			string hashedDefault = BCrypt.HashPassword(password, BCrypt.GenerateSalt()),
				hashedSix = BCrypt.HashPassword(password, BCrypt.GenerateSalt(6));

			Assert.That(hashedDefault, Is.Not.EqualTo(hashedSix));
		}

		[Test]
		public void CheckPassword_UnsaltedPassword_Exception()
		{
			string password = "password";

			Assert.That(() => BCrypt.CheckPassword(password, password), Throws.ArgumentException);
		}

		[Test]
		public void CheckPassword_SameSaltedPassword_True()
		{
			string password = "password";
			string hashed = BCrypt.HashPassword(password, BCrypt.GenerateSalt(6));

			Assert.That(BCrypt.CheckPassword(password, hashed), Is.True);
		}

		[Test]
		public void CheckPassword_AnotherSaltedPassword_False()
		{
			string password = "password";
			string hashed = BCrypt.HashPassword("anotherPassword", BCrypt.GenerateSalt(6));

			Assert.That(BCrypt.CheckPassword(password, hashed), Is.False);
		}
	}
}