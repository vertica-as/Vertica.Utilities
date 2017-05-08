using System;
using System.Security.Principal;

namespace Vertica.Utilities_v4.Security
{
	[Obsolete(".NET Standard")]
	public interface  IIdentityReseter : IDisposable
	{
		IDisposable Set(WindowsIdentity identity);
	}
}