using System;
using System.Globalization;
using NUnit.Framework;
using Testing.Commons.Globalization;
using Testing.Commons.Time;
using Vertica.Utilities.Extensions.TimeExt;
using Vertica.Utilities.Testing;

namespace Vertica.Utilities.Tests.Extensions
{
	[TestFixture]
	public class TimeExtensionsTester
	{
		#region DateTime creation

		[Test]
		public void TomorrowTest()
		{
			Assert.That(11.March(1977).InUtc().Tomorrow(), Is.EqualTo(12.March(1977).InUtc()));
		}

		[Test]
		public void ChainedTomorrowTest()
		{
			var baseDate = 11.March(1977).InUtc();
			var dayAfterAfterTomorrow = baseDate.Tomorrow().Tomorrow().Tomorrow();
			Assert.That(dayAfterAfterTomorrow, Is.EqualTo(baseDate + 3.Days()));
		}

		[Test]
		public void YesterdayTest()
		{
			Assert.That(11.March(1977).InUtc().Yesterday(), Is.EqualTo(10.March(1977).InUtc()));
		}

		private static void assertDate(DateTimeOffset actual, int year, int month, int day)
		{
			Assert.That(actual.Year, Is.EqualTo(year));
			Assert.That(actual.Month, Is.EqualTo(month));
			Assert.That(actual.Day, Is.EqualTo(day));
			Assert.That(actual.Hour, Is.EqualTo(0));
			Assert.That(actual.Minute, Is.EqualTo(0));
			Assert.That(actual.Second, Is.EqualTo(0));
			Assert.That(actual.Millisecond, Is.EqualTo(0));
			Assert.That(actual.TimeOfDay, Is.EqualTo(TimeSpan.Zero));
			Assert.That(actual.Offset, Is.EqualTo(TimeSpan.Zero));
		}

		private static void assertEndDayDate(DateTimeOffset actual, int year, int month, int day)
		{
			Assert.That(actual.Year, Is.EqualTo(year));
			Assert.That(actual.Month, Is.EqualTo(month));
			Assert.That(actual.Day, Is.EqualTo(day));
			Assert.That(actual.Hour, Is.EqualTo(23));
			Assert.That(actual.Minute, Is.EqualTo(59));
			Assert.That(actual.Second, Is.EqualTo(59));
			Assert.That(actual.Millisecond, Is.EqualTo(999));
			Assert.That(actual.TimeOfDay, Is.EqualTo(Time.EndOfDay));
			Assert.That(actual.Offset, Is.EqualTo(TimeSpan.Zero));
		}

		#endregion

		[TestCase(2000, true), TestCase(2001, false)]
		public void IsLeapYear(int year, bool leapYear)
		{
			Assert.That(year.IsLeapYear(), Is.EqualTo(leapYear));
		}

		[Test]
		public void Ago_TimeNowMinusTimeSpan()
		{
			using (TimeReseter.Set(10.February(2000).InUtc()))
			{
				Assert.That(2.Days().Ago(), Is.EqualTo(8.February(2000).InUtc()));
			}
		}

		[Test]
		public void Elapsed_1DayAgo_1Days()
		{
			using (TimeReseter.Set(10.February(2000).InUtc()))
			{
				TimeSpan passed = 9.February(2000).InUtc().Elapsed();
				Assert.That(passed, Is.EqualTo(1.Days()));
			}
		}

		[Test]
		public void Elapsed_1DayFromNow_1Days()
		{
			using (TimeReseter.Set(10.February(2000).InUtc()))
			{
				TimeSpan passed = 11.February(2000).InUtc().Elapsed();
				Assert.That(passed, Is.EqualTo(1.Days()));
			}
		}

		[Test]
		public void FromNow_2Days_2Days()
		{
			using (TimeReseter.Set(14.February(2007).In(2.Hours())))
			{
				Assert.That(2.Days().FromNow(), Is.EqualTo(16.February(2007).In(2.Hours())));
			}
		}

		[Test]
		public void Next_Monday_SevenDaysLater()
		{
			// monday to monday
			DateTimeOffset dt = 15.September(2008).InUtc();
			var nextMonday = dt.Next(DayOfWeek.Monday);
			Assert.That(nextMonday, Is.EqualTo(22.September(2008).InUtc()));
			Assert.That(dt.DayOfWeek, Is.EqualTo(DayOfWeek.Monday));
			Assert.That(nextMonday.DayOfWeek, Is.EqualTo(DayOfWeek.Monday));
			Assert.That(nextMonday - dt, Is.EqualTo(7.Days()));
		}

		[Test]
		public void NextTuesday_Monday_1DayLater()
		{
			// monday to tuesday
			DateTimeOffset dt = 15.September(2008).InUtc();
			var nextTuesday = dt.Next(DayOfWeek.Tuesday);
			Assert.That(nextTuesday, Is.EqualTo(16.September(2008).InUtc()));
			Assert.That(dt.DayOfWeek, Is.EqualTo(DayOfWeek.Monday));
			Assert.That(nextTuesday.DayOfWeek, Is.EqualTo(DayOfWeek.Tuesday));
			Assert.That(nextTuesday - dt, Is.EqualTo(1.Days()));
		}

		#region Describe()

		[Test]
		public void Describe_Now()
		{
			Assert.That(TimeSpan.Zero.Describe(), Is.EqualTo("now"));
		}

		[Test]
		public void Describe_Seconds()
		{
			Assert.That(1.Seconds().Describe(), Is.EqualTo("1 second"));
			Assert.That(2.Seconds().Describe(), Is.EqualTo("2 seconds"));
			Assert.That(59.Seconds().Describe(), Is.EqualTo("59 seconds"));
		}

		[Test]
		public void Describe_MinutesAndSeconds()
		{
			Assert.That(1.Minutes().Add(1.Seconds()).Describe(), Is.EqualTo("about 1 minute"));
			Assert.That(3.Minutes().Add(1.Seconds()).Describe(), Is.EqualTo("about 3 minutes"));
		}

		[Test]
		public void Describe_RoundingMinutesAndSeconds()
		{
			Assert.That(3.Minutes().Add(31.Seconds()).Describe(), Is.EqualTo("about 4 minutes"));
		}

		[Test]
		public void Describe_DaysHours()
		{
			Assert.That(3.Hours().Add(3.Minutes()).Add(1.Seconds()).Describe(), Is.EqualTo("about 3 hours"));
			Assert.That(2.Days().Add(1.Minutes()).Add(1.Seconds()).Describe(), Is.EqualTo("about 2 days"));
		}

		#endregion

		#region DateTime Boundaries

		private static readonly TimeSpan _baseTime = 15.Hours().Minutes(43).Seconds(10);
		private static readonly DateTimeOffset _baseThursday = 12.June(2008).At(_baseTime).InUtc();

		#region Next

		[Test]
		public void DaysTill_DaysSince()
		{
			Assert.That(DayOfWeek.Monday.DaysTill(DayOfWeek.Thursday), Is.EqualTo(3));
			Assert.That(DayOfWeek.Monday.DaysSince(DayOfWeek.Friday), Is.EqualTo(3));
		}

		[Test]
		public void Next()
		{
			var nextMonday = _baseThursday.Next(DayOfWeek.Monday);
			Assert.That(nextMonday.DayOfWeek, Is.EqualTo(DayOfWeek.Monday));

			Assert.That(nextMonday, Is.EqualTo(16.June(2008).At(_baseTime).InUtc()));
		}

		[Test]
		public void Next_WithLeapAndNonLeapYear_29thAnd1st()
		{
			var thu28FebOfLeapYear = 28.February(2008).At(_baseTime).InUtc();

			var nextFriOfLeapYear = thu28FebOfLeapYear.Next(DayOfWeek.Friday);
			Assert.That(nextFriOfLeapYear.DayOfWeek, Is.EqualTo(DayOfWeek.Friday));
			Assert.That(nextFriOfLeapYear, Is.EqualTo(29.February(2008).At(_baseTime).InUtc()));

			var web28FebNonLeapYear = 28.February(2007).At(_baseTime).InUtc();

			var nextThuNonLeapYear = web28FebNonLeapYear.Next(DayOfWeek.Thursday);
			Assert.That(nextThuNonLeapYear.DayOfWeek, Is.EqualTo(DayOfWeek.Thursday));
			Assert.That(nextThuNonLeapYear, Is.EqualTo(1.March(2007).At(_baseTime).InUtc()));
		}

		[Test]
		public void Next_AtMonthBoundary_NextMonth()
		{
			// April 30th is a Wednesday
			var wed30Apr = 30.April(2008).At(_baseTime).InUtc();
			var nextTue = wed30Apr.Next(DayOfWeek.Tuesday);

			Assert.That(nextTue.DayOfWeek, Is.EqualTo(DayOfWeek.Tuesday));
			Assert.That(nextTue, Is.EqualTo(6.May(2008).At(_baseTime).InUtc()));
		}

		[Test]
		public void Next_AtYearBoundary_NextYear()
		{
			// end of 2008 is a Wednesday
			var wed31Dec = 31.December(2008).At(_baseTime).InUtc();

			var nextThu = wed31Dec.Next(DayOfWeek.Thursday);
			Assert.That(nextThu.DayOfWeek, Is.EqualTo(DayOfWeek.Thursday));

			Assert.That(nextThu, Is.EqualTo(1.January(2009).At(_baseTime).InUtc()));
		}

		#endregion

		#region Beginning

		[Test]
		public void BeginningOfWeek_DependsOfCulture()
		{
			var thu12Jun = 12.June(2008).At(_baseTime).InUtc();
			DateTimeOffset weekStart;

			using (CultureReseter.Set("en-US"))
			{
				weekStart = thu12Jun.BeginningOfWeek();
				Assert.That(weekStart.DayOfWeek, Is.EqualTo(DayOfWeek.Sunday));
				assertDate(weekStart, 2008, 6, 8);
			}

			using (CultureReseter.Set("es-ES"))
			{
				weekStart = thu12Jun.BeginningOfWeek();
				Assert.That(weekStart.DayOfWeek, Is.EqualTo(DayOfWeek.Monday));
				assertDate(weekStart, 2008, 6, 9);
			}
		}

		[Test]
		public void BeginningOfMonth()
		{
			var thu12Jun = 12.June(2008).At(_baseTime).InUtc();

			var monthStart = thu12Jun.BeginningOfMonth();

			assertDate(monthStart, 2008, 6, 1);
		}

		[Test]
		public void BeginningOfYear()
		{
			var thu12Jun = 12.June(2008).At(_baseTime).InUtc();

			var yearStart = thu12Jun.BeginningOfYear();

			assertDate(yearStart, 2008, 1, 1);
		}

		[Test]
		public void BeginningOfShortcut_SameAsNonShortcut()
		{
			Assert.That(_baseThursday.BeginningOf(Period.Week), Is.EqualTo(_baseThursday.BeginningOfWeek()));
			Assert.That(_baseThursday.BeginningOf(Period.Month), Is.EqualTo(_baseThursday.BeginningOfMonth()));
			Assert.That(_baseThursday.BeginningOf(Period.Year), Is.EqualTo(_baseThursday.BeginningOfYear()));
		}

		#endregion

		#region End

		[Test]
		public void EndOfWeek_DependsOnCulture()
		{
			var thu12Jun = 12.June(2008).At(_baseTime).InUtc();
			DateTimeOffset weekEnd;

			using (CultureReseter.Set("en-US"))
			{
				weekEnd = thu12Jun.EndOfWeek();
				Assert.That(weekEnd.DayOfWeek, Is.EqualTo(DayOfWeek.Saturday));
				assertEndDayDate(weekEnd, 2008, 6, 14);
			}

			using (CultureReseter.Set("es-ES"))
			{
				weekEnd = thu12Jun.EndOfWeek();
				Assert.That(weekEnd.DayOfWeek, Is.EqualTo(DayOfWeek.Sunday));
				assertEndDayDate(weekEnd, 2008, 6, 15);
			}
		}

		[Test]
		public void EndOfMonth()
		{
			var thu12Jun = 12.June(2008).At(_baseTime).InUtc();
			var monthEnd = thu12Jun.EndOfMonth();
			assertEndDayDate(monthEnd, 2008, 6, 30);
		}

		[Test]
		public void EndOfMonth_LeapAndNotLeapYear()
		{
			var fri1FebLeap = 1.February(2008).At(_baseTime).InUtc();
			var monthEnd = fri1FebLeap.EndOfMonth();
			assertEndDayDate(monthEnd, 2008, 2, 29);

			var thu1FebNopnLeap = 1.February(2007).At(_baseTime).InUtc();
			monthEnd = thu1FebNopnLeap.EndOfMonth();
			assertEndDayDate(monthEnd, 2007, 2, 28);
		}

		[Test]
		public void EndOfYearTest()
		{
			var thu12Jun = 12.June(2008).At(_baseTime).InUtc();

			var yearStart = thu12Jun.EndOfYear();

			assertEndDayDate(yearStart, 2008, 12, 31);
		}

		[Test]
		public void EndShortcut_SameAsNonShortcut()
		{
			Assert.That(_baseThursday.EndOf(Period.Week), Is.EqualTo(_baseThursday.EndOfWeek()));
			Assert.That(_baseThursday.EndOf(Period.Month), Is.EqualTo(_baseThursday.EndOfMonth()));
			Assert.That(_baseThursday.EndOf(Period.Year), Is.EqualTo(_baseThursday.EndOfYear()));
		}

		#endregion

		#endregion

		#region DateTime extended information

		[Test]
		public void Week_Defaults()
		{
			Assert.That(3.January(2008).InUtc().Week(), Is.EqualTo(1));
			Assert.That(14.February(2008).InUtc().Week(), Is.EqualTo(7));
			Assert.That(30.December(2008).InUtc().Week(), Is.EqualTo(53));
		}

		[Test]
		public void Week_WeekCalendar_MakesDifference()
		{
			using (CultureReseter.Set("da-DK"))
			{
				var thuInFirstWeek = 3.January(2008).InUtc();
				Assert.That(thuInFirstWeek.Week(CalendarWeekRule.FirstDay), Is.EqualTo(1));
				Assert.That(thuInFirstWeek.Week(CalendarWeekRule.FirstFourDayWeek), Is.EqualTo(1));
				Assert.That(thuInFirstWeek.Week(CalendarWeekRule.FirstFullWeek), Is.EqualTo(53), "last week from previous year");
			}
		}

		[Test]
		public void Week_Culture_MakesDifferenceAsPerFirstDayOfWeek()
		{
			using (CultureReseter.Set("da-DK"))
			{
				var thuInFirstWeek = 3.January(2008).InUtc();
				Assert.That(thuInFirstWeek.Week(CalendarWeekRule.FirstFullWeek), Is.EqualTo(53), "last week from previous year");
			}

			using (CultureReseter.Set("en-US"))
			{
				var thuInFirstWeek = 3.January(2008).InUtc();
				Assert.That(thuInFirstWeek.Week(CalendarWeekRule.FirstFullWeek), Is.EqualTo(52), "last week from previous year");
			}
		}

		[Test]
		public void Week_WeekAndFirstDayOfWeek_FirstDayChangesHowDaysAreCounted()
		{
			var thuInFirsWeek = 3.January(2008).InUtc();
			Assert.That(thuInFirsWeek.Week(CalendarWeekRule.FirstDay, DayOfWeek.Wednesday), Is.EqualTo(2));
			Assert.That(thuInFirsWeek.Week(CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Wednesday), Is.EqualTo(1));
			Assert.That(thuInFirsWeek.Week(CalendarWeekRule.FirstFullWeek, DayOfWeek.Wednesday), Is.EqualTo(1), "now counts as week of current year");
		}

		[Test]
		public void ToUnixTimestamp()
		{
			Assert.That(Time.UnixEpoch.ToUnixTime(), Is.EqualTo(0d));
			Assert.That(2.January(1970).InUtc().ToUnixTime(), Is.EqualTo(0d + (3600 * 24)));
			Assert.That(13.June(1984).At(Time.Noon).In(TimeSpan.Zero).ToUnixTime(), Is.EqualTo(455976000d));
		}

		#endregion

		#region DateTime comparison

		[Test]
		public void Difference_AbsoluteDifference()
		{
			Assert.That(11.March(1977).InUtc().Difference(14.March(1977).InUtc()), Is.EqualTo(3.Days()));
			Assert.That(11.March(1977).InUtc().Difference(8.March(1977).InUtc()), Is.EqualTo(3.Days()));

			Assert.That(14.March(1977).InUtc().Difference(11.March(1977).InUtc()), Is.EqualTo(3.Days()));
			Assert.That(8.March(1977).InUtc().Difference(11.March(1977).InUtc()), Is.EqualTo(3.Days()));
		}

		[Test]
		public void DifferFrom_RichComparison()
		{
			DateTimeOffset firstFebruary = 1.February(2008).InUtc(), fifthFebruary = 5.February(2008).InUtc();

			Assert.That(firstFebruary.DiffersFrom(fifthFebruary).InAtLeast(2.Days()), Is.True);
			Assert.That(firstFebruary.DiffersFrom(fifthFebruary).InAtMost(4.Days()), Is.True);
			Assert.That(firstFebruary.DiffersFrom(fifthFebruary).InLessThan(1.Weeks()), Is.True);
			Assert.That(firstFebruary.DiffersFrom(fifthFebruary).InMoreThan(Time.OneHour), Is.True);
			Assert.That(firstFebruary.DiffersFrom(fifthFebruary).InLessThan(5.Seconds()), Is.False);

			Assert.That(firstFebruary.DiffersFrom(firstFebruary).InNothing(), Is.True);
			Assert.That(firstFebruary.DiffersFrom(fifthFebruary).InSomething(), Is.True);
		}

		#endregion

		[Test]
		public void AsDateTimeOffset_ChangeOffset_OnlyOffsetChanges()
		{
			TimeSpan plus3 = 3.Hours(), plus2 = 2.Hours();
			DateTime threeOclockPlus3 = new DateTimeOffset(2008, 1, 1, 15, 0, 0, plus3).DateTime;

			assertOffSet(threeOclockPlus3.AsOffset(plus2),
				1.January(2008).At(15.Hours()), plus2);
		}

		[Test]
		public void AsUtcDateTimeOffset_ChangeOffset_ToZero()
		{
			TimeSpan plus3 = 3.Hours();
			DateTime threeOclockPlus3 = new DateTimeOffset(2008, 1, 1, 15, 0, 0, plus3).DateTime;

			assertOffSet(threeOclockPlus3.AsUtcOffset(),
				1.January(2008).At(15.Hours()), TimeSpan.Zero);
		}

		private void assertOffSet(DateTimeOffset dto, DateTime dt, TimeSpan offset)
		{
			Assert.That(dto.Offset, Is.EqualTo(offset));
			Assert.That(dto.Year, Is.EqualTo(dt.Year));
			Assert.That(dto.Month, Is.EqualTo(dt.Month));
			Assert.That(dto.Day, Is.EqualTo(dt.Day));
			Assert.That(dto.Hour, Is.EqualTo(dt.Hour));
			Assert.That(dto.Minute, Is.EqualTo(dt.Minute));
			Assert.That(dto.Second, Is.EqualTo(dt.Second));
			Assert.That(dto.Millisecond, Is.EqualTo(dt.Millisecond));
		}
	}
}
