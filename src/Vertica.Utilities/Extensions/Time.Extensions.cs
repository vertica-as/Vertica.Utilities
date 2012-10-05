using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Vertica.Utilities.Extensions.ComparableExt;
using Vertica.Utilities.Resources;

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

		#region DateTime extended information

		public static int Week(this DateTime dt)
		{
			return Week(dt, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule);
		}

		public static int Week(this DateTime dt, CalendarWeekRule rule)
		{
			return Week(dt, rule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
		}

		public static int Week(this DateTime dt, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dt, rule, firstDayOfWeek);
		}

		/*
		public static Range<DateTimeOffset> Q1(this int year)
		{
			return Time.Q1(year);
		}

		public static Range<DateTimeOffset> Q2(this int year)
		{
			return Time.Q2(year);
		}

		public static Range<DateTimeOffset> Q3(this int year)
		{
			return Time.Q3(year);
		}

		public static Range<DateTimeOffset> Q4(this int year)
		{
			return Time.Q4(year);
		}

		public static Range<DateTimeOffset> Quarter(this int year, Quarter quarter)
		{
			return Time.Quarter(year, quarter);
		}

		public static Quarter Quarter(this DateTime dt)
		{
			return Time.Quarter(dt);
		}*/

		public static bool IsLeapYear(this int year)
		{
			return IsLeapYear(year, CultureInfo.CurrentCulture);
		}

		public static bool IsLeapYear(this int year, CultureInfo culture)
		{
			return culture.Calendar.IsLeapYear(year);
		}

		public static DateTime FromUnixTime(this double timestamp)
		{
			return Time.FromUnixTime(timestamp).DateTime;
		}

		public static double ToUnixTime(this DateTime dt)
		{
			return Time.ToUnixTime(new DateTimeOffset(dt));
		}

		#endregion

		#region Datetime boundaries

		public static DateTime Next(this DateTime dt, DayOfWeek nextDoW)
		{
			return dt + days(dt.DayOfWeek.DaysTill(nextDoW));
		}

		public static DateTime Previous(this DateTime dt, DayOfWeek nextDoW)
		{
			return dt - days(dt.DayOfWeek.DaysSince(nextDoW));
		}

		private static DateTime endOfDay(this DateTime dt)
		{
			return new DateTime(dt.Year, dt.Month, dt.Day) + Time.EndOfDay;
		}

		private static int quarterMonthStart(DateTime dt)
		{
			int intQuarterNum = (dt.Month - 1) / 3 + 1;
			return 3 * intQuarterNum - 2;
		}

		private static int quarterMonthEnd(DateTime dt)
		{
			int intQuarterNum = (dt.Month - 1) / 3 + 1;
			return 3 * intQuarterNum;
		}

		public static DateTime BeginningOf(this DateTime dt, Period period)
		{
			DateTime beginning;
			switch (period)
			{
				case Period.Week:
					beginning = BeginningOfWeek(dt);
					break;
				case Period.Month:
					beginning = BeginningOfMonth(dt);
					break;
				case Period.Quarter:
					beginning = BeginningOfQuarter(dt);
					break;
				case Period.Year:
					beginning = BeginningOfYear(dt);
					break;
				default:
					throw new ArgumentOutOfRangeException("period");
			}
			return beginning;
		}

		public static DateTime EndOf(this DateTime dt, Period period)
		{
			DateTime end;
			switch (period)
			{
				case Period.Week:
					end = EndOfWeek(dt);
					break;
				case Period.Month:
					end = EndOfMonth(dt);
					break;
				case Period.Quarter:
					end = EndOfQuarter(dt);
					break;
				case Period.Year:
					end = EndOfYear(dt);
					break;
				default:
					throw new ArgumentOutOfRangeException("period");
			}
			return end;
		}

		public static DateTime BeginningOfWeek(this DateTime dt)
		{
			return (dt - days(dt.DayOfWeek.DaysSince(Time.FirstDayOfWeek()))).beginningOfDay();
		}

		private static TimeSpan days(int days)
		{
			return TimeSpan.FromDays(days);
		}

		private static DateTime beginningOfDay(this DateTime dt)
		{
			return dt.Date;
		}

		public static DateTime EndOfWeek(this DateTime dt)
		{
			return (dt + days(dt.DayOfWeek.DaysTill(Time.LastDayOfWeek()))).endOfDay();
		}

		public static DateTime BeginningOfMonth(this DateTime dt)
		{
			return new DateTime(dt.Year, dt.Month, 1);
		}

		public static DateTime EndOfMonth(this DateTime dt)
		{
			return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month)).endOfDay();
		}

		public static DateTime BeginningOfQuarter(this DateTime dt)
		{
			var month = quarterMonthStart(dt);
			return new DateTime(dt.Year, month, 1);
		}

		public static DateTime EndOfQuarter(this DateTime dt)
		{
			var month = quarterMonthEnd(dt);
			return new DateTime(dt.Year, month,
				DateTime.DaysInMonth(dt.Year, month)).endOfDay();
		}

		public static DateTime BeginningOfYear(this DateTime dt)
		{
			return new DateTime(dt.Year, 1, 1);
		}

		public static DateTime EndOfYear(this DateTime dt)
		{
			return new DateTime(dt.Year, 12, 31).endOfDay();
		}

		#endregion

		#region DateTime comparison

		public static TimeSpan Difference(this DateTime dt1, DateTime dt2)
		{
			return dt1.Subtract(dt2).Duration();
		}

		public static DifferFromBuilder DifferFrom(this DateTime dt1, DateTime dt2)
		{
			return new DifferFromBuilder(dt1, dt2);
		}

		public class DifferFromBuilder
		{
			private readonly DateTime _dt1;
			private readonly DateTime _dt2;
			internal DifferFromBuilder(DateTime dt1, DateTime dt2)
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

		public static DateTime Ago(this TimeSpan timeSpan)
		{
			return (Time.Now - timeSpan).DateTime;
		}

		public static DateTime FromNow(this TimeSpan timeSpan)
		{
			return (Time.Now + timeSpan).DateTime;
		}

		public static TimeSpan Elapsed(this DateTime time)
		{
			return Time.Now.Subtract(time).Duration();
		}

		public static DateTime UtcAgo(this TimeSpan timeSpan)
		{
			return (Time.UtcNow - timeSpan).DateTime;
		}

		public static DateTime UtcFromNow(this TimeSpan timeSpan)
		{
			return (Time.UtcNow + timeSpan).DateTime;
		}

		public static TimeSpan UtcElapsed(this DateTime time)
		{
			return Time.UtcNow.Subtract(time).Duration();
		}

		public static DateTime After(this TimeSpan ts, DateTime dt)
		{
			return dt + ts;
		}

		public static DateTime Before(this TimeSpan ts, DateTime dt)
		{
			return dt - ts;
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

		#region minimal DateTimeOffset support

		public static DateTimeOffset SetTime(this DateTimeOffset dt, TimeSpan span)
		{
			return new DateTimeOffset(dt.Year, dt.Month, dt.Day, span.Hours, span.Minutes, span.Seconds, span.Milliseconds, Time.Offset);
		}

		public static DateTimeOffset Yesterday(this DateTimeOffset dt)
		{
			return dt - Time.OneDay;
		}

		public static DateTimeOffset Tomorrow(this DateTimeOffset dt)
		{
			return dt + Time.OneDay;
		}

		public static DateTimeOffset AsDateTimeOffset(this DateTime dt, TimeSpan offset)
		{
			return new DateTimeOffset(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, offset);
		}

		public static DateTimeOffset AsUtcDateTimeOffset(this DateTime dt)
		{
			return AsDateTimeOffset(dt, TimeSpan.Zero);
		}

		#endregion

		#region DateTime creation

		
		public static DateTime SetTime(this DateTime dt, int hour, int minute, int second, int miliSecond)
		{
			return new DateTime(dt.Year, dt.Month, dt.Day, hour, minute, second, miliSecond);
		}

		public static DateTime SetTime(this DateTime dt, int hour, int minute, int second)
		{
			return new DateTime(dt.Year, dt.Month, dt.Day, hour, minute, second);
		}

		public static DateTime SetTime(this DateTime dt, TimeSpan span)
		{
			return SetTime(dt, span.Hours, span.Minutes, span.Seconds, span.Milliseconds);
		}

		public static DateTime Yesterday(this DateTime dt)
		{
			return dt - Time.OneDay;
		}

		public static DateTime Tomorrow(this DateTime dt)
		{
			return dt + Time.OneDay;
		}

		public static DateTime RandomSingle(this DateTime startDate, DateTime endDate)
		{
			assertBounds(startDate, endDate);

			var rnd = new Random();
			int dayRange = (endDate - startDate).Days;

			return startDate.AddDays(rnd.Next(dayRange));
		}

		/// <summary>
		/// Please do use .Take() extensions, as this enumerable is infinite
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		public static IEnumerable<DateTime> RandomCollection(this DateTime startDate, DateTime endDate)
		{
			assertBounds(startDate, endDate);

			var rnd = new Random();
			int dayRange = (endDate - startDate).Days;
			while (true) yield return startDate.AddDays(rnd.Next(dayRange));
		}

		private static void assertBounds(DateTime startDate, DateTime endDate)
		{
			if (endDate <= startDate) throw new ArgumentOutOfRangeException("endDate", endDate, string.Format(Exceptions.RandomDate_InvertedRangeTemplate, startDate));
		}

		#endregion

		#region timer

		public static double FractionalSeconds(this Stopwatch watch)
		{
			// figure out how much of a second a Stopwatch tick represents
			double secondsPerTick = (double)1 / Stopwatch.Frequency;

			return watch.ElapsedTicks * secondsPerTick;
		}

		#endregion
	}
}
