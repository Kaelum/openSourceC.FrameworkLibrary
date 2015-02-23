using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Xml.Serialization;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Summary description for EventLogEvent.
	/// </summary>
	[Serializable]
	[XmlType("event")]
	public class EventLogEvent
	{
		#region Class Constructors

		/// <summary>
		///		Create an event log event.
		/// </summary>
		public EventLogEvent() { }

		/// <summary>
		///     Create an event log event for an exception.
		/// </summary>
		/// <param name="e">The Exception object to format.</param>
		public EventLogEvent(Exception e)
			: this(e, null) { }

		/// <summary>
		///     Create an event log event for an exception.
		/// </summary>
		/// <param name="e">The Exception object to format.</param>
		/// <param name="catchMessage">The string to prepend to the exception information.</param>
		public EventLogEvent(Exception e, string catchMessage)
			: this(e, catchMessage, null) { }

		/// <summary>
		///     Create an event log event for an exception.
		/// </summary>
		/// <param name="e">The Exception object to format.</param>
		/// <param name="catchMessage">The string to prepend to the exception information.</param>
		/// <param name="UserID">The UserID of the current user.</param>
		public EventLogEvent(Exception e, string catchMessage, Guid? UserID)
			: this(e, catchMessage, UserID, 0, 0) { }

		/// <summary>
		///     Create an event log event for an exception.
		/// </summary>
		/// <param name="e">The Exception object to format.</param>
		/// <param name="catchMessage">The string to prepend to the exception information.</param>
		/// <param name="category">The category of the event.  Designed for use with the Windows Event Log.</param>
		/// <param name="eventID">The ID of the event.  Designed for use with the Windows Event Log.</param>
		public EventLogEvent(Exception e, string catchMessage, short? category, int? eventID)
			: this(e, catchMessage, null, category, eventID) { }

		/// <summary>
		///     Create an event log event for an exception.
		/// </summary>
		/// <param name="e">The Exception object to format.</param>
		/// <param name="catchMessage">The string to prepend to the exception information.</param>
		/// <param name="userID">The UserID of the current user.</param>
		/// <param name="category">The category of the event.  Designed for use with the Windows Event Log.</param>
		/// <param name="eventID">The ID of the event.  Designed for use with the Windows Event Log.</param>
		public EventLogEvent(Exception e, string catchMessage, Guid? userID, short? category, int? eventID)
			: this(null, null, e.Source, userID, category, eventID, EventLogEntryType.Error)
		{
			// Be very careful by putting a Try/Catch around the entire routine.
			// We should never throw an exception while logging.
			try
			{
				string message = Format.Exception(e, catchMessage);
				string extendedMessage = (e is OscException ? ((OscException)e).ExtendedMessage : null);
				byte[] data;

				if (string.IsNullOrEmpty(extendedMessage))
				{
					data = null;
				}
				else if (message.Length + extendedMessage.Length < 32000)
				{
					StringBuilder sb = new StringBuilder(message);

					if (!message.EndsWith(Environment.NewLine))
					{
						sb.Append(Environment.NewLine);
					}

					sb.Append(Environment.NewLine).Append(extendedMessage);

					message = sb.ToString();
					data = null;
				}
				else
				{
					data = Encoding.UTF8.GetBytes(extendedMessage);
				}

				Message = message;
				Data = data;
				HelpLink = e.HelpLink;
			}
			catch (Exception ex)
			{
				// Ignore any exceptions.
				Debug.WriteLine(ex);
			}
		}

		///// <summary>
		/////     Create an event log event from a <see cref="T:MessageEventArgs"/> object.
		///// </summary>
		///// <param name="messageEventArgs">The message event args object.</param>
		//public EventLogEvent(MessageEventArgs messageEventArgs)
		//	: this(messageEventArgs, null) { }

		///// <summary>
		/////     Create an event log event from a <see cref="T:MessageEventArgs"/> object.
		///// </summary>
		///// <param name="messageEventArgs">The message event args object.</param>
		///// <param name="userID">The UserID of the current user.</param>
		//public EventLogEvent(MessageEventArgs messageEventArgs, Guid? userID)
		//	: this(messageEventArgs, userID, 0, 0) { }

		///// <summary>
		/////     Create an event log event from a <see cref="T:MessageEventArgs"/> object.
		///// </summary>
		///// <param name="messageEventArgs">The message event args object.</param>
		///// <param name="category">The category of the event.  Designed for use with the Windows Event Log.</param>
		///// <param name="eventID">The ID of the event.  Designed for use with the Windows Event Log.</param>
		//public EventLogEvent(MessageEventArgs messageEventArgs, short? category, int? eventID)
		//	: this(messageEventArgs, null, category, eventID) { }

		///// <summary>
		/////     Create an event log event from a <see cref="T:MessageEventArgs"/> object.
		///// </summary>
		///// <param name="messageEventArgs">The message event args object.</param>
		///// <param name="userID">The UserID of the current user.</param>
		///// <param name="category">The category of the event.  Designed for use with the Windows Event Log.</param>
		///// <param name="eventID">The ID of the event.  Designed for use with the Windows Event Log.</param>
		//public EventLogEvent(MessageEventArgs messageEventArgs, Guid? userID, short? category, int? eventID)
		//	: this(messageEventArgs.Message, null, null, userID, category, eventID, GetEntryType(messageEventArgs.MessageLogEntryType))
		//{
		//	LocationInfo = messageEventArgs.LocationInfo;
		//	MessageEntryType = messageEventArgs.MessageLogEntryType;
		//}

		/// <summary>
		///     Create an event log event.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="eventLogEntryType">The event log entry type.</param>
		public EventLogEvent(string message, EventLogEntryType eventLogEntryType)
			: this(message, null, eventLogEntryType) { }

		/// <summary>
		///     Create an event log event.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="data">The data.</param>
		/// <param name="eventLogEntryType">The event log entry type.</param>
		public EventLogEvent(string message, byte[] data, EventLogEntryType eventLogEntryType)
			: this(message, data, null, eventLogEntryType) { }

		/// <summary>
		///     Create an event log event.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="data">The data.</param>
		/// <param name="userID">The UserID of the current user.</param>
		/// <param name="eventLogEntryType">The event log entry type.</param>
		public EventLogEvent(string message, byte[] data, Guid? userID, EventLogEntryType eventLogEntryType)
			: this(message, data, (string)null, userID, eventLogEntryType) { }

		/// <summary>
		///     Create an event log event.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="data">The data.</param>
		/// <param name="category">The category of the event.  Designed for use with the Windows Event Log.</param>
		/// <param name="eventID">The ID of the event.  Designed for use with the Windows Event Log.</param>
		/// <param name="eventLogEntryType">The event log entry type.</param>
		public EventLogEvent(string message, byte[] data, short? category, int? eventID, EventLogEntryType eventLogEntryType)
			: this(message, data, (string)null, category, eventID, eventLogEntryType) { }

		/// <summary>
		///     Create an event log event.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="data">The data.</param>
		/// <param name="userID">The UserID of the current user.</param>
		/// <param name="category">The category of the event.  Designed for use with the Windows Event Log.</param>
		/// <param name="eventID">The ID of the event.  Designed for use with the Windows Event Log.</param>
		/// <param name="eventLogEntryType">The event log entry type.</param>
		public EventLogEvent(string message, byte[] data, Guid? userID, short? category, int? eventID, EventLogEntryType eventLogEntryType)
			: this(message, data, (string)null, userID, category, eventID, eventLogEntryType) { }

		/// <summary>
		///     Create an event log event.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="data">The data.</param>
		/// <param name="source">The name of the application or object creating the event.</param>
		/// <param name="userID">The UserID of the current user.</param>
		/// <param name="eventLogEntryType">The event log entry type.</param>
		public EventLogEvent(string message, byte[] data, string source, Guid? userID, EventLogEntryType eventLogEntryType)
			: this(message, data, source, userID, 0, 0, eventLogEntryType) { }

		/// <summary>
		///     Create an event log event.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="data">The data.</param>
		/// <param name="source">The name of the application or object creating the event.</param>
		/// <param name="category">The category of the event.  Designed for use with the Windows Event Log.</param>
		/// <param name="eventID">The ID of the event.  Designed for use with the Windows Event Log.</param>
		/// <param name="eventLogEntryType">The event log entry type.</param>
		public EventLogEvent(string message, byte[] data, string source, short? category, int? eventID, EventLogEntryType eventLogEntryType)
			: this(message, data, source, null, category, eventID, eventLogEntryType) { }

		/// <summary>
		///     Create an event log event.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="data">The data.</param>
		/// <param name="source">The name of the application or object creating the event.</param>
		/// <param name="userID">The UserID of the current user.</param>
		/// <param name="category">The category of the event.  Designed for use with the Windows Event Log.</param>
		/// <param name="eventID">The ID of the event.  Designed for use with the Windows Event Log.</param>
		/// <param name="eventLogEntryType">The event log entry type.</param>
		public EventLogEvent(string message, byte[] data, string source, Guid? userID, short? category, int? eventID, EventLogEntryType eventLogEntryType)
		{
			// Be very careful by putting a Try/Catch around the entire routine.
			// We should never throw an exception while logging.
			try
			{
				Assembly assembly = Assembly.GetEntryAssembly();
				AssemblyName assemblyName = assembly.GetName();
				GuidAttribute guidAttribute = assembly.GetCustomAttribute(typeof(GuidAttribute)) as GuidAttribute;
				WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();

				EventLogId = null;
				ApplicationId = (guidAttribute == null ? (Guid?)null : new Guid(guidAttribute.Value));
				LogName = "Application";
				Source = (string.IsNullOrWhiteSpace(source) ? assemblyName.Name : source);
				CreateDate = DateTimeOffset.Now;
				EventId = eventID;
				CategoryId = category;
				EntryType = eventLogEntryType;
				LocationInfo = null;
				Message = message;
				Data = data;
				MachineName = Environment.MachineName;
				OSVersion = Environment.OSVersion.VersionString;
				ApplicationName = assemblyName.Name;
				ApplicationVersion = assemblyName.Version.ToString();
				WindowsIdentityName = windowsIdentity.Name;
				ClientIPAddress = null;
				RequestUrl = null;
				RequestData = null;
				UserId = userID;
				HelpLink = null;
			}
			catch (Exception ex)
			{
				// Ignore any exceptions.
				Debug.WriteLine(ex);
			}
		}

		#endregion

		#region Public Properties

		/// <summary>Gets or sets the application identifier.</summary>
		[XmlElement("applicationId")]
		public Guid? ApplicationId { get; set; }

		/// <summary>Gets or sets the name of the application.</summary>
		[XmlElement("applicationName")]
		public string ApplicationName { get; set; }

		/// <summary>Gets or sets the version of the application.</summary>
		[XmlElement("applicationVersion")]
		public string ApplicationVersion { get; set; }

		/// <summary>Gets or sets category identifier.</summary>
		[XmlElement("categoryId")]
		public short? CategoryId { get; set; }

		/// <summary>Gets or sets the IP address of the client.</summary>
		[XmlElement("clientIPAddress")]
		public string ClientIPAddress { get; set; }

		/// <summary>Gets or sets the date and time of the event.</summary>
		[XmlElement("createDate")]
		public DateTimeOffset? CreateDate { get; set; }

		/// <summary>Gets or sets the binary data storage array.</summary>
		[XmlArray("data")]
		public byte[] Data { get; set; }

		/// <summary>Gets or sets the event log entry type.</summary>
		[XmlElement("entryType")]
		public EventLogEntryType EntryType { get; set; }

		/// <summary>Gets or sets the event identifier.</summary>
		[XmlElement("eventId")]
		public int? EventId { get; set; }

		/// <summary>Gets or sets current instance identifier.</summary>
		[XmlElement("eventLogId")]
		public Guid? EventLogId { get; set; }

		/// <summary>Gets or sets the help link.</summary>
		[XmlElement("helpLink")]
		public string HelpLink { get; set; }

		/// <summary>Gets or sets the location information.</summary>
		[XmlElement("locationInfo")]
		public LocationInfo LocationInfo { get; set; }

		/// <summary>Gets or sets the name of the log.</summary>
		[XmlElement("logName")]
		public string LogName { get; set; }

		/// <summary>Gets or sets the name of the machine.</summary>
		[XmlElement("machine")]
		public string MachineName { get; set; }

		/// <summary>Gets or sets the message.</summary>
		[XmlElement("message")]
		public string Message { get; set; }

		/// <summary>Gets or sets the version of the operating system.</summary>
		[XmlElement("osVersion")]
		public string OSVersion { get; set; }

		/// <summary>Gets or sets the request contents (i.e. POST or PUT contents).</summary>
		[XmlElement("requestData")]
		public byte[] RequestData { get; set; }

		/// <summary>Gets or sets the request url.</summary>
		[XmlElement("requestUrl")]
		public string RequestUrl { get; set; }

		/// <summary>Gets or sets the name of the source.</summary>
		[XmlElement("source")]
		public string Source { get; set; }

		/// <summary>Gets or sets identifier of the current user.</summary>
		[XmlElement("userId")]
		public Guid? UserId { get; set; }

		/// <summary>Gets or sets user name of the account that the application is running under.</summary>
		[XmlElement("windowsIdentityName")]
		public string WindowsIdentityName { get; set; }

		#endregion

		#region Public Methods

		/// <summary>
		///		Converts a <see cref="T:MessageLogEntryType"/> to an <see cref="T:EventLogEntryType"/>.
		/// </summary>
		/// <param name="messageLogEntryType">The message log entry type.</param>
		/// <returns>
		///		An <see cref="T:EventLogEntryType"/>.
		/// </returns>
		public static EventLogEntryType GetEventLogEntryType(MessageLogEntryType messageLogEntryType)
		{
			switch (messageLogEntryType)
			{
				case MessageLogEntryType.Debug:
				{
					return EventLogEntryType.Information;
				}

				case MessageLogEntryType.Error:
				{
					return EventLogEntryType.Error;
				}

				case MessageLogEntryType.Fatal:
				{
					return EventLogEntryType.Error;
				}

				case MessageLogEntryType.Information:
				{
					return EventLogEntryType.Information;
				}

				case MessageLogEntryType.Warning:
				{
					return EventLogEntryType.Warning;
				}

				default:
				{
					return EventLogEntryType.Information;
				}
			}
		}

		#endregion
	}
}
