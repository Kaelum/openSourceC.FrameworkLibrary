using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Xml;

using log4net;
using log4net.Config;
using log4net.Core;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		This object is a wrapper for log4net and includes additional features that mirror logged
	///		events to the Message event in the wrapper.  This works nicely with WPF applications
	///		where you can log events to a ListView control.
	/// </summary>
	public class OscLog
	{
		/// <summary>
		///		Provides messages to subscribers.
		/// </summary>
		public event EventHandler<MessageEventArgs> Message;

		private ILog _iLog;

		private static readonly Type _thisDeclaringType = typeof(OscLog);

		private static OscLog _defaultInstance = null;
		private static object _defaultInstanceLock = new object();
		private static string _defaultLoggerName;

		private static XmlElement _defaultInstanceSection = null;


		#region Constructors

		/// <summary>
		///		Static constructor.
		/// </summary>
		static OscLog()
		{
			DefaultLoggerName = null;
		}

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="logger">An existing <see cref="T:OscLog"/> object.</param>
		public OscLog(OscLog logger)
		{
			Initialize(logger._iLog);
		}

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="iLog">An existing log4net <see cref="T:ILog"/> object.</param>
		public OscLog(ILog iLog)
		{
			Initialize(iLog);
		}

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="loggerName">The name of the logger to use.</param>
		public OscLog(string loggerName)
		{
			Initialize(LogManager.GetLogger(loggerName));
		}

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="log4netConfigurationXml">The log4net configuration xml element.</param>
		/// <param name="loggerName">The name of the logger to use.</param>
		public OscLog(XmlElement log4netConfigurationXml, string loggerName)
		{
			XmlConfigurator.Configure(log4netConfigurationXml);

			Initialize(LogManager.GetLogger(loggerName));
		}

		#endregion

		#region Initialize (private)

		private void Initialize(ILog log)
		{
			_iLog = log;

			if (!_iLog.Logger.Repository.Configured)
			{
				XmlConfigurator.Configure(_iLog.Logger.Repository);
			}
		}

		#endregion

		#region Public Properties

		/// <summary>Gets the logger name.</summary>
		public string LoggerName
		{
			get { return _iLog.Logger.Name; }
		}

		#endregion

		#region Instance

		/// <summary>
		///		Gets or sets the default logger name.  Setting to <b>null</b>, causes a default
		///		configuration setting to be used, or "default" if the setting does not exist.
		/// </summary>
		public static string DefaultLoggerName
		{
			get
			{
				if (_defaultLoggerName == null)
				{
					_defaultLoggerName = (ConfigurationManager.AppSettings["DefaultLoggerName"] ?? "default");
				}

				return _defaultLoggerName;
			}

			set
			{
				if (_defaultLoggerName != value)
				{
					lock (_defaultInstanceLock)
					{
						_defaultInstance = null;
						_defaultLoggerName = value;
					}
				}
			}
		}

		/// <summary>
		///		Gets the default (singleton) instance.  If defined, it uses the "DefaultLoggerName"
		///		application setting for the logger name; otherwise, "default" is used.
		///	</summary>
		public static OscLog Instance
		{
			get
			{
				if (_defaultInstance == null)
				{
					lock (_defaultInstanceLock)
					{
						if (_defaultInstance == null)
						{
							if (_defaultInstanceSection == null)
							{
								_defaultInstance = new OscLog(DefaultLoggerName);
							}
							else
							{
								_defaultInstance = new OscLog(_defaultInstanceSection, DefaultLoggerName);
							}
						}
					}
				}

				return _defaultInstance;
			}
		}

		/// <summary>
		///		Sets the section of the configuration to use for the default logger.
		/// </summary>
		/// <param name="sectionName">The configuration section path and name.</param>
		/// <param name="loggerName">The default logger name. (Optional)</param>
		public static void SetDefaultInstanceSection(string sectionName, string loggerName = null)
		{
			_defaultInstanceSection = (XmlElement)ConfigurationManager.GetSection(sectionName);

			DefaultLoggerName = loggerName;
		}

		/// <summary>
		///		Sets the section of the configuration to use for the default logger.
		/// </summary>
		/// <param name="section">The configuration section.</param>
		/// <param name="loggerName">The default logger name. (Optional)</param>
		public static void SetDefaultInstanceSection(XmlElement section, string loggerName = null)
		{
			_defaultInstanceSection = section;

			DefaultLoggerName = loggerName;
		}

		#endregion

		#region Debug

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		public void Debug(Exception exception)
		{
			if (_iLog.IsDebugEnabled)
			{
				Debug(exception.Message, exception);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Debug(Exception exception, IFormatProvider provider, string format, params object[] args)
		{
			if (_iLog.IsDebugEnabled)
			{
				Debug(string.Format(provider, format, args), exception);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Debug(Exception exception, string format, params object[] args)
		{
			if (_iLog.IsDebugEnabled)
			{
				Debug(exception, null, format, args);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Debug(IFormatProvider provider, string format, params object[] args)
		{
			if (_iLog.IsDebugEnabled)
			{
				Debug(string.Format(provider, format, args));
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Debug(string format, params object[] args)
		{
			if (_iLog.IsDebugEnabled)
			{
				Debug((IFormatProvider)null, format, args);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		public void Debug(string message)
		{
			if (_iLog.IsDebugEnabled)
			{
				Debug(message, (Exception)null);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public void Debug(string message, Exception exception)
		{
			if (_iLog.IsDebugEnabled)
			{
				_iLog.Logger.Log(_thisDeclaringType, Level.Debug, message, exception);

				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					MessageEventArgs messageEventArgs = new MessageEventArgs(_thisDeclaringType, MessageLogEntryType.Debug, message, exception);

					messageEvent(this, messageEventArgs);
				}
			}
		}

		#endregion

		#region Error

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		public void Error(Exception exception)
		{
			if (_iLog.IsErrorEnabled)
			{
				Error(exception.Message, exception);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Error(Exception exception, IFormatProvider provider, string format, params object[] args)
		{
			if (_iLog.IsErrorEnabled)
			{
				Error(string.Format(provider, format, args), exception);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Error(Exception exception, string format, params object[] args)
		{
			if (_iLog.IsErrorEnabled)
			{
				Error(exception, null, format, args);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Error(IFormatProvider provider, string format, params object[] args)
		{
			if (_iLog.IsErrorEnabled)
			{
				Error(string.Format(provider, format, args));
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Error(string format, params object[] args)
		{
			if (_iLog.IsErrorEnabled)
			{
				Error((IFormatProvider)null, format, args);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		public void Error(string message)
		{
			if (_iLog.IsErrorEnabled)
			{
				Error(message, (Exception)null);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public void Error(string message, Exception exception)
		{
			if (_iLog.IsErrorEnabled)
			{
				_iLog.Logger.Log(_thisDeclaringType, Level.Error, message, exception);

				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					MessageEventArgs messageEventArgs = new MessageEventArgs(_thisDeclaringType, MessageLogEntryType.Error, message, exception);

					messageEvent(this, messageEventArgs);
				}
			}
		}

		#endregion

		#region Fatal

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		public void Fatal(Exception exception)
		{
			if (_iLog.IsFatalEnabled)
			{
				Fatal(exception.Message, exception);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Fatal(Exception exception, IFormatProvider provider, string format, params object[] args)
		{
			if (_iLog.IsFatalEnabled)
			{
				Fatal(string.Format(provider, format, args), exception);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Fatal(Exception exception, string format, params object[] args)
		{
			if (_iLog.IsFatalEnabled)
			{
				Fatal(exception, null, format, args);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Fatal(IFormatProvider provider, string format, params object[] args)
		{
			if (_iLog.IsFatalEnabled)
			{
				Fatal(string.Format(provider, format, args));
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Fatal(string format, params object[] args)
		{
			if (_iLog.IsFatalEnabled)
			{
				Fatal((IFormatProvider)null, format, args);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		public void Fatal(string message)
		{
			if (_iLog.IsFatalEnabled)
			{
				Fatal(message, (Exception)null);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public void Fatal(string message, Exception exception)
		{
			if (_iLog.IsFatalEnabled)
			{
				_iLog.Logger.Log(_thisDeclaringType, Level.Fatal, message, exception);

				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					MessageEventArgs messageEventArgs = new MessageEventArgs(_thisDeclaringType, MessageLogEntryType.Fatal, message, exception);

					messageEvent(this, messageEventArgs);
				}
			}
		}

		#endregion

		#region Info

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		public void Info(Exception exception)
		{
			if (_iLog.IsInfoEnabled)
			{
				Info(exception.Message, exception);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Info(Exception exception, IFormatProvider provider, string format, params object[] args)
		{
			if (_iLog.IsInfoEnabled)
			{
				Info(string.Format(provider, format, args), exception);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Info(Exception exception, string format, params object[] args)
		{
			if (_iLog.IsInfoEnabled)
			{
				Info(exception, null, format, args);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Info(IFormatProvider provider, string format, params object[] args)
		{
			if (_iLog.IsInfoEnabled)
			{
				Info(string.Format(provider, format, args));
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Info(string format, params object[] args)
		{
			if (_iLog.IsInfoEnabled)
			{
				Info((IFormatProvider)null, format, args);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		public void Info(string message)
		{
			if (_iLog.IsInfoEnabled)
			{
				Info(message, (Exception)null);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public void Info(string message, Exception exception)
		{
			if (_iLog.IsInfoEnabled)
			{
				_iLog.Logger.Log(_thisDeclaringType, Level.Info, message, exception);

				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					MessageEventArgs messageEventArgs = new MessageEventArgs(_thisDeclaringType, MessageLogEntryType.Information, message, exception);

					messageEvent(this, messageEventArgs);
				}
			}
		}

		#endregion

		#region Warn

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		public void Warn(Exception exception)
		{
			if (_iLog.IsWarnEnabled)
			{
				Warn(exception.Message, exception);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Warn(Exception exception, IFormatProvider provider, string format, params object[] args)
		{
			if (_iLog.IsWarnEnabled)
			{
				Warn(string.Format(provider, format, args), exception);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Warn(Exception exception, string format, params object[] args)
		{
			if (_iLog.IsWarnEnabled)
			{
				Warn(exception, null, format, args);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Warn(IFormatProvider provider, string format, params object[] args)
		{
			if (_iLog.IsWarnEnabled)
			{
				Warn(string.Format(provider, format, args));
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Warn(string format, params object[] args)
		{
			if (_iLog.IsWarnEnabled)
			{
				Warn((IFormatProvider)null, format, args);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		public void Warn(string message)
		{
			if (_iLog.IsWarnEnabled)
			{
				Warn(message, (Exception)null);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public void Warn(string message, Exception exception)
		{
			if (_iLog.IsWarnEnabled)
			{
				_iLog.Logger.Log(_thisDeclaringType, Level.Warn, message, exception);

				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					MessageEventArgs messageEventArgs = new MessageEventArgs(_thisDeclaringType, MessageLogEntryType.Warning, message, exception);

					messageEvent(this, messageEventArgs);
				}
			}
		}

		#endregion
	}
}
