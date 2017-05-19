using System;

namespace Vertica.Utilities
{
	public static class UniqueIdentifier
	{
		private static readonly DateTimeOffset _baseDate = new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero);

		public static Guid Comb()
		{
			byte[] guidArray = Guid.NewGuid().ToByteArray();

			DateTimeOffset now = Time.UtcNow;

			// Get the days and milliseconds which will be used to build the byte string 
			var days = new TimeSpan(now.Ticks - _baseDate.Ticks);
			TimeSpan msecs = now.TimeOfDay;

			// Convert to a byte array 
			// Note: SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
			byte[] daysArray = BitConverter.GetBytes(days.Days);
			byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

			// Reverse the bytes to match SQL Servers ordering 
			Array.Reverse(daysArray);
			Array.Reverse(msecsArray);

			// Copy the bytes into the guid 
			Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
			Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

			return new Guid(guidArray);
		}
	}
}
