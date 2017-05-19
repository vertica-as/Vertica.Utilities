using System.Collections.Generic;
using NUnit.Framework;
using Vertica.Utilities.Collections;

namespace Vertica.Utilities.Tests.Collections
{
	[TestFixture]
	public class GenericDictionaryAdapterTester
	{
		[Test]
		public void StringIndexerGetter_ExistingKey_ReturnsValue()
		{
			IDictionary<string, int> adaptee = new Dictionary<string, int>();
			adaptee.Add("key", 0);

			var target = new GenericDictionaryAdapter<string, int>(adaptee);
			Assert.That(target["key"], Is.EqualTo(0));
		}

		[Test]
		public void IntIndexerGetter_NonExistingKey_Exception()
		{
			IDictionary<int, string> adaptee = new Dictionary<int, string>();
			adaptee.Add(1, "one");

			var target = new GenericDictionaryAdapter<int, string>(adaptee);
			Assert.That(() => { string s = target[2]; }, Throws.InstanceOf<KeyNotFoundException>());
		}

		[Test]
		public void StringIndexerSetter_ExistingKey_ChangesValue()
		{
			IDictionary<string, int> adaptee = new Dictionary<string, int>();
			adaptee.Add("key", 0);

			var target = new GenericDictionaryAdapter<string, int>(adaptee);
			target["key"] = 1;
			Assert.That(target["key"], Is.EqualTo(1));
		}

		[Test]
		public void IntIndexerSetter_NonExistingKey_AddsValue()
		{
			IDictionary<int, string> adaptee = new Dictionary<int, string>();

			var target = new GenericDictionaryAdapter<int, string>(adaptee);
			target[2] = "two";
			Assert.That(adaptee.Count, Is.EqualTo(1));
			Assert.That(target[2], Is.EqualTo("two"));
		}
	}
}