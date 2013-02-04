using System.Security.Principal;
using System.Threading;

namespace Vertica.Utilities_v4.Security
{
	public class CurrentThreadSecurityProvider : IWindowsIdentityProvider, IPrincipalProvider
	{
		public void Dispose() { }

		public IIdentity GetIdentity()
		{
			return Thread.CurrentPrincipal.Identity;
		}

		public WindowsIdentity GetWindowsIdentity()
		{
			return (WindowsIdentity)GetIdentity();
		}

		public IPrincipal GetPrincipal()
		{
			return Thread.CurrentPrincipal;
		}
	}
}