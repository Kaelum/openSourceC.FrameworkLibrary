using System;
using System.Diagnostics;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Summary description for MessageEventArgs.
	/// </summary>
	[Serializable]
	public class MessageEventArgs : EventArgs
	{
		/// <summary>Gets the calling method.</summary>
		public LocationInfo LocationInfo { get; private set; }

		/// <summary>Gets the event type.</summary>
		public LogEventType EventType { get; private set; }

		/// <summary>Gets the message.</summary>
		public string Message { get; private set; }


		#region Constructors

		/// <summary>
		///		Constructor.
		/// </summary>
		/// <param name="locationInfo"></param>
		/// <param name="eventType"></param>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public MessageEventArgs(LocationInfo locationInfo, LogEventType eventType, IFormatProvider provider, string format, params object[] args)
			: this(locationInfo, eventType, string.Format(provider, format, args)) { }

		/// <summary>
		///		Constructor.
		/// </summary>
		/// <param name="locationInfo"></param>
		/// <param name="eventType"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public MessageEventArgs(LocationInfo locationInfo, LogEventType eventType, string format, params object[] args)
			: this(locationInfo, eventType, null, format, args) { }

		/// <summary>
		///		Constructor.
		/// </summary>
		/// <param name="locationInfo"></param>
		/// <param name="eventType"></param>
		/// <param name="message"></param>
		public MessageEventArgs(LocationInfo locationInfo, LogEventType eventType, string message)
		{
			LocationInfo = locationInfo;
			EventType = eventType;
			Message = message;
		}

		/// <summary>
		///		Constructor.
		/// </summary>
		/// <param name="callerType"></param>
		/// <param name="eventType"></param>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public MessageEventArgs(Type callerType, LogEventType eventType, IFormatProvider provider, string format, params object[] args)
			: this(callerType, eventType, string.Format(provider, format, args)) { }

		/// <summary>
		///		Constructor.
		/// </summary>
		/// <param name="callerType"></param>
		/// <param name="eventType"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public MessageEventArgs(Type callerType, LogEventType eventType, string format, params object[] args)
			: this(callerType, eventType, null, format, args) { }

		/// <summary>
		///		Constructor.
		/// </summary>
		/// <param name="callerType"></param>
		/// <param name="eventType"></param>
		/// <param name="message"></param>
		public MessageEventArgs(Type callerType, LogEventType eventType, string message)
			: this(new LocationInfo(callerType), eventType, message) { }

		#endregion
	}
}
