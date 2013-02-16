using System.Security.Principal;

namespace Vertica.Utilities_v4.Security
{
	public class CurrentIdentityProvider : WindowsIdentityProviderBase
	{
		public override WindowsIdentity GetWindowsIdentity()
		{
			return WindowsIdentity.GetCurrent();
		}
	}
}