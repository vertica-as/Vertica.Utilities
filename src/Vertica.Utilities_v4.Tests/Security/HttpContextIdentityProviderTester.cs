using System.Security.Principal;
using System.Web;
using NSubstitute;
using NUnit.Framework;
using Vertica.Utilities_v4.Security;

namespace Vertica.Utilities_v4.Tests.Security
{
	[TestFixture]
	public class HttpContextIdentityProviderTester
	{
		[Test]
		public void Identity_FromContext()
		{
			var ctx = Substitute.For<HttpContextBase>();
			var principal = Substitute.For<IPrincipal>();
			var identity = Substitute.For<IIdentity>();
			ctx.User = principal;
			principal.Identity.Returns(identity);

			IIdentityProvider provider = new HttpContextIdentityProvider(ctx);
			Assert.That(provider.GetIdentity(), Is.SameAs(identity));
		}
	}
}