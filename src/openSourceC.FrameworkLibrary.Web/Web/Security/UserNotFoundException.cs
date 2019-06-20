using System;
using System.Runtime.Serialization;

namespace openSourceC.FrameworkLibrary.Web.Security
{
	/// <summary>
	///		The exception that is thrown when a user cannot be found in the user store.
	/// </summary>
	[Serializable]
	public class UserNotFoundException : OscErrorException
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="UserNotFoundException" /> class.
		/// </summary>
		public UserNotFoundException() { }

		/// <summary>
		///		Initializes a new instance of the <see cref="UserNotFoundException" />
		///		class with a specified error message.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		public UserNotFoundException(string message)
			: base(message) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="OscException" />
		///		class with a specified error message.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="userMessage">A user friendly message that can sent to the user.</param>
		public UserNotFoundException(string message, string userMessage)
			: base(message, userMessage) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="UserNotFoundException" />
		///		class with a specified error message and a reference to the
		///		inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause
		///     of the current exception. If the innerException parameter is
		///     not a null reference, the current exception is raised in a
		///     catch block that handles the inner exception.</param>
		public UserNotFoundException(string message, Exception innerException)
			: base(message, innerException) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="UserNotFoundException" />
		///		class with a specified error message and a reference to the
		///		inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="userMessage">A user friendly message that can sent to the user.</param>
		/// <param name="innerException">The exception that is the cause
		///     of the current exception. If the innerException parameter is
		///     not a null reference, the current exception is raised in a
		/// c   atch block that handles the inner exception.</param>
		public UserNotFoundException(string message, string userMessage, Exception innerException)
			: base(message, userMessage, innerException) { }

		/// <summary>
		///     Initializes a new instance of the <see cref="UserNotFoundException" />
		///     class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected UserNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		#endregion
	}
}
