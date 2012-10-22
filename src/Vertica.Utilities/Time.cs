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
		public static DateTimeOffset Now
		{
			get
			{
				return _now ?? DateTimeOffset.Now;
			}
		}

		private static DateTimeOffset? _utcNow;
		public static DateTimeOffset UtcNow
		{
			get { return _utcNow ?? Now.ToUniversalTime(); }
		}

		public static TimeSpan Offset
		{
			get { return Now.Offset; }
		}

		public static DateTimeOffset Today
		{
			get { return Now.SetTime(MidNight); }
		}

		public static DateTimeOffset Tomorrow
		{
			get { return Today.Tomorrow(); }
		}

		public static DateTimeOffset Yesterday
		{
			get { return Today.Yesterday(); }
		}

		public static DateTimeOffset UnixEpoch
		{
			get { return new DateTimeOffset(new DateTime(1970, 1, 1), TimeSpan.Zero); }
		}

		public static double UnixTimestamp
		{
			get
			{
				return ToUnixTime(Now);
			}
		}

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

		// based on http://blog.functionalfun.net/2008/06/extending-stopwatch.html */

		/// <summary>
		/// Times the execution time of the given delegate.
		/// </summary>
		/// <param name="action">The delegate to time</param>
		/// <returns>The total elapsed time when <paramref name="action"/> it's executed</returns>
		public static TimeSpan Action(Action action)
		{
			Stopwatch watch = Stopwatch.StartNew();
			Action(watch, action);
			return watch.Elapsed;
		}

		/// <summary>
		/// Times the execution time of the given delegate.
		/// </summary>
		/// <param name="watch">Instance of watch to measure time with</param>
		/// <param name="action">The delegate to time</param>
		public static void Action(Stopwatch watch, Action action)
		{
			Action(watch, action, 1);
		}

		/// <summary>
		/// Times the execution time of <paramref name="numberOfIterations"/> the given delegate.
		/// </summary>
		/// <param name="watch">Instance of watch to measure time with</param>
		/// <param name="action">The delegate to time</param>
		/// <param name="numberOfIterations">Number of times the delegate must be executed</param>
		public static void Action(Stopwatch watch, Action action, long numberOfIterations)
		{
			watch.Reset();
			watch.Start();

			for (int i = 0; i < numberOfIterations; i++)
			{
				action();
			}

			watch.Stop();
		}

		/// <summary>
		/// Times how long the given delegate takes to complete, averaging over up to 5 seconds of running time
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public static double AverageTime(Action action)
		{
			return AverageAction(action, 5);
		}

		/// <summary>
		/// Times the given delegate
		/// </summary>
		/// <param name="action">The delegate to time</param>
		/// <param name="targetRunTime">The target amount of time to average over (in seconds)</param>
		/// <returns>The running time, in fractional seconds</returns>
		public static double AverageAction(Action action, double targetRunTime)
		{
			if (targetRunTime <= 0)
			{
				throw new ArgumentException("targetRunTime must be greater than 0.", "targetRunTime");
			}

			var watch = new Stopwatch();

			// time the action once to see how fast it is
			Action(watch, action);

			// if the action took more than half of the targetRunTime time, we\'re not going to
			// fit in a second iteration
			double firstIterationTime = watch.FractionalSeconds();
			if (firstIterationTime > targetRunTime / 2)
			{
				return firstIterationTime;
			}

			// otherwise, repeat the action to get an accurate timing
			// aim for targetRunTime seconds of total running time
			long numberOfIterations = (long)(targetRunTime / firstIterationTime);

			// the number of iterations should be at least 1 because firstIterationTime is less than half of
			// targetRunTime
			Guard.Against(numberOfIterations > 0, Exceptions.Time_AverageAction_OneIteration);

			Action(watch, action, numberOfIterations);

			// calculate the length of time per iteration
			return watch.FractionalSeconds() / numberOfIterations;
		}

		#endregion
	}

	public enum Period
	{
		Week,
		Month,
		Quarter,
		Year
	}
}
