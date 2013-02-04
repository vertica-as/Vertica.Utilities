using NUnit.Framework;
using Vertica.Utilities_v4.Security;

namespace Vertica.Utilities_v4.Tests.Security
{
	[TestFixture]
	public class BCryptHasherTester
	{
		[Test]
		public void HashPassword_GeneratesSaltedPassword()
		{
			string password = "password";
			IPasswordHasher subject = new BCryptHasher();

			Assert.That(subject.HashPassword(password), Is.Not.EqualTo(password));
		}

		[Test]
		public void CheckPassword_UnsaltedPassword_Exception()
		{
			string password = "password";
			IPasswordHasher subject = new BCryptHasher();

			Assert.That(() => subject.CheckPassword(password, password), Throws.ArgumentException);
		}

		[Test]
		public void CheckPassword_SameSaltedPassword_True()
		{
			string password = "password";

			IPasswordHasher subject = new BCryptHasher();
			string hashed = subject.HashPassword(password);

			Assert.That(subject.CheckPassword(password, hashed), Is.True);
		}

		[Test]
		public void CheckPassword_AnotherSaltedPassword_False()
		{
			string password = "password";
			IPasswordHasher subject = new BCryptHasher();
			string hashed = subject.HashPassword("anotherPassword");

			Assert.That(subject.CheckPassword(password, hashed), Is.False);
		}
	}
}