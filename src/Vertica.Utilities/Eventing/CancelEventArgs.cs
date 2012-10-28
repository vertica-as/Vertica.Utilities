using System;

namespace Vertica.Utilities.Eventing
{
	public class CancelEventArgs : EventArgs, ICancelEventArgs
	{
		public bool IsCanceled { get; private set; }

		public void Cancel()
		{
			IsCanceled |= true;
		}
	}
}