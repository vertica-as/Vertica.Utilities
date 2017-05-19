using NUnit.Framework;
using Vertica.Utilities.Collections;
using Vertica.Utilities.Web;

namespace Vertica.Utilities.Tests.Web
{
	[TestFixture]
	public class QueryStringHelperTester
	{
		#region QueryString from NameValueCollection

		[Test]
		public void ToQueryString_EmptyCollection_Empty()
		{
			string qs = new KeyValuesCollection().ToQueryString();

			Assert.That(qs, Is.Empty);
		}

		[Test]
		public void ToQueryString_OneElement_PairWithoutAmp()
		{
			string qs = new KeyValuesCollection { { "key", "value" } }
				.ToQueryString();
			
			Assert.That(qs, Is.EqualTo("?key=value"));
		}

		[Test]
		public void ToQueryString_TwoElements_AmpSeparatedPairs()
		{
			string qs = new KeyValuesCollection { { "key1", "value1" }, { "key2", "value2" } }
				.ToQueryString();
			
			Assert.That(qs, Is.EqualTo("?key1=value1&key2=value2"));
		}

		[Test]
		public void ToQueryString_EncodeableElementsEscaped_EncodedKeysAndValues()
		{
			string qs = new KeyValuesCollection { { "key 1", "value /1" } }
				.ToQueryString();
			
			Assert.That(qs, Is.EqualTo("?key+1=value+%2F1"));
		}

		[Test]
		public void ToQueryString_EmptyValuedElements_Kept()
		{
			string qs = new KeyValuesCollection { { "key", string.Empty } }
				.ToQueryString();

			Assert.That(qs, Is.EqualTo("?key="));
		}

		[Test]
		public void ToQueryString_EmptyKeyedElements_Kept()
		{
			string qs = new KeyValuesCollection { { string.Empty, "value" } }
				.ToQueryString();

			Assert.That(qs, Is.EqualTo("?=value"));
		}

		[Test]
		public void ToQueryString_NullValuedElements_Kept()
		{
			string qs = new KeyValuesCollection { { "key", null } }
				.ToQueryString();

			Assert.That(qs, Is.EqualTo("?key="));
		}

		#endregion

		#region Query from NameValueCollection

		[Test]
		public void ToQuery_EmptyCollection_Empty()
		{
			string q = new KeyValuesCollection().ToQuery();

			Assert.That(q, Is.Empty);
		}

		[Test]
		public void ToQuery_OneElement_PairWithoutAmp()
		{
			string q = new KeyValuesCollection { { "key", "value" } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("key=value"));
		}

		[Test]
		public void ToQuery_TwoElements_AmpSeparatedPairs()
		{
			string q = new KeyValuesCollection { { "key1", "value1" }, { "key2", "value2" } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("key1=value1&key2=value2"));
		}

		[Test]
		public void ToQuery_EncodeableElementsEscaped_EncodedKeysAndValues()
		{
			string q = new KeyValuesCollection { { "key 1", "value /1" } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("key+1=value+%2F1"));
		}

		[Test]
		public void ToQuery_EmptyValuedElements_Kept()
		{
			string q = new KeyValuesCollection { { "key", string.Empty } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("key="));
		}

		[Test]
		public void ToQuery_EmptyKeyedElements_Kept()
		{
			string q = new KeyValuesCollection { { string.Empty, "value" } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("=value"));
		}

		[Test]
		public void ToQuery_NullValuedElements_Kept()
		{
			string q = new KeyValuesCollection { { "key", null } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("key="));
		}

		#endregion

		#region DecodedQueryString from NameValueCollection

		[Test]
		public void ToDecodedQueryString_EmptyCollection_Empty()
		{
			string qs = new KeyValuesCollection().ToDecodedQueryString();

			Assert.That(qs, Is.Empty);
		}

		[Test]
		public void ToDecodedQueryString_OneElement_PairWithoutAmp()
		{
			string qs = new KeyValuesCollection { { "key", "value" } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?key=value"));
		}

		[Test]
		public void ToDecodedQueryString_TwoElements_AmpSeparatedPairs()
		{
			string qs = new KeyValuesCollection { { "key1", "value1" }, { "key2", "value2" } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?key1=value1&key2=value2"));
		}

		[Test]
		public void ToDecodedQueryString_EncodeableElementsEscaped_NotEncodedKeysAndValues()
		{
			string qs = new KeyValuesCollection { { "key 1", "value /1" } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?key 1=value /1"));
		}

		[Test]
		public void ToDecodedQueryString_EmptyValuedElements_Kept()
		{
			string qs = new KeyValuesCollection { { "key", string.Empty } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?key="));
		}

		[Test]
		public void ToDecodedQueryString_EmptyKeyedElements_Kept()
		{
			string qs = new KeyValuesCollection { { string.Empty, "value" } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?=value"));
		}

		[Test]
		public void ToDecodedQueryString_NullValuedElements_Kept()
		{
			string qs = new KeyValuesCollection { { "key", null } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?key="));
		}

		[Test]
        public void ToDecodedQueryString_MultiValuedKey_RepeatsKeyQs()
        {
            string qs = new KeyValuesCollection { { "key", "value1" }, { "key", "value2" } }
                .ToDecodedQueryString();

            Assert.That(qs, Is.EqualTo("?key=value1&key=value2"));
        }

        #endregion

        #region DecodedQuery from NameValueCollection

        [Test]
		public void ToDecodedQuery_EmptyCollection_Empty()
		{
			string q = new KeyValuesCollection().ToDecodedQuery();

			Assert.That(q, Is.Empty);
		}

		[Test]
		public void ToDecodedQuery_OneElement_PairWithoutAmp()
		{
			string q = new KeyValuesCollection { { "key", "value" } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("key=value"));
		}

		[Test]
		public void ToDecodedQuery_TwoElements_AmpSeparatedPairs()
		{
			string q = new KeyValuesCollection { { "key1", "value1" }, { "key2", "value2" } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("key1=value1&key2=value2"));
		}

		[Test]
		public void ToDecodedQuery_EncodeableElementsEscaped_NotEncodedKeysAndValues()
		{
			string q = new KeyValuesCollection { { "key 1", "value /1" } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("key 1=value /1"));
		}

		[Test]
		public void ToDecodedQuery_EmptyValuedElements_Kept()
		{
			string q = new KeyValuesCollection { { "key", string.Empty } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("key="));
		}

		[Test]
		public void ToDecodedQuery_EmptyKeyedElements_Kept()
		{
			string q = new KeyValuesCollection { { string.Empty, "value" } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("=value"));
		}

		[Test]
		public void ToDecodedQuery_NullValuedElements_Kept()
		{
			string q = new KeyValuesCollection { { "key", null } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("key="));
		}

		#endregion
	}
}