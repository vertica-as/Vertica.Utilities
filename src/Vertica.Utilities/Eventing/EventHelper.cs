﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Vertica.Utilities.Reflection;

namespace Vertica.Utilities.Eventing
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

		public static IDisposable Observing(this INotifyPropertyChanged notify, PropertyChangedEventHandler handler)
		{
			notify.PropertyChanged += handler;

			return new DisposableAction(() => notify.PropertyChanged -= handler);
		}
	}
}
