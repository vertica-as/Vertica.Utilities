using System.Security.Principal;
using System.Web;

namespace Vertica.Utilities_v4.Security
{
	public class HttpContextIdentityProvider : IIdentityProvider
	{
		private readonly HttpContextBase _context;
		public HttpContextIdentityProvider()
			: this(new HttpContextWrapper(HttpContext.Current)) { }

		public HttpContextIdentityProvider(HttpContextBase context)
		{
			_context = context;
		}

		public IIdentity GetIdentity()
		{
			return _context.User.Identity;
		}
	}
}