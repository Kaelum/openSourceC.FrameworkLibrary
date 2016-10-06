using System;

namespace openSourceC.FrameworkLibrary.ServiceProcess
{
	/// <summary>
	///		Provides an API for application access to Windows service events.
	/// </summary>
	public interface IServiceApplication : IServiceApplicationBase
	{
		#region Service Properties

		/// <summary>Gets a value indicating that the processors are closing.</summary>
		bool CloseProcessors { get; }

		/// <summary>Gets a value indicating that the service is paused.</summary>
		bool IsPaused { get; }

		/// <summary>Gets a value indicating that the service is running.</summary>
		bool IsRunning { get; }

		/// <summary>Gets a value indicating that the processors are pausing.</summary>
		bool PauseProcessors { get; }

		#endregion
	}
}
