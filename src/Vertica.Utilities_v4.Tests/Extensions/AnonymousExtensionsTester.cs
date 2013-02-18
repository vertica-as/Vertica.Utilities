using System;
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

			var typed = enumerable
				.Select(a => a.Cast().ByExample(example));

			Assert.That(typed, Has.All.InstanceOf(example.GetType()));

			Assert.That(typed, Must.Be.Constrained(
				Has.Property("P1").EqualTo("p1_1") & Has.Property("P2").EqualTo(1),
				Has.Property("P1").EqualTo("p1_2") & Has.Property("P2").EqualTo(2)));
		}
	}
}