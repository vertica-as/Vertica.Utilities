using System;

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
	}
}
