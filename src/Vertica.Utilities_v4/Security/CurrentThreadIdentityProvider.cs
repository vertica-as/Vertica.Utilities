using System.Security.Principal;
using System.Threading;

namespace Vertica.Utilities_v4.Security
{
	public class CurrentThreadIdentityProvider : IWindowsIdentityProvider
	{
		public IIdentity GetIdentity()
		{
			return Thread.CurrentPrincipal.Identity;
		}

		public WindowsIdentity GetWindowsIdentity()
		{
			return (WindowsIdentity)GetIdentity();
		}

		public void Dispose() { }
	}
}