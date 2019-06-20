using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Security.Permissions;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;
using System.Xml;

using openSourceC.FrameworkLibrary.Web.UI;
using openSourceC.FrameworkLibrary.Web.Util;

namespace openSourceC.FrameworkLibrary.Web
{
	/// <summary>
	///		The <see cref="T:System.Web.OscSiteMapProvider"/> class is derived from the
	///		<see cref="T:System.Web.SiteMapProvider"/> class and is the default site map provider
	///		for ASP.NET. The <see cref="T:System.Web.OscSiteMapProvider"/> class generates site map
	///		trees from XML files.
	///	</summary>
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class OscSiteMapProvider : OscStaticSiteMapProvider, IDisposable
	{
		private ArrayList _childProviderList;
		private Hashtable _childProviderTable;
		private XmlDocument _document;
		private string _filename;
		private FileChangeEventHandler _handler;
		private bool _initialized;
		private VirtualPath _normalizedVirtualPath;
		private StringCollection _parentSiteMapFileCollection;
		private const string _providerAttribute = "provider";
		private const char _resourceKeySeparator = ',';
		private const string _resourcePrefix = "$resources:";
		private const int _resourcePrefixLength = 10;
		private static readonly char[] _seperators = new char[] { ';', ',' };
		private const string _siteMapFileAttribute = "siteMapFile";
		private SiteMapNode _siteMapNode;
		private const string _siteMapNodeName = "siteMapNode";
		private VirtualPath _virtualPath;
		private const string _xmlSiteMapFileExtension = ".sitemap";


		/// <summary>Adds a <see cref="T:System.Web.SiteMapNode"></see> object to the collections that are maintained by the current provider.</summary>
		/// <param name="node">The <see cref="T:System.Web.SiteMapNode"></see> to add to the provider.</param>
		/// <param name="parentNode">The <see cref="T:System.Web.SiteMapNode"></see> under which to add node.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Web.OscSiteMapProvider"></see> is not the provider associated with node.</exception>
		/// <exception cref="T:System.InvalidOperationException">A node with the same URL or key is already registered with the <see cref="T:System.Web.OscSiteMapProvider"></see>. - or -A duplicate site map node has been encountered programmatically, such as when linking two site map providers.- or -node is the root node of the <see cref="T:System.Web.OscSiteMapProvider"></see>.</exception>
		/// <exception cref="T:System.ArgumentNullException">node or parentNode is null.</exception>
		protected override void AddNode(SiteMapNode node, SiteMapNode parentNode)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (parentNode == null)
			{
				throw new ArgumentNullException("parentNode");
			}
			SiteMapProvider provider = node.Provider;
			SiteMapProvider provider2 = parentNode.Provider;
			if (provider != this)
			{
				throw new ArgumentException(SR.GetString("XmlSiteMapProvider_cannot_add_node", new object[] { node.ToString() }), "node");
			}
			if (provider2 != this)
			{
				throw new ArgumentException(SR.GetString("XmlSiteMapProvider_cannot_add_node", new object[] { parentNode.ToString() }), "parentNode");
			}
			lock (base.providerLock)
			{
				this.RemoveNode(node);
				this.AddNodeInternal(node, parentNode, null);
			}
		}

		private void AddNodeInternal(SiteMapNode node, SiteMapNode parentNode, XmlNode xmlNode)
		{
			lock (base.providerLock)
			{
				string url = node.Url;
				string key = node.Key;
				bool flag = false;
				if (!string.IsNullOrEmpty(url))
				{
					if (base.UrlTable[url] != null)
					{
						if (xmlNode != null)
						{
							throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Multiple_Nodes_With_Identical_Url", new object[] { url }), xmlNode);
						}
						throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Multiple_Nodes_With_Identical_Url", new object[] { url }));
					}
					flag = true;
				}
				if (base.KeyTable.Contains(key))
				{
					if (xmlNode != null)
					{
						throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Multiple_Nodes_With_Identical_Key", new object[] { key }), xmlNode);
					}
					throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Multiple_Nodes_With_Identical_Key", new object[] { key }));
				}
				if (flag)
				{
					base.UrlTable[url] = node;
				}
				base.KeyTable[key] = node;
				if (parentNode != null)
				{
					base.ParentNodeTable[node] = parentNode;
					if (base.ChildNodeCollectionTable[parentNode] == null)
					{
						base.ChildNodeCollectionTable[parentNode] = new SiteMapNodeCollection();
					}
					((SiteMapNodeCollection)base.ChildNodeCollectionTable[parentNode]).Add(node);
				}
			}
		}

		/// <summary>Links a child site map provider to the current provider. </summary>
		/// <param name="parentNode">A site map node of the current site map provider under which the root node and all nodes of the child provider is added.</param>
		/// <param name="providerName">The name of one of the <see cref="T:System.Web.SiteMapProvider"></see> objects currently registered in the <see cref="P:System.Web.SiteMap.Providers"></see>.</param>
		/// <exception cref="T:System.InvalidOperationException">The site map file used by providerName is already in use within the provider hierarchy. -or-The root node returned by providerName is null.-or-The root node returned by providerName has a URL or key that is already registered with the parent <see cref="T:System.Web.OscSiteMapProvider"></see>.   </exception>
		/// <exception cref="T:System.Configuration.Provider.ProviderException">providerName cannot be resolved.</exception>
		/// <exception cref="T:System.ArgumentNullException">parentNode is null.</exception>
		protected virtual void AddProvider(string providerName, SiteMapNode parentNode)
		{
			if (parentNode == null)
			{
				throw new ArgumentNullException("parentNode");
			}
			if (parentNode.Provider != this)
			{
				throw new ArgumentException(SR.GetString("XmlSiteMapProvider_cannot_add_node", new object[] { parentNode.ToString() }), "parentNode");
			}
			SiteMapNode nodeFromProvider = this.GetNodeFromProvider(providerName);
			this.AddNodeInternal(nodeFromProvider, parentNode, null);
		}

		/// <summary>Loads the site map information from an XML file and builds it in memory.</summary>
		/// <returns>Returns the root <see cref="T:System.Web.SiteMapNode"></see> of the site map navigation structure.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Web.OscSiteMapProvider"></see> was not initialized properly.- or -A siteMapFile is parsed for a &lt;siteMapNode&gt; that is not unique.- or -The file specified by the siteMapFile does not have the file name extension .sitemap.- or -The file specified by the siteMapFile does not exist.- or -A provider configured in the provider of a &lt;siteMapNode&gt; returns a null root node. </exception>
		/// <exception cref="T:System.Configuration.ConfigurationErrorsException">There is an error loading the configuration file.- or -The top element of the configuration file is not named &lt;siteMap&gt;.- or - More than one top node exists in the configuration file.- or -A child of the &lt;siteMap&gt; has a name other than &lt;siteMapNode&gt;. - or -An unexpected attribute is parsed for the &lt;siteMapNode&gt;.- or -Sub-elements are nested beneath a &lt;siteMapNode&gt; where the provider is set.- or -The roles of the &lt;siteMapNode&gt; contain characters that are not valid.- or - A url is parsed for a &lt;siteMapNode&gt; that is not unique.- or - A <see cref="T:System.Web.SiteMapNode"></see> was encountered with a duplicate value for <see cref="P:System.Web.SiteMapNode.Key"></see>. - or -The <see cref="P:System.Web.SiteMapNode.ResourceKey"></see> or <see cref="P:System.Web.SiteMapNode.Title"></see> was specified on a <see cref="T:System.Web.SiteMapNode"></see> or a custom attribute defined for the node contained an explicit resource expression.- or -An explicit resource expression was applied either to the <see cref="P:System.Web.SiteMapNode.Title"></see> or <see cref="P:System.Web.SiteMapNode.Description"></see> or to a custom attribute of a <see cref="T:System.Web.SiteMapNode"></see> but the explicit information was not valid.- or -An error occurred while parsing the <see cref="P:System.Web.SiteMapNode.Url"></see> of a <see cref="T:System.Web.SiteMapNode"></see>.</exception>
		/// <exception cref="T:System.Web.HttpException">A siteMapFile of a &lt;siteMapNode&gt; uses a physical path.- or -An error occurred while attempting to parse the virtual path to the file specified in the siteMapFile.</exception>
		/// <exception cref="T:System.ArgumentNullException">A &lt;siteMapNode&gt; referencing a site map file contains an empty string for the siteMapFile.</exception>
		/// <exception cref="T:System.ArgumentException">The siteMapFile is specified but the path lies outside the current directory structure for the application.</exception>
		/// <exception cref="T:System.Configuration.Provider.ProviderException">A named provider cannot be found in the current site map providers collection. </exception>
		public override SiteMapNode BuildSiteMap()
		{
			SiteMapNode node = this._siteMapNode;
			//if (node != null)
			//{
			return node;
			//}
			//XmlDocument configDocument = this.GetConfigDocument();
			//lock (base.providerLock)
			//{
			//    if (this._siteMapNode == null)
			//    {
			//        this.Clear();
			//        this.CheckSiteMapFileExists();
			//        try
			//        {
			//            using (Stream stream = this._normalizedVirtualPath.OpenFile())
			//            {
			//                XmlReader reader = new XmlTextReader(stream);
			//                configDocument.Load(reader);
			//            }
			//        }
			//        catch (XmlException exception)
			//        {
			//            string virtualPathString = this._virtualPath.VirtualPathString;
			//            string path = this._normalizedVirtualPath.MapPath();
			//            if ((path != null))// && HttpRuntime.HasPathDiscoveryPermission(path))
			//            {
			//                virtualPathString = path;
			//            }
			//            throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Error_loading_Config_file", new object[] { this._virtualPath, exception.Message }), exception, virtualPathString, exception.LineNumber);
			//        }
			//        catch (Exception exception2)
			//        {
			//            throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Error_loading_Config_file", new object[] { this._virtualPath, exception2.Message }), exception2);
			//        }
			//        XmlNode node2 = null;
			//        foreach (XmlNode node3 in configDocument.ChildNodes)
			//        {
			//            if (string.Equals(node3.Name, "siteMap", StringComparison.Ordinal))
			//            {
			//                node2 = node3;
			//                break;
			//            }
			//        }
			//        if (node2 == null)
			//        {
			//            throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Top_Element_Must_Be_SiteMap"), configDocument);
			//        }
			//        bool val = false;
			//        System.Web.Configuration.HandlerBase.GetAndRemoveBooleanAttribute(node2, "enableLocalization", ref val);
			//        base.EnableLocalization = val;
			//        XmlNode node4 = null;
			//        foreach (XmlNode node5 in node2.ChildNodes)
			//        {
			//            if (node5.NodeType == XmlNodeType.Element)
			//            {
			//                if (!"siteMapNode".Equals(node5.Name))
			//                {
			//                    throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Only_SiteMapNode_Allowed"), node5);
			//                }
			//                if (node4 != null)
			//                {
			//                    throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Only_One_SiteMapNode_Required_At_Top"), node5);
			//                }
			//                node4 = node5;
			//            }
			//        }
			//        if (node4 == null)
			//        {
			//            throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Only_One_SiteMapNode_Required_At_Top"), node2);
			//        }
			//        Queue queue = new Queue(50);
			//        queue.Enqueue(null);
			//        queue.Enqueue(node4);
			//        this._siteMapNode = this.ConvertFromXmlNode(queue);
			//    }
			//    return this._siteMapNode;
			//}
		}

		private void CheckSiteMapFileExists()
		{
			if (!UI.Util.VirtualFileExistsWithAssert(this._normalizedVirtualPath))
			{
				throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_FileName_does_not_exist", new object[] { this._virtualPath }));
			}
		}

		/// <summary>Removes all elements in the collections of child and parent site map nodes and site map providers that the <see cref="T:System.Web.OscSiteMapProvider"></see> object internally tracks as part of its state.</summary>
		protected override void Clear()
		{
			lock (base.providerLock)
			{
				this.ChildProviderTable.Clear();
				this._siteMapNode = null;
				this._childProviderList = null;
				base.Clear();
			}
		}

		private SiteMapNode ConvertFromXmlNode(Queue queue)
		{
			SiteMapNode node = null;
			//while (queue.Count != 0)
			//{
			//    SiteMapNode parentNode = (SiteMapNode)queue.Dequeue();
			//    XmlNode node3 = (XmlNode)queue.Dequeue();
			//    SiteMapNode nodeFromProvider = null;
			//    if (!"siteMapNode".Equals(node3.Name))
			//    {
			//        throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Only_SiteMapNode_Allowed"), node3);
			//    }
			//    string val = null;
			//    System.Web.Configuration.HandlerBase.GetAndRemoveNonEmptyStringAttribute(node3, "provider", ref val);
			//    if (val != null)
			//    {
			//        nodeFromProvider = this.GetNodeFromProvider(val);
			//        System.Web.Configuration.HandlerBase.CheckForUnrecognizedAttributes(node3);
			//        System.Web.Configuration.HandlerBase.CheckForNonCommentChildNodes(node3);
			//    }
			//    else
			//    {
			//        string str2 = null;
			//        System.Web.Configuration.HandlerBase.GetAndRemoveNonEmptyStringAttribute(node3, "siteMapFile", ref str2);
			//        if (str2 != null)
			//        {
			//            nodeFromProvider = this.GetNodeFromSiteMapFile(node3, VirtualPath.Create(str2));
			//        }
			//        else
			//        {
			//            nodeFromProvider = this.GetNodeFromXmlNode(node3, queue);
			//        }
			//    }
			//    this.AddNodeInternal(nodeFromProvider, parentNode, node3);
			//    if (node == null)
			//    {
			//        node = nodeFromProvider;
			//    }
			//}
			return node;
		}

		/// <summary>Notifies the file monitor of the Web.sitemap file that the <see cref="T:System.Web.OscSiteMapProvider"></see> object no longer requires the file to be monitored.</summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Notifies the file monitor of the Web.sitemap file that the <see cref="T:System.Web.OscSiteMapProvider"></see> object no longer requires the file to be monitored. The <see cref="M:System.Web.OscSiteMapProvider.Dispose(System.Boolean)"></see> method takes a Boolean parameter indicating whether the method is called by user code.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (this._handler != null)
			{
				//HttpRuntime.FileChangesMonitor.StopMonitoringFile(this._filename, this._handler);
			}
		}

		private void EnsureChildSiteMapProviderUpToDate(SiteMapProvider childProvider)
		{
			SiteMapNode node = (SiteMapNode)this.ChildProviderTable[childProvider];
			SiteMapNode rootNodeCore = ((OscSiteMapProvider)childProvider).GetRootNodeCore();
			if (rootNodeCore == null)
			{
				throw new ProviderException(SR.GetString("XmlSiteMapProvider_invalid_sitemapnode_returned", new object[] { childProvider.Name }));
			}
			if (!node.Equals(rootNodeCore) && (node != null))
			{
				lock (base.providerLock)
				{
					node = (SiteMapNode)this.ChildProviderTable[childProvider];
					if (node != null)
					{
						rootNodeCore = ((OscSiteMapProvider)childProvider).GetRootNodeCore();
						if (rootNodeCore == null)
						{
							throw new ProviderException(SR.GetString("XmlSiteMapProvider_invalid_sitemapnode_returned", new object[] { childProvider.Name }));
						}
						if (!node.Equals(rootNodeCore))
						{
							if (this._siteMapNode.Equals(node))
							{
								base.UrlTable.Remove(node.Url);
								base.KeyTable.Remove(node.Key);
								base.UrlTable.Add(rootNodeCore.Url, rootNodeCore);
								base.KeyTable.Add(rootNodeCore.Key, rootNodeCore);
								this._siteMapNode = rootNodeCore;
							}
							SiteMapNode node3 = (SiteMapNode)base.ParentNodeTable[node];
							if (node3 != null)
							{
								SiteMapNodeCollection nodes = (SiteMapNodeCollection)base.ChildNodeCollectionTable[node3];
								int index = nodes.IndexOf(node);
								if (index != -1)
								{
									nodes.Remove(node);
									nodes.Insert(index, rootNodeCore);
								}
								else
								{
									nodes.Add(rootNodeCore);
								}
								base.ParentNodeTable[rootNodeCore] = node3;
								base.ParentNodeTable.Remove(node);
								base.UrlTable.Remove(node.Url);
								base.KeyTable.Remove(node.Key);
								base.UrlTable.Add(rootNodeCore.Url, rootNodeCore);
								base.KeyTable.Add(rootNodeCore.Key, rootNodeCore);
							}
							else
							{
								OscSiteMapProvider parentProvider = this.ParentProvider as OscSiteMapProvider;
								if (parentProvider != null)
								{
									parentProvider.EnsureChildSiteMapProviderUpToDate(this);
								}
							}
							this.ChildProviderTable[childProvider] = rootNodeCore;
							this._childProviderList = null;
						}
					}
				}
			}
		}

		/// <summary>Retrieves a <see cref="T:System.Web.SiteMapNode"></see> object that represents the page at the specified URL.</summary>
		/// <returns>A <see cref="T:System.Web.SiteMapNode"></see> that represents the page identified by rawURL.</returns>
		/// <param name="rawUrl">A URL that identifies the page for which to retrieve a <see cref="T:System.Web.SiteMapNode"></see>. </param>
		/// <exception cref="T:System.Configuration.Provider.ProviderException">A child provider linked to the current site map provider returned a node that is not valid.</exception>
		public override SiteMapNode FindSiteMapNode(string rawUrl)
		{
			SiteMapNode node = base.FindSiteMapNode(rawUrl);
			if (node == null)
			{
				foreach (SiteMapProvider provider in this.ChildProviderList)
				{
					this.EnsureChildSiteMapProviderUpToDate(provider);
					node = provider.FindSiteMapNode(rawUrl);
					if (node != null)
					{
						return node;
					}
				}
			}
			return node;
		}

		/// <summary>Retrieves a <see cref="T:System.Web.SiteMapNode"></see> object based on a specified key.</summary>
		/// <returns>A <see cref="T:System.Web.SiteMapNode"></see> that represents the page identified by key; otherwise, null, if security trimming is enabled and the node cannot be shown to the current user or the node is not found by key in the node collection.</returns>
		/// <param name="key">A lookup key with which to search for a <see cref="T:System.Web.SiteMapNode"></see>.</param>
		/// <exception cref="T:System.Configuration.Provider.ProviderException">A child provider linked to the current site map provider returned a node that is not valid.</exception>
		public override SiteMapNode FindSiteMapNodeFromKey(string key)
		{
			SiteMapNode node = base.FindSiteMapNodeFromKey(key);
			if (node == null)
			{
				foreach (SiteMapProvider provider in this.ChildProviderList)
				{
					this.EnsureChildSiteMapProviderUpToDate(provider);
					node = provider.FindSiteMapNodeFromKey(key);
					if (node != null)
					{
						return node;
					}
				}
			}
			return node;
		}

		private XmlDocument GetConfigDocument()
		{
			if (this._document == null)
			{
				if (!this._initialized)
				{
					throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Not_Initialized"));
				}
				if (this._virtualPath == null)
				{
					throw new ArgumentException(SR.GetString("XmlSiteMapProvider_missing_siteMapFile", new object[] { "siteMapFile" }));
				}
				if (!this._virtualPath.Extension.Equals(".sitemap", StringComparison.OrdinalIgnoreCase))
				{
					throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Invalid_Extension", new object[] { this._virtualPath }));
				}
				this._normalizedVirtualPath = this._virtualPath.CombineWithAppRoot();
				this._normalizedVirtualPath.FailIfNotWithinAppRoot();
				this.CheckSiteMapFileExists();
				this._parentSiteMapFileCollection = new StringCollection();
				OscSiteMapProvider parentProvider = this.ParentProvider as OscSiteMapProvider;
				if ((parentProvider != null) && (parentProvider._parentSiteMapFileCollection != null))
				{
					if (parentProvider._parentSiteMapFileCollection.Contains(this._normalizedVirtualPath.VirtualPathString))
					{
						throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_FileName_already_in_use", new object[] { this._virtualPath }));
					}
					foreach (string str in parentProvider._parentSiteMapFileCollection)
					{
						this._parentSiteMapFileCollection.Add(str);
					}
				}
				this._parentSiteMapFileCollection.Add(this._normalizedVirtualPath.VirtualPathString);
				this._filename = HostingEnvironment.MapPath(this._normalizedVirtualPath.VirtualPathString);
				if (!string.IsNullOrEmpty(this._filename))
				{
					this._handler = new FileChangeEventHandler(this.OnConfigFileChange);
					//HttpRuntime.FileChangesMonitor.StartMonitoringFile(this._filename, this._handler);
					base.ResourceKey = new FileInfo(this._filename).Name;
				}
				this._document = new ConfigXmlDocument();
			}
			return this._document;
		}

		private SiteMapNode GetNodeFromProvider(string providerName)
		{
			SiteMapProvider providerFromName = this.GetProviderFromName(providerName);
			SiteMapNode rootNodeCore = null;
			if (providerFromName is OscSiteMapProvider)
			{
				OscSiteMapProvider provider2 = (OscSiteMapProvider)providerFromName;
				StringCollection strings = new StringCollection();
				if (this._parentSiteMapFileCollection != null)
				{
					foreach (string str in this._parentSiteMapFileCollection)
					{
						strings.Add(str);
					}
				}
				provider2.BuildSiteMap();
				strings.Add(this._normalizedVirtualPath.VirtualPathString);
				if (strings.Contains(VirtualPath.GetVirtualPathString(provider2._normalizedVirtualPath)))
				{
					throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_FileName_already_in_use", new object[] { provider2._virtualPath }));
				}
				provider2._parentSiteMapFileCollection = strings;
			}
			rootNodeCore = ((OscSiteMapProvider)providerFromName).GetRootNodeCore();
			if (rootNodeCore == null)
			{
				throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_invalid_GetRootNodeCore", new object[] { providerFromName.Name }));
			}
			this.ChildProviderTable.Add(providerFromName, rootNodeCore);
			this._childProviderList = null;
			providerFromName.ParentProvider = this;
			return rootNodeCore;
		}

		private SiteMapNode GetNodeFromSiteMapFile(XmlNode xmlNode, VirtualPath siteMapFile)
		{
			SiteMapNode node = null;
			//bool securityTrimmingEnabled = base.SecurityTrimmingEnabled;
			//System.Web.Configuration.HandlerBase.GetAndRemoveBooleanAttribute(xmlNode, "securityTrimmingEnabled", ref securityTrimmingEnabled);
			//System.Web.Configuration.HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
			//System.Web.Configuration.HandlerBase.CheckForNonCommentChildNodes(xmlNode);
			//OscSiteMapProvider key = new OscSiteMapProvider();
			//siteMapFile = this._normalizedVirtualPath.Parent.Combine(siteMapFile);
			//key.ParentProvider = this;
			//key.Initialize(siteMapFile, securityTrimmingEnabled);
			//key.BuildSiteMap();
			//node = key._siteMapNode;
			//this.ChildProviderTable.Add(key, node);
			//this._childProviderList = null;
			return node;
		}

		private SiteMapNode GetNodeFromXmlNode(XmlNode xmlNode, Queue queue)
		{
			SiteMapNode node = null;
			//string val = null;
			//string str2 = null;
			//string str3 = null;
			//string str4 = null;
			//string str5 = null;
			//System.Web.Configuration.HandlerBase.GetAndRemoveStringAttribute(xmlNode, "url", ref str2);
			//System.Web.Configuration.HandlerBase.GetAndRemoveStringAttribute(xmlNode, "title", ref val);
			//System.Web.Configuration.HandlerBase.GetAndRemoveStringAttribute(xmlNode, "description", ref str3);
			//System.Web.Configuration.HandlerBase.GetAndRemoveStringAttribute(xmlNode, "roles", ref str4);
			//System.Web.Configuration.HandlerBase.GetAndRemoveStringAttribute(xmlNode, "resourceKey", ref str5);
			//if (!string.IsNullOrEmpty(str5) && !this.ValidateResource(base.ResourceKey, str5 + ".title"))
			//{
			//    str5 = null;
			//}
			//System.Web.Configuration.HandlerBase.CheckForbiddenAttribute(xmlNode, "securityTrimmingEnabled");
			//NameValueCollection collection = null;
			//bool allowImplicitResource = string.IsNullOrEmpty(str5);
			//this.HandleResourceAttribute(xmlNode, ref collection, "title", ref val, allowImplicitResource);
			//this.HandleResourceAttribute(xmlNode, ref collection, "description", ref str3, allowImplicitResource);
			//ArrayList list = new ArrayList();
			//if (str4 != null)
			//{
			//    int index = str4.IndexOf('?');
			//    if (index != -1)
			//    {
			//        object[] args = new object[] { str4[index].ToString(CultureInfo.InvariantCulture) };
			//        throw new ConfigurationErrorsException(SR.GetString("Auth_rule_names_cant_contain_char", args), xmlNode);
			//    }
			//    foreach (string str6 in str4.Split(_seperators))
			//    {
			//        string str7 = str6.Trim();
			//        if (str7.Length > 0)
			//        {
			//            list.Add(str7);
			//        }
			//    }
			//}
			//list = ArrayList.ReadOnly(list);
			//string key = null;
			//if (!string.IsNullOrEmpty(str2))
			//{
			//    str2 = str2.Trim();
			//    if (!UrlPath.IsAbsolutePhysicalPath(str2) && UrlPath.IsRelativeUrl(str2))
			//    {
			//        str2 = UrlPath.Combine(HttpRuntime.AppDomainAppVirtualPathString, str2);
			//    }
			//    string b = HttpUtility.UrlDecode(str2);
			//    if (!string.Equals(str2, b, StringComparison.Ordinal))
			//    {
			//        throw new ConfigurationErrorsException(SR.GetString("Property_Had_Malformed_Url", new object[] { "url", str2 }), xmlNode);
			//    }
			//    key = str2.ToLowerInvariant();
			//}
			//else
			//{
			//    key = Guid.NewGuid().ToString();
			//}
			//ReadOnlyNameValueCollection attributes = new ReadOnlyNameValueCollection();
			//attributes.SetReadOnly(false);
			//foreach (XmlAttribute attribute in xmlNode.Attributes)
			//{
			//    string text = attribute.Value;
			//    this.HandleResourceAttribute(xmlNode, ref collection, attribute.Name, ref text, allowImplicitResource);
			//    attributes[attribute.Name] = text;
			//}
			//attributes.SetReadOnly(true);
			//node = new SiteMapNode(this, key, str2, val, str3, list, attributes, collection, str5)
			//{
			//    ReadOnly = true
			//};
			//foreach (XmlNode node2 in xmlNode.ChildNodes)
			//{
			//    if (node2.NodeType == XmlNodeType.Element)
			//    {
			//        queue.Enqueue(node);
			//        queue.Enqueue(node2);
			//    }
			//}
			return node;
		}

		private SiteMapProvider GetProviderFromName(string providerName)
		{
			SiteMapProvider provider = SiteMap.Providers[providerName];
			if (provider == null)
			{
				throw new ProviderException(SR.GetString("Provider_Not_Found", new object[] { providerName }));
			}
			return provider;
		}

		/// <summary>Retrieves the top-level node of the current site map data structure.</summary>
		/// <returns>A <see cref="T:System.Web.SiteMapNode"></see> that represents the top-level node in the current site map data structure.</returns>
		protected override SiteMapNode GetRootNodeCore()
		{
			this.BuildSiteMap();
			return this._siteMapNode;
		}

		private void HandleResourceAttribute(XmlNode xmlNode, ref NameValueCollection collection, string attrName, ref string text, bool allowImplicitResource)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string str = null;
				string str2 = text.TrimStart(new char[] { ' ' });
				if (((str2 != null) && (str2.Length > 10)) && str2.ToLower(CultureInfo.InvariantCulture).StartsWith("$resources:", StringComparison.Ordinal))
				{
					if (!allowImplicitResource)
					{
						throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_multiple_resource_definition", new object[] { attrName }), xmlNode);
					}
					str = str2.Substring(11);
					if (str.Length == 0)
					{
						throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_resourceKey_cannot_be_empty"), xmlNode);
					}
					string str3 = null;
					string str4 = null;
					int index = str.IndexOf(',');
					if (index == -1)
					{
						throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_invalid_resource_key", new object[] { str }), xmlNode);
					}
					str3 = str.Substring(0, index);
					str4 = str.Substring(index + 1);
					int length = str4.IndexOf(',');
					if (length != -1)
					{
						text = str4.Substring(length + 1);
						str4 = str4.Substring(0, length);
					}
					else
					{
						text = null;
					}
					if (collection == null)
					{
						collection = new NameValueCollection();
					}
					collection.Add(attrName, str3.Trim());
					collection.Add(attrName, str4.Trim());
				}
			}
		}

		/// <summary>Initializes the <see cref="T:System.Web.OscSiteMapProvider"></see> object. The <see cref="M:System.Web.OscSiteMapProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"></see> method does not actually build a site map, it only prepares the state of the <see cref="T:System.Web.OscSiteMapProvider"></see> to do so.</summary>
		/// <param name="name">The <see cref="T:System.Web.OscSiteMapProvider"></see> to initialize. </param>
		/// <param name="attributes">A <see cref="T:System.Collections.Specialized.NameValueCollection"></see> that can contain additional attributes to help initialize name. These attributes are read from the <see cref="T:System.Web.OscSiteMapProvider"></see> configuration in the Web.config file. </param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Web.OscSiteMapProvider"></see> is initialized more than once.</exception>
		/// <exception cref="T:System.Web.HttpException">A <see cref="T:System.Web.SiteMapNode"></see> used a physical path to reference a site map file.- or -An error occurred while attempting to parse the virtual path supplied for the siteMapFile attribute.</exception>
		public override void Initialize(string name, NameValueCollection attributes)
		{
			if (this._initialized)
			{
				throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Cannot_Be_Inited_Twice"));
			}
			if (attributes != null)
			{
				if (string.IsNullOrEmpty(attributes["description"]))
				{
					attributes.Remove("description");
					attributes.Add("description", SR.GetString("XmlSiteMapProvider_Description"));
				}
				string val = null;
				ProviderUtil.GetAndRemoveStringAttribute(attributes, "siteMapFile", name, ref val);
				this._virtualPath = VirtualPath.CreateAllowNull(val);
			}
			base.Initialize(name, attributes);
			if (attributes != null)
			{
				ProviderUtil.CheckUnrecognizedAttributes(attributes, name);
			}
			this._initialized = true;
		}

		private void Initialize(VirtualPath virtualPath, bool secuityTrimmingEnabled)
		{
			NameValueCollection config = new NameValueCollection();
			config.Add("siteMapFile", virtualPath.VirtualPathString);
			config.Add("securityTrimmingEnabled", UI.Util.GetStringFromBool(secuityTrimmingEnabled));
			this.Initialize(virtualPath.VirtualPathString, config);
		}

		private void OnConfigFileChange(object sender, FileChangeEvent e)
		{
			OscSiteMapProvider parentProvider = this.ParentProvider as OscSiteMapProvider;
			if (parentProvider != null)
			{
				parentProvider.OnConfigFileChange(sender, e);
			}
			this.Clear();
		}

		/// <summary>Removes the specified <see cref="T:System.Web.SiteMapNode"></see> object from all node collections that are tracked by the provider.</summary>
		/// <param name="node">The node to remove from the node collections.</param>
		/// <exception cref="T:System.ArgumentNullException">node is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">node is the root node of the site map provider that owns it.- or -node is not managed by the provider nor by a provider in the chain of parent providers for this provider.</exception>
		protected override void RemoveNode(SiteMapNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			SiteMapProvider provider = node.Provider;
			if (provider != this)
			{
				for (SiteMapProvider provider2 = provider.ParentProvider; provider2 != this; provider2 = provider2.ParentProvider)
				{
					if (provider2 == null)
					{
						throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_cannot_remove_node", new object[] { node.ToString(), this.Name, provider.Name }));
					}
				}
			}
			if (node.Equals(((OscSiteMapProvider)provider).GetRootNodeCore()))
			{
				throw new InvalidOperationException(SR.GetString("SiteMapProvider_cannot_remove_root_node"));
			}
			if (provider != this)
			{
				((OscSiteMapProvider)provider).RemoveNode(node);
			}
			base.RemoveNode(node);
		}

		/// <summary>Removes a linked child site map provider from the hierarchy for the current provider.</summary>
		/// <param name="providerName">The name of one of the <see cref="T:System.Web.SiteMapProvider"></see> objects currently registered in the <see cref="P:System.Web.SiteMap.Providers"></see>.</param>
		/// <exception cref="T:System.ArgumentNullException">providerName is null.</exception>
		/// <exception cref="T:System.Configuration.Provider.ProviderException">providerName cannot be resolved.</exception>
		/// <exception cref="T:System.InvalidOperationException">providerName is not a registered child provider of the current site map provider.</exception>
		protected virtual void RemoveProvider(string providerName)
		{
			if (providerName == null)
			{
				throw new ArgumentNullException("providerName");
			}
			lock (base.providerLock)
			{
				SiteMapProvider providerFromName = this.GetProviderFromName(providerName);
				SiteMapNode node = (SiteMapNode)this.ChildProviderTable[providerFromName];
				if (node == null)
				{
					throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_cannot_find_provider", new object[] { providerFromName.Name, this.Name }));
				}
				providerFromName.ParentProvider = null;
				this.ChildProviderTable.Remove(providerFromName);
				this._childProviderList = null;
				base.RemoveNode(node);
			}
		}

		private bool ValidateResource(string classKey, string resourceKey)
		{
			try
			{
				HttpContext.GetGlobalResourceObject(classKey, resourceKey);
			}
			catch (MissingManifestResourceException)
			{
				return false;
			}
			return true;
		}

		private ArrayList ChildProviderList
		{
			get
			{
				if (this._childProviderList == null)
				{
					lock (base.providerLock)
					{
						if (this._childProviderList == null)
						{
							this._childProviderList = ArrayList.ReadOnly(new ArrayList(this.ChildProviderTable.Keys));
						}
					}
				}
				return this._childProviderList;
			}
		}

		private Hashtable ChildProviderTable
		{
			get
			{
				if (this._childProviderTable == null)
				{
					lock (base.providerLock)
					{
						if (this._childProviderTable == null)
						{
							this._childProviderTable = new Hashtable();
						}
					}
				}
				return this._childProviderTable;
			}
		}

		/// <summary>Gets the root node of the site map.</summary>
		/// <returns>A <see cref="T:System.Web.SiteMapNode"></see> that represents the root node of the site map; otherwise, null, if security trimming is enabled and the root node is not accessible to the current user.</returns>
		public override SiteMapNode RootNode
		{
			get
			{
				this.BuildSiteMap();
				return base.ReturnNodeIfAccessible(this._siteMapNode);
			}
		}

		private class ReadOnlyNameValueCollection : NameValueCollection
		{
			public ReadOnlyNameValueCollection()
			{
				base.IsReadOnly = true;
			}

			internal void SetReadOnly(bool isReadonly)
			{
				base.IsReadOnly = isReadonly;
			}
		}
	}
}
