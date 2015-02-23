using System;
using System.Diagnostics;
using System.Xml;

namespace openSourceC.FrameworkLibrary.ServiceProcess
{
	/// <summary>
	///		Summary description of ServiceProcessorBase.
	/// </summary>
	///	<remarks>
	///		DUE TO A BUG IN VISUAL STUDIO, THIS CLASS CAN'T BE MARKED ABSTRACT AS IT SHOULD BE IF
	///		YOU INTEND TO USE THE SERVICE DESIGNER.  ONCE THE BUG IF FIXED, THIS CLASS SHOULD BE
	///		MARKED ABSTRACT.
	///	</remarks>
	public class ServiceProcessorBase
	{
		/// <summary>
		///		Provides formatted messages to subscribers, like a ListView control in a WPF window.
		/// </summary>
		public event EventHandler<MessageEventArgs> Message;

		/// <summary>Gets the <see cref="T:OscLog"/> object.</summary>
		protected OscLog Log { get; private set; }


		#region Constructors

		/// <summary>
		///		!!! DO NOT USE !!!
		/// </summary>
		///	<remarks>
		///		DUE TO A BUG IN VISUAL STUDIO, THIS CONSTRUCTOR IS REQUIRED IN ORDER FOR THE SERVICE
		///		DESIGNER TO WORK.  ONCE THE BUG IS FIXED, THIS CONSTRUCTOR SHOULD BE DELETED.
		///	</remarks>
		public ServiceProcessorBase() { }

		/// <summary>
		///		Creates an instance of ServiceProcessorBase using the specified
		///		<see cref="T:OscLog"/> object for messaging.
		/// </summary>
		/// <param name="log">The <see cref="T:OscLog"/> log to use.</param>
		public ServiceProcessorBase(OscLog log)
		{
			Log = log;
			Log.Message += new EventHandler<MessageEventArgs>(OscLog_Message);
		}

		/// <summary>
		///		Creates an instance of ServiceProcessorBase using the specified logger for
		///		messaging.
		/// </summary>
		/// <param name="loggerName">The name of the logger to use.</param>
		public ServiceProcessorBase(string loggerName)
			: this(new OscLog(loggerName)) { }

		/// <summary>
		///		Creates an instance of ServiceProcessorBase using the specified configuration
		///		element and logger for messaging.
		/// </summary>
		/// <param name="log4netConfigurationXml">The log4net configuration xml element.</param>
		/// <param name="loggerName">The name of the logger to use.</param>
		public ServiceProcessorBase(XmlElement log4netConfigurationXml, string loggerName)
			: this(new OscLog(log4netConfigurationXml, loggerName)) { }

		#endregion

		#region Event Handlers

		private void OscLog_Message(object sender, MessageEventArgs e)
		{
			string message = e.ToString();

			EventHandler<MessageEventArgs> eventMessageEvent = Message;

			if (eventMessageEvent != null)
			{
				eventMessageEvent(this, new MessageEventArgs(e.LocationInfo, e.MessageLogEntryType, message));
			}

			Debug.WriteLine(message);
		}

		#endregion
	}
}
