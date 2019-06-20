using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace openSourceC.FrameworkLibrary.Web.Http.Formatting
{
	/// <summary>
	///		
	/// </summary>
	public class PlainTextMediaTypeFormatter : MediaTypeFormatter
	{
		/// <summary>
		///		
		/// </summary>
		public PlainTextMediaTypeFormatter()
		{
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));

			SupportedEncodings.Add(new UTF8Encoding(false, true));
			SupportedEncodings.Add(new UnicodeEncoding(false, true, true));
		}

		#region Public Method Overrides

		/// <summary>
		///		
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public override bool CanReadType(Type type)
		{
			return false;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public override bool CanWriteType(Type type)
		{
			return (type == typeof(string));
		}

		//public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
		//{
		//    return base.GetPerRequestFormatterInstance(type, request, mediaType);
		//}

		/// <summary>
		///		
		/// </summary>
		/// <param name="type"></param>
		/// <param name="readStream"></param>
		/// <param name="content"></param>
		/// <param name="formatterLogger"></param>
		/// <returns></returns>
		public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
		{
			throw new NotSupportedException();
		}

		//public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
		//{
		//    base.SetDefaultContentHeaders(type, headers, mediaType);
		//}

		/// <summary>
		///		
		/// </summary>
		/// <param name="type"></param>
		/// <param name="value"></param>
		/// <param name="writeStream"></param>
		/// <param name="content"></param>
		/// <param name="transportContext"></param>
		/// <returns></returns>
		public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			if (writeStream == null)
			{
				throw new ArgumentNullException("writeStream");
			}

			return TaskHelpers.RunSynchronously(delegate
			{
				Encoding encoding = this.SelectCharacterEncoding((content != null) ? content.Headers : null);

				using (StreamWriter writer = new StreamWriter(writeStream, encoding))
				{
					writer.Write(value);
				}
			}, new CancellationToken());
		}

		#endregion
	}
}
