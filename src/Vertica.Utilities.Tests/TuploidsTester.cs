using System.Collections.Generic;
using NUnit.Framework;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public class TuploidsTester
	{
		[Test]
		public void Pair_Parse_Pair_RightElements()
		{
			var sSubject = Pair<string>.Parse("ab,cd", ',');

			Assert.That(sSubject.First, Is.EqualTo("ab"));
			Assert.That(sSubject.Second, Is.EqualTo("cd"));

			var iSubject = Pair<int>.Parse("1#2", '#');
			Assert.That(iSubject.First, Is.EqualTo(1));
			Assert.That(iSubject.Second, Is.EqualTo(2));
		}

		[Test]
		public void Pair_Parse_NullOrEmptyString_Null()
		{
			Assert.That(Pair<int>.Parse(null, '_'), Is.EqualTo(default(Pair<int>)));
			Assert.That(Pair<int>.Parse(string.Empty, '_'), Is.EqualTo(default(Pair<int>)));
		}

		[Test]
		public void Pair_Parse_PairWrongCount_Exception()
		{
			Assert.That(() => Pair<int>.Parse("asd", '¤'), Throws.ArgumentException);
			Assert.That(() => Pair<int>.Parse("1¤2¤3", '¤'), Throws.ArgumentException);
		}

		[Test]
		public void Pair_ToKeyValuePair_FirstAsKeySecondAsValue()
		{
			var subject = new Pair<string>("first", "second");

			assertKeyValuePair(subject.ToKeyValuePair(), "first", "second");
		}

		private static void assertKeyValuePair<T, K>(KeyValuePair<T, K> pair, T key, K value)
		{
			Assert.That(pair.Key, Is.EqualTo(key));
			Assert.That(pair.Value, Is.EqualTo(value));
		}
	}
}