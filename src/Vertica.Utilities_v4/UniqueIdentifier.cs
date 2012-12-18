using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Vertica.Utilities_v4
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

		[DllImport("rpcrt4.dll", SetLastError = true)]
		private static extern int UuidCreateSequential(out Guid guid); 
		private const int RPC_S_OK = 0;

        // Used to sort the bytes of the Guid according to Alberto Ferrari article (http://sqlblog.com/blogs/alberto_ferrari/archive/2007/08/31/how-are-guids-sorted-by-sql-server.aspx)
        private static readonly int[] _sqlOrderMap = new[] {3, 2, 1, 0, 5, 4, 7, 6, 9, 8, 15, 14, 13, 12, 11, 10}; 

		public static Guid Sequential()
		{
			Guid guid = generateSequentialGuid();
			return setSortOrder(guid); 
		}

		private static Guid generateSequentialGuid()
		{
			Guid sequentialguid;

			int hr = UuidCreateSequential(out sequentialguid);

			if (hr != RPC_S_OK)
			{
				throw new Win32Exception(hr, string.Format("UuidCreateSequential() call failed: {0}", hr));
			}

			return sequentialguid;
		}

		private static Guid setSortOrder(Guid guid)
		{
			byte[] bytes = guid.ToByteArray();
			var copyBytes = new byte[16];

			bytes.CopyTo(copyBytes, 0);

			for (int mapIndex = 0; mapIndex < 10; mapIndex++)
			{
				bytes[mapIndex] = copyBytes[_sqlOrderMap[mapIndex]];
			}

			guid = new Guid(bytes);
			return guid;
		} 
	}
}
