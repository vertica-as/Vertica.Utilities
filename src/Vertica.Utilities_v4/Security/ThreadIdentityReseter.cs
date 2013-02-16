using System;
using System.Security.Principal;
using System.Threading;

namespace Vertica.Utilities_v4.Security
{
	public class ThreadIdentityReseter : IIdentityReseter
	{
		private readonly IPrincipal _previous;
		private ThreadIdentityReseter(WindowsIdentity identity)
		{
			_previous = Thread.CurrentPrincipal;
			Thread.CurrentPrincipal = new WindowsPrincipal(identity);
		}

		public IDisposable Set(WindowsIdentity identity)
		{
			return new ThreadIdentityReseter(identity);
		}

		public void Dispose()
		{
			Thread.CurrentPrincipal = _previous;
		}
	}
}