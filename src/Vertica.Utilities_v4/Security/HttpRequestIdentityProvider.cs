using System.Security.Principal;
using System.Web;

namespace Vertica.Utilities_v4.Security
{
	public class HttpRequestIdentityProvider : WindowsIdentityProviderBase
	{
		private readonly HttpRequestBase _request;
		internal HttpRequestIdentityProvider()
			: this(new HttpRequestWrapper(HttpContext.Current.Request)) { }

		internal HttpRequestIdentityProvider(HttpRequestBase request)
		{
			_request = request;
		}

		public override WindowsIdentity GetWindowsIdentity()
		{
			return _request.LogonUserIdentity;
		}
	}
}