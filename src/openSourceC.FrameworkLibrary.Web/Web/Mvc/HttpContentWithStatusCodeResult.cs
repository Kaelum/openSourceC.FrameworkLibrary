using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace openSourceC.FrameworkLibrary.Web.Mvc
{
	/// <summary>
	///		Summary description for HttpStatusCodeWithContentResult.
	/// </summary>
	public class HttpStatusCodeWithContentResult : ActionResult
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="T:HttpStatusCodeWithContentResult" />
		///		class using a status code.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		public HttpStatusCodeWithContentResult(int statusCode)
			: this(statusCode, null) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="T:HttpStatusCodeWithContentResult" />
		///		class using a status code.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		public HttpStatusCodeWithContentResult(HttpStatusCode statusCode)
			: this(statusCode, null) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="T:HttpStatusCodeWithContentResult" />
		///		class using a status code and status description.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		/// <param name="statusDescription">The status description.</param>
		public HttpStatusCodeWithContentResult(int statusCode, string statusDescription)
		{
			StatusCode = statusCode;
			StatusDescription = statusDescription;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="T:HttpStatusCodeWithContentResult" />
		///		class using a status code and status description.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		/// <param name="statusDescription">The status description.</param>
		public HttpStatusCodeWithContentResult(HttpStatusCode statusCode, string statusDescription)
			: this((int)statusCode, statusDescription) { }

		#endregion

		#region Public Properties

		/// <summary>Gets or sets the content.</summary>
		/// <returns>The content.</returns>
		public string Content { get; set; }

		/// <summary>Gets or sets the content encoding.</summary>
		/// <returns>The content encoding.</returns>
		public Encoding ContentEncoding { get; set; }

		/// <summary>Gets or sets the type of the content.</summary>
		/// <returns>The type of the content.</returns>
		public string ContentType { get; set; }

		/// <summary>Gets the HTTP status code.</summary>
		/// <returns>The HTTP status code.</returns>
		public int StatusCode { get; private set; }

		/// <summary>Gets the HTTP status description.</summary>
		/// <returns>The HTTP status description.</returns>
		public string StatusDescription { get; private set; }

		#endregion

		#region Override Methods

		/// <summary>
		///		Enables processing of the result of an action method by a custom type that inherits
		///		from the <see cref="T:System.Web.Mvc.ActionResult" /> class.
		/// </summary>
		/// <param name="context">
		///		The context in which the result is executed. The context information includes the
		///		controller, HTTP content, request context, and route data.
		/// </param>
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			context.HttpContext.Response.StatusCode = StatusCode;

			if (StatusDescription != null)
			{
				context.HttpContext.Response.StatusDescription = StatusDescription;
			}

			HttpResponseBase response = context.HttpContext.Response;

			if (!string.IsNullOrEmpty(ContentType))
			{
				response.ContentType = ContentType;
			}

			if (ContentEncoding != null)
			{
				response.ContentEncoding = ContentEncoding;
			}

			if (Content != null)
			{
				response.Write(Content);
			}
		}

		#endregion
	}
}
