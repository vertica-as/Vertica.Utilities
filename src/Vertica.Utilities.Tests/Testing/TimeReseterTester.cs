using System;
using NUnit.Framework;
using Testing.Commons.Time;
using Vertica.Utilities.Testing;

namespace Vertica.Utilities.Tests.Testing
{
	[TestFixture]
	public class TimeReseterTester
	{
		[Test]
		public void Set_SetsTimeNow()
		{
			var born = 11.March(1977);
			using (TimeReseter.Set(born))
			{
				Assert.That(Time.Now.DateTime, Is.EqualTo(born));
			}
		}

		[Test]
		public void Set_SetsBackTimeWhenDisposed()
		{
			var born = 11.March(1977);
			var reseter = TimeReseter.Set(born);
			reseter.Dispose();
			DateTimeOffset timeNow = Time.Now, dtNow = DateTime.Now;
			Assert.That(timeNow - dtNow, Is.LessThan(Time.OneSecond));
		}

		[Test]
		public void Set_DoesNotSetUtcNow()
		{
			var born = 11.March(1977);
			using (TimeReseter.Set(born))
			{
				Assert.That(Time.UtcNow.DateTime, Is.Not.EqualTo(born));
			}
		}

		[Test]
		public void SetUtc_SetsUtcTime()
		{
			var born = 11.March(1977).InUtc();
			using (TimeReseter.SetUtc(born))
			{
				Assert.That(Time.UtcNow, Is.EqualTo(born));
			}
		}

		[Test]
		public void SetUtc_NonUtcTime_Exception()
		{
			DateTimeOffset nonUtc = 11.March(1977).In(2.Hours());
			Assert.That(()=> TimeReseter.SetUtc(nonUtc), Throws.InstanceOf<InvalidTimeZoneException>());
		}

		[Test]
		public void SetUtc_SetsBackUtcTimeWhenDisposed()
		{
			var born = 11.March(1977).InUtc();
			var reseter = TimeReseter.SetUtc(born);
			reseter.Dispose();
			DateTimeOffset timeNow = Time.UtcNow, dtNow = DateTimeOffset.UtcNow;
			Assert.That(timeNow - dtNow, Is.LessThan(Time.OneSecond));
		}
	}
}
