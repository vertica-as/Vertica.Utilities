using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Vertica.Utilities_v4.Security
{
	public class Impersonate
	{
		private readonly IWindowsIdentityProvider _provider;

		internal Impersonate(IWindowsIdentityProvider provider)
		{
			_provider = provider;
		}

		public static Impersonate Using(IWindowsIdentityProvider provider)
		{
			return new Impersonate(provider);
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool CloseHandle(IntPtr handle);
		public void Do(Action action)
		{
			using (_provider)
			{
				using (WindowsIdentity id = _provider.GetWindowsIdentity())
				{
					using (WindowsImpersonationContext wic = id.Impersonate())
					{
						try
						{
							action();
						}
						finally
						{
							wic.Undo();
							CloseHandle(id.Token);
						}
					}
				}
			}
		}
	}
}