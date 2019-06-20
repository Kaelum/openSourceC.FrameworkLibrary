using System;
using System.Collections;
using System.Security.Permissions;
using System.Web;
using System.Web.Util;

using openSourceC.FrameworkLibrary.Web.Util;

namespace openSourceC.FrameworkLibrary.Web
{
	/// <summary>
	///		Serves as a partial implementation of the abstract <see cref="T:System.Web.SiteMapProvider"/>
	///		class and serves as a base class for the <see cref="T:System.Web.XmlSiteMapProvider"/>
	///		class, which is the default site map provider in ASP.NET.
	///	</summary>
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class OscStaticSiteMapProvider : SiteMapProvider
	{
		/// <summary></summary>
		protected readonly object providerLock = new object();

		private Hashtable _childNodeCollectionTable;
		private Hashtable _keyTable;
		private Hashtable _parentNodeTable;
		private Hashtable _urlTable;

		private static SiteMapNodeCollection EmptySiteMapNodeCollection = SiteMapNodeCollection.ReadOnly(new SiteMapNodeCollection());


		/// <summary>Initializes a new instance of the <see cref="T:System.Web.OscStaticSiteMapProvider"/> class. </summary>
		protected OscStaticSiteMapProvider()
		{
		}

		/// <summary>Adds a <see cref="T:System.Web.SiteMapNode"/> to the collections that are maintained by the site map provider and establishes a parent/child relationship between the <see cref="T:System.Web.SiteMapNode"/> objects.</summary>
		/// <param name="node">The <see cref="T:System.Web.SiteMapNode"/> to add to the site map provider. </param>
		/// <param name="parentNode">The <see cref="T:System.Web.SiteMapNode"/> under which to add node.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Web.SiteMapNode.Url"/> or <see cref="P:System.Web.SiteMapNode.Key"/> is already registered with the <see cref="T:System.Web.OscStaticSiteMapProvider"/>. A site map node must be made up of pages with unique URLs or keys. </exception>
		/// <exception cref="T:System.ArgumentNullException">node is null. </exception>
		protected override void AddNode(SiteMapNode node, SiteMapNode parentNode)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			lock (providerLock)
			{
				bool flag = false;
				string url = node.Url;
				if (!string.IsNullOrEmpty(url))
				{
					if (HttpRuntime.AppDomainAppVirtualPath != null)
					{
						if (!UrlPath.IsAbsolutePhysicalPath(url))
						{
							url = UrlPath.MakeVirtualPathAppAbsolute(UrlPath.Combine(HttpRuntime.AppDomainAppVirtualPath, url));
						}
						if (this.UrlTable[url] != null)
						{
							throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Multiple_Nodes_With_Identical_Url", new object[] { url }));
						}
					}
					flag = true;
				}
				string key = node.Key;
				if (this.KeyTable.Contains(key))
				{
					throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Multiple_Nodes_With_Identical_Key", new object[] { key }));
				}
				this.KeyTable[key] = node;
				if (flag)
				{
					this.UrlTable[url] = node;
				}
				if (parentNode != null)
				{
					this.ParentNodeTable[node] = parentNode;
					if (this.ChildNodeCollectionTable[parentNode] == null)
					{
						this.ChildNodeCollectionTable[parentNode] = new SiteMapNodeCollection();
					}
					((SiteMapNodeCollection)this.ChildNodeCollectionTable[parentNode]).Add(node);
				}
			}
		}

		/// <summary>When overridden in a derived class, loads the site map information from persistent storage and builds it in memory.</summary>
		/// <returns>The root <see cref="T:System.Web.SiteMapNode"/> of the site map navigation structure.</returns>
		public abstract SiteMapNode BuildSiteMap();
		/// <summary>Removes all elements in the collections of child and parent site map nodes that the <see cref="T:System.Web.OscStaticSiteMapProvider"/> tracks as part of its state.</summary>
		protected virtual void Clear()
		{
			lock (providerLock)
			{
				if (this._childNodeCollectionTable != null)
				{
					this._childNodeCollectionTable.Clear();
				}
				if (this._urlTable != null)
				{
					this._urlTable.Clear();
				}
				if (this._parentNodeTable != null)
				{
					this._parentNodeTable.Clear();
				}
				if (this._keyTable != null)
				{
					this._keyTable.Clear();
				}
			}
		}

		/// <summary>Retrieves a <see cref="T:System.Web.SiteMapNode"/> object that represents the page at the specified URL.</summary>
		/// <returns>A <see cref="T:System.Web.SiteMapNode"/> that represents the page identified by rawURL; otherwise, null, if no corresponding site map node is found.</returns>
		/// <param name="rawUrl">A URL that identifies the page for which to retrieve a <see cref="T:System.Web.SiteMapNode"/>. </param>
		/// <exception cref="T:System.ArgumentNullException">rawURL is null. </exception>
		public override SiteMapNode FindSiteMapNode(string rawUrl)
		{
			if (rawUrl == null)
			{
				throw new ArgumentNullException("rawUrl");
			}
			rawUrl = rawUrl.Trim();
			if (rawUrl.Length == 0)
			{
				return null;
			}
			if (UrlPath.IsAppRelativePath(rawUrl))
			{
				rawUrl = UrlPath.MakeVirtualPathAppAbsolute(rawUrl);
			}
			this.BuildSiteMap();
			return ReturnNodeIfAccessible((SiteMapNode)this.UrlTable[rawUrl]);
		}

		/// <summary>Retrieves a <see cref="T:System.Web.SiteMapNode"/> object based on a specified key.</summary>
		/// <returns>A <see cref="T:System.Web.SiteMapNode"/> that represents the page identified by key; otherwise, null, if security trimming is enabled and the site map node cannot be shown to the current user or the site map node is not found in the site map node collection by key. </returns>
		/// <param name="key">A lookup key with which a <see cref="T:System.Web.SiteMapNode"/> is created.</param>
		public override SiteMapNode FindSiteMapNodeFromKey(string key)
		{
			SiteMapNode node = base.FindSiteMapNodeFromKey(key);
			if (node == null)
			{
				node = (SiteMapNode)this.KeyTable[key];
			}
			return ReturnNodeIfAccessible(node);
		}

		/// <summary>Retrieves the child site map nodes of a specific <see cref="T:System.Web.SiteMapNode"/> object.</summary>
		/// <returns>A read-only <see cref="T:System.Web.SiteMapNodeCollection"/> that contains the child site map nodes of node. If security trimming is enabled, the collection contains only site map nodes that the user is permitted to see.</returns>
		/// <param name="node">The <see cref="T:System.Web.SiteMapNode"/> for which to retrieve all child site map nodes. </param>
		/// <exception cref="T:System.ArgumentNullException">node is null. </exception>
		public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this.BuildSiteMap();
			SiteMapNodeCollection collection = (SiteMapNodeCollection)this.ChildNodeCollectionTable[node];
			if (collection == null)
			{
				SiteMapNode node2 = (SiteMapNode)this.KeyTable[node.Key];
				if (node2 != null)
				{
					collection = (SiteMapNodeCollection)this.ChildNodeCollectionTable[node2];
				}
			}
			if (collection == null)
			{
				return EmptySiteMapNodeCollection;
			}
			if (!base.SecurityTrimmingEnabled)
			{
				return SiteMapNodeCollection.ReadOnly(collection);
			}
			HttpContext current = HttpContext.Current;
			SiteMapNodeCollection nodes2 = new SiteMapNodeCollection(collection.Count);
			foreach (SiteMapNode node3 in collection)
			{
				if (node3.IsAccessibleToUser(current))
				{
					nodes2.Add(node3);
				}
			}
			return SiteMapNodeCollection.ReadOnly(nodes2);
		}

		/// <summary>Retrieves the parent site map node of a specific <see cref="T:System.Web.SiteMapNode"/> object.</summary>
		/// <returns>A <see cref="T:System.Web.SiteMapNode"/> that represents the parent of the specified <see cref="T:System.Web.SiteMapNode"/>; otherwise, null, if no parent site map node exists or the user is not permitted to see the parent site map node.</returns>
		/// <param name="node">The <see cref="T:System.Web.SiteMapNode"/> for which to retrieve the parent site map node. </param>
		/// <exception cref="T:System.ArgumentNullException">node is null. </exception>
		public override SiteMapNode GetParentNode(SiteMapNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this.BuildSiteMap();
			SiteMapNode parentNode = (SiteMapNode)this.ParentNodeTable[node];
			if (parentNode == null)
			{
				SiteMapNode node3 = (SiteMapNode)this.KeyTable[node.Key];
				if (node3 != null)
				{
					parentNode = (SiteMapNode)this.ParentNodeTable[node3];
				}
			}
			if ((parentNode == null) && (this.ParentProvider != null))
			{
				parentNode = this.ParentProvider.GetParentNode(node);
			}
			return ReturnNodeIfAccessible(parentNode);
		}

		/// <summary>Removes the specified <see cref="T:System.Web.SiteMapNode"/> object from all site map node collections that are tracked by the site map provider.</summary>
		/// <param name="node">The site map node to remove from the site map node collections. </param>
		/// <exception cref="T:System.ArgumentNullException">node is null. </exception>
		protected override void RemoveNode(SiteMapNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			lock (providerLock)
			{
				SiteMapNode node2 = (SiteMapNode)this.ParentNodeTable[node];
				if (this.ParentNodeTable.Contains(node))
				{
					this.ParentNodeTable.Remove(node);
				}
				if (node2 != null)
				{
					SiteMapNodeCollection nodes = (SiteMapNodeCollection)this.ChildNodeCollectionTable[node2];
					if ((nodes != null) && nodes.Contains(node))
					{
						nodes.Remove(node);
					}
				}
				string url = node.Url;
				if (((url != null) && (url.Length > 0)) && this.UrlTable.Contains(url))
				{
					this.UrlTable.Remove(url);
				}
				string key = node.Key;
				if (this.KeyTable.Contains(key))
				{
					this.KeyTable.Remove(key);
				}
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		protected SiteMapNode ReturnNodeIfAccessible(SiteMapNode node)
		{
			if ((node != null) && node.IsAccessibleToUser(HttpContext.Current))
			{
				return node;
			}
			return null;
		}

		internal IDictionary ChildNodeCollectionTable
		{
			get
			{
				if (this._childNodeCollectionTable == null)
				{
					lock (providerLock)
					{
						if (this._childNodeCollectionTable == null)
						{
							this._childNodeCollectionTable = new Hashtable();
						}
					}
				}
				return this._childNodeCollectionTable;
			}
		}

		internal IDictionary KeyTable
		{
			get
			{
				if (this._keyTable == null)
				{
					lock (providerLock)
					{
						if (this._keyTable == null)
						{
							this._keyTable = new Hashtable();
						}
					}
				}
				return this._keyTable;
			}
		}

		internal IDictionary ParentNodeTable
		{
			get
			{
				if (this._parentNodeTable == null)
				{
					lock (providerLock)
					{
						if (this._parentNodeTable == null)
						{
							this._parentNodeTable = new Hashtable();
						}
					}
				}
				return this._parentNodeTable;
			}
		}

		internal IDictionary UrlTable
		{
			get
			{
				if (this._urlTable == null)
				{
					lock (providerLock)
					{
						if (this._urlTable == null)
						{
							this._urlTable = new Hashtable(StringComparer.OrdinalIgnoreCase);
						}
					}
				}
				return this._urlTable;
			}
		}
	}
}
