using System;
using System.Configuration;
using System.Configuration.Internal;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Util;

using openSourceC.FrameworkLibrary.Web.Util;

namespace openSourceC.FrameworkLibrary.Web
{
	/// <summary>
	///		
	/// </summary>
	public class CachedPathData
	{
		///// <summary></summary>
		//public const int FInited = 1;
		///// <summary></summary>
		//public const int FCompletedFirstRequest = 2;
		///// <summary></summary>
		//public const int FExists = 4;
		///// <summary></summary>
		//public const int FOwnsConfigRecord = 0x10;
		///// <summary></summary>
		//public const int FClosed = 0x20;
		///// <summary></summary>
		//public const int FCloseNeeded = 0x40;

		private string _configPath;
		private SafeBitVector32 _flags;
		//private HandlerMappingMemo _handlerMemo;
		private string _physicalPath;
		//private System.Web.Configuration.RuntimeConfig _runtimeConfig = System.Web.Configuration.RuntimeConfig.GetErrorRuntimeConfig();
		private VirtualPath _virtualPath;
		//private static CacheItemRemovedCallback s_callback = new CacheItemRemovedCallback(CachedPathData.OnCacheItemRemoved);


		/// <summary>
		///		
		/// </summary>
		/// <param name="configPath"></param>
		/// <param name="virtualPath"></param>
		/// <param name="physicalPath"></param>
		/// <param name="exists"></param>
		public CachedPathData(string configPath, VirtualPath virtualPath, string physicalPath, bool exists)
		{
			this._configPath = configPath;
			this._virtualPath = virtualPath;
			this._physicalPath = physicalPath;
			this._flags[4] = exists;
			string schemeDelimiter = Uri.SchemeDelimiter;
		}

		//private void Close()
		//{
		//    if ((this._flags[1] && this._flags.ChangeValue(0x20, true)) && this._flags[0x10])
		//    {
		//        this.ConfigRecord.Remove();
		//    }
		//}

		private static string CreateKey(string configPath)
		{
			return ("d" + configPath);
		}

		public static CachedPathData GetApplicationPathData()
		{
			if (!HostingEnvironment.IsHosted)
			{
				return GetRootWebPathData();
			}
			return GetConfigPathData(HostingEnvironment..AppConfigPath);
		}

		private static CachedPathData GetConfigPathData(string configPath)
		{
			string key = CreateKey(configPath);
			Cache cache = HttpRuntime.Cache;
			CachedPathData data = (CachedPathData)cache.Get(key);
			if (data != null)
			{
				data.WaitForInit();
				return data;
			}
			CachedPathData parentData = null;
			CacheDependency dependencies = null;
			VirtualPath vpath = null;
			string physicalPath = null;
			bool exists = false;
			string[] filenames = null;
			string[] cachekeys = null;
			string siteID = null;
			bool flag2 = false;
			if (WebConfigurationHost.IsMachineConfigPath(configPath))
			{
				exists = true;
				flag2 = true;
			}
			else
			{
				string parent = System.Configuration.ConfigPathUtility.GetParent(configPath);
				parentData = GetConfigPathData(parent);
				string str5 = CreateKey(parent);
				cachekeys = new string[] { str5 };
				if (!WebConfigurationHost.IsVirtualPathConfigPath(configPath))
				{
					exists = true;
					flag2 = true;
				}
				else
				{
					WebConfigurationHost.GetSiteIDAndVPathFromConfigPath(configPath, out siteID, out vpath);
					try
					{
						physicalPath = vpath.MapPath();
					}
					catch (HttpException exception)
					{
						if (exception.GetHttpCode() == 500)
						{
							throw new HttpException(0x194, string.Empty);
						}
						throw;
					}
					FileUtil.CheckSuspiciousPhysicalPath(physicalPath);
					bool isDirectory = false;
					if (string.IsNullOrEmpty(physicalPath))
					{
						exists = false;
					}
					else
					{
						FileUtil.PhysicalPathStatus(physicalPath, false, false, out exists, out isDirectory);
					}
					if (exists && !isDirectory)
					{
						filenames = new string[] { physicalPath };
					}
				}
				try
				{
					dependencies = new CacheDependency(0, filenames, cachekeys);
				}
				catch
				{
				}
			}
			CachedPathData data3 = null;
			bool flag4 = false;
			bool flag5 = false;
			CacheItemPriority priority = flag2 ? CacheItemPriority.NotRemovable : CacheItemPriority.Normal;
			try
			{
				using (dependencies)
				{
					data3 = new CachedPathData(configPath, vpath, physicalPath, exists);
					try
					{
					}
					finally
					{
						data = (CachedPathData)cache.UtcAdd(key, data3, dependencies, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, priority, s_callback);
						if (data == null)
						{
							flag4 = true;
						}
					}
				}
				if (!flag4)
				{
					data.WaitForInit();
					return data;
				}
				lock (data3)
				{
					try
					{
						data3.Init(parentData);
						flag5 = true;
					}
					finally
					{
						data3._flags[1] = true;
						Monitor.PulseAll(data3);
						if (data3._flags[0x40])
						{
							data3.Close();
						}
					}
					return data3;
				}
			}
			finally
			{
				if (flag4)
				{
					if (!data3._flags[1])
					{
						lock (data3)
						{
							data3._flags[1] = true;
							Monitor.PulseAll(data3);
							if (data3._flags[0x40])
							{
								data3.Close();
							}
						}
					}
					if (!flag5 || ((data3.ConfigRecord != null) && data3.ConfigRecord.HasInitErrors))
					{
						if (dependencies != null)
						{
							if (!flag5)
							{
								dependencies = new CacheDependency(0, null, cachekeys);
							}
							else
							{
								dependencies = new CacheDependency(0, filenames, cachekeys);
							}
						}
						using (dependencies)
						{
							cache.UtcInsert(key, data3, dependencies, DateTime.UtcNow.AddSeconds(5.0), Cache.NoSlidingExpiration, CacheItemPriority.Normal, s_callback);
						}
					}
				}
			}
			return data3;
		}

		//public static CachedPathData GetMachinePathData()
		//{
		//    return GetConfigPathData("machine");
		//}

		public static CachedPathData GetRootWebPathData()
		{
			return GetConfigPathData("machine/webroot");
		}

		//public static CachedPathData GetVirtualPathData(VirtualPath virtualPath, bool permitPathsOutsideApp)
		//{
		//    if (!HostingEnvironment.IsHosted)
		//    {
		//        return GetRootWebPathData();
		//    }
		//    if (virtualPath != null)
		//    {
		//        virtualPath.FailIfRelativePath();
		//    }
		//    if ((virtualPath != null) && virtualPath.IsWithinAppRoot)
		//    {
		//        return GetConfigPathData(WebConfigurationHost.GetConfigPathFromSiteIDAndVPath(HostingEnvironment.SiteID, virtualPath));
		//    }
		//    if (!permitPathsOutsideApp)
		//    {
		//        throw new ArgumentException(System.Web.SR.GetString("Cross_app_not_allowed", new object[] { (virtualPath != null) ? virtualPath.VirtualPathString : "null" }));
		//    }
		//    return GetApplicationPathData();
		//}

		//private void Init(CachedPathData parentData)
		//{
		//    if (!HttpConfigurationSystem.UseHttpConfigurationSystem)
		//    {
		//        this._runtimeConfig = null;
		//    }
		//    else
		//    {
		//        IInternalConfigRecord uniqueConfigRecord = HttpConfigurationSystem.GetUniqueConfigRecord(this._configPath);
		//        if (uniqueConfigRecord.ConfigPath.Length == this._configPath.Length)
		//        {
		//            this._flags[0x10] = true;
		//            this._runtimeConfig = new System.Web.Configuration.RuntimeConfig(uniqueConfigRecord);
		//        }
		//        else
		//        {
		//            this._runtimeConfig = parentData._runtimeConfig;
		//        }
		//    }
		//}

		//public static void MarkCompleted(CachedPathData pathData)
		//{
		//    CacheInternal cacheInternal = HttpRuntime.CacheInternal;
		//    string configPath = pathData._configPath;
		//Label_000D:
		//    pathData.CompletedFirstRequest = true;
		//    configPath = System.Configuration.ConfigPathUtility.GetParent(configPath);
		//    if (configPath != null)
		//    {
		//        string key = CreateKey(configPath);
		//        pathData = (CachedPathData)cacheInternal.Get(key);
		//        if ((pathData != null) && !pathData.CompletedFirstRequest)
		//        {
		//            goto Label_000D;
		//        }
		//    }
		//}

		//private static void OnCacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
		//{
		//    CachedPathData data = (CachedPathData)value;
		//    data._flags[0x40] = true;
		//    data.Close();
		//}

		//public static void RemoveBadPathData(CachedPathData pathData)
		//{
		//    CacheInternal cacheInternal = HttpRuntime.CacheInternal;
		//    string configPath = pathData._configPath;
		//    string key = CreateKey(configPath);
		//    while (((pathData != null) && !pathData.CompletedFirstRequest) && !pathData.Exists)
		//    {
		//        cacheInternal.Remove(key);
		//        configPath = System.Configuration.ConfigPathUtility.GetParent(configPath);
		//        if (configPath == null)
		//        {
		//            return;
		//        }
		//        key = CreateKey(configPath);
		//        pathData = (CachedPathData)cacheInternal.Get(key);
		//    }
		//}

		private void WaitForInit()
		{
			if (!this._flags[1])
			{
				lock (this)
				{
					if (!this._flags[1])
					{
						Monitor.Wait(this);
					}
				}
			}
		}

		//public HandlerMappingMemo CachedHandler
		//{
		//    get
		//    {
		//        return this._handlerMemo;
		//    }
		//    set
		//    {
		//        this._handlerMemo = value;
		//    }
		//}

		//public bool CompletedFirstRequest
		//{
		//    get
		//    {
		//        return this._flags[2];
		//    }
		//    set
		//    {
		//        this._flags[2] = value;
		//    }
		//}

		//public IInternalConfigRecord ConfigRecord
		//{
		//    get
		//    {
		//        if (this._runtimeConfig == null)
		//        {
		//            return null;
		//        }
		//        return this._runtimeConfig.ConfigRecord;
		//    }
		//}

		//public bool Exists
		//{
		//    get
		//    {
		//        return this._flags[4];
		//    }
		//}

		//public VirtualPath Path
		//{
		//    get
		//    {
		//        return this._virtualPath;
		//    }
		//}

		//public string PhysicalPath
		//{
		//    get
		//    {
		//        return this._physicalPath;
		//    }
		//}

		//public System.Web.Configuration.RuntimeConfig RuntimeConfig
		//{
		//    get
		//    {
		//        return this._runtimeConfig;
		//    }
		//}
	}
}
