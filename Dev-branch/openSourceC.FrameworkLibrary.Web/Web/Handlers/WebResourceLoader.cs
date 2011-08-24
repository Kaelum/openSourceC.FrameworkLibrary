using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.UI;

//using openSourceC.FrameworkLibrary.Web.RegularExpressions;
using openSourceC.FrameworkLibrary.Web.Util;
using System.Security;
using System.Configuration;

namespace openSourceC.FrameworkLibrary.Web.Handlers
{
	/// <summary>
	///		Provides an HTTP handler used to load embedded resources from assemblies.  This class
	///		cannot be inherited.
	/// </summary>
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebResourceLoader : IHttpHandler
	{
		// Fields
		internal const string WebResourceUrl = "OscWebResource.axd";

		private static IDictionary _assemblyInfoCache = Hashtable.Synchronized(new Hashtable());
		private static bool _handlerExistenceChecked;
		private static bool _handlerExists;
		//private static bool _smartNavPageChecked;
		//private static VirtualPath _smartNavPageLocation;
		//private static bool _smartNavScriptChecked;
		//private static VirtualPath _smartNavScriptLocation;
		private static IDictionary _typeAssemblyCache = Hashtable.Synchronized(new Hashtable());
		private static IDictionary _urlCache = Hashtable.Synchronized(new Hashtable());
		//private static bool _webFormsScriptChecked;
		//private static VirtualPath _webFormsScriptLocation;
		private static IDictionary _webResourceCache = Hashtable.Synchronized(new Hashtable());
		//private static bool _webUIValidationScriptChecked;
		//private static VirtualPath _webUIValidationScriptLocation;

		private static readonly Regex webResourceRegex = new Regex("<%\\s*=\\s*OscWebResource\\(\"(?<resourceName>[^\"]*)\"\\)\\s*%>", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.Compiled);
		private static readonly Assembly _systemWebAssembly = typeof(HttpContext).Assembly;


		#region Public Methods

		/// <summary>
		///		
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="resourceName"></param>
		/// <param name="htmlEncoded"></param>
		/// <returns></returns>
		public static string GetWebResourceUrlInternal(Assembly assembly, string resourceName, bool htmlEncoded)
		{
			EnsureHandlerExistenceChecked();

			if (!_handlerExists)
			{
				throw new InvalidOperationException(SR.AssemblyResourceLoader_HandlerNotRegistered);
			}

			Pair assemblyInfo = GetAssemblyInfo(assembly);
			AssemblyName first = (AssemblyName)assemblyInfo.First;
			long second = (long)assemblyInfo.Second;
			string str = first.Version.ToString();
			int num2 = CreateWebResourceUrlCacheKey(assembly, resourceName, htmlEncoded);
			string str2 = (string)_urlCache[num2];

			if (str2 == null)
			{
				string str3;

				if (assembly.GlobalAssemblyCache)
				{
					if (assembly == _systemWebAssembly)
					{
						str3 = "s";
					}
					else
					{
						StringBuilder builder = new StringBuilder();
						builder.Append('f');
						builder.Append(first.Name);
						builder.Append(',');
						builder.Append(str);
						builder.Append(',');

						if (first.CultureInfo != null)
						{
							builder.Append(first.CultureInfo.ToString());
						}

						builder.Append(',');
						byte[] publicKeyToken = first.GetPublicKeyToken();

						for (int i = 0; i < publicKeyToken.Length; i++)
						{
							builder.Append(publicKeyToken[i].ToString("x2", CultureInfo.InvariantCulture));
						}

						str3 = builder.ToString();
					}
				}
				else
				{
					str3 = "p" + first.Name;
				}

				str2 = FormatWebResourceUrl(str3, resourceName, second, htmlEncoded);
				_urlCache[num2] = str2;
			}

			return str2;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static bool IsValidWebResourceRequest(HttpContext context)
		{
			EnsureHandlerExistenceChecked();

			if (!_handlerExists)
			{
				return false;
			}

			string b = UrlPath.Combine(HttpRuntime.AppDomainAppVirtualPath, WebResourceUrl);
			return string.Equals(context.Request.Path, b, StringComparison.OrdinalIgnoreCase);
		}

		#endregion

		#region IHttpHandler Members

		bool IHttpHandler.IsReusable
		{
			// Return false in case your Managed Handler cannot be reused for another request.
			// Usually this would be false in case you have some state information preserved per request.
			get { return true; }
		}

		void IHttpHandler.ProcessRequest(HttpContext context)
		{
			context.Response.Clear();
			string encryptedQueryString = context.Request.QueryString["d"];

			if (string.IsNullOrEmpty(encryptedQueryString))
			{
				throw new HttpException(0x194, SR.AssemblyResourceLoader_InvalidRequest);
			}

			string decryptedQueryString = MachineKeyEncryption.DecryptString(encryptedQueryString);
			int index = decryptedQueryString.IndexOf('|');
			string longAssemblyName = decryptedQueryString.Substring(0, index);

			if (string.IsNullOrEmpty(longAssemblyName))
			{
				throw new HttpException(0x194, SR.GetString(SR.AssemblyResourceLoader_AssemblyNotFound, new object[] { longAssemblyName }));
			}

			string webResource = decryptedQueryString.Substring(index + 1);

			if (string.IsNullOrEmpty(webResource))
			{
				throw new HttpException(0x194, SR.GetString(SR.AssemblyResourceLoader_ResourceNotFound, new object[] { webResource }));
			}

			char ch = longAssemblyName[0];
			longAssemblyName = longAssemblyName.Substring(1);

			Assembly assembly = null;

			switch (ch)
			{
				case 's':
					assembly = typeof(WebResourceLoader).Assembly;
					//assembly = HttpContext.SystemWebAssembly;
					break;

				case 'p':
					assembly = Assembly.Load(longAssemblyName);
					break;

				case 'f':
					{
						string[] strArray = longAssemblyName.Split(new char[] { ',' });

						if (strArray.Length != 4)
						{
							throw new HttpException(0x194, SR.AssemblyResourceLoader_InvalidRequest);
						}

						AssemblyName assemblyRef = new AssemblyName
						{
							Name = strArray[0],
							Version = new Version(strArray[1])
						};

						string name = strArray[2];

						if (name.Length > 0)
						{
							assemblyRef.CultureInfo = new CultureInfo(name);
						}
						else
						{
							assemblyRef.CultureInfo = CultureInfo.InvariantCulture;
						}

						string str6 = strArray[3];
						byte[] publicKeyToken = new byte[str6.Length / 2];

						for (int i = 0; i < publicKeyToken.Length; i++)
						{
							publicKeyToken[i] = byte.Parse(str6.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
						}

						assemblyRef.SetPublicKeyToken(publicKeyToken);
						assembly = Assembly.Load(assemblyRef);
						break;
					}

				default:
					throw new HttpException(0x194, SR.AssemblyResourceLoader_InvalidRequest);
			}

			bool third = false;
			bool first = false;
			string second = string.Empty;

			if (assembly != null)
			{
				int num3 = HashCodeCombiner.CombineHashCodes(assembly.GetHashCode(), webResource.GetHashCode());
				Triplet triplet = (Triplet)_webResourceCache[num3];

				if (triplet != null)
				{
					first = (bool)triplet.First;
					second = (string)triplet.Second;
					third = (bool)triplet.Third;
				}
				else
				{
					object[] customAttributes = assembly.GetCustomAttributes(false);

					for (int j = 0; j < customAttributes.Length; j++)
					{
						WebResourceAttribute attribute = customAttributes[j] as WebResourceAttribute;

						if ((attribute != null) && (string.Compare(attribute.WebResource, webResource, StringComparison.Ordinal) == 0))
						{
							webResource = attribute.WebResource;
							first = true;
							second = attribute.ContentType;
							third = attribute.PerformSubstitution;
							break;
						}
					}

					Triplet triplet2 = new Triplet
					{
						First = first,
						Second = second,
						Third = third
					};

					_webResourceCache[num3] = triplet2;
				}

				if (!first)
				{
					throw new HttpException(404, SR.GetString(SR.AssemblyResourceLoader_InvalidRequest, new object[] { longAssemblyName }));
				}

				HttpCachePolicy cache = context.Response.Cache;
				cache.SetCacheability(HttpCacheability.Public);
				cache.VaryByParams["d"] = true;
				cache.SetOmitVaryStar(true);
				cache.SetExpires(DateTime.Now + TimeSpan.FromDays(365.0));
				cache.SetValidUntilExpires(true);

				Pair assemblyInfo = GetAssemblyInfo(assembly);
				cache.SetLastModified(new DateTime((long)assemblyInfo.Second));

				Stream manifestResourceStream = null;
				StreamReader reader = null;

				try
				{
					manifestResourceStream = assembly.GetManifestResourceStream(webResource);

					if (manifestResourceStream != null)
					{
						context.Response.ContentType = second;

						if (third)
						{
							reader = new StreamReader(manifestResourceStream, true);
							string input = reader.ReadToEnd();
							MatchCollection matchs = webResourceRegex.Matches(input);
							int startIndex = 0;
							StringBuilder builder = new StringBuilder();

							foreach (Match match in matchs)
							{
								builder.Append(input.Substring(startIndex, match.Index - startIndex));
								Group group = match.Groups["resourceName"];

								if (group != null)
								{
									string a = group.ToString();

									if (a.Length > 0)
									{
										if (string.Equals(a, webResource, StringComparison.Ordinal))
										{
											throw new HttpException(0x194, SR.GetString(SR.AssemblyResourceLoader_NoCircularReferences, new object[] { webResource }));
										}

										builder.Append(GetWebResourceUrlInternal(assembly, a, false));
									}
								}
								startIndex = match.Index + match.Length;
							}

							builder.Append(input.Substring(startIndex, input.Length - startIndex));
							StreamWriter writer = new StreamWriter(context.Response.OutputStream, reader.CurrentEncoding);
							writer.Write(builder.ToString());
							writer.Flush();
						}
						else
						{
							byte[] buffer = new byte[0x400];
							Stream outputStream = context.Response.OutputStream;
							int count = 1;

							while (count > 0)
							{
								count = manifestResourceStream.Read(buffer, 0, 0x400);
								outputStream.Write(buffer, 0, count);
							}

							outputStream.Flush();
						}
					}
				}
				finally
				{
					if (reader != null)
					{
						reader.Close();
					}

					if (manifestResourceStream != null)
					{
						manifestResourceStream.Close();
					}
				}
			}

			//context.Response.IgnoreFurtherWrites();
		}

		#endregion

		#region Private Properties

		private static bool DebugMode
		{
			get { return HttpContext.Current.IsDebuggingEnabled; }
		}

		#endregion

		#region Private Methods

		private static int CreateWebResourceUrlCacheKey(Assembly assembly, string resourceName, bool htmlEncoded)
		{
			return HashCodeCombiner.CombineHashCodes(HashCodeCombiner.CombineHashCodes(assembly.GetHashCode(), resourceName.GetHashCode()), htmlEncoded.GetHashCode());
		}

		private static void EnsureHandlerExistenceChecked()
		{
			if (!_handlerExistenceChecked)
			{
				if (!AppDomain.CurrentDomain.PermissionSet.IsUnrestricted())
				{
					_handlerExists = true;
				}
				else
				{
					HttpContext current = HttpContext.Current;
					string path = VirtualPathUtility.MakeRelative(VirtualPathUtility.AppendTrailingSlash(current.Request.ApplicationPath), WebResourceUrl);

					if (IsIIS7Request(current))
					{
						object target = Type.GetType("Microsoft.Web.Administration.WebConfigurationManager, Microsoft.Web.Administration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35").InvokeMember("GetSection", BindingFlags.InvokeMethod, null, null, new object[] { "system.webServer/handlers" });
						IEnumerable enumerable = (IEnumerable)target.GetType().InvokeMember("GetCollection", BindingFlags.InvokeMethod, null, target, new object[0]);
						foreach (object obj3 in enumerable)
						{
							string str = (string)obj3.GetType().InvokeMember("GetAttributeValue", BindingFlags.InvokeMethod, null, obj3, new object[] { "path" });
							string typeName = (string)obj3.GetType().InvokeMember("GetAttributeValue", BindingFlags.InvokeMethod, null, obj3, new object[] { "type" });
							if ((str == path) && Type.GetType(typeName).Equals(typeof(WebResourceLoader)))
							{
								_handlerExists = true;
								break;
							}
						}
					}
					else
					{
						HttpHandlersSection section = (HttpHandlersSection)ConfigurationManager.GetSection("system.web/httpHandlers");

						foreach (HttpHandlerAction action in section.Handlers)
						{
							if (
								action.Path.Equals(WebResourceUrl, StringComparison.InvariantCultureIgnoreCase)
								&& action.Verb.Equals("GET", StringComparison.InvariantCultureIgnoreCase)
								&& Type.GetType(action.Type).Equals(typeof(WebResourceLoader))
							)
							{
								_handlerExists = true;
								break;
							}
						}
					}
				}

				_handlerExistenceChecked = true;
			}
		}

		private static string FormatWebResourceUrl(string assemblyName, string resourceName, long assemblyDate, bool htmlEncoded)
		{
			string str = MachineKeyEncryption.EncryptString(assemblyName + "|" + resourceName);

			if (htmlEncoded)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}?d={1}&amp;t={2}", WebResourceUrl, str, assemblyDate);
			}

			return string.Format(CultureInfo.InvariantCulture, "{0}?d={1}&t={2}", WebResourceUrl, str, assemblyDate);
		}

		private static Pair GetAssemblyInfo(Assembly assembly)
		{
			Pair assemblyInfoWithAssertInternal = _assemblyInfoCache[assembly] as Pair;
			if (assemblyInfoWithAssertInternal == null)
			{
				assemblyInfoWithAssertInternal = GetAssemblyInfoWithAssertInternal(assembly);
				_assemblyInfoCache[assembly] = assemblyInfoWithAssertInternal;
			}
			return assemblyInfoWithAssertInternal;
		}

		[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
		private static Pair GetAssemblyInfoWithAssertInternal(Assembly assembly)
		{
			AssemblyName x = assembly.GetName();
			return new Pair(x, File.GetLastWriteTime(new Uri(x.CodeBase).LocalPath).Ticks);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static bool IsIIS7Request(HttpContext context)
		{
			IServiceProvider provider = context;
			HttpWorkerRequest service = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));

			return service.GetType().ToString().Contains("System.Web.Hosting.IIS7WorkerRequest");
		}

		//[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
		//private static VirtualPath GetDiskResourcePath(string resourceName)
		//{
		//    VirtualPath path2 = Util.GetScriptLocation().SimpleCombine(resourceName);
		//    if (File.Exists(path2.MapPath()))
		//    {
		//        return path2;
		//    }
		//    return null;
		//}

		//internal static string GetWebResourceUrl(Type type, string resourceName)
		//{
		//    return GetWebResourceUrl(type, resourceName, false);
		//}

		//internal static string GetWebResourceUrl(Type type, string resourceName, bool htmlEncoded)
		//{
		//    Assembly assembly = (Assembly)_typeAssemblyCache[type];
		//    if (assembly == null)
		//    {
		//        assembly = type.Assembly;
		//        _typeAssemblyCache[type] = assembly;
		//    }
		//    if (assembly == typeof(WebResourceLoader).Assembly)
		//    {
		//        if (string.Equals(resourceName, "WebForms.js", StringComparison.Ordinal))
		//        {
		//            if (!_webFormsScriptChecked)
		//            {
		//                _webFormsScriptLocation = GetDiskResourcePath(resourceName);
		//                _webFormsScriptChecked = true;
		//            }
		//            if (_webFormsScriptLocation != null)
		//            {
		//                return _webFormsScriptLocation.VirtualPathString;
		//            }
		//        }
		//        else if (string.Equals(resourceName, "WebUIValidation.js", StringComparison.Ordinal))
		//        {
		//            if (!_webUIValidationScriptChecked)
		//            {
		//                _webUIValidationScriptLocation = GetDiskResourcePath(resourceName);
		//                _webUIValidationScriptChecked = true;
		//            }
		//            if (_webUIValidationScriptLocation != null)
		//            {
		//                return _webUIValidationScriptLocation.VirtualPathString;
		//            }
		//        }
		//        else if (string.Equals(resourceName, "SmartNav.htm", StringComparison.Ordinal))
		//        {
		//            if (!_smartNavPageChecked)
		//            {
		//                _smartNavPageLocation = GetDiskResourcePath(resourceName);
		//                _smartNavPageChecked = true;
		//            }
		//            if (_smartNavPageLocation != null)
		//            {
		//                return _smartNavPageLocation.VirtualPathString;
		//            }
		//        }
		//        else if (string.Equals(resourceName, "SmartNav.js", StringComparison.Ordinal))
		//        {
		//            if (!_smartNavScriptChecked)
		//            {
		//                _smartNavScriptLocation = GetDiskResourcePath(resourceName);
		//                _smartNavScriptChecked = true;
		//            }
		//            if (_smartNavScriptLocation != null)
		//            {
		//                return _smartNavScriptLocation.VirtualPathString;
		//            }
		//        }
		//    }
		//    return UrlPath.Combine(HttpRuntime.AppDomainAppVirtualPathString, GetWebResourceUrlInternal(assembly, resourceName, htmlEncoded));
		//}

		#endregion
	}
}
