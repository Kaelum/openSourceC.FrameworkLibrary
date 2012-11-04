using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace openSourceC.FrameworkLibrary.ServiceProcess
{
	/// <summary>
	///		Summary description for ServiceApplicationBase.
	///		<para>Due to a bug in Visual Studio, this class can't be marked abstract as it should
	///		be if you intend to use the service designer.  Once the bug if fixed, this class
	///		should be marked abstract.</para>
	/// </summary>
	public class ServiceApplicationBase : ServiceBase, IServiceApplicationBase
	{
		/// <summary>
		///		Provides messages to subscribers.
		/// </summary>
		public event EventHandler<MessageEventArgs> Message;

		/// <summary>
		///		Gets the <see cref="T:OscLogger"/> object.
		/// </summary>
		protected OscLogger Log { get; private set; }


		#region Constructors

		/// <summary>
		///		Constructor
		///		<para>Due to a bug in Visual Studio, this constructor is required in order for the
		///		service designer to work.  Once the bug is fixed, this constructor should be
		///		deleted.</para>
		/// </summary>
		public ServiceApplicationBase() { }

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="log">The <see cref="T:OscLogger"/> logger to use.</param>
		public ServiceApplicationBase(OscLogger log)
		{
			Log = log;
			Log.Message += new EventHandler<MessageEventArgs>(OscLogger_Message);
		}

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

		#region Event Handlers

		private void OscLogger_Message(object sender, MessageEventArgs e)
		{
			string message = string.Format("{0:MM/dd/yyyy HH:mm:ss tt}: {1}.{2}: {3}", DateTime.Now, e.LocationInfo.ClassName, e.LocationInfo.MethodName, e.Message);

			EventHandler<MessageEventArgs> eventMessageEvent = Message;

			if (eventMessageEvent != null)
			{
				eventMessageEvent(this, new MessageEventArgs(e.LocationInfo, e.EventType, message));
			}

			Debug.WriteLine(message);
		}

		#endregion
	}
}
