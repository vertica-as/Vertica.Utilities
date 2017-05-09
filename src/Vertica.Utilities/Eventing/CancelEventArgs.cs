using System;

namespace Vertica.Utilities.Eventing
{
	public class CancelEventArgs : EventArgs, ICancelEventArgs
	{
		public bool IsCancelled { get; private set; }

		public void Cancel()
		{
			IsCancelled = true;
		}
	}
}