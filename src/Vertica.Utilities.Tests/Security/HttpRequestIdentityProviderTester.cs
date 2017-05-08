using System.Security.Principal;
using System.Web;
using NSubstitute;
using NUnit.Framework;
using Vertica.Utilities_v4.Security;

namespace Vertica.Utilities_v4.Tests.Security
{
	[TestFixture]
	public class HttpRequestIdentityProviderTester
	{
		[Test]
		public void WindowsIdentity_FromRequest()
		{
			var request = Substitute.For<HttpRequestBase>();
			request.LogonUserIdentity.Returns(default(WindowsIdentity));

			IWindowsIdentityProvider provider = new HttpRequestIdentityProvider(request);
			Assert.That(provider.GetWindowsIdentity(), Is.Null);
		}
	}
}