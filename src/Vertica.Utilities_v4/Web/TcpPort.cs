namespace Vertica.Utilities_v4.Web
{
	public abstract class TcpPort
	{
		protected TcpPort()
		{
			_portNumber = DefaultPort;
		}
		protected TcpPort(ushort portNumber)
		{
			_portNumber = portNumber;
		}

		private readonly ushort _portNumber;
		public int PortNumber
		{
			get { return _portNumber; }
		}

		public const char PortDelimiter = ':';

		public virtual bool IsDefaultPort
		{
			get { return _portNumber == DefaultPort; }
		}

		public virtual bool IsWellKnown { get { return _portNumber <= 1023; } }

		public virtual bool IsRegistered { get { return _portNumber >= 1024 && _portNumber <= 49151;}}

		public abstract ushort DefaultPort { get; }
	}
}