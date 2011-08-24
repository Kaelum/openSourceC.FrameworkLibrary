using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;

namespace openSourceC.FrameworkLibrary.ServiceProcess
{
	/// <summary>
	///		Summary description for ServiceApplicationBase.
	/// </summary>
	public class ServiceApplicationBase : ServiceBase, IServiceApplicationBase
	{
		/// <summary>
		///		Provides status messages to subscribers.
		/// </summary>
		public event EventHandler<StatusMessageEventArgs> StatusMessage;


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

		#region Protected Methods

		/// <summary>
		///		
		/// </summary>
		/// <param name="statusMessage"></param>
		protected void OnStatusMessage(string statusMessage)
		{
			if (statusMessage == null)
			{
				throw new ArgumentNullException("statusMessage");
			}

			EventHandler<StatusMessageEventArgs> statusMessageEvent = StatusMessage;

			if (statusMessageEvent != null)
			{
				statusMessageEvent(this, new StatusMessageEventArgs(statusMessage));
			}

			Debug.WriteLine(statusMessage);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		protected void OnStatusMessage(string format, params object[] args)
		{
			if ((format == null) || (args == null))
			{
				throw new ArgumentNullException((format == null) ? "format" : "args");
			}

			OnStatusMessage(string.Format(format, args));
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="callingMethod"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		protected void OnStatusMessage(MethodBase callingMethod, string format, params object[] args)
		{
			if (callingMethod == null)
			{
				throw new ArgumentNullException("callingMethod");
			}

			if ((format == null) || (args == null))
			{
				throw new ArgumentNullException((format == null) ? "format" : "args");
			}

			OnStatusMessage(string.Format("{0:MM/dd/yyyy HH:mm:ss tt}: {1}.{2}: {3}", DateTime.Now, callingMethod.ReflectedType.Name, callingMethod.Name, string.Format(format, args)));
		}

		#endregion
	}
}
