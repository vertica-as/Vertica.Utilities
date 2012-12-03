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
		#region documentation

		[Test, Category("Exploratory")]
		public void Explore()
		{
			var thirtySomething = new Age(11.March(1977)); // terminus is injectable (Time.xxxNow)
			var oneWeekOld = new Age(7.November(2012), 14.November(2012));

			DateTime terminus = 27.November(2012);
			DateTime advent = terminus.AddYears(-300).AddDays(-15).AddHours(-3).AddMinutes(-30).AddSeconds(-15).AddMilliseconds(-3);
			Age subject = new Age(advent, terminus);
			Assert.That(subject.ToString(), Is.EqualTo("300 years 2 weeks 1 day 3 hours 30 minutes 15 seconds"));
			Assert.That(subject.ToString(2), Is.EqualTo("300 years 2 weeks"));
			Assert.That(subject.ToString(1), Is.EqualTo("300 years"));

			Age twoYears1977 = new Age(11.March(1977), 11.March(1979)),
				anotherTwoYears1977 = new Age(11.March(1977), 11.March(1979));
			Age twoYears1981 = new Age(11.March(1981), 11.March(1983));
			Assert.That(twoYears1977.Equals(anotherTwoYears1977), Is.True);
			Assert.That(twoYears1977.Equals(twoYears1981), Is.False);
			Assert.That(twoYears1977 == anotherTwoYears1977, Is.True);
			Assert.That(twoYears1977 != twoYears1981, Is.True);
		}

		#endregion

		private static readonly DateTime _keyDateInHistory = new DateTime(1977, 3, 11);
		private static readonly Age _twoYearsFromKeyDate = new Age(_keyDateInHistory, _keyDateInHistory.AddYears(2));
		private static readonly Age _oneYearFromKeyDate = new Age(_keyDateInHistory, _keyDateInHistory.AddYears(1));

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
					.Elapsed(1.Days())
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

		[Test]
		public void ToString_FormatWithSignificance2And3PartAge_LowerLevelTrimmed()
		{
			DateTime terminus = _keyDateInHistory;
			DateTime advent = terminus.AddYears(-300).AddDays(-1).AddHours(-3);
			Age subject = new Age(advent, terminus);
			Assert.That(subject.ToString(), Is.EqualTo("300 years 1 day 3 hours"));
			Assert.That(subject.ToString("g2"), Is.EqualTo("300 years 1 day"));
			Assert.That(subject.ToString("2"), Is.EqualTo("300 years 1 day"));
		}

		#endregion

		#region equality

		[Test]
		public void Equality_SameSpan_TrueWhenEqualAdventAndTerminus()
		{
			Age twoYears1977 = new Age(11.March(1977), 11.March(1979)),
				anotherTwoYears1977 = new Age(11.March(1977), 11.March(1979));
			Age twoYears1981 = new Age(11.March(1981), 11.March(1983));

			Assert.That(twoYears1977.Equals(twoYears1977), Is.True);
			Assert.That(twoYears1977.Equals(anotherTwoYears1977), Is.True);
			Assert.That(anotherTwoYears1977.Equals(twoYears1977), Is.True);

			Assert.That(twoYears1977 == twoYears1977, Is.True);
			Assert.That(twoYears1977 == anotherTwoYears1977, Is.True);
			Assert.That(anotherTwoYears1977 == twoYears1977, Is.True);

			Assert.That(twoYears1977.Equals(twoYears1981), Is.False);
			Assert.That(twoYears1981.Equals(twoYears1977), Is.False);

			Assert.That(twoYears1977 == twoYears1981, Is.False);
			Assert.That(twoYears1981 == twoYears1977, Is.False);
		}

		[Test]
		public void Equality_DifferentSpan_False()
		{
			Age oneYearOld = new Age(12.October(2007), 12.October(2008));
			Age twoYearsOld = new Age(12.October(2006), 12.October(2008));

			Assert.That(oneYearOld.Equals(twoYearsOld), Is.False);
			Assert.That(twoYearsOld.Equals(oneYearOld), Is.False);

			Assert.That(oneYearOld == twoYearsOld, Is.False);
			Assert.That(twoYearsOld == oneYearOld, Is.False);
		}

		[Test]
		public void InEquality_SameSpan_FalseWhenEqualAdventAndTerminus()
		{
			Age twoYears1977 = new Age(11.March(1977), 11.March(1979)),
				anotherTwoYears1977 = new Age(11.March(1977), 11.March(1979));
			Age twoYears1981 = new Age(11.March(1981), 11.March(1983));

			Assert.That(twoYears1977 != anotherTwoYears1977, Is.False);
			Assert.That(anotherTwoYears1977 != twoYears1977, Is.False);

			Assert.That(twoYears1977 != twoYears1981, Is.True);
			Assert.That(twoYears1981 != twoYears1977, Is.True);
		}

		[Test]
		public void InEquality_DifferentSpan_True()
		{
			Age oneYearOld = new Age(12.October(2007), 12.October(2008));
			Age twoYearsOld = new Age(12.October(2006), 12.October(2008));

			Assert.That(oneYearOld != twoYearsOld, Is.True);
			Assert.That(twoYearsOld != oneYearOld, Is.True);
		}

		#endregion

		#region comparisons

		[Test]
		public void CompareTo_NonCompatibleType_Exception()
		{
			Age twoYears = new Age(_keyDateInHistory, _keyDateInHistory.AddYears(2));
			Assert.Throws<ArgumentException>(() => twoYears.CompareTo(3));
		}

		[Test]
		public void CompareTo_Ages()
		{
			Assert.That(_twoYearsFromKeyDate.CompareTo(_oneYearFromKeyDate), Is.GreaterThan(0));
			Assert.That(_oneYearFromKeyDate.CompareTo(_twoYearsFromKeyDate), Is.LessThan(0));
			Assert.That(_oneYearFromKeyDate.CompareTo(_oneYearFromKeyDate), Is.EqualTo(0));
		}

		[Test]
		public void CompareTo_AgesWithDifferentAdventAndTerminus_JustElapsedTimeIsCompared()
		{
			DateTime now = 12.October(2008);
			Age oneYear = new Age(now, now.AddYears(1));

			Assert.That(_twoYearsFromKeyDate.CompareTo(oneYear), Is.GreaterThan(0));
			Assert.That(oneYear.CompareTo(_twoYearsFromKeyDate), Is.LessThan(0));
		}

		[Test]
		public void CompareTo_OneYearAgesWithDifferentAdvent_ComparationZero()
		{
			DateTime oneYearBeforeKeyDate = _keyDateInHistory.AddYears(-1);
			Age twoYearsToKeyDate = new Age(oneYearBeforeKeyDate, oneYearBeforeKeyDate.AddYears(2));

			Assert.That(twoYearsToKeyDate.CompareTo(_twoYearsFromKeyDate), Is.EqualTo(0),
				"Just elapsed time is compared");
		}

		[Test]
		public void CompareTo_OneDayTimeSpan_ComparisonAsExpected()
		{
			Age twoDays = new Age(_keyDateInHistory, _keyDateInHistory.AddDays(2));
			TimeSpan oneDay = 1.Days();

			Assert.That(twoDays.CompareTo(oneDay), Is.GreaterThan(0));
			Assert.That(twoDays.CompareTo(2.Days()), Is.EqualTo(0));

			Assert.That(() => oneDay.CompareTo(twoDays), Throws.InstanceOf<ArgumentException>(),
				"there is automatic conversion from Age to TimeSpan");
		}

		#region gt
		
		[Test]
		public void GreaterThanOperator_EqualDates_SameAsTimeStamp()
		{
			DateTime oneYearBeforeKeyDate = _keyDateInHistory.AddYears(-1);
			Age twoYearsBeforeKeyDate = new Age(oneYearBeforeKeyDate, oneYearBeforeKeyDate.AddYears(2));

			Assert.That(_twoYearsFromKeyDate > _twoYearsFromKeyDate, Is.False);
			Assert.That(_twoYearsFromKeyDate > twoYearsBeforeKeyDate, Is.False);
			Assert.That(twoYearsBeforeKeyDate > _twoYearsFromKeyDate, Is.False);

			Assert.That(_twoYearsFromKeyDate.Elapsed > _twoYearsFromKeyDate.Elapsed, Is.False);
			Assert.That(_twoYearsFromKeyDate.Elapsed > twoYearsBeforeKeyDate.Elapsed, Is.False);
			Assert.That(twoYearsBeforeKeyDate.Elapsed > _twoYearsFromKeyDate.Elapsed, Is.False);
		}

		[Test]
		public void GreaterThanOperator_NonEqualDates_SameAsTimeStamp()
		{
			Assert.That(_twoYearsFromKeyDate > _oneYearFromKeyDate, Is.True);
			Assert.That(_oneYearFromKeyDate > _twoYearsFromKeyDate, Is.False);

			Assert.That(_twoYearsFromKeyDate.Elapsed > _oneYearFromKeyDate.Elapsed, Is.True);
			Assert.That(_oneYearFromKeyDate.Elapsed > _twoYearsFromKeyDate.Elapsed, Is.False);
		}

		[Test]
		public void GreaterThanOperator_ConsistentBehaviorWithCompareTo()
		{
			Assert.That(_twoYearsFromKeyDate > _oneYearFromKeyDate, Is.True);
			Assert.That(_twoYearsFromKeyDate > _oneYearFromKeyDate,
				Is.EqualTo(_twoYearsFromKeyDate.CompareTo(_oneYearFromKeyDate) > 0));

			Assert.That(_oneYearFromKeyDate > _twoYearsFromKeyDate,
				Is.Not.EqualTo(_oneYearFromKeyDate.CompareTo(_twoYearsFromKeyDate) < 0));
			Assert.That(_oneYearFromKeyDate > _twoYearsFromKeyDate, Is.False);

			Assert.That(_oneYearFromKeyDate > _oneYearFromKeyDate,
				Is.Not.EqualTo(_oneYearFromKeyDate.CompareTo(_oneYearFromKeyDate) == 0));
			Assert.That(_oneYearFromKeyDate > _oneYearFromKeyDate, Is.False);
		}

		#endregion

		#region goet

		public void GreaterOrEqualThanOperator_EqualDates_SameAsTimeStamp()
		{
			DateTime oneYearBeforeKeyDate = _keyDateInHistory.AddYears(-1);
			Age twoYearsBeforeKeyDate = new Age(oneYearBeforeKeyDate, oneYearBeforeKeyDate.AddYears(2));

			Assert.That(_twoYearsFromKeyDate >= _twoYearsFromKeyDate, Is.True);
			Assert.That(_twoYearsFromKeyDate >= twoYearsBeforeKeyDate, Is.True);
			Assert.That(twoYearsBeforeKeyDate >= _twoYearsFromKeyDate, Is.True);

			Assert.That(_twoYearsFromKeyDate.Elapsed >= _twoYearsFromKeyDate.Elapsed, Is.True);
			Assert.That(_twoYearsFromKeyDate.Elapsed >= twoYearsBeforeKeyDate.Elapsed, Is.True);
			Assert.That(twoYearsBeforeKeyDate.Elapsed >= _twoYearsFromKeyDate.Elapsed, Is.True);
		}

		[Test]
		public void GreaterOrEqualThanOperator_NonEqualDates_SameAsTimeStamp()
		{
			Assert.That(_twoYearsFromKeyDate >= _oneYearFromKeyDate, Is.True);
			Assert.That(_oneYearFromKeyDate >= _twoYearsFromKeyDate, Is.False);

			Assert.That(_twoYearsFromKeyDate.Elapsed >= _oneYearFromKeyDate.Elapsed, Is.True);
			Assert.That(_oneYearFromKeyDate.Elapsed >= _twoYearsFromKeyDate.Elapsed, Is.False);
		}

		[Test]
		public void GreaterOrEqualThanOperator_ConsistentBehaviorWithCompareTo()
		{
			Assert.That(_twoYearsFromKeyDate >= _oneYearFromKeyDate, Is.True);
			Assert.That(_twoYearsFromKeyDate >= _oneYearFromKeyDate,
				Is.EqualTo(_twoYearsFromKeyDate.CompareTo(_oneYearFromKeyDate) > 0));

			Assert.That(_oneYearFromKeyDate >= _twoYearsFromKeyDate, Is.False);
			Assert.That(_oneYearFromKeyDate >= _twoYearsFromKeyDate,
				Is.Not.EqualTo(_oneYearFromKeyDate.CompareTo(_twoYearsFromKeyDate) < 0));

			Assert.That(_oneYearFromKeyDate >= _oneYearFromKeyDate, Is.True);
			Assert.That(_oneYearFromKeyDate >= _oneYearFromKeyDate,
				Is.EqualTo(_oneYearFromKeyDate.CompareTo(_oneYearFromKeyDate) == 0));
		}

		#endregion

		#region lt

		public void LessThanOperator_EqualDates_SameAsTimeStamp()
		{
			DateTime oneYearBeforeKeyDate = _keyDateInHistory.AddYears(-1);
			Age twoYearsBeforeKeyDate = new Age(oneYearBeforeKeyDate, oneYearBeforeKeyDate.AddYears(2));

			Assert.That(_twoYearsFromKeyDate < _twoYearsFromKeyDate, Is.False);
			Assert.That(_twoYearsFromKeyDate < twoYearsBeforeKeyDate, Is.False);
			Assert.That(twoYearsBeforeKeyDate < _twoYearsFromKeyDate, Is.False);

			Assert.That(_twoYearsFromKeyDate.Elapsed < _twoYearsFromKeyDate.Elapsed, Is.False);
			Assert.That(_twoYearsFromKeyDate.Elapsed < twoYearsBeforeKeyDate.Elapsed, Is.False);
			Assert.That(twoYearsBeforeKeyDate.Elapsed < _twoYearsFromKeyDate.Elapsed, Is.False);
		}

		[Test]
		public void LessThanOperator_NonEqualDates_SameAsTimeStamp()
		{
			Assert.That(_twoYearsFromKeyDate < _oneYearFromKeyDate, Is.False);
			Assert.That(_oneYearFromKeyDate < _twoYearsFromKeyDate, Is.True);

			Assert.That(_twoYearsFromKeyDate.Elapsed < _oneYearFromKeyDate.Elapsed, Is.False);
			Assert.That(_oneYearFromKeyDate.Elapsed < _twoYearsFromKeyDate.Elapsed, Is.True);
		}

		[Test]
		public void LessThanOperator_ConsistentBehaviorWithCompareTo()
		{
			Assert.That(_twoYearsFromKeyDate < _oneYearFromKeyDate, Is.False);
			Assert.That(_twoYearsFromKeyDate < _oneYearFromKeyDate,
				Is.EqualTo(_twoYearsFromKeyDate.CompareTo(_oneYearFromKeyDate) < 0));

			Assert.That(_oneYearFromKeyDate < _twoYearsFromKeyDate,
				Is.EqualTo(_oneYearFromKeyDate.CompareTo(_twoYearsFromKeyDate) < 0));
			Assert.That(_oneYearFromKeyDate < _twoYearsFromKeyDate, Is.True);

			Assert.That(_oneYearFromKeyDate < _oneYearFromKeyDate,
				Is.Not.EqualTo(_oneYearFromKeyDate.CompareTo(_oneYearFromKeyDate) == 0));
			Assert.That(_oneYearFromKeyDate < _oneYearFromKeyDate, Is.False);
		}

		#endregion

		#region loet

		public void LessOrEqualThanOperator_EqualDates_SameAsTimeStamp()
		{
			DateTime oneYearBeforeKeyDate = _keyDateInHistory.AddYears(-1);
			Age twoYearsBeforeKeyDate = new Age(oneYearBeforeKeyDate, oneYearBeforeKeyDate.AddYears(2));

			Assert.That(_twoYearsFromKeyDate <= _twoYearsFromKeyDate, Is.True);
			Assert.That(_twoYearsFromKeyDate <= twoYearsBeforeKeyDate, Is.True);
			Assert.That(twoYearsBeforeKeyDate <= _twoYearsFromKeyDate, Is.True);

			Assert.That(_twoYearsFromKeyDate.Elapsed <= _twoYearsFromKeyDate.Elapsed, Is.True);
			Assert.That(_twoYearsFromKeyDate.Elapsed <= twoYearsBeforeKeyDate.Elapsed, Is.True);
			Assert.That(twoYearsBeforeKeyDate.Elapsed <= _twoYearsFromKeyDate.Elapsed, Is.True);
		}

		[Test]
		public void LessOrEqualThanOperator_NonEqualDates_SameAsTimeStamp()
		{
			Assert.That(_twoYearsFromKeyDate <= _oneYearFromKeyDate, Is.False);
			Assert.That(_oneYearFromKeyDate <= _twoYearsFromKeyDate, Is.True);

			Assert.That(_twoYearsFromKeyDate.Elapsed <= _oneYearFromKeyDate.Elapsed, Is.False);
			Assert.That(_oneYearFromKeyDate.Elapsed <= _twoYearsFromKeyDate.Elapsed, Is.True);
		}

		[Test]
		public void LessOrEqualThanOperator_ConsistentBehaviorWithCompareTo()
		{
			Assert.That(_twoYearsFromKeyDate <= _oneYearFromKeyDate, Is.False);
			Assert.That(_twoYearsFromKeyDate <= _oneYearFromKeyDate,
				Is.EqualTo(_twoYearsFromKeyDate.CompareTo(_oneYearFromKeyDate) < 0));

			Assert.That(_oneYearFromKeyDate <= _twoYearsFromKeyDate,
				Is.EqualTo(_oneYearFromKeyDate.CompareTo(_twoYearsFromKeyDate) < 0));
			Assert.That(_oneYearFromKeyDate <= _twoYearsFromKeyDate, Is.True);

			Assert.That(_oneYearFromKeyDate <= _oneYearFromKeyDate,
				Is.EqualTo(_oneYearFromKeyDate.CompareTo(_oneYearFromKeyDate) == 0));
			Assert.That(_oneYearFromKeyDate <= _oneYearFromKeyDate, Is.True);
		}

		#endregion

		#endregion
	}
}