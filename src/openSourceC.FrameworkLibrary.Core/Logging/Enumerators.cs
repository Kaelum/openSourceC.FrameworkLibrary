using System;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Identifies the type of message log entry.
	/// </summary>
	[Serializable]
	public enum MessageLogEntryType
	{
		/// <summary>Debugging trace.</summary>
		Debug,

		/// <summary>Recoverable error.</summary>
		Error,

		/// <summary>Fatal error or application crash.</summary>
		Fatal,

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
}
