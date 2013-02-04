using NUnit.Framework;
using Vertica.Utilities_v4.Security;

namespace Vertica.Utilities_v4.Tests.Security
{
	[TestFixture]
	public class PasswordHasherTester
	{
		[Test]
		public void HashPassword_GeneratesSaltedPassword()
		{
			string password = "password";
			IPasswordHasher subject = new PasswordHasher("userName");

			Assert.That(subject.HashPassword(password), Is.Not.EqualTo(password));
		}

		[Test]
		public void CheckPassword_UnsaltedPassword_False()
		{
			string password = "password";
			IPasswordHasher subject = new PasswordHasher("userName");

			Assert.That(subject.CheckPassword(password, password), Is.False);
		}

		[Test]
		public void CheckPassword_SameSaltedPassword_True()
		{
			string password = "password";

			IPasswordHasher subject = new PasswordHasher("userName");
			string hashed = subject.HashPassword(password);

			Assert.That(subject.CheckPassword(password, hashed), Is.True);
		}

		[Test]
		public void CheckPassword_DifferentUserPassword_False()
		{
			string password = "password";

			IPasswordHasher oneHasher = new PasswordHasher("user1"), 
				twoHasher = new PasswordHasher("user2");
			string hashedWithOne = oneHasher.HashPassword(password);

			Assert.That(twoHasher.CheckPassword(password, hashedWithOne), Is.False);
		}

		[Test]
		public void CheckPassword_AnotherSaltedPassword_False()
		{
			string password = "password";
			IPasswordHasher subject = new PasswordHasher("userName");
			string hashed = subject.HashPassword("anotherPassword");

			Assert.That(subject.CheckPassword(password, hashed), Is.False);
		}
	}
}