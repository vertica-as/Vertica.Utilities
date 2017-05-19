using System;
using NUnit.Framework;
using Vertica.Utilities.Tests.Eventing.Support;
using Vertica.Utilities.Eventing;

namespace Vertica.Utilities.Tests.Eventing
{
	[TestFixture]
	public class EventHelperTester
	{
		#region subject

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

		private static event ChainedEventHandler<ChainedEventArgs> _chained;

		internal class EventChainSubject
		{
			internal event ChainedEventHandler<ChainedEventArgs> Event;
			internal bool OnChainedEvent(ChainedEventArgs args)
			{
				return Event.Raise(this, args);
			}
		}

		private static event ChainedEventHandler<MutableValueEventArgs<string>, string> _abortable;

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

		[Test]
		public void Raise_PropertyChangeExtension_EventFired()
		{
			var raiser = new NotifySubject();
			string changed = string.Empty;
			raiser.PropertyChanged += (sender, e) => { changed = e.PropertyName; };
			raiser.D = 3m;

			Assert.That(changed, Is.EqualTo("D"));
		}

		#endregion

		#region Notify

		#region property changing

		[Test]
		public void Notify_Changing_NoValues()
		{
			var subject = new NotifySubject();
			string propertyChangingName = null;
			subject.PropertyChanging += (sender, args) => propertyChangingName = args.PropertyName;

			subject.S = "2";
			Assert.That(propertyChangingName, Is.EqualTo("S"));
			subject.I = 2;
			Assert.That(propertyChangingName, Is.EqualTo("I"));
		}

		[Test]
		public void Notify_Changing_Values()
		{
			var subject = new NotifySubject { I = 1 };

			string propertyChangingName = null;
			int oldValue = 0, newValue = 0;
			subject.PropertyChanging += (sender, args) =>
			{
				// nasty casting due to inflexibility of generics
				var extended = (PropertyValueChangingEventArgs<int>)args;
				propertyChangingName = extended.PropertyName;
				oldValue = extended.OldValue;
				newValue = extended.NewValue;
			};
			
			subject.I = 2;
			Assert.That(propertyChangingName, Is.EqualTo("I"));
			Assert.That(oldValue, Is.EqualTo(1));
			Assert.That(newValue, Is.EqualTo(2));
		}

		[Test]
		public void Notify_HowToCancel_OnProperlyImplementedProperties()
		{
			var subject = new NotifySubject { I = 1, F = 2.0f };

			subject.PropertyChanging += (sender, args) =>
			{
				var cancellable = args as ICancelEventArgs;
				if (cancellable != null) cancellable.Cancel();
			};

			subject.I = 2;
			// I does not obbey cancellation rules
			Assert.That(subject.I, Is.EqualTo(2));
			subject.F = 3.0f;
			// F obbeys cancellation rules and since it is being, regardless, cancelled
			// the new value will never be set
			Assert.That(subject.F, Is.EqualTo(2.0f));
		}

		#endregion

		#region property changed

		[Test]
		public void Notify_Changed_NoValues()
		{
			var subject = new NotifySubject();
			string propertyChangedName = null;
			subject.PropertyChanged += (sender, args) => propertyChangedName = args.PropertyName;

			subject.S = "2";
			Assert.That(propertyChangedName, Is.EqualTo("S"));
			subject.I = 2;
			Assert.That(propertyChangedName, Is.EqualTo("I"));
		}

		[Test]
		public void Notify_Changed_Values()
		{
			var subject = new NotifySubject { I = 1 };

			string propertyChangedName = null;
			int oldValue = 0, newValue = 0;
			subject.PropertyChanged += (sender, args) =>
			{
				// nasty casting due to inflexibility of generics
				var extended = (PropertyValueChangedEventArgs<int>)args;
				propertyChangedName = extended.PropertyName;
				oldValue = extended.OldValue;
				newValue = extended.NewValue;
			};


			subject.I = 2;
			Assert.That(propertyChangedName, Is.EqualTo("I"));
			Assert.That(oldValue, Is.EqualTo(1));
			Assert.That(newValue, Is.EqualTo(2));
		}

		#endregion

		#endregion

		#region observations

		[Test]
		public void Observed_PropertyChanged_Changes()
		{
			var subject = new NotifySubject();
			string propertyChangedName = null;
			using (subject.Observed((sender, e) => propertyChangedName = e.PropertyName))
			{
				subject.S = "2";
			}
			// this happens after the handler has been unregistered
			subject.I = 2;

			Assert.That(propertyChangedName, Is.EqualTo("S"));
		}

		[Test]
		public void Observing_PropertyChanging_Changes()
		{
			var subject = new NotifySubject();
			string propertyChangingName = null;
			using (subject.Observing((sender, e) => propertyChangingName = e.PropertyName))
			{
				subject.S = "2";
			}
			// this happens after the handler has been unregistered
			subject.I = 2;

			Assert.That(propertyChangingName, Is.EqualTo("S"));
		}

		#endregion

		#region chains

		[Test]
		public void Raise_OwnEvent_HandledBySecond_NoFurtherCallbackExecuted()
		{
			string lastHandler = string.Empty;
			int numberOfHandlersExecuted = 0;

			_chained += (handler, e) =>
			{ 
				numberOfHandlersExecuted++;
				lastHandler = "one";
			};
			_chained += (handler, e) =>
			{
				numberOfHandlersExecuted++;
				lastHandler = "two";
				e.Handled = true;
			};
			_chained += (handler, e) =>
			{
				numberOfHandlersExecuted++;
				lastHandler = "three";
			};

			var args = new ChainedEventArgs();

			bool result = _chained.Raise(this, args);

			Assert.That(result, Is.True);
			Assert.That(args.Handled, Is.True);
			Assert.That(numberOfHandlersExecuted, Is.EqualTo(2));
			Assert.That(lastHandler, Is.EqualTo("two"));
		}

		[Test]
		public void Raise_ExternalClassEvent_HandledBySecond_NoFurtherCallbackExecuted()
		{
			int numberOfHandlersExecuted = 0;

			var subject = new EventChainSubject();
			subject.Event += (sender, e) => numberOfHandlersExecuted++;
			subject.Event += (sender, e) =>
			{
				numberOfHandlersExecuted++;
				e.Handled = true;
			};
			subject.Event += (sender, e) => numberOfHandlersExecuted++;

			var args = new ChainedEventArgs();
			bool result = subject.OnChainedEvent(args);

			Assert.That(result, Is.True);
			Assert.That(args.Handled, Is.True);
			Assert.That(numberOfHandlersExecuted, Is.EqualTo(2));
		}

		[Test]
		public void Raise_EmptyRegistratinList_NoHandling()
		{
			var subject = new EventChainSubject();
			var args = new ChainedEventArgs();
			bool result = subject.OnChainedEvent(args);

			Assert.That(result, Is.False);
			Assert.That(args.Handled, Is.False);
		}

		[Test]
		public void RaiseUntil_AbortableChain_HandledWhenValueIs2_NoFurtherCallbacksExecuted()
		{
			int numberOfHandlersExecuted = 0;
			_abortable += (handler, e) =>
			{
				numberOfHandlersExecuted++;
				return e.Value = "1";
			};
			_abortable += (handler, e) =>
			{
				numberOfHandlersExecuted++;
				return e.Value = "2";
			};
			_abortable += (handler, e) =>
			{
				numberOfHandlersExecuted++;
				return e.Value = "3";
			};
			
			var args = new MutableValueEventArgs<string>();

			bool result = _abortable.RaiseUntil(this, args, e => e.Equals("2", StringComparison.OrdinalIgnoreCase));

			Assert.That(result, Is.True);
			Assert.That(args.Value, Is.EqualTo("2"));
			Assert.That(numberOfHandlersExecuted, Is.EqualTo(2));
		}

		[Test]
		public void RaiseUntil_AbortableChain_NotHandled_AllCallbacksExecuted()
		{
			int numberOfHandlersExecuted = 0;
			_abortable += (handler, e) =>
			{
				numberOfHandlersExecuted++;
				return e.Value = "1";
			};
			_abortable += (handler, e) =>
			{
				numberOfHandlersExecuted++;
				return e.Value = "2";
			};
			_abortable += (handler, e) =>
			{
				numberOfHandlersExecuted++;
				return e.Value = "3";
			};

			var args = new MutableValueEventArgs<string>();

			bool result = _abortable.RaiseUntil(this, args, e => e.Equals("5", StringComparison.OrdinalIgnoreCase));

			Assert.That(result, Is.False);
			Assert.That(args.Value, Is.EqualTo("3"), "value of the last callback");
			Assert.That(numberOfHandlersExecuted, Is.EqualTo(3));
		}

		#endregion
	}
}
