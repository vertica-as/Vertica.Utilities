using NUnit.Framework;
using Vertica.Utilities_v4.Data;

namespace Vertica.Utilities_v4.Tests.Data
{
	[TestFixture]
	public class StorageTester
	{
		[Test]
		public void Can_Save_Data_Outside_HttpContext()
		{
			var key = new object();
			string value = "value";
			Storage.Data[key] = value;

			Assert.That(Storage.Data[key], Is.SameAs(value));
		}

		[Test]
		public void Clear_RemovesAllElements()
		{
			Storage.Data.Clear();
			Storage.Data["one"] = "This is a string";
			Storage.Data["two"] = 99.9m;

			Assert.That(Storage.Data.Count, Is.EqualTo(2));
			Storage.Data.Clear();
			Assert.That(Storage.Data.Count, Is.EqualTo(0));
		}

		[Test]
		public void Data_NonExistingKey_Null()
		{
			var nonExistingKey = new object();
			Assert.That(Storage.Data[nonExistingKey], Is.Null);
		}
	}
}
