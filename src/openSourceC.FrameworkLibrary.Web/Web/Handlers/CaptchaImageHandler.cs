using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;

using openSourceC.FrameworkLibrary.Web.UI.WebControls;

namespace openSourceC.FrameworkLibrary.Web.Handlers
{
	/// <summary>
	///		
	/// </summary>
	public class CaptchaImageHandler : IHttpHandler, IRequiresSessionState
	{
		#region IHttpHandler Members

		/// <summary>
		///		
		/// </summary>
		/// <param name="context"></param>
		public void ProcessRequest(HttpContext context)
		{
			HttpApplication app = context.ApplicationInstance;
			CaptchaImage ci;


			if (app.Request.QueryString["guid"] == null)
			{
				app.Response.StatusCode = 404;
				app.CompleteRequest();
				return;
			}

			string guid = app.Request.QueryString["guid"];
			string s = app.Request.QueryString["s"];

			if (string.IsNullOrEmpty(s))
			{
				ci = (CaptchaImage)context.Cache[guid];
			}
			else
			{
				ci = (CaptchaImage)context.Session[guid];
			}

			if (ci == null)
			{
				app.Response.StatusCode = 404;
				app.CompleteRequest();
				return;
			}

			using (Bitmap b = ci.RenderImage())
			{
				b.Save(app.Context.Response.OutputStream, ImageFormat.Jpeg);
			}

			app.Response.ContentType = "image/jpg";
			app.Response.StatusCode = 200;
			app.CompleteRequest();
		}

		/// <summary>
		///		
		/// </summary>
		public bool IsReusable
		{
			get { return true; }
		}

		#endregion
	}
}
