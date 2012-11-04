using System;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Identifies the type of event that has caused the message.
	/// </summary>
	[Serializable]
	public enum LogEventType
	{
		/// <summary>Fatal error or application crash.</summary>
		Critical,

		/// <summary>Debugging trace.</summary>
		Debug,

		/// <summary>Recoverable error.</summary>
		Error,

		/// <summary>Informational message.</summary>
		Information,

		///// <summary>Resumption of a logical operation.</summary>
		//Resume,

		///// <summary>Starting of a logical operation.</summary>
		//Start,

		///// <summary>Stopping of a logical operation.</summary>
		//Stop,

		///// <summary>Suspension of a logical operation.</summary>
		//Suspend,

		///// <summary>Changing of correlation identity.</summary>
		//Transfer,

		/// <summary>Noncritical problem.</summary>
		Warning,
	}

	/// <summary>
	///		Specifies the message level to log.
	/// </summary>
	[Serializable]
	public enum LogLevel
	{
		/// <summary>Do not log any messages.</summary>
		Off = 0,

		/// <summary>Log error-handling messages.</summary>
		Error = 1,

		/// <summary>Log warnings and error-handling messages.</summary>
		Warning = 2,

		/// <summary>Log informational messages, warnings, and error-handling messages.</summary>
		Info = 3,

		/// <summary>Log all debugging and tracing messages.</summary>
		Verbose = 4,
	}
}
