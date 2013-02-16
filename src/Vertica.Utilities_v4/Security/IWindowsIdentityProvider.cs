using System;
using System.Security.Principal;

namespace Vertica.Utilities_v4.Security
{
	public interface IWindowsIdentityProvider : IIdentityProvider, IDisposable
	{
		WindowsIdentity GetWindowsIdentity();
	}

	public abstract class WindowsIdentityProviderBase : IWindowsIdentityProvider
	{
		public IIdentity GetIdentity()
		{
			return GetWindowsIdentity();
		}

		public abstract WindowsIdentity GetWindowsIdentity();
		public virtual void Dispose() { }
	}
}