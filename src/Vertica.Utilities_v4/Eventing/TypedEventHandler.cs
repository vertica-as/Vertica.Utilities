using System;

namespace Vertica.Utilities_v4.Eventing
{
	public delegate void EventHandler<in TEventArgs, in TSender>(TSender sender, TEventArgs e) where TEventArgs : EventArgs;

	public delegate void ChainedEventHandler<in T>(object sender, T e) where T : ChainedEventArgs;

	public delegate K ChainedEventHandler<in T, out K>(object sender, T e) where T : IMutableValueEventArgs<K>;
}
