using System.Security.Principal;

namespace Vertica.Utilities_v4.Security
{
	public interface IIdentityProvider
	{
		IIdentity GetIdentity();
	}
}