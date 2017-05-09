using System;
using NUnit.Framework;
using Vertica.Utilities.Security;

namespace Vertica.Utilities.Tests.Security
{
	[TestFixture]
	public class CredentialTester
	{
		[Test]
		public void ToString_NoDomain_OnlyUserName()
		{
			string userName = "user";
			var subject = new Credential(userName, null);
			Assert.That(subject.ToString(), Is.EqualTo(userName));

			subject = new Credential(userName, null, null);
			Assert.That(subject.ToString(), Is.EqualTo(userName));

			subject = new Credential(userName, null, string.Empty);
			Assert.That(subject.ToString(), Is.EqualTo(userName));
		}

		[Test]
		public void LogonName_WithDomain_DomainAndUser()
		{
			var subject = new Credential("user", null, "domain");
			Assert.That(subject.LogonName(), Is.EqualTo("domain\\user"));
		}

		[Test]
		public void LogonName_NoDomainOrUser_Exception()
		{
			Assert.Throws<InvalidOperationException>(() => new Credential("user", null, null).LogonName());
			Assert.Throws<InvalidOperationException>(() => new Credential("user", null, string.Empty).LogonName());
			Assert.Throws<InvalidOperationException>(() => new Credential(null, null, "domain").LogonName());
			Assert.Throws<InvalidOperationException>(() => new Credential(string.Empty, null, "domain").LogonName());
		}

		[Test]
		public void UserPrincipalName_WithDomain_UserAtDomain()
		{
			var subject = new Credential("user", null, "domain");
			Assert.That(subject.UserPrincipalName(), Is.EqualTo("user@domain"));
		}

		[Test]
		public void UserPrincipalName_NoDomain_Exception()
		{
			Assert.Throws<InvalidOperationException>(() => new Credential("user", null, null).UserPrincipalName());
			Assert.Throws<InvalidOperationException>(() => new Credential("user", null, string.Empty).UserPrincipalName());
			Assert.Throws<InvalidOperationException>(() => new Credential(null, null, "domain").UserPrincipalName());
			Assert.Throws<InvalidOperationException>(() => new Credential(string.Empty, null, "domain").UserPrincipalName());
		}

		[Test]
		public void ToString_Domain_DomainAndUserName()
		{
			string userName = "user", domain = "domain";
			string expected = domain + "\\" + userName;
			var subject = new Credential(userName, null, domain);
			Assert.That(subject.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void TryParseLogonName_Correct_TrueWithUserNameAndPassword()
		{
			string logonName = "DOMAIN\\user", userName, domain;
			Assert.That(Credential.TryParseLogonName(logonName, out userName, out domain), Is.True);
			Assert.That(userName, Is.EqualTo("user"));
			Assert.That(domain, Is.EqualTo("DOMAIN"));
		}


		[TestCase(null)]
		[TestCase("")]
		[TestCase("JustUser")]
		[TestCase("\\nodomain")]
		[TestCase("nousername\\")]
		public void TryParseLogonName_NotValid_False(string invalidLogonName)
		{
			string userName, domain;
			Assert.That(Credential.TryParseLogonName(invalidLogonName, out userName, out domain), Is.False);
		}


		[TestCase("user@DOMAIN", "user", "DOMAIN")]
		[TestCase("user@DOMAIN.dom", "user", "DOMAIN.dom")]
		public void TryParseUserPrincipalName_Correct_TrueWithUserNameAndPassword(string validUpn, string expectedUserName, string expectedDomain)
		{
			string userName, domain;
			Assert.That(Credential.TryParseUserPrincipalName(validUpn, out userName, out domain), Is.True);
			Assert.That(userName, Is.EqualTo(expectedUserName));
			Assert.That(domain, Is.EqualTo(expectedDomain));
		}


		[TestCase(null)]
		[TestCase("")]
		[TestCase("JustUser")]
		[TestCase("nodomain@")]
		[TestCase("@nousername")]
		public void TryParseUserPrincipalName_NotValid_False(string invalidUpn)
		{
			string userName, domain;
			Assert.That(Credential.TryParseUserPrincipalName(invalidUpn, out userName, out domain), Is.False);
		}

		[Test]
		public void ToUserPrincipalName_LogonName_UserAtDomain()
		{
			Assert.That(Credential.ToUserPrincipalName("domain\\user"), Is.EqualTo("user@domain"));
		}

		[Test]
		public void ToUserPrincipalName_UserAndDomain_UserAtDomain()
		{
			Assert.That(Credential.ToUserPrincipalName("user", "domain"), Is.EqualTo("user@domain"));
		}


		[TestCase(null)]
		[TestCase("")]
		[TestCase("JustUser")]
		[TestCase("\\nodomain")]
		[TestCase("nousername\\")]
		public void ToUserPrincipalName_NotValid_False(string invalidLogonName)
		{
			Assert.Throws<ArgumentException>(() => Credential.ToUserPrincipalName(invalidLogonName));
		}

		[Test]
		public void ToUserPrincipalName_NoDomainOrUser_Exception()
		{
			Assert.Throws<InvalidOperationException>(() => Credential.ToUserPrincipalName("user", null));
			Assert.Throws<InvalidOperationException>(() => Credential.ToUserPrincipalName("user", string.Empty));
			Assert.Throws<InvalidOperationException>(() => Credential.ToUserPrincipalName(null, "domain"));
			Assert.Throws<InvalidOperationException>(() => Credential.ToUserPrincipalName(string.Empty, "domain"));
		}


		[TestCase("user@DOMAIN", "DOMAIN\\user")]
		[TestCase("user@DOMAIN.dom", "DOMAIN.dom\\user")]
		public void ToLogonName_LogonName_UserAtDomain(string validUpn, string logonName)
		{
			Assert.That(Credential.ToLogonName(validUpn), Is.EqualTo(logonName));
		}

		[Test]
		public void ToLogonName_UserAndDomain_DomainAndUser()
		{
			Assert.That(Credential.ToLogonName("user", "domain"), Is.EqualTo("domain\\user"));
		}


		[TestCase(null)]
		[TestCase("")]
		[TestCase("JustUser")]
		[TestCase("nodomain@")]
		[TestCase("@nousername")]
		public void ToLogonName_NotValid_False(string invalidUpn)
		{
			Assert.Throws<ArgumentException>(() => Credential.ToLogonName(invalidUpn));
		}

		[Test]
		public void ToLogonName_NoDomain_Exception()
		{
			Assert.Throws<InvalidOperationException>(() => Credential.ToLogonName("user", null));
			Assert.Throws<InvalidOperationException>(() => Credential.ToLogonName("user", string.Empty));
			Assert.Throws<InvalidOperationException>(() => Credential.ToLogonName(null, "domain"));
			Assert.Throws<InvalidOperationException>(() => Credential.ToLogonName(string.Empty, "domain"));
		}

		[Test]
		public void RemoveDomainIfPresent_LogonName_UserName()
		{
			Assert.That(Credential.RemoveDomainIfPresent("domain\\user"), Is.EqualTo("user"));
		}

		[Test]
		public void RemoveDomainIfPresent_JustUserName_UserName()
		{
			Assert.That(Credential.RemoveDomainIfPresent("user"), Is.EqualTo("user"));
		}
	}
}