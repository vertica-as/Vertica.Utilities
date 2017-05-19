using NUnit.Framework;
using Vertica.Utilities.Security;

namespace Vertica.Utilities.Tests.Security
{
	[TestFixture]
	public class SimplePasswordHasherTester
	{
		[Test]
		public void HashPassword_GeneratesSaltedPassword()
		{
			string password = "password";
			IPasswordHasher subject = new SimplePasswordHasher("userName");

			Assert.That(subject.HashPassword(password), Is.Not.EqualTo(password));
		}

		[Test]
		public void CheckPassword_UnsaltedPassword_False()
		{
			string password = "password";
			IPasswordHasher subject = new SimplePasswordHasher("userName");

			Assert.That(subject.CheckPassword(password, password), Is.False);
		}

		[Test]
		public void CheckPassword_SameSaltedPassword_True()
		{
			string password = "password";

			IPasswordHasher subject = new SimplePasswordHasher("userName");
			string hashed = subject.HashPassword(password);

			Assert.That(subject.CheckPassword(password, hashed), Is.True);
		}

		[Test]
		public void CheckPassword_DifferentUserPassword_False()
		{
			string password = "password";

			IPasswordHasher oneHasher = new SimplePasswordHasher("user1"), 
				twoHasher = new SimplePasswordHasher("user2");
			string hashedWithOne = oneHasher.HashPassword(password);

			Assert.That(twoHasher.CheckPassword(password, hashedWithOne), Is.False);
		}

		[Test]
		public void CheckPassword_AnotherSaltedPassword_False()
		{
			string password = "password";
			IPasswordHasher subject = new SimplePasswordHasher("userName");
			string hashed = subject.HashPassword("anotherPassword");

			Assert.That(subject.CheckPassword(password, hashed), Is.False);
		}
	}
}