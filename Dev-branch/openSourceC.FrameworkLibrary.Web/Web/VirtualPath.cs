using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Util;

using Microsoft.Win32;
using openSourceC.FrameworkLibrary.Web.Util;

namespace openSourceC.FrameworkLibrary.Web
{
	/// <summary>
	///		
	/// </summary>
	[Serializable]
	public sealed class VirtualPath : IComparable
	{
		/// <summary></summary>
		public static VirtualPath RootVirtualPath = Create("/");

		private string _appRelativeVirtualPath;
		private string _virtualPath;
		private const int appRelativeAttempted = 4;
		private SimpleBitVector32 flags;
		private const int isWithinAppRoot = 2;
		private const int isWithinAppRootComputed = 1;
		private static char[] s_illegalVirtualPathChars = new char[] { ':', '?', '*', '\0' };
		private static char[] s_illegalVirtualPathChars_VerCompat = new char[1];
		private static object s_VerCompatLock = new object();
		private static bool s_VerCompatRegLookedUp = false;


		private VirtualPath() { }

		private VirtualPath(string virtualPath)
		{
			if (UrlPath.IsAppRelativePath(virtualPath))
			{
				this._appRelativeVirtualPath = virtualPath;
			}
			else
			{
				this._virtualPath = virtualPath;
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="relativePath"></param>
		/// <returns></returns>
		public VirtualPath Combine(VirtualPath relativePath)
		{
			if (relativePath == null)
			{
				throw new ArgumentNullException("relativePath");
			}
			if (!relativePath.IsRelative)
			{
				return relativePath;
			}
			this.FailIfRelativePath();
			return new VirtualPath(UrlPath.Combine(this.VirtualPathStringWhicheverAvailable, relativePath.VirtualPathString));
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns></returns>
		public static VirtualPath Combine(VirtualPath v1, VirtualPath v2)
		{
			if (v1 == null)
			{
				v1 = VirtualPath.Create(HttpRuntime.AppDomainAppVirtualPath);
			}
			if (v1 == null)
			{
				v2.FailIfRelativePath();
				return v2;
			}
			return v1.Combine(v2);
		}

		/// <summary>
		///		
		/// </summary>
		/// <returns></returns>
		public VirtualPath CombineWithAppRoot()
		{
			return VirtualPath.Create(HttpRuntime.AppDomainAppVirtualPath).Combine(this);
		}

		private static bool ContainsIllegalVirtualPathChars(string virtualPath)
		{
			if (!s_VerCompatRegLookedUp)
			{
				LookUpRegForVerCompat();
			}
			return (virtualPath.IndexOfAny(s_illegalVirtualPathChars) >= 0);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <param name="mask"></param>
		private void CopyFlagsFrom(VirtualPath virtualPath, int mask)
		{
			this.flags.IntegerValue |= virtualPath.flags.IntegerValue & mask;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static VirtualPath Create(string virtualPath)
		{
			return Create(virtualPath, VirtualPathOptions.AllowAllPath);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static VirtualPath Create(string virtualPath, VirtualPathOptions options)
		{
			if (virtualPath != null)
			{
				virtualPath = virtualPath.Trim();
			}
			if (string.IsNullOrEmpty(virtualPath))
			{
				if ((options & VirtualPathOptions.AllowNull) == 0)
				{
					throw new ArgumentNullException("virtualPath");
				}
				return null;
			}
			if (ContainsIllegalVirtualPathChars(virtualPath))
			{
				throw new HttpException(SR.GetString("Invalid_vpath", new object[] { virtualPath }));
			}
			string objB = UrlPath.FixVirtualPathSlashes(virtualPath);
			if (((options & VirtualPathOptions.FailIfMalformed) != 0) && !object.ReferenceEquals(virtualPath, objB))
			{
				throw new HttpException(SR.GetString("Invalid_vpath", new object[] { virtualPath }));
			}
			virtualPath = objB;
			if ((options & VirtualPathOptions.EnsureTrailingSlash) != 0)
			{
				virtualPath = UrlPath.AppendSlashToPathIfNeeded(virtualPath);
			}
			VirtualPath path = new VirtualPath();
			if (UrlPath.IsAppRelativePath(virtualPath))
			{
				virtualPath = UrlPath.ReduceVirtualPath(virtualPath);
				if (virtualPath[0] == '~')
				{
					if ((options & VirtualPathOptions.AllowAppRelativePath) == 0)
					{
						throw new ArgumentException(SR.GetString("VirtualPath_AllowAppRelativePath", new object[] { virtualPath }));
					}
					path._appRelativeVirtualPath = virtualPath;
					return path;
				}
				if ((options & VirtualPathOptions.AllowAbsolutePath) == 0)
				{
					throw new ArgumentException(SR.GetString("VirtualPath_AllowAbsolutePath", new object[] { virtualPath }));
				}
				path._virtualPath = virtualPath;
				return path;
			}
			if (virtualPath[0] != '/')
			{
				if ((options & VirtualPathOptions.AllowRelativePath) == 0)
				{
					throw new ArgumentException(SR.GetString("VirtualPath_AllowRelativePath", new object[] { virtualPath }));
				}
				path._virtualPath = virtualPath;
				return path;
			}
			if ((options & VirtualPathOptions.AllowAbsolutePath) == 0)
			{
				throw new ArgumentException(SR.GetString("VirtualPath_AllowAbsolutePath", new object[] { virtualPath }));
			}
			path._virtualPath = UrlPath.ReduceVirtualPath(virtualPath);
			return path;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static VirtualPath CreateAbsolute(string virtualPath)
		{
			return Create(virtualPath, VirtualPathOptions.AllowAbsolutePath);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static VirtualPath CreateAbsoluteAllowNull(string virtualPath)
		{
			return Create(virtualPath, VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.AllowNull);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static VirtualPath CreateAbsoluteTrailingSlash(string virtualPath)
		{
			return Create(virtualPath, VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.EnsureTrailingSlash);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static VirtualPath CreateAllowNull(string virtualPath)
		{
			return Create(virtualPath, VirtualPathOptions.AllowAllPath | VirtualPathOptions.AllowNull);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static VirtualPath CreateNonRelative(string virtualPath)
		{
			return Create(virtualPath, VirtualPathOptions.AllowAppRelativePath | VirtualPathOptions.AllowAbsolutePath);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static VirtualPath CreateNonRelativeAllowNull(string virtualPath)
		{
			return Create(virtualPath, VirtualPathOptions.AllowAppRelativePath | VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.AllowNull);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static VirtualPath CreateNonRelativeTrailingSlash(string virtualPath)
		{
			return Create(virtualPath, VirtualPathOptions.AllowAppRelativePath | VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.EnsureTrailingSlash);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static VirtualPath CreateNonRelativeTrailingSlashAllowNull(string virtualPath)
		{
			return Create(virtualPath, VirtualPathOptions.AllowAppRelativePath | VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.EnsureTrailingSlash | VirtualPathOptions.AllowNull);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static VirtualPath CreateTrailingSlash(string virtualPath)
		{
			return Create(virtualPath, VirtualPathOptions.AllowAllPath | VirtualPathOptions.EnsureTrailingSlash);
		}

		/// <summary>
		///		
		/// </summary>
		/// <returns></returns>
		public bool DirectoryExists()
		{
			return HostingEnvironment.VirtualPathProvider.DirectoryExists(this.VirtualPathString);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool Equals(object value)
		{
			if (value == null)
			{
				return false;
			}
			VirtualPath path = value as VirtualPath;
			if (path == null)
			{
				return false;
			}
			return EqualsHelper(path, this);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns></returns>
		public static bool Equals(VirtualPath v1, VirtualPath v2)
		{
			return ((v1 == v2) || (((v1 != null) && (v2 != null)) && EqualsHelper(v1, v2)));
		}

		private static bool EqualsHelper(VirtualPath v1, VirtualPath v2)
		{
			return (StringComparer.InvariantCultureIgnoreCase.Compare(v1.VirtualPathString, v2.VirtualPathString) == 0);
		}

		/// <summary>
		///		
		/// </summary>
		public void FailIfNotWithinAppRoot()
		{
			if (!this.IsWithinAppRoot)
			{
				throw new ArgumentException(SR.GetString("Cross_app_not_allowed", new object[] { this.VirtualPathString }));
			}
		}

		/// <summary>
		///		
		/// </summary>
		public void FailIfRelativePath()
		{
			if (this.IsRelative)
			{
				throw new ArgumentException(SR.GetString("VirtualPath_AllowRelativePath", new object[] { this._virtualPath }));
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <returns></returns>
		public bool FileExists()
		{
			return HostingEnvironment.VirtualPathProvider.FileExists(this.VirtualPathString);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static string GetAppRelativeVirtualPathString(VirtualPath virtualPath)
		{
			if (virtualPath != null)
			{
				return virtualPath.AppRelativeVirtualPathString;
			}
			return null;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static string GetAppRelativeVirtualPathStringOrEmpty(VirtualPath virtualPath)
		{
			if (virtualPath != null)
			{
				return virtualPath.AppRelativeVirtualPathString;
			}
			return string.Empty;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPathDependencies"></param>
		/// <param name="utcStart"></param>
		/// <returns></returns>
		public CacheDependency GetCacheDependency(IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			return HostingEnvironment.VirtualPathProvider.GetCacheDependency(this.VirtualPathString, virtualPathDependencies, utcStart);
		}

		/// <summary>
		///		
		/// </summary>
		/// <returns></returns>
		public string GetCacheKey()
		{
			return HostingEnvironment.VirtualPathProvider.GetCacheKey(this.VirtualPathString);
		}

		/// <summary>
		///		
		/// </summary>
		/// <returns></returns>
		public VirtualDirectory GetDirectory()
		{
			return HostingEnvironment.VirtualPathProvider.GetDirectory(this.VirtualPathString);
		}

		/// <summary>
		///		
		/// </summary>
		/// <returns></returns>
		public VirtualFile GetFile()
		{
			return HostingEnvironment.VirtualPathProvider.GetFile(this.VirtualPathString);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPathDependencies"></param>
		/// <returns></returns>
		public string GetFileHash(IEnumerable virtualPathDependencies)
		{
			return HostingEnvironment.VirtualPathProvider.GetFileHash(this.VirtualPathString, virtualPathDependencies);
		}

		/// <summary>
		///		
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return StringComparer.InvariantCultureIgnoreCase.GetHashCode(this.VirtualPathString);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static string GetVirtualPathString(VirtualPath virtualPath)
		{
			if (virtualPath != null)
			{
				return virtualPath.VirtualPathString;
			}
			return null;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static string GetVirtualPathStringNoTrailingSlash(VirtualPath virtualPath)
		{
			if (virtualPath != null)
			{
				return virtualPath.VirtualPathStringNoTrailingSlash;
			}
			return null;
		}

		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		private static void LookUpRegForVerCompat()
		{
			lock (s_VerCompatLock)
			{
				if (!s_VerCompatRegLookedUp)
				{
					try
					{
						object obj2 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\ASP.NET", "VerificationCompatibility", 0);
						if (((obj2 != null) && ((obj2 is int) || (obj2 is uint))) && (((int)obj2) == 1))
						{
							s_illegalVirtualPathChars = s_illegalVirtualPathChars_VerCompat;
						}
						s_VerCompatRegLookedUp = true;
					}
					catch
					{
					}
				}
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="toVirtualPath"></param>
		/// <returns></returns>
		public VirtualPath MakeRelative(VirtualPath toVirtualPath)
		{
			VirtualPath path = new VirtualPath();
			this.FailIfRelativePath();
			toVirtualPath.FailIfRelativePath();
			path._virtualPath = UrlPath.MakeRelative(this.VirtualPathString, toVirtualPath.VirtualPathString);
			return path;
		}

		/// <summary>
		///		
		/// </summary>
		/// <returns></returns>
		public string MapPath()
		{
			return HostingEnvironment.MapPath(this.VirtualPathString);
		}

		//public string MapPathInternal()
		//{
		//    return HostingEnvironment.MapPath(this.VirtualPathString);
		//}

		//public string MapPathInternal(bool permitNull)
		//{
		//    return HostingEnvironment.MapPath(this.VirtualPathString, permitNull);
		//}

		//public string MapPathInternal(VirtualPath baseVirtualDir, bool allowCrossAppMapping)
		//{
		//    return HostingEnvironment.MapPath(this.VirtualPathString, baseVirtualDir, allowCrossAppMapping);
		//}

		/// <summary>
		///		
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns></returns>
		public static bool operator ==(VirtualPath v1, VirtualPath v2)
		{
			return Equals(v1, v2);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns></returns>
		public static bool operator !=(VirtualPath v1, VirtualPath v2)
		{
			return !Equals(v1, v2);
		}

		/// <summary>
		///		
		/// </summary>
		/// <returns></returns>
		public Stream OpenFile()
		{
			return VirtualPathProvider.OpenFile(this.VirtualPathString);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="relativePath"></param>
		/// <returns></returns>
		public VirtualPath SimpleCombine(string relativePath)
		{
			return this.SimpleCombine(relativePath, false);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="addTrailingSlash"></param>
		/// <returns></returns>
		private VirtualPath SimpleCombine(string filename, bool addTrailingSlash)
		{
			string virtualPath = this.VirtualPathStringWhicheverAvailable + filename;
			if (addTrailingSlash)
			{
				virtualPath = virtualPath + "/";
			}
			VirtualPath path = new VirtualPath(virtualPath);
			path.CopyFlagsFrom(this, 7);
			return path;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="directoryName"></param>
		/// <returns></returns>
		public VirtualPath SimpleCombineWithDir(string directoryName)
		{
			return this.SimpleCombine(directoryName, true);
		}

		int IComparable.CompareTo(object obj)
		{
			VirtualPath path = obj as VirtualPath;
			if (path == null)
			{
				throw new ArgumentException();
			}
			if (path == this)
			{
				return 0;
			}
			return StringComparer.InvariantCultureIgnoreCase.Compare(this.VirtualPathString, path.VirtualPathString);
		}

		/// <summary>
		///		
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if ((this._virtualPath == null) && (HttpRuntime.AppDomainAppVirtualPath == null))
			{
				return this._appRelativeVirtualPath;
			}
			return this.VirtualPathString;
		}

		[Conditional("DBG")]
		private void ValidateState()
		{
		}

		/// <summary>
		///		
		/// </summary>
		public string AppRelativeVirtualPathString
		{
			get
			{
				string appRelativeVirtualPathStringOrNull = this.AppRelativeVirtualPathStringOrNull;
				if (appRelativeVirtualPathStringOrNull == null)
				{
					return this._virtualPath;
				}
				return appRelativeVirtualPathStringOrNull;
			}
		}

		/// <summary>
		///		
		/// </summary>
		public string AppRelativeVirtualPathStringIfAvailable
		{
			get { return this._appRelativeVirtualPath; }
		}

		/// <summary>
		///		
		/// </summary>
		public string AppRelativeVirtualPathStringOrNull
		{
			get
			{
				if (this._appRelativeVirtualPath == null)
				{
					if (this.flags[4])
					{
						return null;
					}
					if (HttpRuntime.AppDomainAppVirtualPath == null)
					{
						throw new HttpException(SR.GetString("VirtualPath_CantMakeAppRelative", new object[] { this._virtualPath }));
					}
					this._appRelativeVirtualPath = UrlPath.MakeVirtualPathAppRelativeOrNull(this._virtualPath);
					this.flags[4] = true;
					if (this._appRelativeVirtualPath == null)
					{
						return null;
					}
				}
				return this._appRelativeVirtualPath;
			}
		}

		/// <summary>
		///		
		/// </summary>
		public string Extension
		{
			get { return UrlPath.GetExtension(this.VirtualPathString); }
		}

		/// <summary>
		///		
		/// </summary>
		public string FileName
		{
			get { return UrlPath.GetFileName(this.VirtualPathStringNoTrailingSlash); }
		}

		/// <summary>
		///		
		/// </summary>
		public bool HasTrailingSlash
		{
			get
			{
				if (this._virtualPath != null)
				{
					return UrlPath.HasTrailingSlash(this._virtualPath);
				}
				return UrlPath.HasTrailingSlash(this._appRelativeVirtualPath);
			}
		}

		/// <summary>
		///		
		/// </summary>
		public bool IsRelative
		{
			get { return ((this._virtualPath != null) && (this._virtualPath[0] != '/')); }
		}

		/// <summary>
		///		
		/// </summary>
		public bool IsRoot
		{
			get { return (this._virtualPath == "/"); }
		}

		/// <summary>
		///		
		/// </summary>
		public bool IsWithinAppRoot
		{
			get
			{
				if (!this.flags[1])
				{
					if (HttpRuntime.AppDomainId == null)
					{
						return true;
					}
					if (this.flags[4])
					{
						this.flags[2] = this._appRelativeVirtualPath != null;
					}
					else
					{
						this.flags[2] = UrlPath.IsEqualOrSubpath(HttpRuntime.AppDomainAppVirtualPath, this.VirtualPathString);
					}
					this.flags[1] = true;
				}
				return this.flags[2];
			}
		}

		/// <summary>
		///		
		/// </summary>
		public VirtualPath Parent
		{
			get
			{
				this.FailIfRelativePath();
				if (this.IsRoot)
				{
					return null;
				}
				string virtualPathStringNoTrailingSlash = UrlPath.RemoveSlashFromPathIfNeeded(this.VirtualPathStringWhicheverAvailable);
				if (virtualPathStringNoTrailingSlash == "~")
				{
					virtualPathStringNoTrailingSlash = this.VirtualPathStringNoTrailingSlash;
				}
				int num = virtualPathStringNoTrailingSlash.LastIndexOf('/');
				if (num == 0)
				{
					return RootVirtualPath;
				}
				return new VirtualPath(virtualPathStringNoTrailingSlash.Substring(0, num + 1));
			}
		}

		/// <summary>
		///		
		/// </summary>
		public string VirtualPathString
		{
			get
			{
				if (this._virtualPath == null)
				{
					if (HttpRuntime.AppDomainAppVirtualPath == null)
					{
						throw new HttpException(SR.GetString("VirtualPath_CantMakeAppAbsolute", new object[] { this._appRelativeVirtualPath }));
					}
					if (this._appRelativeVirtualPath.Length == 1)
					{
						this._virtualPath = HttpRuntime.AppDomainAppVirtualPath;
					}
					else
					{
						this._virtualPath = HttpRuntime.AppDomainAppVirtualPath + this._appRelativeVirtualPath.Substring(1);
					}
				}
				return this._virtualPath;
			}
		}

		/// <summary>
		///		
		/// </summary>
		public string VirtualPathStringIfAvailable
		{
			get { return this._virtualPath; }
		}

		/// <summary>
		///		
		/// </summary>
		public string VirtualPathStringNoTrailingSlash
		{
			get { return UrlPath.RemoveSlashFromPathIfNeeded(this.VirtualPathString); }
		}

		/// <summary>
		///		
		/// </summary>
		public string VirtualPathStringWhicheverAvailable
		{
			get
			{
				if (this._virtualPath == null)
				{
					return this._appRelativeVirtualPath;
				}
				return this._virtualPath;
			}
		}
	}
}
