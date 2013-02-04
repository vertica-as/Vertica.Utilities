using System;
using System.Security.Principal;

namespace Vertica.Utilities_v4.Security
{
	public class ProtocolTransitioningIdentityProvider : WindowsIdentityProviderBase
	{
		private readonly Func<string> _principalName;
		public ProtocolTransitioningIdentityProvider(string userName, string domainUser)
		{
			_principalName = () => Credential.ToUserPrincipalName(userName, domainUser);
		}

		public ProtocolTransitioningIdentityProvider(string logonName)
		{
			_principalName = () => Credential.ToUserPrincipalName(logonName);
		}

		public override WindowsIdentity GetWindowsIdentity()
		{
			return new WindowsIdentity(_principalName());
		}
	}
}