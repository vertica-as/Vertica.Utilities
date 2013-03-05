using System;
using NUnit.Framework;
using Vertica.Utilities_v4.Eventing;
using Vertica.Utilities_v4.Extensions.DelegateExt;

namespace Vertica.Utilities_v4.Tests.Extensions
{
	[TestFixture]
	public class DeletegateExtensionsTester
	{
		[Test]
		public void Cast_CompatibleAction_Transformed()
		{
			string message = null;
			Action<ValueEventArgs<string>> concreteAction = e => message = e.Value;

			Action<IValueEventArgs<string>> abstractAction = concreteAction
				.Cast<IValueEventArgs<string>, ValueEventArgs<string>>();


			abstractAction(new ValueEventArgs<string>("value"));
			Assert.That(message, Is.EqualTo("value"));
		}

		#region Cast(delegate)

		[Test]
		public void Cast_CompatibleDelegate_Success()
		{
			Predicate<int> oddPredicate = i => i % 2 != 0;
			Func<int, bool> oddFunc = oddPredicate.Cast<Func<int, bool>>();
			Assert.That(oddFunc(2), Is.False);
			Assert.That(oddFunc(1), Is.True);
		}

		[Test]
		public void Cast_CompatibleNullDelegate_Null()
		{
			Predicate<int> nullPredicate = null;
			Func<int, bool> nullFunc = nullPredicate.Cast<Func<int, bool>>();
			Assert.That(nullFunc, Is.Null);
			Assert.Throws<NullReferenceException>(() => nullFunc(0));
		}

		[Test]
		public void Cast_IncompatibleDelegate_Exception()
		{
			Predicate<int> oddPredicate = i => i % 2 != 0;
			Assert.Throws<ArgumentException>(() => oddPredicate.Cast<Func<string, bool>>());

			Assert.Throws<ArgumentException>(() => oddPredicate.Cast<Func<int, int>>());

			Assert.Throws<ArgumentException>(() => oddPredicate.Cast<Func<long, bool>>());
		}

		[Test]
		public void Cast_IncompatibleNullDelegate_Null()
		{
			Predicate<int> nullPredicate = null;
			Func<int, int> nullFunc = nullPredicate.Cast<Func<int, int>>();
			Assert.That(nullFunc, Is.Null);
			Assert.Throws<NullReferenceException>(() => nullFunc(0));
		}

		#endregion
	}
}