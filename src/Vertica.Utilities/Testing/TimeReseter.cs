using System;

namespace Vertica.Utilities_v4.Testing
{
	public class TimeReseter : IDisposable
	{
		private TimeReseter() { }

		public static TimeReseter Set(DateTimeOffset now)
		{
			Time.SetNow(now);
			return new TimeReseter();
		}

		public static TimeReseter SetUtc(DateTimeOffset now)
		{
			Time.SetUtcNow(now);
			return new TimeReseter();
		}

		public void Dispose()
		{
			Time.ResetNow();
			Time.ResetUtcNow();
		}
	}
}
