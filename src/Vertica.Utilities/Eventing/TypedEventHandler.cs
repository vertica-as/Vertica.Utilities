using System;

namespace Vertica.Utilities.Eventing
{
	public delegate void EventHandler<in TEventArgs, in TSender>(TSender sender, TEventArgs e) where TEventArgs : EventArgs;

	public delegate void ChainedEventHandler<in T>(object sender, T e) where T : IChainedEventArgs;

	public delegate K ChainedEventHandler<in T, out K>(object sender, T e) where T : IMutableValueEventArgs<K>;
}
