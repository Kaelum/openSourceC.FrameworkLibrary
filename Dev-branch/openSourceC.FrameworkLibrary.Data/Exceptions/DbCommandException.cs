using System;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Text;

using openSourceC.FrameworkLibrary.Common;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Initializes a new instance of the <see cref="DbCommandException" />
	///		class with a specified error message and a reference to the
	///		inner exception that is the cause of this exception.
	/// </summary>
	[Serializable]
	public class DbCommandException : OscErrorException
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" /> class.
		/// </summary>
		public DbCommandException()
			: base() { SaveParameters(null, null); }

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" />
		///		class with the command that caused the exception.
		/// </summary>
		/// <param name="command">The <see cref="DbCommand"/> that caused the exception.</param>
		public DbCommandException(DbCommand command)
			: base(BuildMessage(null, null)) { SaveParameters(command, null); }

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" />
		///		class with the return code and the command that caused the exception.
		/// </summary>
		/// <param name="command">The <see cref="DbCommand"/> that caused the exception.</param>
		/// <param name="returnCode">The code that was returned from the data access object.</param>
		public DbCommandException(DbCommand command, int? returnCode)
			: base(BuildMessage(null, returnCode)) { SaveParameters(command, returnCode); }

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" />
		///		class with the command that caused the exception.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="command">The <see cref="DbCommand"/> that caused the exception.</param>
		public DbCommandException(string message, DbCommand command)
			: base(BuildMessage(message, null)) { SaveParameters(command, null); }

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" />
		///		class with code that was returned and command that caused the exception.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="command">The <see cref="DbCommand"/> that caused the exception.</param>
		/// <param name="returnCode">The code that was returned from the data access object.</param>
		public DbCommandException(string message, DbCommand command, int? returnCode)
			: base(BuildMessage(message, returnCode)) { SaveParameters(command, returnCode); }

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" />
		///		class with the command that caused the exception.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="userMessage">A user friendly message that can sent to the user.</param>
		/// <param name="command">The <see cref="DbCommand"/> that caused the exception.</param>
		public DbCommandException(string message, string userMessage, DbCommand command)
			: base(BuildMessage(message, null), userMessage) { SaveParameters(command, null); }

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" />
		///		class with code that was returned and command that caused the exception.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="userMessage">A user friendly message that can sent to the user.</param>
		/// <param name="command">The <see cref="DbCommand"/> that caused the exception.</param>
		/// <param name="returnCode">The code that was returned from the data access object.</param>
		public DbCommandException(string message, string userMessage, DbCommand command, int? returnCode)
			: base(BuildMessage(message, returnCode), userMessage) { SaveParameters(command, returnCode); }

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" />
		///		class with a specified error message and a reference to the
		///		inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="command">The <see cref="DbCommand"/> that caused the exception.</param>
		/// <param name="innerException">The exception that is the cause
		///     of the current exception. If the innerException parameter is
		///     not a null reference, the current exception is raised in a
		///     catch block that handles the inner exception.</param>
		public DbCommandException(DbCommand command, Exception innerException)
			: base(BuildMessage(null, null), innerException) { SaveParameters(command, null); }

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" />
		///		class with the return code and the command that caused the exception.
		/// </summary>
		/// <param name="command">The <see cref="DbCommand"/> that caused the exception.</param>
		/// <param name="returnCode">The code that was returned from the data access object.</param>
		/// <param name="innerException">The exception that is the cause
		///     of the current exception. If the innerException parameter is
		///     not a null reference, the current exception is raised in a
		///     catch block that handles the inner exception.</param>
		public DbCommandException(DbCommand command, int? returnCode, Exception innerException)
			: base(BuildMessage(null, returnCode), innerException) { SaveParameters(command, returnCode); }

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" />
		///		class with a specified error message and a reference to the
		///		inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="command">The <see cref="DbCommand"/> that caused the exception.</param>
		/// <param name="innerException">The exception that is the cause
		///     of the current exception. If the innerException parameter is
		///     not a null reference, the current exception is raised in a
		///     catch block that handles the inner exception.</param>
		public DbCommandException(string message, DbCommand command, Exception innerException)
			: base(BuildMessage(message, null), innerException) { SaveParameters(command, null); }

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" />
		///		class with a specified error message and a reference to the
		///		inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="command">The <see cref="DbCommand"/> that caused the exception.</param>
		/// <param name="returnCode">The code that was returned from the data access object.</param>
		/// <param name="innerException">The exception that is the cause
		///     of the current exception. If the innerException parameter is
		///     not a null reference, the current exception is raised in a
		///     catch block that handles the inner exception.</param>
		public DbCommandException(string message, DbCommand command, int? returnCode, Exception innerException)
			: base(BuildMessage(message, returnCode), innerException) { SaveParameters(command, returnCode); }

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" />
		///		class with a specified error message and a reference to the
		///		inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="userMessage">A user friendly message that can sent to the user.</param>
		/// <param name="command">The <see cref="DbCommand"/> that caused the exception.</param>
		/// <param name="innerException">The exception that is the cause
		///     of the current exception. If the innerException parameter is
		///     not a null reference, the current exception is raised in a
		///     catch block that handles the inner exception.</param>
		public DbCommandException(string message, string userMessage, DbCommand command, Exception innerException)
			: base(BuildMessage(message, null), userMessage, innerException) { SaveParameters(command, null); }

		/// <summary>
		///		Initializes a new instance of the <see cref="DbCommandException" />
		///		class with a specified error message and a reference to the
		///		inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="userMessage">A user friendly message that can sent to the user.</param>
		/// <param name="command">The <see cref="DbCommand"/> that caused the exception.</param>
		/// <param name="returnCode">The code that was returned from the data access object.</param>
		/// <param name="innerException">The exception that is the cause
		///     of the current exception. If the innerException parameter is
		///     not a null reference, the current exception is raised in a
		///     catch block that handles the inner exception.</param>
		public DbCommandException(string message, string userMessage, DbCommand command, int? returnCode, Exception innerException)
			: base(BuildMessage(message, returnCode), userMessage, innerException) { SaveParameters(command, returnCode); }

		/// <summary>
		///     Initializes a new instance of the <see cref="DbCommandException" />
		///     class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected DbCommandException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		#endregion

		#region Public Properties

		/// <summary>
		///		Gets the code that was returned from the data access object.
		/// </summary>
		public string ExecutionString
		{
			get { return base.Data["ExecutionString"] as string; }
			private set { base.Data["ExecutionString"] = value; }
		}

		/// <summary>
		///		Gets the code that was returned from the data access object.
		/// </summary>
		public int? ReturnCode
		{
			get { return base.Data["ReturnCode"] as int?; }
			private set { base.Data["ReturnCode"] = value; }
		}

		#endregion

		#region Private Methods

		private static string BuildMessage(string message, int? returnCode)
		{
			StringBuilder sb = new StringBuilder();


			if (!string.IsNullOrEmpty(message))
			{
				sb.AppendFormat("{0}.", message);
			}

			if (returnCode.HasValue)
			{
				if (sb.Length > 0)
				{
					sb.Append(Environment.NewLine);
				}

				sb.AppendFormat("Return Code: {0}", returnCode);
			}

			return sb.ToString();
		}

		private void SaveParameters(DbCommand command, int? returnCode)
		{
			if (command != null)
			{
				MessageExtension = DbHelper.CommandToString(command);
			}

			ReturnCode = returnCode;
			ExecutionString = MessageExtension;
		}

		#endregion
	}
}
