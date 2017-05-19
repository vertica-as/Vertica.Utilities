using NUnit.Framework;
using Vertica.Utilities.Web;

namespace Vertica.Utilities.Tests.Web
{
	[TestFixture]
	public class TcpPortTester
	{
		[Test]
		public void Ctor_NoValue_PortIsDefaultPort()
		{
			TcpPort p = new HttpPort();
			Assert.That(p.PortNumber, Is.EqualTo(80));
			Assert.That(p.DefaultPort, Is.EqualTo(80));
		}

		[Test]
		public void Ctor_Value_PortIsNotDefaultPort()
		{
			TcpPort p = new HttpPort(8080);
			Assert.That(p.PortNumber, Is.EqualTo(8080));
			Assert.That(p.DefaultPort, Is.EqualTo(80));
		}

		[Test]
		public void WellKnownPorts_CanBeInstantiated()
		{
			var ftp = new FtpPort();
			Assert.That(ftp.IsWellKnown, Is.True);

			var https = new HttpsPort();
			Assert.That(https.IsWellKnown, Is.True);
		}

		[Test]
		public void WellKnownPortsCanMove_AndStopBeingWellKnown()
		{
			var http = new HttpPort(8081);
			Assert.That(http.IsWellKnown, Is.False);
		}

		class QuoteOfTheDay : TcpPort
		{
			public QuoteOfTheDay() { }
			public QuoteOfTheDay(ushort portNumber) : base(portNumber) { }
			public override ushort DefaultPort { get { return 17; } }
		}

		[Test]
		public void WellKnownPorts_CanBeDefined()
		{
			var qod = new QuoteOfTheDay();
			Assert.That(qod.PortNumber, Is.EqualTo(qod.DefaultPort));
			Assert.That(qod.IsDefaultPort, Is.True);
			Assert.That(qod.IsWellKnown, Is.True);
		}

		[Test]
		public void WellKnownPorts_CanMove_IfAllowedSo()
		{
			var qod = new QuoteOfTheDay(62000);
			Assert.That(qod.IsDefaultPort, Is.False);
			Assert.That(qod.IsWellKnown, Is.False);
			Assert.That(qod.IsRegistered, Is.False);
		}

		class SqlServer : TcpPort
		{
			public SqlServer() { }
			public SqlServer(ushort portNumber) : base(portNumber) { }
			public override ushort DefaultPort { get { return 1433; } }
		}

		[Test]
		public void RegisteredPorts_CanBeDefined()
		{
			var sql = new SqlServer();
			Assert.That(sql.PortNumber, Is.EqualTo(sql.DefaultPort));
			Assert.That(sql.IsDefaultPort, Is.True);
			Assert.That(sql.IsWellKnown, Is.False);
			Assert.That(sql.IsRegistered, Is.True);
		}

		[Test]
		public void RegisteredPorts_CanMove_IfAllowedSo()
		{
			var sql = new SqlServer(62000);
			Assert.That(sql.IsDefaultPort, Is.False);
			Assert.That(sql.IsWellKnown, Is.False);
			Assert.That(sql.IsRegistered, Is.False);
		}
		
		class Dynamic : TcpPort
		{
			public Dynamic() { }
			public Dynamic(ushort portNumber) : base(portNumber) { }
			public override ushort DefaultPort { get { return 60000; } }
		}

		[Test]
		public void DynamicPorts_CanBeDefined()
		{
			var @dynamic = new Dynamic(61000);
			Assert.That(@dynamic.PortNumber, Is.Not.EqualTo(@dynamic.DefaultPort));
			Assert.That(@dynamic.IsDefaultPort, Is.False);
			Assert.That(@dynamic.IsWellKnown, Is.False);
			Assert.That(@dynamic.IsRegistered, Is.False);
		}

		class DefaultOnly : TcpPort
		{
			public override ushort DefaultPort { get { return ushort.MaxValue; } }
		}

		[Test]
		public void DefaultOnlyPorts_CanBeDefined()
		{
			var @default = new DefaultOnly();
			Assert.That(@default.PortNumber, Is.EqualTo(@default.DefaultPort));
			Assert.That(@default.IsDefaultPort, Is.True, "always true");
			Assert.That(@default.IsWellKnown, Is.False);
			Assert.That(@default.IsRegistered, Is.False);
		}
	}
}