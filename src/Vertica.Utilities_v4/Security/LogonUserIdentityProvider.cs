using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Vertica.Utilities_v4.Resources;

namespace Vertica.Utilities_v4.Security
{
	internal class LogonUserIdentityProvider : WindowsIdentityProviderBase
	{
		private readonly Credential _credential;

		internal LogonUserIdentityProvider(Credential credential)
		{
			_credential = credential;
		}

		private const uint LOGON32_LOGON_NETWORK = 3;
		private const uint LOGON32_PROVIDER_DEFAULT = 0;

		[DllImport("advapi32.dll", SetLastError = true)]
		static extern bool LogonUser(
			string principal,
			string authority,
			string password,
			uint logonType,
			uint logonProvider,
			out IntPtr token);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool CloseHandle(IntPtr handle);

		private static IntPtr _token = IntPtr.Zero;

		public override WindowsIdentity GetWindowsIdentity()
		{
			bool result = LogonUser(_credential.UserName, _credential.Domain, _credential.Password,
				LOGON32_LOGON_NETWORK, LOGON32_PROVIDER_DEFAULT, out _token);
			if (!result)
			{
				int error = Marshal.GetLastWin32Error();
				ExceptionHelper.Throw<InvalidOperationException>(Exceptions.LogonUserIdentityProvider_LogonUserErrorTemplate,
					error.ToString(CultureInfo.InvariantCulture),
					_credential.ToString());
			}
			return new WindowsIdentity(_token);
		}

		public override void Dispose()
		{
			base.Dispose();
			CloseHandle(_token);
		}
	}
}