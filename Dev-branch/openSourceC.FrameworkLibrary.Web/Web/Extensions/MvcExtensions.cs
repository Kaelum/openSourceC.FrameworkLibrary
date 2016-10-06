using System;
using System.Net;
using System.Text;
using System.Web.Mvc;

using openSourceC.FrameworkLibrary.Web.Mvc;

namespace openSourceC.FrameworkLibrary.Web.Extensions
{
	/// <summary>
	///		
	/// </summary>
	public static class MvcExtensions
	{
		#region HttpStatusCodeWithContent

		/// <summary>
		///		Creates a <see cref="T:HttpStatusCodeWithContentResult" /> object using a status
		///		code.
		/// </summary>
		/// <param name="controller">The <see cref="T:Controller"/> object to be extended.</param>
		/// <param name="statusCode">The status code.</param>
		public static HttpStatusCodeWithContentResult HttpStatusCodeWithContent(this Controller controller, int statusCode)
		{
			return new HttpStatusCodeWithContentResult(statusCode);
		}

		/// <summary>
		///		Creates a <see cref="T:HttpStatusCodeWithContentResult" /> object using a status
		///		code.
		/// </summary>
		/// <param name="controller">The <see cref="T:Controller"/> object to be extended.</param>
		/// <param name="statusCode">The status code.</param>
		public static HttpStatusCodeWithContentResult HttpStatusCodeWithContent(this Controller controller, HttpStatusCode statusCode)
		{
			return new HttpStatusCodeWithContentResult(statusCode);
		}

		/// <summary>
		///		Creates a <see cref="T:HttpStatusCodeWithContentResult" /> object using a status
		///		code and status description.
		/// </summary>
		/// <param name="controller">The <see cref="T:Controller"/> object to be extended.</param>
		/// <param name="statusCode">The status code.</param>
		/// <param name="statusDescription">The status description.</param>
		public static HttpStatusCodeWithContentResult HttpStatusCodeWithContent(this Controller controller, int statusCode, string statusDescription)
		{
			return new HttpStatusCodeWithContentResult(statusCode, statusDescription);
		}

		/// <summary>
		///		Creates a <see cref="T:HttpStatusCodeWithContentResult" /> object using a status
		///		code and status description.
		/// </summary>
		/// <param name="controller">The <see cref="T:Controller"/> object to be extended.</param>
		/// <param name="statusCode">The status code.</param>
		/// <param name="statusDescription">The status description.</param>
		public static HttpStatusCodeWithContentResult HttpStatusCodeWithContent(this Controller controller, HttpStatusCode statusCode, string statusDescription)
		{
			return new HttpStatusCodeWithContentResult(statusCode, statusDescription);
		}

		#endregion

		#region JsonNet

		/// <summary>
		///		Creates a <see cref="T:JsonNetResult" /> object that serializes the specified object
		///		to JavaScript Object Notation (JSON).
		/// </summary>
		/// <param name="controller">The <see cref="T:Controller"/> object to be extended.</param>
		/// <param name="data">The JavaScript object graph to serialize.</param>
		/// <returns>
		///		The JSON result object that serializes the specified object to JSON format. The
		///		result object that is prepared by this method is written to the response by the
		///		ASP.NET MVC framework when the object is executed.
		/// </returns>
		public static JsonNetResult JsonNet(this Controller controller, object data)
		{
			return new JsonNetResult
			{
				Data = data,
			};
		}

		/// <summary>
		///		Creates a <see cref="T:JsonNetResult" /> object that serializes the specified object
		///		to JavaScript Object Notation (JSON) format using the specified JSON request
		///		behavior.
		/// </summary>
		/// <param name="controller">The <see cref="T:Controller"/> object to be extended.</param>
		/// <param name="data">The JavaScript object graph to serialize.</param>
		/// <param name="behavior">The JSON request behavior.</param>
		/// <returns>
		///		The result object that serializes the specified object to JSON format.
		/// </returns>
		public static JsonNetResult JsonNet(this Controller controller, object data, JsonRequestBehavior behavior)
		{
			return new JsonNetResult
			{
				Data = data,
				JsonRequestBehavior = behavior,
			};
		}

		/// <summary>
		///		Creates a <see cref="T:JsonNetResult" /> object that serializes the specified object
		///		to JavaScript Object Notation (JSON) format.
		/// </summary>
		/// <param name="controller">The <see cref="T:Controller"/> object to be extended.</param>
		/// <param name="data">The JavaScript object graph to serialize.</param>
		/// <param name="contentType">The content type (MIME type).</param>
		/// <returns>
		///		The JSON result object that serializes the specified object to JSON format.
		/// </returns>
		public static JsonNetResult JsonNet(this Controller controller, object data, string contentType)
		{
			return new JsonNetResult
			{
				Data = data,
				ContentType = contentType,
			};
		}

		/// <summary>
		///		Creates a <see cref="T:JsonNetResult" /> object that serializes the specified object
		///		to JavaScript Object Notation (JSON) format.
		/// </summary>
		/// <param name="controller">The <see cref="T:Controller"/> object to be extended.</param>
		/// <param name="data">The JavaScript object graph to serialize.</param>
		/// <param name="contentType">The content type (MIME type).</param>
		/// <param name="contentEncoding">The content encoding.</param>
		/// <returns>
		///		The JSON result object that serializes the specified object to JSON format.
		/// </returns>
		public static JsonNetResult JsonNet(this Controller controller, object data, string contentType, Encoding contentEncoding)
		{
			return new JsonNetResult
			{
				Data = data,
				ContentType = contentType,
				ContentEncoding = contentEncoding,
			};
		}

		/// <summary>
		///		Creates a <see cref="T:JsonNetResult" /> object that serializes the specified object
		///		to JavaScript Object Notation (JSON) format using the specified content type and
		///		JSON request behavior.
		/// </summary>
		/// <param name="controller">The <see cref="T:Controller"/> object to be extended.</param>
		/// <param name="data">The JavaScript object graph to serialize.</param>
		/// <param name="contentType">The content type (MIME type).</param>
		/// <param name="behavior">The JSON request behavior</param>
		/// <returns>
		///		The result object that serializes the specified object to JSON format.
		/// </returns>
		public static JsonNetResult JsonNet(this Controller controller, object data, string contentType, JsonRequestBehavior behavior)
		{
			return new JsonNetResult
			{
				Data = data,
				ContentType = contentType,
				JsonRequestBehavior = behavior,
			};
		}

		/// <summary>
		///		Creates a <see cref="T:JsonNetResult" /> object that serializes the specified object
		///		to JavaScript Object Notation (JSON) format using the content type, content
		///		encoding, and the JSON request behavior.
		///	</summary>
		/// <param name="controller">The <see cref="T:Controller"/> object to be extended.</param>
		/// <param name="data">The JavaScript object graph to serialize.</param>
		/// <param name="contentType">The content type (MIME type).</param>
		/// <param name="contentEncoding">The content encoding.</param>
		/// <param name="behavior">The JSON request behavior</param>
		/// <returns>
		///		The result object that serializes the specified object to JSON format.
		///	</returns>
		public static JsonNetResult JsonNet(this Controller controller, object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			return new JsonNetResult
			{
				Data = data,
				ContentType = contentType,
				ContentEncoding = contentEncoding,
				JsonRequestBehavior = behavior,
			};
		}

		#endregion
	}
}
