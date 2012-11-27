using System;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.Time;
using Vertica.Utilities_v4.Extensions.TimeExt;
using Vertica.Utilities_v4.Testing;
using Vertica.Utilities_v4.Tests.Support;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class AgeTester
	{
		private static readonly DateTime _keyDateInHistory = new DateTime(1977, 3, 11);

		#region construction

		[Test]
		public void AdventConstructor_OneDayAgo_PropertiesSetTo1DayAgo()
		{
			DateTime now = _keyDateInHistory;
			using (TimeReseter.Set(now))
			{
				DateTime yesterday = Time.Yesterday.DateTime;
				var subject = new Age(yesterday);

				Assert.That(subject, Must.Be.Age()
					.WithBounds(advent: yesterday, terminus: now)
					.Elapsed(1.	Days())
					.WithComponents(days: 1, weeks: 0, months: 0, years: 0));
			}
		}

		[Test]
		public void AdventConstructor_OneWeekAgo_PropertiesSetTo1WeekAgo()
		{
			DateTime now = _keyDateInHistory;
			using (TimeReseter.Set(now))
			{
				DateTime oneWeekAgo = now.AddDays(-7);
				var subject = new Age(oneWeekAgo);

				Assert.That(subject, Must.Be.Age()
					.WithBounds(advent: oneWeekAgo, terminus: now)
					.Elapsed(1.Weeks())
					.WithComponents(days: 0, weeks: 1, months: 0, years: 0));
			}
		}

		[Test]
		public void AdventConstructor_TwoMonthsThreeDaysAgo_PropertiesSet()
		{
			DateTime now = _keyDateInHistory;
			using (TimeReseter.Set(now))
			{
				DateTime twoMonthsThreeDaysAgo = now.AddMonths(-2).AddDays(-3);
				var subject = new Age(twoMonthsThreeDaysAgo);
				Assert.That(subject, Must.Be.Age()
					.WithBounds(twoMonthsThreeDaysAgo, now)
					.Elapsed(now - twoMonthsThreeDaysAgo)
					.WithComponents(days: 3, weeks: 0, months: 2, years: 0));
			}
		}

		[Test]
		public void AdventConstructor_OneWeekAgo_SetsTimeProperties()
		{
			DateTime now = _keyDateInHistory;
			using (TimeReseter.Set(now))
			{
				var subject = new Age(1.Weeks().Ago().DateTime);

				Assert.That(subject.Years, Is.EqualTo(0));
				Assert.That(subject.Months, Is.EqualTo(0));
				Assert.That(subject.Weeks, Is.EqualTo(1));
				Assert.That(subject.Days, Is.EqualTo(0));
			}
		}

		[Test]
		public void Empty_ReturnsEmptyDate()
		{
			Age empty = Age.Empty;
			Assert.That(empty, Must.Be.Age()
					.WithBounds(DateTime.MinValue, DateTime.MinValue)
					.Elapsed(TimeSpan.Zero)
					.WithComponents(days: 0, weeks: 0, months: 0, years: 0));

			Assert.That(empty.IsEmpty, Is.True);
		}

		[Test]
		public void Default_IsAlsoEmpty()
		{
			Age @default = new Age();
			Assert.That(@default, Must.Be.Age()
					.WithBounds(DateTime.MinValue, DateTime.MinValue)
					.Elapsed(TimeSpan.Zero)
					.WithComponents(days: 0, weeks: 0, months: 0, years: 0));

			Assert.That(@default.IsEmpty, Is.True);
		}

		#endregion

		#region ToString

		[Test]
		public void ToString_OneYearAndTwoWeeks_EnglishText()
		{
			DateTime oneYearAndTwoWeeks = _keyDateInHistory.AddYears(1).AddDays(14);
			Age subject = new Age(_keyDateInHistory, oneYearAndTwoWeeks);
			Assert.That(subject.ToString(), Is.EqualTo("1 year 2 weeks"));
		}

		[Test]
		public void ToString_3PartAgeVariousSignificances_LowerLevelTrimmed()
		{
			DateTime terminus = _keyDateInHistory;
			DateTime advent = terminus.AddYears(-300).AddDays(-1).AddHours(-3);
			Age subject = new Age(advent, terminus);
			Assert.That(subject.ToString(3), Is.EqualTo(subject.ToString()));
			Assert.That(subject.ToString(2), Is.EqualTo("300 years 1 day"));
			Assert.That(subject.ToString(1), Is.EqualTo("300 years"));
		}

		[Test]
		public void ToString_NegativeSignificance_NoTrim()
		{
			DateTime advent = _keyDateInHistory;
			DateTime terminus = advent.Add(new TimeSpan(367, 13, 12, 2, 300));

			Age subject = new Age(advent, terminus);
			Assert.That(subject.ToString(-1), Is.EqualTo(subject.ToString()));
		}

		[Test]
		public void ToString_DoNotIncludeTime_NoHoursMinutesOrSeconds()
		{
			DateTime advent = _keyDateInHistory;
			DateTime terminus = advent.Add(new TimeSpan(367, 13, 12, 2, 300));

			Age subject = new Age(advent, terminus);
			Assert.That(subject.ToString(20, true), Is.EqualTo("1 year 2 days 13 hours 12 minutes 2 seconds"));
			Assert.That(subject.ToString(20, false), Is.EqualTo("1 year 2 days"));
		}

		#endregion

		#region IFormattable



		#endregion
	}
}