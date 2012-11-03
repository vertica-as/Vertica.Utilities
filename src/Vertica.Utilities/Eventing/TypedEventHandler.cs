using System;

namespace Vertica.Utilities.Eventing
{
	public delegate void EventHandler<in TEventArgs, in TSender>(TSender sender, TEventArgs e) where TEventArgs : EventArgs;
}
