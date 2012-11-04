using System;

namespace Vertica.Utilities_v4.Eventing
{
	public delegate void EventHandler<in TEventArgs, in TSender>(TSender sender, TEventArgs e) where TEventArgs : EventArgs;
}
