using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Vertica.Utilities_v4.Reflection;

namespace Vertica.Utilities_v4.Eventing
{
	public static class EventHelper
	{
		public static void Raise(this EventHandler handler, object sender, EventArgs e)
		{
			EventHandler copy = handler;
			if (copy != null)
			{
				copy(sender, e);
			}
		}

		public static void Raise<TEventArgs>(this EventHandler<TEventArgs> handler,
			object sender, TEventArgs e) where TEventArgs : EventArgs
		{
			EventHandler<TEventArgs> copy = handler;
			if (copy != null)
			{
				copy(sender, e);
			}
		}

		public static void Raise<TEventArgs, TSender>(this EventHandler<TEventArgs> handler,
			TSender sender, TEventArgs e) where TEventArgs : EventArgs
		{
			EventHandler<TEventArgs> copy = handler;
			if (copy != null)
			{
				copy(sender, e);
			}
		}

		public static void Raise(this PropertyChangedEventHandler handler, object sender, string propertyName)
		{
			PropertyChangedEventHandler copy = handler;
			if (copy != null)
			{
				copy(sender, new PropertyChangedEventArgs(propertyName));
			}
		}

		public static void Raise(this PropertyChangingEventHandler handler, object sender, string propertyName)
		{
			PropertyChangingEventHandler copy = handler;
			if (copy != null)
			{
				copy(sender, new PropertyChangingEventArgs(propertyName));
			}
		}

		public static void Notify<T, TValue>(
			this T instance,
			PropertyChangedEventHandler handler,
			Expression<Func<T, TValue>> selector,
			TValue oldValue = default(TValue),
			TValue newValue = default(TValue))
			where T : INotifyPropertyChanged
		{
			if (handler == null) return;

			handler(instance, new PropertyValueChangedEventArgs<TValue>(Name.Of(selector), oldValue, newValue));
		}

		public static bool Notify<T, TValue>(
			this T instance,
			PropertyChangingEventHandler handler,
			Expression<Func<T, TValue>> selector,
			TValue oldValue = default(TValue),
			TValue newValue = default(TValue))
			where T : INotifyPropertyChanging
		{
			if (handler == null) return false;
			var args = new PropertyValueChangingEventArgs<TValue>(Name.Of(selector), oldValue, newValue);
			handler(instance, args);
			return args.IsCancelled;
		}

		public static IDisposable Observed(this INotifyPropertyChanged notify, PropertyChangedEventHandler handler)
		{
			notify.PropertyChanged += handler;

			return new DisposableAction(() => notify.PropertyChanged -= handler);
		}

		public static IDisposable Observing(this INotifyPropertyChanging notify, PropertyChangingEventHandler handler)
		{
			notify.PropertyChanging += handler;

			return new DisposableAction(() => notify.PropertyChanging -= handler);
		}

		/// <summary>
		/// Fires each event in the invocation list in the order in which
		/// the events were added until an event handler sets the handled
		/// property to true.
		/// Any exception that the event throws must be caught by the caller.
		/// </summary>
		/// <param name="delegates">The multicast delegate (event).</param>
		/// <param name="sender">The event source instance.</param>
		/// <param name="arg">The event argument.</param>
		/// <returns>Returns true if an event sink handled the event,
		/// false otherwise.</returns>
		public static bool Raise<T>(this ChainedEventHandler<T> delegates, object sender, T arg) where T : ChainedEventArgs
		{
			bool handled = false;
			// Assuming the multicast delegate is not null...
			if (delegates != null)
			{
				// Call the methods until one of them handles the event
				// or all the methods in the delegate list are processed.
				Delegate[] invocationList = delegates.GetInvocationList();

				for (int i = 0; i < invocationList.Length && !handled; i++)
				{
					((ChainedEventHandler<T>)invocationList[i])(sender, arg);
					handled = arg.Handled;
				}
			}
			// Return a flag indicating whether an event sink handled
			// the event.
			return handled;
		}

		public static bool RaiseUntil<T, K>(ValueEventHandler<T, K> delegates, object sender, T args, Predicate<K> predicate) where T : ValueEventArgs<K>
		{
			bool handled = false;

			if (delegates != null)
			{
				Delegate[] invocationList = delegates.GetInvocationList();
				for (int i = 0; i < invocationList.Length && !handled; i++)
				{
					K result = ((ValueEventHandler<T, K>)invocationList[i])(sender, args);
					handled = predicate(result);
				}
			}
			return handled;
		}
	}
}
