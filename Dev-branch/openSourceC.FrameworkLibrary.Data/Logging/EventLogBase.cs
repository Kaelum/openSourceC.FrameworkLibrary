using System;
using System.Diagnostics;
using System.Threading;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///     The base class for a strongly typed event logger.
	/// </summary>
	/// <typeparam name="TEventLog">The type of the event logger.</typeparam>
	public abstract class EventLogBase<TEventLog>
		where TEventLog : EventLogBase<TEventLog>, new()
	{
		//// TraceSwitch for changing which values are actually written to the
		//// trace output file.
		//private TraceSwitch _loggingSwitch;

		//// This object is added as a debug listener.
		//private StreamWriter _debugWriter;

		private static TEventLog _instance;
		private static object _instanceLock = new object();


		#region Class Constructors

		/// <summary>
		///		Class constructor.
		/// </summary>
		/// <remarks>Prevents the parameterless creation of this object externaly.</remarks>
		protected EventLogBase()
		{
			EventLogName = DefaultEventLogName;
			EventLogMachineName = Environment.MachineName;
			EventLogSourceName = DefaultEventLogSourceName;
		}

		#endregion

		#region Instance

		/// <summary>
		///		Gets the current instance of the application event log object.
		/// </summary>
		public static TEventLog Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_instanceLock)
					{
						if (_instance == null)
						{
							// Be very careful by putting a Try/Catch around the entire routine.
							// We should never throw an exception while logging.
							try
							{
								_instance = new TEventLog();
								//_instance._loggingSwitch = new TraceSwitch("Logging", "Entire Application");
								//_instance._debugWriter = null;

								if (!EventLog.Exists(_instance.EventLogName))
								{
									if (EventLog.SourceExists(_instance.EventLogSourceName))
									{
										EventLog.DeleteEventSource(_instance.EventLogSourceName);
									}

									EventLog.CreateEventSource(_instance.EventLogSourceName, _instance.EventLogName);
									Thread.Sleep(3000);
								}
							}
							catch (Exception ex)
							{
								try
								{
									// Write the message to the system event log.
									EventLog eventLog = new EventLog(_instance.EventLogName, _instance.EventLogMachineName, _instance.EventLogSourceName);

									//Write the entry to the event log
									eventLog.WriteEntry(Format.Exception(ex), EventLogEntryType.Error);
									eventLog.Close();
								}
								catch (Exception ex2)
								{
									// Ignore any exceptions.
									Debug.WriteLine(ex2);
								}
							}
						}
					}
				}

				return _instance;
			}
		}

		#endregion

		#region Public Properties

		///// <summary>
		/////		Gets or sets the stream to write debug messages to.
		///// </summary>
		//public StreamWriter DebugWriter
		//{
		//    get { return _debugWriter; }
		//    set { _debugWriter = value; }
		//}

		/// <value>
		///     Property EventLogMachineName is used to get the machine name to
		///     log the event to, defaults to an empty string, indicating the
		///     current machine.  A machine name (without \\), may be empty.
		/// </value>
		public string EventLogMachineName { get; protected set; }

		/// <value>
		///     Property EventLogName is used to get the name of the event log, 
		/// </value>
		public string EventLogName { get; protected set; }

		/// <value>
		///     Property EventLogMachineName is used to get the source of the error to be written to the event log, 
		/// </value>
		public string EventLogSourceName { get; protected set; }

		///// <summary>
		/////		Gets the Logging switch for the application.
		///// </summary>
		//public TraceSwitch LoggingSwitch
		//{
		//    get { return _loggingSwitch; }
		//}

		#endregion

		#region Abstract Properties

		/// <value>
		///     Gets the default EventLog name to be used.
		/// </value>
		protected abstract string DefaultEventLogName { get; }

		/// <value>
		///     Property EventLogMachineName is used to get the source of the error to be written to the event log, 
		/// </value>
		protected abstract string DefaultEventLogSourceName { get; }

		#endregion

		#region Public Methods

		/// <summary>
		///     Write the specified <see cref="ApplicationEvent"/> to the application event log.
		/// </summary>
		/// <param name="applicationEvent">The <see cref="ApplicationEvent"/> to be logged.</param>
		public void WriteEvent(ApplicationEvent applicationEvent)
		{
			// Be very careful by putting a Try/Catch around the entire routine.
			// We should never throw an exception while logging.
			try
			{
				string source = (string.IsNullOrWhiteSpace(applicationEvent.Source) ? EventLogSourceName : applicationEvent.Source);

				try
				{
					if (!EventLog.SourceExists(source, EventLogMachineName))
					{
						EventSourceCreationData sourceDate = new EventSourceCreationData(source, EventLogName);
						sourceDate.MachineName = EventLogMachineName;

						EventLog.CreateEventSource(sourceDate);
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Exception: ", ex);
					source = EventLogSourceName;
				}

				// Write the message to the system event log.
				EventLog eventLog = new EventLog(EventLogName, EventLogMachineName, source);

				//Write the entry to the event log
				eventLog.WriteEntry(
					applicationEvent.Message,
					applicationEvent.Type,
					applicationEvent.EventID.HasValue ? applicationEvent.EventID.Value : -1,
					applicationEvent.Category.HasValue ? applicationEvent.Category.Value : (short)-1,
					applicationEvent.Data
				);

				eventLog.Close();

				WriteEventToSecondary(applicationEvent);
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry(EventLogSourceName, Format.Exception(ex), EventLogEntryType.Error);

				Debug.WriteLine("==EventLogFailure".PadRight(80, '='));
				Debug.WriteLine(Format.Exception(ex));
			}
		}

		/// <summary>
		///     Write the specified <see cref="ApplicationEvent"/> to the application event log if
		///     the condition is <b>true</b>.
		/// </summary>
		/// <param name="condition">true to cause a message to be written; otherwise, false.</param>
		/// <param name="applicationEvent">The <see cref="ApplicationEvent"/> to be logged.</param>
		public void WriteEventIf(bool condition, ApplicationEvent applicationEvent)
		{
			if (condition)
			{
				WriteEvent(applicationEvent);
			}
		}

		/// <summary>
		///     Write an exception to the application event log.
		/// </summary>
		/// <param name="exception">The <see cref="Exception"/> to be logged.</param>
		public void WriteException(Exception exception)
		{
			// Be very careful by putting a Try/Catch around the entire routine.
			// We should never throw an exception while logging.
			try
			{
				WriteEvent(new ApplicationEvent(exception));
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry(EventLogSourceName, Format.Exception(ex), EventLogEntryType.Error);

				Debug.WriteLine("==EventLogFailure".PadRight(80, '='));
				Debug.WriteLine(Format.Exception(ex));
			}
		}

		/// <summary>
		///     Write an exception to the application event log if the condition is <b>true</b>.
		/// </summary>
		/// <param name="condition">true to cause a message to be written; otherwise, false.</param>
		/// <param name="exception">The <see cref="Exception"/> to be logged.</param>
		public void WriteExceptionIf(bool condition, Exception exception)
		{
			if (condition)
			{
				WriteException(exception);
			}
		}

		#endregion

		#region Abstract Methods

		/// <summary>
		///     Write the specified <see cref="ApplicationEvent"/> to a secondary location.
		/// </summary>
		/// <param name="applicationEvent">The <see cref="ApplicationEvent"/> to be logged.</param>
		protected abstract void WriteEventToSecondary(ApplicationEvent applicationEvent);

		#endregion
	}
}
