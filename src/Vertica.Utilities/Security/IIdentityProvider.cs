using System;
using System.Security.Principal;

namespace Vertica.Utilities_v4.Security
{
	[Obsolete(".NET Standard")]
	public interface IIdentityProvider
	{
		IIdentity GetIdentity();
	}
}