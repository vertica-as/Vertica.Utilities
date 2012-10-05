using System;
using NUnit.Framework;
using Testing.Commons.Globalization;
using Testing.Commons.Time;
using Vertica.Utilities.Extensions.TimeExt;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public class TimeTester
	{
		[Test]
		public void Shortcuts_AreCorrect()
		{
			Assert.That(Time.OneDay, Is.EqualTo(TimeSpan.FromDays(1)));
			Assert.That(Time.OneHour, Is.EqualTo(TimeSpan.FromHours(1)));
			Assert.That(Time.OneMinute, Is.EqualTo(TimeSpan.FromMinutes(1)));
			Assert.That(Time.OneSecond, Is.EqualTo(TimeSpan.FromSeconds(1)));
			Assert.That(Time.Noon, Is.EqualTo(TimeSpan.FromHours(12)));
			Assert.That(Time.MidNight, Is.EqualTo(TimeSpan.Zero));
			Assert.That(Time.EndOfDay, Is.EqualTo(new TimeSpan(0, 23, 59, 59, 999)));
			Assert.That(Time.BeginningOfDay, Is.EqualTo(TimeSpan.Zero));
			Assert.That(Time.OneWeek, Is.EqualTo(TimeSpan.FromDays(7)));
		}

		[Test]
		public void Now_DefaultsToDateTimeNow()
		{
			DateTimeOffset timeNow = Time.Now, dtNow = DateTimeOffset.Now;

			Assert.That(timeNow - dtNow, Is.LessThan(Time.OneSecond));
		}

		[Test]
		public void SetNowReset_SetsTimeNow_AndSetsItBack()
		{
			DateTimeOffset born = 11.March(1977);
			Time.SetNow(born);
			Assert.That(Time.Now, Is.EqualTo(born));

			Time.ResetNow();
			DateTimeOffset timeNow = Time.Now, dtNow = DateTime.Now;
			Assert.That(timeNow - dtNow, Is.LessThan(Time.OneSecond));
		}

		[Test]
		public void SetUtcNowReset_SetsTimeUtcNow_AndSetsItBack()
		{
			DateTimeOffset born = 11.March(1977).ToUniversalTime();
			Time.SetUtcNow(born);
			Assert.That(Time.UtcNow, Is.EqualTo(born));

			Time.ResetUtcNow();
			DateTimeOffset timeNow = Time.UtcNow, dtNow = DateTime.Now.ToUniversalTime();
			Assert.That(timeNow - dtNow, Is.LessThan(Time.OneSecond));
		}

		[Test]
		public void SetUtcNow_NonUtcOffset_Exception()
		{
			try
			{
				Assert.DoesNotThrow(() => Time.SetUtcNow(DateTimeOffset.UtcNow));

				var plus1Hour = TimeZoneInfo.CreateCustomTimeZone("test", 1.Hours(), "test", "test");
				DateTimeOffset notUtc = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, plus1Hour);
				var ex = Assert.Throws<InvalidTimeZoneException>(() => Time.SetUtcNow(notUtc));
				Assert.That(ex.Message, Is.StringContaining("1"));
			}
			finally
			{
				Time.ResetUtcNow();
			}
		}

		[Test]
		public void SetNowReset_CanBeUsedWithDateTimes()
		{
			DateTime born = 11.March(1977);
			Time.SetNow(born);
			Assert.That(Time.Now.DateTime, Is.EqualTo(born));

			Time.ResetNow();
		}

		[Test]
		public void ToUnixTimestamp()
		{
			Assert.That(Time.ToUnixTime(Time.UnixEpoch), Is.EqualTo(0d));
			Assert.That(Time.ToUnixTime(new DateTimeOffset(2.January(1970), TimeSpan.Zero)), Is.EqualTo(3600d * 24));
			Assert.That(Time.ToUnixTime(new DateTimeOffset(13.June(1984).SetTime(Time.Noon), TimeSpan.Zero)), Is.EqualTo(455976000d));
		}

		[Test]
		public void FromUnixTimestamp()
		{
			Assert.That(Time.FromUnixTime(0d), Is.EqualTo(Time.UnixEpoch));
			Assert.That(Time.FromUnixTime(3600d * 24), Is.EqualTo(new DateTimeOffset(2.January(1970), TimeSpan.Zero)));
			Assert.That(Time.FromUnixTime(455976000d), Is.EqualTo(new DateTimeOffset(13.June(1984).SetTime(Time.Noon), TimeSpan.Zero)));
		}


		[Test]
		public void UnixTimestamp_DateIsFirstOfJanuary1970_VerifyResult()
		{
			using (TimeReseter.Set(Time.UnixEpoch))
			{
				Assert.That(Time.UnixTimestamp, Is.EqualTo(0d));
			}
		}

		[Test]
		public void UnixTimestamp_DateIsSecondOfJanuary1970_VerifyResult()
		{
			using (TimeReseter.Set(new DateTimeOffset(2.January(1970), TimeSpan.Zero)))
			{
				Assert.That(Time.UnixTimestamp, Is.EqualTo(3600d * 24));
			}
		}

		[Test]
		public void UnixTimestamp_DateIs13OfJune1984_VerifyResult()
		{
			using (TimeReseter.Set(new DateTimeOffset(13.June(1984).SetTime(Time.Noon), TimeSpan.Zero)))
			{
				Assert.That(Time.UnixTimestamp, Is.EqualTo(455976000d));
			}
		}


		[Test]
		public void FirstDayOfWeek_DependsOnCulture()
		{
			using (CultureReseter.Set("en-US"))
			{
				Assert.That(Time.FirstDayOfWeek(), Is.EqualTo(DayOfWeek.Sunday));
			}

			using (CultureReseter.Set("es-ES"))
			{
				Assert.That(Time.FirstDayOfWeek(), Is.EqualTo(DayOfWeek.Monday));
			}
		}

		[Test]
		public void LastDayOfWeek_DependsOnCulture()
		{
			using (CultureReseter.Set("en-US"))
			{
				Assert.That(Time.LastDayOfWeek(), Is.EqualTo(DayOfWeek.Saturday));
			}

			using (CultureReseter.Set("es-ES"))
			{
				Assert.That(Time.LastDayOfWeek(), Is.EqualTo(DayOfWeek.Sunday));
			}
		}

		[Test]
		public void LastDayOfWeek_WithFirstDay_DoesNotDependOnCulture()
		{
			DayOfWeek usLastDay, esLastDay;
			DayOfWeek firstDay = DayOfWeek.Thursday;
			using (CultureReseter.Set("en-US"))
			{
				usLastDay = Time.LastDayOfWeek(firstDay);
			}

			using (CultureReseter.Set("es-ES"))
			{
				esLastDay = Time.LastDayOfWeek(firstDay);
			}
			Assert.That(usLastDay, Is.EqualTo(esLastDay));
			Assert.That(usLastDay, Is.EqualTo(DayOfWeek.Wednesday));
		}
	}
}
