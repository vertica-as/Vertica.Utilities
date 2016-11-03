using System;
using System.Security.Principal;

namespace Vertica.Utilities_v4.Security
{
	[Obsolete(".NET Standard")]
	public interface IWindowsIdentityProvider : IIdentityProvider, IDisposable
	{
		WindowsIdentity GetWindowsIdentity();
	}

	[Obsolete(".NET Standard")]
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