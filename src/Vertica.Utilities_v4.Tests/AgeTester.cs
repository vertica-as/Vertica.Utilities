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

		#endregion

	}
}