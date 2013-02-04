using System.Security.Principal;
using System.Web;

namespace Vertica.Utilities_v4.Security
{
	public class HttpContextSecurityProvider : IIdentityProvider, IPrincipalProvider
	{
		private readonly HttpContextBase _context;
		public HttpContextSecurityProvider()
			: this(new HttpContextWrapper(HttpContext.Current)) { }

		public HttpContextSecurityProvider(HttpContextBase context)
		{
			_context = context;
		}

		public IIdentity GetIdentity()
		{
			return _context.User.Identity;
		}

		public void Dispose() { }

		public IPrincipal GetPrincipal()
		{
			return _context.User;
		}
	}
}