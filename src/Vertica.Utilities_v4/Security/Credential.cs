using System;
using System.Globalization;
using Vertica.Utilities_v4.Extensions.StringExt;

namespace Vertica.Utilities_v4.Security
{
	public class Credential
	{
		public Credential(string userName, string password) : this(userName, password, string.Empty) { }

		public Credential(string userName, string password, string domain)
		{
			UserName = userName;
			Password = password;
			Domain = domain;
		}

		public string UserName { get; private set; }
		public string Password { get; private set; }
		public string Domain { get; private set; }

		private void assertHasUserAndDomain()
		{
			Guard.Against(UserName.IsEmpty() || Domain.IsEmpty(), Resources.Exceptions.Credential_NoDomainOrUser);
		}

		public string UserPrincipalName()
		{
			assertHasUserAndDomain();
			return string.Concat(UserName, UpnSeparator, Domain);
		}

		public string LogonName()
		{
			assertHasUserAndDomain();
			return string.Concat(Domain, LogonNameSeparator, UserName);
		}

		private static readonly char LogonNameSeparator = '\\';
		public static bool TryParseLogonName(string logonName, out string userName, out string domainName)
		{
			bool result = false;
			userName = domainName = null;
			if (logonName.IsNotEmpty())
			{
				int domainIndex = 0, userNameIndex = 1;
				string[] parsed = logonName.Split(LogonNameSeparator);
				result = parsed.Length.Equals(2) && parsed[domainIndex].IsNotEmpty() && parsed[userNameIndex].IsNotEmpty();
				if (result)
				{
					domainName = parsed[domainIndex];
					userName = parsed[userNameIndex];
				}
			}
			return result;
		}

		private static readonly char UpnSeparator = '@';
		public static bool TryParseUserPrincipalName(string userPrincipalName, out string userName, out string domainName)
		{
			bool result = false;
			userName = domainName = null;
			if (userPrincipalName.IsNotEmpty())
			{
				int domainIndex = 1, userNameIndex = 0;
				string[] parsed = userPrincipalName.Split(UpnSeparator);
				result = parsed.Length.Equals(2) && parsed[domainIndex].IsNotEmpty() && parsed[userNameIndex].IsNotEmpty();
				if (result)
				{
					domainName = parsed[domainIndex];
					userName = parsed[userNameIndex];
				}
			}
			return result;
		}

		public static string ToUserPrincipalName(string logonName)
		{
			string userName, domain;
			if (TryParseLogonName(logonName, out userName, out domain))
			{
				return new Credential(userName, null, domain).UserPrincipalName();
			}
			throw new ArgumentException("logonName");
		}

		public static string ToLogonName(string userPrincipalName)
		{
			string userName, domain;
			if (TryParseUserPrincipalName(userPrincipalName, out userName, out domain))
			{
				return new Credential(userName, null, domain).LogonName();
			}
			throw new ArgumentException("userPrincipalName");
		}

		public static string ToUserPrincipalName(string userName, string domainName)
		{
			return new Credential(userName, null, domainName).UserPrincipalName();
		}

		public static string ToLogonName(string userName, string domainName)
		{
			return new Credential(userName, null, domainName).LogonName();
		}

		public override string ToString()
		{
			return Domain.IsEmpty() ? UserName : string.Concat(Domain, LogonNameSeparator, UserName);
		}

		public static string RemoveDomainIfPresent(string accountName)
		{
			string userName = accountName.RightFromFirst(LogonNameSeparator.ToString(CultureInfo.InvariantCulture));
			return userName ?? accountName;
		}
	}
}