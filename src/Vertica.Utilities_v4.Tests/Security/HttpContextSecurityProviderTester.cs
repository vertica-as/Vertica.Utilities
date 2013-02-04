using System.Security.Principal;
using System.Web;
using NSubstitute;
using NUnit.Framework;
using Vertica.Utilities_v4.Security;

namespace Vertica.Utilities_v4.Tests.Security
{
	[TestFixture]
	public class HttpContextSecurityProviderTester
	{
		[Test]
		public void Identity_FromContext()
		{
			var ctx = Substitute.For<HttpContextBase>();
			var principal = Substitute.For<IPrincipal>();
			var identity = Substitute.For<IIdentity>();
			ctx.User = principal;
			principal.Identity.Returns(identity);

			IIdentityProvider provider = new HttpContextSecurityProvider(ctx);
			Assert.That(provider.GetIdentity(), Is.SameAs(identity));
		}

		[Test]
		public void Principal_FromContext()
		{
			var ctx = Substitute.For<HttpContextBase>();
			var principal = Substitute.For<IPrincipal>();

			ctx.User = principal;

			var provider = new HttpContextPrincipalProvider(ctx);
			Assert.That(provider.GetPrincipal(), Is.SameAs(principal));
		}
	}
}