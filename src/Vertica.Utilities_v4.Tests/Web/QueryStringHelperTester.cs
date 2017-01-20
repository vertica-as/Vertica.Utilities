using System;
using System.Collections.Specialized;
using NUnit.Framework;
using Vertica.Utilities_v4.Web;

namespace Vertica.Utilities_v4.Tests.Web
{
	[TestFixture]
	public class QueryStringHelperTester
	{
		#region QueryString from Uri

		[Test]
		public void QueryString_TwoValues_CollectionOfTwo()
		{
			NameValueCollection querystring = new Uri("http://localhost/asd.aspx?key1=value1&key2=value2").QueryString();

			Assert.That(querystring.Count, Is.EqualTo(2));
			Assert.That(querystring["key1"], Is.EqualTo("value1"));
			Assert.That(querystring["key2"], Is.EqualTo("value2"));
		}

		[Test]
		public void QueryString_NoQueryString_NoElements()
		{
			NameValueCollection querystring = new Uri("http://localhost/asd.aspx").QueryString();
			Assert.That(querystring.Count, Is.EqualTo(0));
		}

		[Test]
		public void QueryString_RepeatedElements_MultiValue()
		{
			NameValueCollection querystring = new Uri("http://localhost/asd.aspx?key1=value1&key1=value2").QueryString();
			Assert.That(querystring.Count, Is.EqualTo(1));
			Assert.That(querystring["key1"], Is.EqualTo("value1,value2"));
		}

		[Test]
		public void QueryString_UntrimmedKeysAndValues_UntrimmedKeysAndValues()
		{
			NameValueCollection querystring = new Uri("http://localhost/asd.aspx?key1 =value1 &key2= value2").QueryString();
			Assert.That(querystring.Count, Is.EqualTo(2));
			Assert.That(querystring["key1 "], Is.EqualTo("value1 "));
			Assert.That(querystring["key2"], Is.EqualTo(" value2"));
		}

		[Test]
		public void QueryString_NotEncoded_ValuesOrKeysNotEncoded()
		{
			NameValueCollection querystring = new Uri("http://localhost/asd.aspx?øl=mørk/søde").QueryString();
			Assert.That(querystring.Count, Is.EqualTo(1));
			Assert.That(querystring["øl"], Is.EqualTo("mørk/søde"));
		}

		[Test]
		public void QueryString_Encoded_ValuesAndKeysDecoded()
		{
			NameValueCollection querystring = new Uri("http://localhost/asd.aspx?%c3%b8=a%2fb").QueryString();
			Assert.That(querystring.Count, Is.EqualTo(1));
			Assert.That(querystring["ø"], Is.EqualTo("a/b"));
		}

		#endregion

		#region QueryString from NameValueCollection

		[Test]
		public void ToQueryString_EmptyCollection_Empty()
		{
			string qs = new NameValueCollection().ToQueryString();

			Assert.That(qs, Is.Empty);
		}

		[Test]
		public void ToQueryString_OneElement_PairWithoutAmp()
		{
			string qs = new NameValueCollection { { "key", "value" } }
				.ToQueryString();
			
			Assert.That(qs, Is.EqualTo("?key=value"));
		}

		[Test]
		public void ToQueryString_TwoElements_AmpSeparatedPairs()
		{
			string qs = new NameValueCollection { { "key1", "value1" }, { "key2", "value2" } }
				.ToQueryString();
			
			Assert.That(qs, Is.EqualTo("?key1=value1&key2=value2"));
		}

		[Test]
		public void ToQueryString_EncodeableElementsEscaped_EncodedKeysAndValues()
		{
			string qs = new NameValueCollection { { "key 1", "value /1" } }
				.ToQueryString();
			
			Assert.That(qs, Is.EqualTo("?key+1=value+%2f1"));
		}

		[Test]
		public void ToQueryString_EmptyValuedElements_Kept()
		{
			string qs = new NameValueCollection { { "key", string.Empty } }
				.ToQueryString();

			Assert.That(qs, Is.EqualTo("?key="));
		}

		[Test]
		public void ToQueryString_EmptyKeyedElements_Kept()
		{
			string qs = new NameValueCollection { { string.Empty, "value" } }
				.ToQueryString();

			Assert.That(qs, Is.EqualTo("?=value"));
		}

		[Test]
		public void ToQueryString_NullValuedElements_Kept()
		{
			string qs = new NameValueCollection { { "key", null } }
				.ToQueryString();

			Assert.That(qs, Is.EqualTo("?key="));
		}

		[Test]
		public void ToQueryString_NullKeyedElements_Stripped()
		{
			string qs = new NameValueCollection { { null, "value" } }
				.ToQueryString();

			Assert.That(qs, Is.Empty);
		}

		[Test]
		public void ToQueryString_MixedNullKeyedElements_Stripped()
		{
			string qs = new NameValueCollection { {"k", "v"}, { null, "value" }, {"", null} }
				.ToQueryString();

			Assert.That(qs, Is.EqualTo("?k=v&="));
		}

		#endregion

		#region Query from NameValueCollection

		[Test]
		public void ToQuery_EmptyCollection_Empty()
		{
			string q = new NameValueCollection().ToQuery();

			Assert.That(q, Is.Empty);
		}

		[Test]
		public void ToQuery_OneElement_PairWithoutAmp()
		{
			string q = new NameValueCollection { { "key", "value" } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("key=value"));
		}

		[Test]
		public void ToQuery_TwoElements_AmpSeparatedPairs()
		{
			string q = new NameValueCollection { { "key1", "value1" }, { "key2", "value2" } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("key1=value1&key2=value2"));
		}

		[Test]
		public void ToQuery_EncodeableElementsEscaped_EncodedKeysAndValues()
		{
			string q = new NameValueCollection { { "key 1", "value /1" } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("key+1=value+%2f1"));
		}

		[Test]
		public void ToQuery_EmptyValuedElements_Kept()
		{
			string q = new NameValueCollection { { "key", string.Empty } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("key="));
		}

		[Test]
		public void ToQuery_EmptyKeyedElements_Kept()
		{
			string q = new NameValueCollection { { string.Empty, "value" } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("=value"));
		}

		[Test]
		public void ToQuery_NullValuedElements_Kept()
		{
			string q = new NameValueCollection { { "key", null } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("key="));
		}

		[Test]
		public void ToQuery_NullKeyedElements_Stripped()
		{
			string q = new NameValueCollection { { null, "value" } }
				.ToQuery();

			Assert.That(q, Is.Empty);
		}

		[Test]
		public void ToQuery_MixedNullKeyedElements_Stripped()
		{
			string q = new NameValueCollection { { "k", "v" }, { null, "value" }, { "", null } }
				.ToQuery();

			Assert.That(q, Is.EqualTo("k=v&="));
		}

		#endregion

		#region DecodedQueryString from NameValueCollection

		[Test]
		public void ToDecodedQueryString_EmptyCollection_Empty()
		{
			string qs = new NameValueCollection().ToDecodedQueryString();

			Assert.That(qs, Is.Empty);
		}

		[Test]
		public void ToDecodedQueryString_OneElement_PairWithoutAmp()
		{
			string qs = new NameValueCollection { { "key", "value" } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?key=value"));
		}

		[Test]
		public void ToDecodedQueryString_TwoElements_AmpSeparatedPairs()
		{
			string qs = new NameValueCollection { { "key1", "value1" }, { "key2", "value2" } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?key1=value1&key2=value2"));
		}

		[Test]
		public void ToDecodedQueryString_EncodeableElementsEscaped_NotEncodedKeysAndValues()
		{
			string qs = new NameValueCollection { { "key 1", "value /1" } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?key 1=value /1"));
		}

		[Test]
		public void ToDecodedQueryString_EmptyValuedElements_Kept()
		{
			string qs = new NameValueCollection { { "key", string.Empty } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?key="));
		}

		[Test]
		public void ToDecodedQueryString_EmptyKeyedElements_Kept()
		{
			string qs = new NameValueCollection { { string.Empty, "value" } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?=value"));
		}

		[Test]
		public void ToDecodedQueryString_NullValuedElements_Kept()
		{
			string qs = new NameValueCollection { { "key", null } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?key="));
		}

		[Test]
		public void ToDecodedQueryString_NullKeyedElements_Stripped()
		{
			string qs = new NameValueCollection { { null, "value" } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.Empty);
		}

		[Test]
		public void ToDecodedQueryString_MixedNullKeyedElements_Stripped()
		{
			string qs = new NameValueCollection { { "k", "v" }, { null, "value" }, { "", null } }
				.ToDecodedQueryString();

			Assert.That(qs, Is.EqualTo("?k=v&="));
		}

        [Test]
        public void ToDecodedQueryString_MultiValuedKey_RepeatsKeyQs()
        {
            string qs = new NameValueCollection { { "key", "value1" }, { "key", "value2" } }
                .ToDecodedQueryString();

            Assert.That(qs, Is.EqualTo("?key=value1&key=value2"));
        }

        #endregion

        #region DecodedQuery from NameValueCollection

        [Test]
		public void ToDecodedQuery_EmptyCollection_Empty()
		{
			string q = new NameValueCollection().ToDecodedQuery();

			Assert.That(q, Is.Empty);
		}

		[Test]
		public void ToDecodedQuery_OneElement_PairWithoutAmp()
		{
			string q = new NameValueCollection { { "key", "value" } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("key=value"));
		}

		[Test]
		public void ToDecodedQuery_TwoElements_AmpSeparatedPairs()
		{
			string q = new NameValueCollection { { "key1", "value1" }, { "key2", "value2" } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("key1=value1&key2=value2"));
		}

		[Test]
		public void ToDecodedQuery_EncodeableElementsEscaped_NotEncodedKeysAndValues()
		{
			string q = new NameValueCollection { { "key 1", "value /1" } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("key 1=value /1"));
		}

		[Test]
		public void ToDecodedQuery_EmptyValuedElements_Kept()
		{
			string q = new NameValueCollection { { "key", string.Empty } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("key="));
		}

		[Test]
		public void ToDecodedQuery_EmptyKeyedElements_Kept()
		{
			string q = new NameValueCollection { { string.Empty, "value" } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("=value"));
		}

		[Test]
		public void ToDecodedQuery_NullValuedElements_Kept()
		{
			string q = new NameValueCollection { { "key", null } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("key="));
		}

		[Test]
		public void ToDecodedQuery_NullKeyedElements_Stripped()
		{
			string q = new NameValueCollection { { null, "value" } }
				.ToDecodedQuery();

			Assert.That(q, Is.Empty);
		}

		[Test]
		public void ToDecodedQuery_MixedNullKeyedElements_Stripped()
		{
			string q = new NameValueCollection { { "k", "v" }, { null, "value" }, { "", null } }
				.ToDecodedQuery();

			Assert.That(q, Is.EqualTo("k=v&="));
		}

		#endregion
	}
}