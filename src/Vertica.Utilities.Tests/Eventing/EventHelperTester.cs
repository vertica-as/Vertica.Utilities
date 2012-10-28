using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Vertica.Utilities.Eventing;

namespace Vertica.Utilities.Tests.Eventing
{
	[TestFixture]
	public class EventHelperTester
	{
		#region subject

		[NonSerialized]
		private EventHandler<ValueEventArgs<int>> _complexEvent;

		public event EventHandler<ValueEventArgs<int>> ComplexEvent
		{
			add
			{
				lock (this)
				{
					_complexEvent += value;
				}
			}
			remove
			{
				lock (this)
				{
					_complexEvent -= value;
				}
			}
		}

		[NonSerialized]
		private EventHandler _simpleEvent;
		public event EventHandler SimpleEvent
		{
			add
			{
				lock (this)
				{
					_simpleEvent += value;
				}
			}
			remove
			{
				lock (this)
				{
					_simpleEvent -= value;
				}
			}
		}

		public event EventHandler<ValueEventArgs<string>> NoAccessorEvent;

		#endregion

		#region Raise

		[Test]
		public void Raise_NoSubscribers_NoException()
		{
			_simpleEvent = null;
			Assert.DoesNotThrow(() => _simpleEvent.Raise(this, EventArgs.Empty));
		}

		[Test]
		public void Raise_EventsFired_EmptyEventArgs()
		{
			int numberOfEventsFired = 0;

			SimpleEvent += (sender, args) => numberOfEventsFired++;
			SimpleEvent += (sender, args) => numberOfEventsFired++;

			_simpleEvent.Raise(this, EventArgs.Empty);

			Assert.That(numberOfEventsFired, Is.EqualTo(2));
		}

		[Test]
		public void Raise_EventsFired_NonEmptyEventArgs()
		{
			int eventResult = 0;
			ComplexEvent += (sender, e) => eventResult = e.Value;

			_complexEvent.Raise(this, new ValueEventArgs<int>(2));

			Assert.That(eventResult, Is.EqualTo(2));
		}

		[Test]
		public void Raise_NoAccessorEvent_EventFired()
		{
			string eventValue = string.Empty;
			NoAccessorEvent += (sender, e) => eventValue = e.Value;

			NoAccessorEvent.Raise(this, new ValueEventArgs<string>("asd"));

			Assert.That(eventValue, Is.EqualTo("asd"));
		}

		[Test]
		public void RaiseExtension_NoSubscribers_NoException()
		{
			_simpleEvent = null;
			Assert.That(() => _simpleEvent.Raise(this, EventArgs.Empty), Throws.Nothing);
		}

		#endregion
	}
}
