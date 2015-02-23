using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Xml;

namespace openSourceC.FrameworkLibrary.ServiceProcess
{
	/// <summary>
	///		Summary description for ServiceApplicationBase.
	/// </summary>
	///	<remarks>
	///		DUE TO A BUG IN VISUAL STUDIO, THIS CLASS CAN'T BE MARKED ABSTRACT AS IT SHOULD BE IF
	///		YOU INTEND TO USE THE SERVICE DESIGNER.  ONCE THE BUG IF FIXED, THIS CLASS SHOULD BE
	///		MARKED ABSTRACT.
	///	</remarks>
	public class ServiceApplicationBase : ServiceBase, IServiceApplicationBase
	{
		/// <summary>
		///		Provides messages to subscribers.
		/// </summary>
		public event EventHandler<MessageEventArgs> Message;

		/// <summary>Gets the <see cref="T:OscLog"/> object.</summary>
		protected OscLog Log { get; private set; }


		#region Constructors

		/// <summary>
		///		DO NOT USE THIS CONSTRUCTOR!
		/// </summary>
		///	<remarks>
		///		DUE TO A BUG IN VISUAL STUDIO, THIS CONSTRUCTOR IS REQUIRED IN ORDER FOR THE SERVICE
		///		DESIGNER TO WORK.  ONCE THE BUG IS FIXED, THIS CONSTRUCTOR SHOULD BE DELETED.
		///	</remarks>
		[Obsolete("Use of this constructor is not supported.", true)]
		public ServiceApplicationBase() { }

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="log">The <see cref="T:OscLog"/> log to use.</param>
		public ServiceApplicationBase(OscLog log)
		{
			Log = log;

			// Forward messages from the logger to the Message event.
			Log.Message += new EventHandler<MessageEventArgs>(
				delegate(object sender, MessageEventArgs e)
				{
					string message = e.ToString();

					EventHandler<MessageEventArgs> eventMessageEvent = Message;

					if (eventMessageEvent != null)
					{
						eventMessageEvent(this, new MessageEventArgs(e.LocationInfo, e.MessageLogEntryType, message));
					}

					Debug.WriteLine(message);
				}
			);
		}

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="loggerName">The name of the logger to use.</param>
		public ServiceApplicationBase(string loggerName)
			: this(new OscLog(loggerName)) { }

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="log4netConfigurationXml">The log4net configuration xml element.</param>
		/// <param name="loggerName">The name of the logger to use.</param>
		public ServiceApplicationBase(XmlElement log4netConfigurationXml, string loggerName)
			: this(new OscLog(log4netConfigurationXml, loggerName)) { }

		#endregion

		#region Explicit IServiceApplicationBase Implementations

		/// <summary>
		///		Emulates an <see cref="ServiceBase.OnContinue"/> command being sent to the service by the Service Control
		///		Manager (SCM).
		/// </summary>
		void IServiceApplicationBase.ExecuteOnContinue()
		{
			OnContinue();
		}

		/// <summary>
		///		Emulates an <see cref="ServiceBase.OnCustomCommand"/> command being sent to the service by the Service Control Manager (SCM).
		/// </summary>
		/// <param name="command"></param>
		void IServiceApplicationBase.ExecuteOnCustomCommand(int command)
		{
			OnCustomCommand(command);
		}

		/// <summary>
		///		Emulates an <see cref="ServiceBase.OnPause"/> command being sent to the service by the Service Control Manager (SCM).
		/// </summary>
		void IServiceApplicationBase.ExecuteOnPause()
		{
			OnPause();
		}

		/// <summary>
		///		Emulates an <see cref="ServiceBase.OnPowerEvent"/> command being sent to the service by the Service Control Manager (SCM).
		/// </summary>
		/// <param name="powerStatus"></param>
		/// <returns></returns>
		bool IServiceApplicationBase.ExecuteOnPowerEvent(PowerBroadcastStatus powerStatus)
		{
			return OnPowerEvent(powerStatus);
		}

		/// <summary>
		///		Emulates an <see cref="ServiceBase.OnStop"/> command being sent to the service by the Service Control Manager (SCM).
		/// </summary>
		/// <param name="changeDescription"></param>
		void IServiceApplicationBase.ExecuteOnSessionChange(SessionChangeDescription changeDescription)
		{
			OnSessionChange(changeDescription);
		}

		/// <summary>
		///		Emulates an <see cref="ServiceBase.OnShutdown"/> command being sent to the service by the Service Control Manager (SCM).
		/// </summary>
		void IServiceApplicationBase.ExecuteOnShutdown()
		{
			OnShutdown();
		}

		/// <summary>
		///		Emulates an <see cref="ServiceBase.OnStart"/> command being sent to the service by the Service Control Manager (SCM).
		/// </summary>
		/// <param name="args"></param>
		void IServiceApplicationBase.ExecuteOnStart(string[] args)
		{
			OnStart(args);
		}

		/// <summary>
		///		Emulates an <see cref="ServiceBase.OnStop"/> command being sent to the service by the Service Control Manager (SCM).
		/// </summary>
		void IServiceApplicationBase.ExecuteOnStop()
		{
			OnStop();
		}

		#endregion
	}
}
