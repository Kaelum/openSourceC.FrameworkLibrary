using System;

namespace openSourceC.FrameworkLibrary.Extensions
{
	/// <summary>
	///		Summary description for NullTestExtensions.
	/// </summary>
	public static class NullTestExtensions
	{
		/// <summary>
		///		Throw <see cref="T:ArgumentNullException"/> with the specified parameter name, if
		///		<b>source</b> is null, 
		/// </summary>
		/// <param name="source">The extension object.</param>
		/// <param name="paramName">The name of the parameter that caused the exception.</param>
		public static void ThrowIfNull(this object source, string paramName)
		{
			if (source == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		/// <summary>
		///		Throw <see cref="T:ArgumentNullException"/> with the specified parameter name and
		///		message, if <b>source</b> is null, 
		/// </summary>
		/// <param name="source">The extension object.</param>
		/// <param name="paramName">The name of the parameter that caused the exception.</param>
		/// <param name="message">A message that describes the error.</param>
		public static void ThrowIfNull(this object source, string paramName, string message)
		{
			if (source == null)
			{
				throw new ArgumentNullException(paramName, message);
			}
		}
	}
}
