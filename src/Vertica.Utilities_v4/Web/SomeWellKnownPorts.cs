namespace Vertica.Utilities_v4.Web
{
	public class EchoPort : TcpPort
	{
		public EchoPort() { }
		public EchoPort(ushort portNumber) : base(portNumber) { }

		public override ushort DefaultPort { get { return 7; } }
	}

	public class FtpPort : TcpPort
	{
		public FtpPort() { }
		public FtpPort(ushort portNumber) : base(portNumber) { }

		public override ushort DefaultPort { get { return 21; } }
	}

	public class SshPort : TcpPort
	{
		public SshPort() { }
		public SshPort(ushort portNumber) : base(portNumber) { }

		public override ushort DefaultPort { get { return 22; } }
	}

	public class TelnetPort : TcpPort
	{
		public TelnetPort() { }
		public TelnetPort(ushort portNumber) : base(portNumber) { }

		public override ushort DefaultPort { get { return 23; } }
	}

	public class SmtpPort : TcpPort
	{
		public SmtpPort() { }
		public SmtpPort(ushort portNumber) : base(portNumber) { }

		public override ushort DefaultPort { get { return 25; } }
	}

	public class DnsPort : TcpPort
	{
		public DnsPort() { }
		public DnsPort(ushort portNumber) : base(portNumber) { }

		public override ushort DefaultPort { get { return 53; } }
	}

	public class HttpPort : TcpPort
	{
		public HttpPort() { }
		public HttpPort(ushort portNumber): base(portNumber) { }

		public override ushort DefaultPort { get { return 80; } }
	}
	
	public class Pop3Port : TcpPort
	{
		public Pop3Port() { }
		public Pop3Port(ushort portNumber): base(portNumber) { }

		public override ushort DefaultPort { get { return 110; } }
	}

	public class ImapPort : TcpPort
	{
		public ImapPort() { }
		public ImapPort(ushort portNumber): base(portNumber) { }

		public override ushort DefaultPort { get { return 143; } }
	}

	public class LdapPort : TcpPort
	{
		public LdapPort() { }
		public LdapPort(ushort portNumber) : base(portNumber) { }

		public override ushort DefaultPort { get { return 389; } }
	}

	public class HttpsPort : TcpPort
	{
		public HttpsPort() { }
		public HttpsPort(ushort portNumber) : base(portNumber) { }

		public override ushort DefaultPort { get { return 443; } }
	}
}