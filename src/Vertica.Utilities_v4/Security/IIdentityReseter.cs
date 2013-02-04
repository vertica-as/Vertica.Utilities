using System;
using System.Security.Principal;

namespace Vertica.Utilities_v4.Security
{
	public interface  IIdentityReseter : IDisposable
	{
		IDisposable Set(WindowsIdentity identity);
	}
}