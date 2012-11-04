using System;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Summary description for OscLogger.
	/// </summary>
	public class OscLogger
	{
		/// <summary>
		///		Provides messages to subscribers.
		/// </summary>
		public event EventHandler<MessageEventArgs> Message;

		private LogLevel _logLevel;


		#region Constructors

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="traceLevel">The name of the logger to use.</param>
		public OscLogger(LogLevel traceLevel)
		{
			_logLevel = traceLevel;
		}

		#endregion

		#region Critical

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Critical(Exception exception, IFormatProvider provider, string format, params object[] args)
		{
			if (_logLevel >= LogLevel.Error)
			{
				Critical(string.Format(provider, format, args), exception);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Critical(Exception exception, string format, params object[] args)
		{
			if (_logLevel >= LogLevel.Error)
			{
				Critical(exception, null, format, args);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Critical(IFormatProvider provider, string format, params object[] args)
		{
			if (_logLevel >= LogLevel.Error)
			{
				Critical(string.Format(provider, format, args));
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Critical(string format, params object[] args)
		{
			if (_logLevel >= LogLevel.Error)
			{
				Critical((IFormatProvider)null, format, args);
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		public void Critical(string message)
		{
			if (_logLevel >= LogLevel.Error)
			{
				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					messageEvent(this, new MessageEventArgs(this.GetType(), LogEventType.Critical, message));
				}
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public void Critical(string message, Exception exception)
		{
			if (_logLevel >= LogLevel.Error)
			{
				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					string exceptionMessage = Format.Exception(exception, message);

					messageEvent(this, new MessageEventArgs(this.GetType(), LogEventType.Critical, exceptionMessage));
				}
			}
		}

		#endregion

		#region Debug

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Debug(Exception exception, IFormatProvider provider, string format, params object[] args)
		{
			if (_logLevel >= LogLevel.Verbose)
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
			if (_logLevel >= LogLevel.Verbose)
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
			if (_logLevel >= LogLevel.Verbose)
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
			if (_logLevel >= LogLevel.Verbose)
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
			if (_logLevel >= LogLevel.Verbose)
			{
				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					messageEvent(this, new MessageEventArgs(this.GetType(), LogEventType.Debug, message));
				}
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public void Debug(string message, Exception exception)
		{
			if (_logLevel >= LogLevel.Verbose)
			{
				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					string exceptionMessage = Format.Exception(exception, message);

					messageEvent(this, new MessageEventArgs(this.GetType(), LogEventType.Debug, exceptionMessage));
				}
			}
		}

		#endregion

		#region Error

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Error(Exception exception, IFormatProvider provider, string format, params object[] args)
		{
			if (_logLevel >= LogLevel.Error)
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
			if (_logLevel >= LogLevel.Error)
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
			if (_logLevel >= LogLevel.Error)
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
			if (_logLevel >= LogLevel.Error)
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
			if (_logLevel >= LogLevel.Error)
			{
				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					messageEvent(this, new MessageEventArgs(this.GetType(), LogEventType.Error, message));
				}
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public void Error(string message, Exception exception)
		{
			if (_logLevel >= LogLevel.Error)
			{
				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					string exceptionMessage = Format.Exception(exception, message);

					messageEvent(this, new MessageEventArgs(this.GetType(), LogEventType.Error, exceptionMessage));
				}
			}
		}

		#endregion

		#region Info

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Info(Exception exception, IFormatProvider provider, string format, params object[] args)
		{
			if (_logLevel >= LogLevel.Info)
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
			if (_logLevel >= LogLevel.Info)
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
			if (_logLevel >= LogLevel.Info)
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
			if (_logLevel >= LogLevel.Info)
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
			if (_logLevel >= LogLevel.Info)
			{
				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					messageEvent(this, new MessageEventArgs(this.GetType(), LogEventType.Information, message));
				}
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public void Info(string message, Exception exception)
		{
			if (_logLevel >= LogLevel.Info)
			{
				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					string exceptionMessage = Format.Exception(exception, message);

					messageEvent(this, new MessageEventArgs(this.GetType(), LogEventType.Information, exceptionMessage));
				}
			}
		}

		#endregion

		#region Warn

		/// <summary>
		///		
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Warn(Exception exception, IFormatProvider provider, string format, params object[] args)
		{
			if (_logLevel >= LogLevel.Warning)
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
			if (_logLevel >= LogLevel.Warning)
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
			if (_logLevel >= LogLevel.Warning)
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
			if (_logLevel >= LogLevel.Warning)
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
			if (_logLevel >= LogLevel.Warning)
			{
				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					messageEvent(this, new MessageEventArgs(this.GetType(), LogEventType.Warning, message));
				}
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public void Warn(string message, Exception exception)
		{
			if (_logLevel >= LogLevel.Warning)
			{
				EventHandler<MessageEventArgs> messageEvent = Message;

				if (messageEvent != null)
				{
					string exceptionMessage = Format.Exception(exception, message);

					messageEvent(this, new MessageEventArgs(this.GetType(), LogEventType.Warning, exceptionMessage));
				}
			}
		}

		#endregion
	}
}
