using System;
using System.Diagnostics;
using System.Xml;

namespace openSourceC.FrameworkLibrary.ServiceProcess
{
	/// <summary>
	///		Summary description of IServiceProcessorBase.
	/// </summary>
	public interface IServiceProcessor
	{
		#region Execute

		/// <summary>
		///		The method to execute.
		/// </summary>
		void Execute();

		#endregion
	}
}
