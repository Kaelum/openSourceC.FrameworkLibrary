using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using Newtonsoft.Json;

namespace openSourceC.FrameworkLibrary.Net.Http
{
	/// <summary>
	///		Summary description for JsonContent.
	/// </summary>
	public class JsonContent<T> : ByteArrayContent
	{
		private const string mediaType = "application/json";


		#region Constructors

		/// <summary>
		///		Creates an instance of JsonContent.
		/// </summary>
		/// <param name="content"></param>
		public JsonContent(T content)
			: base(GetContentByteArray(content))
		{
			MediaTypeHeaderValue contentType = new MediaTypeHeaderValue(mediaType)
			{
				CharSet = Encoding.UTF8.WebName,
			};

			base.Headers.ContentType = contentType;
		}

		#endregion

		#region Private Methods

		private static byte[] GetContentByteArray(T content)
		{
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}

			string json = JsonConvert.SerializeObject(content);

			return Encoding.UTF8.GetBytes(json);
		}

		#endregion
	}
}