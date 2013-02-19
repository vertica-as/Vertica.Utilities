using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;
using Vertica.Utilities_v4.Extensions.AnonymousExt;
using Vertica.Utilities_v4.Extensions.Infrastructure;

namespace Vertica.Utilities_v4.Tests.Extensions
{
	[TestFixture]
	public class AnonymousExtensionsTester
	{
		public object returnAnonymous(string p1, int p2)
		{
			return new { P1 = p1, P2 = p2 };
		}

		#region Cast_ByExamaple

		[Test]
		public void Cast_ByExample_OnObjects_CanStrongTypeSubsequentUses()
		{
			var example = new { P1 = default(string), P2 = default(int) };

			var typed = returnAnonymous("P1", 2).Cast().ByExample(example);

			Assert.That(typed.P1, Is.EqualTo("P1"));
			Assert.That(typed.P2, Is.EqualTo(2));
		}

		[Test]
		public void Cast_ByExample_OnEnumerables_CanStrongTypeSubsequentUses()
		{
			var example = new { P1 = default(string), P2 = default(int) };
			var enumerable = new[] { returnAnonymous("p1_1", 1), returnAnonymous("p1_2", 2) };

			var typed = enumerable.Select(a => a.Cast().ByExample(example));

			Assert.That(typed, Has.All.InstanceOf(example.GetType()));

			Assert.That(typed, Must.Be.Constrained(Has.Property("P1").EqualTo("p1_1") & Has.Property("P2").EqualTo(1), Has.Property("P1").EqualTo("p1_2") & Has.Property("P2").EqualTo(2)));
		}

		#endregion


		#region AsTuples

		[Test]
		public void AsTuples_TuplesWithPropertyNamesAndValues()
		{
			var a = new { P1 = "1", P2 = 2 };

			Assert.That(a.AsTuples(), Is.EquivalentTo(new[]
			{
				new Tuple<string, object>("P1", "1"),
				new Tuple<string, object>("P2", 2)
			}));
		}

		[Test]
		public void AsTuples_NullProperties_AreSkipped()
		{
			var a = new { P1 = "1", P2 = (string)null, P3 = 0 };

			Assert.That(a.AsTuples(), Is.EquivalentTo(new[]
			{
				new Tuple<string, object>("P1", "1"),
				new Tuple<string, object>("P3", 0)
			}));
		}

		[Test]
		public void AsTuples_DefaultValueType_NoValueSkipped()
		{
			var a = new { P1 = 1, P2 = default(int), P3 = 3 };

			Assert.That(a.AsTuples(), Is.EquivalentTo(new[]
			{
				new Tuple<string, object>("P1", 1), 
				new Tuple<string, object>("P2", 0), 
				new Tuple<string, object>("P3", 3)
			}));
		}

		#endregion

		#region AsKeyValuePairs

		[Test]
		public void AsKeyValuePairs_PairsWithPropertyNamesAndValues()
		{
			var a = new { P1 = "1", P2 = 2 };

			Assert.That(a.AsKeyValuePairs(), Is.EquivalentTo(new[]
			{
				new KeyValuePair<string, object>("P1", "1"),
				new KeyValuePair<string, object>("P2", 2)
			}));
		}

		[Test]
		public void AsAsKeyValuePairs_NullProperties_AreSkipped()
		{
			var a = new { P1 = "1", P2 = (string)null, P3 = 0 };

			Assert.That(a.AsKeyValuePairs(), Is.EquivalentTo(new[]
			{
				new KeyValuePair<string, object>("P1", "1"),
				new KeyValuePair<string, object>("P3", 0)
			}));
		}

		[Test]
		public void AsKeyValuePairsAsTuples_DefaultValueType_NoValueSkipped()
		{
			var a = new { P1 = 1, P2 = default(int), P3 = 3 };

			Assert.That(a.AsKeyValuePairs(), Is.EquivalentTo(new[]
			{
				new KeyValuePair<string, object>("P1", 1), 
				new KeyValuePair<string, object>("P2", 0), 
				new KeyValuePair<string, object>("P3", 3)
			}));
		}

		#endregion

		#region AsDictionary

		[Test]
		public void AsDictionary_PairsWithPropertyNamesAndValues()
		{
			var a = new { P1 = "1", P2 = 2 };

			Assert.That(a.AsDictionary(), Is.EquivalentTo(new[]
			{
				new KeyValuePair<string, object>("P1", "1"),
				new KeyValuePair<string, object>("P2", 2)
			}));
		}

		[Test]
		public void AsDictionarys_NullProperties_AreSkipped()
		{
			var a = new { P1 = "1", P2 = (string)null, P3 = 0 };

			Assert.That(a.AsDictionary(), Is.EquivalentTo(new[]
			{
				new KeyValuePair<string, object>("P1", "1"),
				new KeyValuePair<string, object>("P3", 0)
			}));
		}

		[Test]
		public void AsDictionary_DefaultValueType_NoValueSkipped()
		{
			var a = new { P1 = 1, P2 = default(int), P3 = 3 };

			Assert.That(a.AsDictionary(), Is.EquivalentTo(new[]
			{
				new KeyValuePair<string, object>("P1", 1), 
				new KeyValuePair<string, object>("P2", 0), 
				new KeyValuePair<string, object>("P3", 3)
			}));
		}

		#endregion

		#region AsAnonymous

		[Test]
		public void AsAnonymous_AnonymousAsPerPrototype()
		{
			var d = new Dictionary<string, object>
			{
				{ "P1", "1" },
				{ "P2", 2 }
			};

			var prototype = new { P1 = default(string), P2 = default(int) };
			Assert.That(d.AsAnonymous(prototype), Is.EqualTo(new { P1 = "1", P2 = 2 }));
		}

		[Test]
		public void AsAnonymous_SkippedNullElements()
		{
			var d = new Dictionary<string, object>
			{
				{ "P1", "1" },
				{ "P2", null },
				{ "P3", 0 }
			};

			var prototype = new {P1 = default(string), P3 = default(int)};
			Assert.That(d.AsAnonymous(prototype), Is.EqualTo(new { P1 = "1", P3 = 0 }));
		}

		#endregion

	}
}