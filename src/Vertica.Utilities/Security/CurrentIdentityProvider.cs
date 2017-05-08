using System;
using System.Security.Principal;

namespace Vertica.Utilities_v4.Security
{
	[Obsolete(".NET Standard")]
	public class CurrentIdentityProvider : WindowsIdentityProviderBase
	{
		public override WindowsIdentity GetWindowsIdentity()
		{
			return WindowsIdentity.GetCurrent();
		}
	}
}