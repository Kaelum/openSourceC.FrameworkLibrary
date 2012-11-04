using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>Leverages Win32API calls to impersonate a domain user.  Allows file operations on remote file systems using UNC paths.</summary>
	public static class Impersonation
	{
		/// <summary>LogonUser Win32API method</summary>
		/// <param name="lpszUsername"></param>
		/// <param name="lpszDomain"></param>
		/// <param name="lpszPassword"></param>
		/// <param name="dwLogonType"></param>
		/// <param name="dwLogonProvider"></param>
		/// <param name="phToken"></param>
		/// <returns>Status Code (non-zero indicates success)</returns>
		[DllImport("advapi32.dll")]
		public static extern int LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

		/// <summary>CloseHandle Win32API method</summary>
		/// <param name="hToken"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll")]
		public extern static bool CloseHandle(IntPtr hToken);

		/// <summary>Stores the ImpersonationContext, which has an undo() method, to end impersonation</summary>
		public static WindowsImpersonationContext ImpersonationContext;

		/// <summary>Wrapper to impersonate a user</summary>
		/// <param name="username">The Windows username</param>
		/// <param name="password">The unencrypted Windows password</param>
		/// <param name="domain">The Windows domain</param>
		/// <returns>true if impersonation was successful, false if not</returns>
		public static bool Impersonate(string username, string password, string domain)
		{
			WindowsIdentity tempWindowsIdentity;
			IntPtr token = IntPtr.Zero;
			IntPtr tokenDuplicate = IntPtr.Zero;

			// request default security provider a logon token with LOGON32_LOGON_NEW_CREDENTIALS,
			// token returned is impersonation token, no need to duplicate
			if (LogonUser(username, domain, password, 9, 0, ref token) != 0)
			{
				tempWindowsIdentity = new WindowsIdentity(token);
				ImpersonationContext = tempWindowsIdentity.Impersonate();

				// Close impersonation token, no longer needed
				CloseHandle(token);
				if (ImpersonationContext != null)
				{
					return true;
				}
			}

			return false; // Failed to impersonate.
		}
	}
}
