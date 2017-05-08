using System;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace Vertica.Utilities_v4.Security
{
	[Obsolete(".NET Standard")]
	public interface IPrincipalProvider : IDisposable
	{
		IPrincipal GetPrincipal();
	}

	[Obsolete(".NET Standard")]
	public class HttpContextPrincipalProvider : IPrincipalProvider
	{
		private readonly HttpContextBase _context;
		public HttpContextPrincipalProvider() : this(new HttpContextWrapper(HttpContext.Current)) { }

		public HttpContextPrincipalProvider(HttpContextBase context)
		{
			_context = context;
		}

		public void Dispose() { }

		public IPrincipal GetPrincipal()
		{
			return _context.User;
		}
	}
}