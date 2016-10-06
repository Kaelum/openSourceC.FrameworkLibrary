using System;
using System.Windows;

namespace openSourceC.FrameworkLibrary.Windows
{
	/// <summary>
	///		Pop-up exception MessageBox.
	/// </summary>
	public class ExceptionBox
	{
		// Disables external instantiation of this object.
		private ExceptionBox() { }

		/// <summary>
		///		Displays a message box for an <code>OSCException</code> based exception.
		/// </summary>
		/// <param name="ex">The exception to display.</param>
		/// <returns>One of the <code>DialogResult</code> values.</returns>
		public static MessageBoxResult Show(Exception ex)
		{
			// Be very careful by putting a Try/Catch around the entire routine.
			// We should never throw an exception while displaying an exception.
			try
			{
				return Show(ex, null);
			}
			catch
			{
				try { return MessageBox.Show("An unexpected exception has occurred while trying to display an exception.", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
				catch { return MessageBoxResult.None; }
			}
		}

		/// <summary>
		///		Displays a message box for an <see cref="OscException"/> based exception.
		/// </summary>
		/// <param name="ex">The exception to display.</param>
		/// <param name="message">Additional message to display if the exception is not derived from <code>OSCException</code>.</param>
		/// <returns>One of the <see cref="MessageBoxResult"/> values.</returns>
		public static MessageBoxResult Show(Exception ex, string message)
		{
			// Be very careful by putting a Try/Catch around the entire routine.
			// We should never throw an exception while displaying an exception.
			try
			{
				if (ex is OscErrorException)
				{
#if DEBUG
					return MessageBox.Show(string.Format("{0}{2}{2}{1}", ex.Message, Format.Exception(ex.InnerException), Environment.NewLine), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
#else
					return MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
				}
				else if (ex is OscFailureAuditException)
				{
#if DEBUG
					return MessageBox.Show(string.Format("{0}{2}{2}{1}", ex.Message, Format.Exception(ex.InnerException), Environment.NewLine), "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
#else
					return MessageBox.Show(ex.Message, "Failure Audit", MessageBoxButton.OK, MessageBoxImage.Hand);
#endif
				}
				else if (ex is OscInformationException)
				{
#if DEBUG
					return MessageBox.Show(string.Format("{0}{2}{2}{1}", ex.Message, Format.Exception(ex.InnerException), Environment.NewLine), "Error", MessageBoxButton.OK, MessageBoxImage.Information);
#else
					return MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
#endif
				}
				else if (ex is OscSuccessAuditException)
				{
#if DEBUG
					return MessageBox.Show(string.Format("{0}{2}{2}{1}", ex.Message, Format.Exception(ex.InnerException), Environment.NewLine), "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
#else
					return MessageBox.Show(ex.Message, "Success Audit", MessageBoxButton.OK, MessageBoxImage.Asterisk);
#endif
				}
				else if (ex is OscWarningException)
				{
#if DEBUG
					return MessageBox.Show(string.Format("{0}{2}{2}{1}", ex.Message, Format.Exception(ex.InnerException), Environment.NewLine), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
#else
					return MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
#endif
				}
				else
				{
#if DEBUG
					string messageFormat;


					if (message == null || message == string.Empty)
					{
						messageFormat = "{1}";
					}
					else
					{
						messageFormat = "{0}{2}{2}{1}";
					}

					return MessageBox.Show(string.Format(messageFormat, message, Format.Exception(ex, message), Environment.NewLine), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
#else
					return MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
				}
			}
			catch
			{
				try { return MessageBox.Show("An unexpected exception has occurred while trying to display an exception.", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
				catch { return MessageBoxResult.None; }
			}
		}
	}
}
