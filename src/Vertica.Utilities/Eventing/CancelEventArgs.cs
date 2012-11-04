using System;

namespace Vertica.Utilities_v4.Eventing
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