using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;

namespace openSourceC.FrameworkLibrary.Web.Mvc
{
	/// <summary>
	///		Represents a class that is used to send JSON-formatted content to the response.
	/// </summary>
	public class JsonNetResult : ActionResult
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="T:JsonNetResult" /> class.
		/// </summary>
		public JsonNetResult()
		{
			JsonRequestBehavior = JsonRequestBehavior.DenyGet;

			Settings = new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Error,
#if DEBUG
				Formatting = Formatting.Indented,
#endif
			};
		}

		#endregion

		#region Public Methods

		/// <summary>Gets or sets the content encoding.</summary>
		/// <returns>The content encoding.</returns>
		public Encoding ContentEncoding { get; set; }

		/// <summary>Gets or sets the type of the content.</summary>
		/// <returns>The type of the content.</returns>
		public string ContentType { get; set; }

		/// <summary>Gets or sets the data.</summary>
		/// <returns>The data.</returns>
		public object Data { get; set; }

		/// <summary>Gets or sets a value that indicates whether HTTP GET requests from the client are allowed.</summary>
		/// <returns>A value that indicates whether HTTP GET requests from the client are allowed.</returns>
		public JsonRequestBehavior JsonRequestBehavior { get; set; }

		/// <summary>Gets or sets the <see cref="T:JsonSerializerSettings"/>.</summary>
		/// <returns>The <see cref="T:JsonSerializerSettings"/>.</returns>
		public JsonSerializerSettings Settings { get; private set; }

		#endregion

		#region Public Methods

		/// <summary>Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.</summary>
		/// <param name="context">The context within which the result is executed.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="context" /> parameter is null.</exception>
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			if (JsonRequestBehavior == System.Web.Mvc.JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
			{
				throw new InvalidOperationException("JSON GET is not allowed");
			}

			HttpResponseBase response = context.HttpContext.Response;
			response.ContentType = (string.IsNullOrEmpty(this.ContentType) ? "application/json" : ContentType);

			if (ContentEncoding != null)
			{
				response.ContentEncoding = ContentEncoding;
			}

			if (Data != null)
			{
				response.Write(JsonConvert.SerializeObject(Data, Settings));

				//JsonSerializer serializer = JsonSerializer.Create(Settings);

				//using (StringWriter sw = new StringWriter())
				//{
				//	serializer.Serialize(sw, this.Data);
				//	response.Write(sw.ToString());
				//}
			}
		}

		#endregion
	}
}
