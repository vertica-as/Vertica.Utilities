using System;
using System.Globalization;
using System.Linq;
using Vertica.Utilities.Extensions.ComparableExt;

namespace Vertica.Utilities.Extensions.TimeExt
{
	public static class TimeExtensions
	{
		#region Days of week

		private static readonly DayOfWeek MinDay = DayOfWeek.Sunday, MaxDay = DayOfWeek.Saturday;

		public static bool InRange(this DayOfWeek doW)
		{
			return doW >= MinDay && doW <= MaxDay;
		}

		public static void CheckRange(this DayOfWeek dow)
		{
			if (!dow.InRange())
				ExceptionHelper.ThrowArgumentException<ArgumentOutOfRangeException>("dow",
					"{0} has to be between {1} and {2}.",
					dow.ToString(),
					MinDay.ToString(),
					MaxDay.ToString());
		}


		public static int DaysTill(this DayOfWeek dt, DayOfWeek nextDoW)
		{
			dt.CheckRange();
			nextDoW.CheckRange();

			return daysBetween((int)nextDoW, (int)dt);
		}

		/// <summary>
		/// Determines the number of days since one DayOfWeek since the previous.
		/// </summary>
		/// <param name="dt">the starting DayOfWeek</param>
		/// <param name="prevDoW">The previous DayOfWeek</param>
		/// <returns>the number of days since <paramref name="dt"/> to <paramref name="prevDoW"/></returns>
		/// <exception cref="ArgumentOutOfRangeException">If either DayOfWeek is not 
		/// <see cref="InRange"/></exception>
		/// <example>
		/// Debug.Assert(DayOfWeek.Tuesday.DaysSince(DayOfWeek.Monday) == 1);
		/// </example>
		public static int DaysSince(this DayOfWeek dt, DayOfWeek prevDoW)
		{
			dt.CheckRange();
			prevDoW.CheckRange();

			return daysBetween((int)dt, (int)prevDoW);
		}

		private static int daysBetween(int first, int second)
		{
			if (first > second)
			{
				return first - second;
			}

			return ((int)MaxDay) - (second - first) + 1;
		}


		#endregion

		#region extended information

		public static int Week(this DateTimeOffset dt)
		{
			return Week(dt, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule);
		}

		public static int Week(this DateTimeOffset dt, CalendarWeekRule rule)
		{
			return Week(dt, rule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
		}

		public static int Week(this DateTimeOffset dt, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dt.DateTime, rule, firstDayOfWeek);
		}

		public static bool IsLeapYear(this int year)
		{
			return IsLeapYear(year, CultureInfo.CurrentCulture);
		}

		public static bool IsLeapYear(this int year, CultureInfo culture)
		{
			return culture.Calendar.IsLeapYear(year);
		}

		public static double ToUnixTime(this DateTimeOffset dt)
		{
			return Time.ToUnixTime(dt);
		}

		#endregion

		#region Datetime boundaries

		public static DateTimeOffset Next(this DateTimeOffset dt, DayOfWeek nextDoW)
		{
			return dt + days(dt.DayOfWeek.DaysTill(nextDoW));
		}

		public static DateTimeOffset Previous(this DateTimeOffset dt, DayOfWeek nextDoW)
		{
			return dt - days(dt.DayOfWeek.DaysSince(nextDoW));
		}

		private static DateTimeOffset endOfDay(this DateTimeOffset dt)
		{
			return new DateTimeOffset(dt.Year, dt.Month, dt.Day, 0, 0, 0, dt.Offset) + Time.EndOfDay;
		}

		public static DateTimeOffset BeginningOf(this DateTimeOffset dt, Period period)
		{
			DateTimeOffset beginning;
			switch (period)
			{
				case Period.Week:
					beginning = BeginningOfWeek(dt);
					break;
				case Period.Month:
					beginning = BeginningOfMonth(dt);
					break;
				case Period.Year:
					beginning = BeginningOfYear(dt);
					break;
				default:
					throw new ArgumentOutOfRangeException("period");
			}
			return beginning;
		}

		public static DateTimeOffset EndOf(this DateTimeOffset dt, Period period)
		{
			DateTimeOffset end;
			switch (period)
			{
				case Period.Week:
					end = EndOfWeek(dt);
					break;
				case Period.Month:
					end = EndOfMonth(dt);
					break;
				case Period.Year:
					end = EndOfYear(dt);
					break;
				default:
					throw new ArgumentOutOfRangeException("period");
			}
			return end;
		}

		public static DateTimeOffset BeginningOfWeek(this DateTimeOffset dt)
		{
			return (dt - days(dt.DayOfWeek.DaysSince(Time.FirstDayOfWeek()))).BeginningOfDay();
		}

		private static TimeSpan days(int days)
		{
			return TimeSpan.FromDays(days);
		}

		public static DateTimeOffset BeginningOfDay(this DateTimeOffset dt)
		{
			return dt.SetTime(Time.MidNight);
		}

		public static DateTimeOffset EndOfWeek(this DateTimeOffset dt)
		{
			return (dt + days(dt.DayOfWeek.DaysTill(Time.LastDayOfWeek()))).endOfDay();
		}

		public static DateTimeOffset BeginningOfMonth(this DateTimeOffset dt)
		{
			return new DateTimeOffset(dt.Year, dt.Month, 1, 0, 0, 0, dt.Offset);
		}

		public static DateTimeOffset EndOfMonth(this DateTimeOffset dt)
		{
			return new DateTimeOffset(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month), 0, 0, 0, dt.Offset).endOfDay();
		}

		public static DateTimeOffset BeginningOfYear(this DateTimeOffset dt)
		{
			return new DateTimeOffset(dt.Year, 1, 1, 0, 0, 0, dt.Offset);
		}

		public static DateTimeOffset EndOfYear(this DateTimeOffset dt)
		{
			return new DateTimeOffset(dt.Year, 12, 31, 0, 0, 0, dt.Offset).endOfDay();
		}

		#endregion

		#region DateTime comparison

		public static TimeSpan Difference(this DateTimeOffset dt1, DateTimeOffset dt2)
		{
			return dt1.Subtract(dt2).Duration();
		}

		public static DifferFromBuilder DiffersFrom(this DateTimeOffset dt1, DateTimeOffset dt2)
		{
			return new DifferFromBuilder(dt1, dt2);
		}

		public class DifferFromBuilder
		{
			private readonly DateTimeOffset _dt1;
			private readonly DateTimeOffset _dt2;
			internal DifferFromBuilder(DateTimeOffset dt1, DateTimeOffset dt2)
			{
				_dt1 = dt1;
				_dt2 = dt2;
			}

			public bool InLessThan(TimeSpan ts)
			{
				return _dt1.Difference(_dt2).IsLessThan(ts);
			}

			public bool InMoreThan(TimeSpan ts)
			{
				return _dt1.Difference(_dt2).IsMoreThan(ts);
			}

			public bool InAtLeast(TimeSpan ts)
			{
				return _dt1.Difference(_dt2).IsAtLeast(ts);
			}

			public bool InAtMost(TimeSpan ts)
			{
				return _dt1.Difference(_dt2).IsAtMost(ts);
			}

			public bool InSomething()
			{
				return _dt1.Difference(_dt2).IsDifferentFrom(TimeSpan.Zero);
			}

			public bool InNothing()
			{
				return _dt1.Difference(_dt2).IsEqualTo(TimeSpan.Zero);
			}

			public bool In(TimeSpan ts)
			{
				return _dt1.Difference(_dt2).IsEqualTo(ts);
			}
		}

		#endregion

		public static DateTimeOffset Ago(this TimeSpan timeSpan)
		{
			return (Time.Now - timeSpan);
		}

		public static DateTimeOffset FromNow(this TimeSpan timeSpan)
		{
			return (Time.Now + timeSpan);
		}

		public static TimeSpan Elapsed(this DateTimeOffset time)
		{
			return Time.Now.Subtract(time).Duration();
		}

		public static DateTimeOffset UtcAgo(this TimeSpan timeSpan)
		{
			return (Time.UtcNow - timeSpan);
		}

		public static DateTimeOffset UtcFromNow(this TimeSpan timeSpan)
		{
			return (Time.UtcNow + timeSpan);
		}

		public static TimeSpan UtcElapsed(this DateTimeOffset time)
		{
			return Time.UtcNow.Subtract(time).Duration();
		}

		private static readonly string[] NAMES =
		{
			"day",
			"hour",
			"minute",
			"second"
		};
		private static readonly Func<int, string, string> _tense = (q, n) =>
				q == 1 ?
					"1 " + n :
					string.Format("{0} {1}s", q, n);

		public static string Describe(this TimeSpan ts)
		{
			int[] ints =
			{
				ts.Days,
				ts.Hours,
				ts.Minutes,
				ts.Seconds
			};

			double[] doubles =
			{
				ts.TotalDays,
				ts.TotalHours,
				ts.TotalMinutes,
				ts.TotalSeconds
			};

			var firstNonZero = ints.Select((value, index) => new { value, index }).FirstOrDefault(x => x.value != 0);
			if (firstNonZero == null)
			{
				return "now";
			}

			int i = firstNonZero.index;
			string prefix = (i >= 3) ? string.Empty : "about ";
			var quantity = (int)Math.Round(doubles[i]);
			//string suffix = " ago";
			return prefix + _tense(quantity, NAMES[i]);
		}

		public static string AsLapseDescription(this TimeSpan ts)
		{
			string suffix = " ago";
			return Describe(ts) + suffix;
		}

		#region DateTime creation

		public static DateTimeOffset AsOffset(this DateTime dt, TimeSpan offset)
		{
			return new DateTimeOffset(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, offset);
		}

		public static DateTimeOffset AsUtcOffset(this DateTime dt)
		{
			return AsOffset(dt, TimeSpan.Zero);
		}

		public static DateTimeOffset SetTime(this DateTimeOffset dt, int hour, int minute, int second, int miliSecond)
		{
			return new DateTimeOffset(dt.Year, dt.Month, dt.Day, hour, minute, second, miliSecond, dt.Offset);
		}

		public static DateTimeOffset SetTime(this DateTimeOffset dt, int hour, int minute, int second)
		{
			return new DateTimeOffset(dt.Year, dt.Month, dt.Day, hour, minute, second, dt.Offset);
		}

		public static DateTimeOffset SetTime(this DateTimeOffset dt, TimeSpan span)
		{
			return SetTime(dt, span.Hours, span.Minutes, span.Seconds, span.Milliseconds);
		}

		public static DateTimeOffset Yesterday(this DateTimeOffset dt)
		{
			return dt - Time.OneDay;
		}

		public static DateTimeOffset Tomorrow(this DateTimeOffset dt)
		{
			return dt + Time.OneDay;
		}

		#endregion
	}
}
