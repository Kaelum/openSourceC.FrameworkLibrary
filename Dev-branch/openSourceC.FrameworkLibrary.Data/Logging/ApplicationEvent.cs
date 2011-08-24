using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using System.Xml.Serialization;

using openSourceC.FrameworkLibrary.Common;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for ApplicationEvent.
	/// </summary>
	[Serializable]
	[XmlType("applicationEvent")]
	public class ApplicationEvent : SimpleItemBase<ApplicationEvent>
	{
		#region Class Constructors

		/// <summary>
		///		Class contructor.
		/// </summary>
		public ApplicationEvent()
		{
		}

		/// <summary>
		///     Writes a message to the application log for an exception.
		/// </summary>
		/// <param name="e">The Exception object to format.</param>
		public ApplicationEvent(Exception e)
			: this(e, null)
		{
		}

		/// <summary>
		///     Writes a message to the application log for an exception.
		/// </summary>
		/// <param name="e">The Exception object to format.</param>
		/// <param name="catchMessage">The string to prepend to the exception information.</param>
		public ApplicationEvent(Exception e, string catchMessage)
			: this(e, catchMessage, null)
		{
		}

		/// <summary>
		///     Writes a message to the application log for an exception.
		/// </summary>
		/// <param name="e">The Exception object to format.</param>
		/// <param name="catchMessage">The string to prepend to the exception information.</param>
		/// <param name="category">The category of the event.  Designed for use with the Windows Event Log.</param>
		/// <param name="eventID">The ID of the event.  Designed for use with the Windows Event Log.</param>
		public ApplicationEvent(Exception e, string catchMessage, short? category, int? eventID)
			: this(e, catchMessage, null, category, eventID)
		{
		}

		/// <summary>
		///     Writes a message to the application log for an exception.
		/// </summary>
		/// <param name="e">The Exception object to format.</param>
		/// <param name="catchMessage">The string to prepend to the exception information.</param>
		/// <param name="UserID">The UserID of the current user.</param>
		public ApplicationEvent(Exception e, string catchMessage, long? UserID)
			: this(e, catchMessage, UserID, 0, 0)
		{
		}

		/// <summary>
		///     Writes a message to the application log for an exception.
		/// </summary>
		/// <param name="e">The Exception object to format.</param>
		/// <param name="catchMessage">The string to prepend to the exception information.</param>
		/// <param name="userID">The UserID of the current user.</param>
		/// <param name="category">The category of the event.  Designed for use with the Windows Event Log.</param>
		/// <param name="eventID">The ID of the event.  Designed for use with the Windows Event Log.</param>
		public ApplicationEvent(Exception e, string catchMessage, long? userID, short? category, int? eventID)
		{
			// Be very careful by putting a Try/Catch around the entire routine.
			// We should never throw an exception while logging.
			try
			{
				WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
				string message = Format.Exception(e, catchMessage);
				string messageExtension = (e is OscException ? ((OscException)e).MessageExtension : null);
				byte[] data;

				if (string.IsNullOrEmpty(messageExtension))
				{
					data = null;
				}
				else if (message.Length + messageExtension.Length < 32000)
				{
					StringBuilder sb = new StringBuilder(message);

					if (!message.EndsWith(Environment.NewLine))
					{
						sb.Append(Environment.NewLine);
					}

					sb.Append(Environment.NewLine).Append(messageExtension);

					message = sb.ToString();
					data = null;
				}
				else
				{
					data = Encoding.UTF8.GetBytes(messageExtension);
				}

				ApplicationEventID = null;
				ApplicationID = null;
				Log = string.Empty;
				Source = string.Empty;
				Type = EventLogEntryType.Error;
				Category = category;
				EventID = eventID;
				Username = windowsIdentity.Name;
				Machine = Environment.MachineName;
				OSVersion = Environment.OSVersion.ToString();
				CatchMessage = catchMessage;
				Message = message;
				Data = data;
				IPAddress = null;
				UserAgent = null;
				HttpReferrer = null;
				HttpVerb = null;
				PathAndQuery = null;
				UserID = userID;
				CreatedDate = null;

				//foreach (DictionaryEntry de in e.Data)
				//{
				//    de.Key;
				//    de.Value;
				//}
			}
			catch (Exception ex)
			{
				// Ignore any exceptions.
				Debug.WriteLine(ex);
			}
		}

		#endregion

		#region Field Properties

		/// <summary>
		///     Gets or sets current instance identifier.
		/// </summary>
		[XmlElement("applicationEventID")]
		public Guid? ApplicationEventID { get; set; }

		/// <summary>Gets or sets the application identifier.</summary>
		[XmlElement("applicationID")]
		public Guid? ApplicationID { get; set; }

		/// <summary>Gets or sets the date and time of the event.</summary>
		[XmlElement("createdDate")]
		public DateTime? CreatedDate { get; set; }

		/// <summary>Gets or sets the catch message.</summary>
		[XmlElement("catchMessage")]
		public string CatchMessage { get; set; }

		/// <summary>Gets or sets category of event for the Event Viewer.</summary>
		[XmlElement("category")]
		public short? Category { get; set; }

		/// <summary>Gets or sets the optional data field for the Event Viewer.</summary>
		[XmlArray("data")]
		public byte[] Data { get; set; }

		/// <summary>Gets or sets the event ID for the Event Viewer.</summary>
		[XmlElement("eventID")]
		public int? EventID { get; set; }

		/// <summary>Gets or sets the referrer (origin) of the current request.</summary>
		[XmlElement("httpReferrer")]
		public string HttpReferrer { get; set; }

		/// <summary>Gets or sets the verb of the current request.</summary>
		[XmlElement("httpVerb")]
		public string HttpVerb { get; set; }

		/// <summary>Gets or sets the IPAddress of the client.</summary>
		[XmlElement("ipAddress")]
		public string IPAddress { get; set; }

		/// <summary>Gets or sets the name of the log.</summary>
		[XmlElement("log")]
		public string Log { get; set; }

		/// <summary>Gets or sets the name of the machine.</summary>
		[XmlElement("machine")]
		public string Machine { get; set; }

		/// <summary>Gets or sets the full message with stack trace.</summary>
		[XmlElement("message")]
		public string Message { get; set; }

		/// <summary>Gets or sets the version of the operating system being used.</summary>
		[XmlElement("osVersion")]
		public string OSVersion { get; set; }

		/// <summary>Gets or sets the path and query for the current request.</summary>
		[XmlElement("pathAndQuery")]
		public string PathAndQuery { get; set; }

		/// <summary>Gets or sets the name of the source.</summary>
		[XmlElement("source")]
		public string Source { get; set; }

		/// <summary>Gets or sets type of event.</summary>
		[XmlElement("type")]
		public EventLogEntryType Type { get; set; }

		/// <summary>Gets or sets the user agent of the client.</summary>
		[XmlElement("userAgent")]
		public string UserAgent { get; set; }

		/// <summary>Gets or sets the User ID of the account logged into the application.</summary>
		[XmlElement("userID")]
		public long? UserID { get; set; }

		/// <summary>Gets or sets username of the user account logged into the machine.</summary>
		[XmlElement("username")]
		public string Username { get; set; }

		#endregion

		#region Public Methods

		/// <summary>
		///		Compares the current object with another object of the same type.
		/// </summary>
		/// <param name="other">The object to compare with this object.</param>
		/// <returns>
		///		A 32-bit signed integer that indicates the relative order of the
		///		comparands. The return value has these meanings:
		///		<list type="bullet">
		///			<item>Less than zero, this instance is less than <paramref name="other"/>.</item>
		///			<item>Zero, this instance is equal to <paramref name="other"/>.</item>
		///			<item>Greater than zero, this instance is greater than <paramref name="other"/>.</item>
		///		</list>
		///	</returns>
		public override int CompareTo(ApplicationEvent other)
		{
			return Nullable.Compare<Guid>(ApplicationEventID, other.ApplicationEventID);
		}

		/// <summary>
		///		Determines whether the committed object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		///		<para>
		///			<b>true</b> if the specified object is equal to the committed object; otherwise <b>false</b>.
		///		</para>
		/// </returns>
		public override bool Equals(ApplicationEvent other)
		{
			return Nullable.Equals<Guid>(ApplicationEventID, other.ApplicationEventID);
		}

		/// <summary>
		///		Returns the hash code for the committed object.
		/// </summary>
		/// <returns>A hash code for the committed object.</returns>
		public override int GetHashCode()
		{
			return ApplicationEventID.GetHashCode();
		}

		/// <summary>
		///		Returns a string that represents this instance.
		/// </summary>
		public override string ToString()
		{
			return ToString(null);
		}

		/// <summary>
		///		Returns a string that represents the contents of this instance.
		/// </summary>
		/// <param name="format">A format string.</param>
		public override string ToString(string format)
		{
			if (format == null)
			{
				return GetHashCode().ToString();
			}

			throw new ArgumentException("Unknown format", "format");
		}

		#endregion
	}

	/// <summary>
	///		Summary description for ApplicationEventList.
	/// </summary>
	[Serializable]
	public class ApplicationEventList : SimpleItemListBase<ApplicationEventList, ApplicationEvent>
	{
		#region Class Constructors

		/// <summary>
		///		Class contructor.
		/// </summary>
		public ApplicationEventList()
		{
		}

		#endregion

		#region Public IComparer<> Classes

		/// <summary>
		///		Defines a method that a type implements to compare two objects.
		/// </summary>
		public class CompareByDate : IComparer<ApplicationEvent>
		{
			/// <summary>
			///		Compares two objects and returns a value indicating whether one is less than,
			///		equal to, or greater than the other.
			/// </summary>
			/// <param name="objA">The first object to compare.</param>
			/// <param name="objB">The second object to compare.</param>
			/// <returns>
			///		Less than zero if x is less than y,
			///		- or -
			///		Zero if x equals y,
			///		- or -
			///		Greater than zero if x is greater than y.
			/// </returns>
			public int Compare(ApplicationEvent objA, ApplicationEvent objB)
			{
				if (objA == null || objB == null)
				{
					if (objA != null)
					{
						return int.MaxValue;
					}

					if (objB != null)
					{
						return int.MinValue;
					}

					return 0;
				}

				return Nullable.Compare<DateTime>(objA.CreatedDate, objB.CreatedDate);
			}
		}

		#endregion
	}
}
