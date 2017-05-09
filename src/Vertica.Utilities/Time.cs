using System;
using System.Diagnostics;
using System.Globalization;
using Vertica.Utilities.Extensions.TimeExt;
using Vertica.Utilities.Resources;

namespace Vertica.Utilities
{
	public static class Time
	{
		public static DayOfWeek FirstDayOfWeek()
		{
			return CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
		}

		public static DayOfWeek LastDayOfWeek()
		{
			return LastDayOfWeek(FirstDayOfWeek());
		}

		public static DayOfWeek LastDayOfWeek(DayOfWeek firstDoW)
		{
			return (DayOfWeek)(((int)firstDoW + 6) % 7);
		}

		private static DateTimeOffset? _now;
		public static DateTimeOffset Now => _now ?? DateTimeOffset.Now;

		private static DateTimeOffset? _utcNow;
		public static DateTimeOffset UtcNow => _utcNow ?? Now.ToUniversalTime();

		public static TimeSpan Offset => Now.Offset;

		public static DateTimeOffset Today => Now.SetTime(MidNight);

		public static DateTimeOffset Tomorrow => Today.Tomorrow();

		public static DateTimeOffset Yesterday => Today.Yesterday();

		public static DateTimeOffset UnixEpoch => new DateTimeOffset(new DateTime(1970, 1, 1), TimeSpan.Zero);

		public static double UnixTimestamp => ToUnixTime(Now);

		public static double ToUnixTime(DateTimeOffset date)
		{
			TimeSpan diff = date - UnixEpoch;
			return Math.Floor(diff.TotalSeconds);
		}

		public static DateTimeOffset FromUnixTime(double timeStamp)
		{
			return UnixEpoch.AddSeconds(timeStamp);
		}

		public static void SetNow(DateTimeOffset now)
		{
			_now = now;
		}

		public static void SetUtcNow(DateTimeOffset now)
		{
			Guard.Against<InvalidTimeZoneException>(!now.Offset.Equals(TimeSpan.Zero), Exceptions.Time_MustBeUtcTemplate, now.Offset.ToString());
			if (now.Offset.Equals(TimeSpan.Zero))
			{
				_utcNow = now;
			}
		}

		public static void ResetNow()
		{
			_now = null;
		}

		public static void ResetUtcNow()
		{
			_utcNow = null;
		}

		public static TimeSpan Noon = new TimeSpan(0, 12, 0, 0, 0);
		public static TimeSpan MidNight = TimeSpan.Zero;
		public static TimeSpan EndOfDay = new TimeSpan(0, 23, 59, 59, 999);
		public static TimeSpan BeginningOfDay = MidNight;

		public static TimeSpan OneDay = new TimeSpan(1, 0, 0, 0);
		public static TimeSpan OneHour = new TimeSpan(0, 1, 0, 0);
		public static TimeSpan OneMinute = new TimeSpan(0, 0, 1, 0);
		public static TimeSpan OneSecond = new TimeSpan(0, 0, 0, 1);
		public static TimeSpan OneWeek = new TimeSpan(7, 0, 0, 0);

		#region Timer

		/// <summary>
		/// Times the execution time of the given delegate.
		/// </summary>
		/// <param name="action">The delegate to time</param>
		/// <returns>The total elapsed time when <paramref name="action"/> it's executed</returns>
		public static TimeSpan Measure(Action action)
		{
			Stopwatch watch = Stopwatch.StartNew();
			Measure(watch, action);
			return watch.Elapsed;
		}

		public static TimeSpan Measure(Action action, long numberOfIterations)
		{
			Stopwatch watch = Stopwatch.StartNew();
			Measure(watch, action, numberOfIterations);
			return watch.Elapsed;
		}

		/// <summary>
		/// Times the execution time of the given delegate.
		/// </summary>
		/// <param name="watch">Instance of watch to measure time with</param>
		/// <param name="action">The delegate to time</param>
		public static void Measure(Stopwatch watch, Action action)
		{
			Measure(watch, action, 1);
		}

		/// <summary>
		/// Times the execution time of <paramref name="numberOfIterations"/> the given delegate.
		/// </summary>
		/// <param name="watch">Instance of watch to measure time with</param>
		/// <param name="action">The delegate to time</param>
		/// <param name="numberOfIterations">Number of times the delegate must be executed</param>
		public static void Measure(Stopwatch watch, Action action, long numberOfIterations)
		{
			watch.Reset();
			watch.Start();

			for (int i = 0; i < numberOfIterations; i++)
			{
				action();
			}

			watch.Stop();
		}

		#endregion
	}

	public enum Period
	{
		Week,
		Month,
		Year
	}
}
