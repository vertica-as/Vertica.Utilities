using System;
using NUnit.Framework;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public class DisposableActionTester
	{
		[Test]
		public void Dispose_ActionExecuted()
		{
			bool executed = false;
			IDisposable subject = new DisposableAction(() => executed = true);

			Assert.That(executed, Is.False);
			subject.Dispose();
			Assert.That(executed, Is.True);
		}

		[Test]
		public void UsingPattern_AlsoExecutesAction()
		{
			bool executed = false;
			using (new DisposableAction(() => executed = true))
			{
				Assert.That(executed, Is.False);	
			}
			Assert.That(executed, Is.True);
		}
	}
}
