using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Web;
using NUnit.Framework;
using Testing.Commons.Time;
using Testing.Commons.Web;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class StorageTester
	{
		#region subjects
		class StorageSubject
		{
			public string S { get; set; }
			public DateTimeOffset? D { get; set; }
		}

		#endregion

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

		[Test]
		public void Can_Save_Data_Inside_HttpContext()
		{
			using (HttpContextReseter.Set(new HttpContextBuilder()))
			{
				var key = new object();
				string value = "value";
				Storage.Data[key] = value;

				Assert.That(Storage.Data[key], Is.SameAs(value));
				
				Assert.That(webHashTable.Contains(key), Is.True);
				Assert.That(webHashTable[key], Is.SameAs(value));
			}
		}

		private Hashtable webHashTable
		{
			get { return (Hashtable) HttpContext.Current.Items[Storage.LocalStorageKey]; }
		}

		[Test]
		public void HttpContext_Data_NonExistingKey_Null()
		{
			using (HttpContextReseter.Set(new HttpContextBuilder()))
			{
				var nonExistingKey = new object();

				Assert.That(Storage.Data[nonExistingKey], Is.Null);
				
				Assert.That(webHashTable.Contains(nonExistingKey), Is.False);
				Assert.That(webHashTable[nonExistingKey], Is.Null);
			}
		}

		[Test]
		public void Internal_Web_Data_Not_Initialized_Until_First_Use()
		{
			using (HttpContextReseter.Set(new HttpContextBuilder()))
			{
				Assert.That(webHashTable, Is.Null);

				var key = new object();
				object data = Storage.Data[key];
				Assert.That(webHashTable, Is.Not.Null);
				Assert.That(webHashTable, Is.Empty);
			}
		}

		[Test]
		public void Can_store_values_in_local_data()
		{
			Storage.Data["one"] = "This is a string";
			Storage.Data["two"] = 99.9m;
			var person = new StorageSubject { S = "John Doe", D = 11.March(1977) };
			Storage.Data[1] = person;

			// previous test might count
			Assert.That(Storage.Data.Count, Is.AtLeast(3));
			Assert.That(Storage.Data["one"], Is.EqualTo("This is a string"));
			Assert.That(Storage.Data["two"], Is.EqualTo(99.9m));
			Assert.That(Storage.Data[1], Is.SameAs(person));
		}

		private ManualResetEvent _event;
		[Test]
		public void Local_data_is_thread_local()
		{
			debug("Starting in main");

			Storage.Data["one"] = "This is a string";
			Assert.That(Storage.Data.Count, Is.EqualTo(1));
			Assert.That(Storage.Data["one"], Is.EqualTo("This is a string"));

			_event = new ManualResetEvent(false);
			var backgroundThread = new Thread(runInOtherThread);
			backgroundThread.Start();

			// give the background thread some time to do its job
			Thread.Sleep(100);

			// we still have only one entry (in this thread)
			Assert.That(Storage.Data.Count, Is.EqualTo(1));

			debug("Signaling background thread from main");

			_event.Set();
			backgroundThread.Join();
		}

		private static void debug(string message)
		{
			Debug.WriteLine("{0} thread {1}", message, Thread.CurrentThread.ManagedThreadId);
		}

		private void runInOtherThread()
		{
			debug("Starting (background-)");

			// initially the local data must be empty for this NEW thread!
			Assert.That(Storage.Data.Count, Is.EqualTo(0));

			Storage.Data["one"] = "This is another string";
			Assert.That(Storage.Data.Count, Is.EqualTo(1));
			Assert.That(Storage.Data["one"], Is.EqualTo("This is another string"));

			debug("Waiting on (background-)");

			_event.WaitOne();

			debug("Ending (background-)");
		}
	}
}
