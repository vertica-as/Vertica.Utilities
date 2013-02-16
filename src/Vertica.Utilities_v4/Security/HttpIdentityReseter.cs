using System;
using System.Security.Principal;
using System.Web;

namespace Vertica.Utilities_v4.Security
{
	public class HttpIdentityReseter : IIdentityReseter
	{
		private readonly IPrincipal _previous;
		private HttpIdentityReseter(WindowsIdentity identity)
		{
			_previous = HttpContext.Current.User;
			HttpContext.Current.User = new WindowsPrincipal(identity);
		}

		public IDisposable Set(WindowsIdentity identity)
		{
			return new HttpIdentityReseter(identity);
		}

		public void Dispose()
		{
			HttpContext.Current.User = _previous;
		}
	}
}