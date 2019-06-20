using System;
using System.Diagnostics;
using System.Xml;

namespace openSourceC.FrameworkLibrary.ServiceProcess
{
	/// <summary>
	///		Summary description of IServiceProcessorBase.
	/// </summary>
	public interface IServiceProcessorBase
	{
		#region Service Processor Events

		/// <summary>
		///		Provides formatted messages to subscribers, like a ListView control in a WPF window.
		/// </summary>
		event EventHandler<MessageEventArgs> Message;

		#endregion
	}
}
