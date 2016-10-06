using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using openSourceC.FrameworkLibrary.Web.Resources;

namespace openSourceC.FrameworkLibrary.Web.UI.WebControls
{
	/// <summary>
	///		Summary
	/// </summary>
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Designer("openSourceC.FrameworkLibrary.Web.UI.Design.WebControls.ContentBlockDesigner, openSourceC.FrameworkLibrary.Design")]
	//[Designer("System.Web.UI.Design.WebControls.CompositeControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ParseChildren(true)]
	[PersistChildren(false)]
	[Themeable(true)]
	[DefaultProperty("ContentTemplate")]
	[ToolboxData("<{0}:ContentBlock runat=\"server\"><HeaderTemplate>HEADER</HeaderTemplate><ContentTemplate>CONTENT</ContentTemplate></{0}:ContentBlock>")]
	[System.Drawing.ToolboxBitmap(typeof(EmbeddedResourceFinder), "ContentBlock.ico")]
	public class ContentBlock : WebControl, INamingContainer, ICompositeControlDesignerAccessor, IPostBackEventHandler
	{
		private const string ControlToken = "container";

		private TableRow _headerRow;
		private TableCell _collapseImageCell;
		private HtmlGenericControl _collapseStateImage;
		private TemplateContainerControl _headerTemplateContainer;
		private ITemplate _headerTemplate;

		private TableRow _contentRow;
		private TemplateContainerControl _contentTemplateContainer;
		private ITemplate _contentTemplate;


		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="T:ContentBlock" /> class.
		///	</summary>
		public ContentBlock() : base(HtmlTextWriterTag.Table) { }

		#endregion

		#region Public Properties

		/// <summary>
		///		
		/// </summary>
		[Bindable(true)]
		[Category("Layout")]
		[DefaultValue(ClearStyle.None)]
		[Description("Clear")]
		public virtual ClearStyle Clear
		{
			get { return (ClearStyle)(ViewState["Clear"] ?? ClearStyle.None); }
			set { ViewState["Clear"] = value; }
		}

		/// <summary>
		///		
		/// </summary>
		[Category("Appearance")]
		[CssClassProperty]
		[DefaultValue("")]
		[Description("CollapseImageCellCssClass")]
		public virtual string CollapseImageCellCssClass
		{
			get { return (string)(ViewState["CollapseImageCellCssClass"] ?? string.Empty); }
			set { ViewState["CollapseImageCellCssClass"] = value; }
		}

		/// <summary>
		///		
		/// </summary>
		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(CollapseModeState.Disabled)]
		[Description("The mode of the collapse control.")]
		public virtual CollapseModeState CollapseMode
		{
			get { return (CollapseModeState)(ViewState["CollapseMode"] ?? CollapseModeState.Disabled); }
			set { ViewState["CollapseMode"] = value; }
		}

		/// <summary>
		///		Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server
		///		control on the client.
		/// </summary>
		/// <returns>
		///		The CSS class rendered by the Web server control on the client. The default is
		///		<see cref="F:String.Empty"/>.
		/// </returns>
		[Category("Appearance")]
		[CssClassProperty]
		[DefaultValue("")]
		[Description("ContentCellCssClass")]
		public virtual string ContentCellCssClass
		{
			get { return (string)(ViewState["ContentCellCssClass"] ?? string.Empty); }
			set { ViewState["ContentCellCssClass"] = value; }
		}

		/// <summary>
		///		
		/// </summary>
		[Category("Appearance")]
		[CssClassProperty]
		[DefaultValue("")]
		[Description("ContentRowCssClass")]
		public virtual string ContentRowCssClass
		{
			get { return (string)(ViewState["ContentRowCssClass"] ?? string.Empty); }
			set { ViewState["ContentRowCssClass"] = value; }
		}

		/// <summary>
		///		
		/// </summary>
		[Bindable(true)]
		[Category("Layout")]
		[DefaultValue(typeof(Unit), null)]
		[Description("The height of the ContentTemplate section.")]
		public virtual Unit ContentHeight
		{
			get { return (Unit)(ViewState["ContentHeight"] ?? Unit.Empty); }
			set { ViewState["ContentHeight"] = value; }
		}

		/// <summary>
		///		Gets or sets the template that defines the content of the
		///		<see cref="T:ContentBlock"/> control.
		///	</summary>
		/// <returns>
		///		An <see cref="T:ITemplate"/> instance that defines the content of the
		///		<see cref="T:ContentBlock"/> control. The default is null.
		///	</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:ContentBlock.ContentTemplate"/>
		///		property of the <see cref="T:ContentBlock" /> control is set after an instance of
		///		the template is created or after the content template container is created.
		///	</exception>
		[TemplateContainer(typeof(TemplateContainerControl))]
		[TemplateInstance(TemplateInstance.Single)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[Browsable(false)]
		[DefaultValue((string)null)]
		[Description("ContentBlock_HeaderTemplate")]
		public ITemplate ContentTemplate
		{
			get { return _contentTemplate; }

			set
			{
				if (!base.DesignMode && _contentTemplate != null && ((Control)_contentTemplate).Parent != null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The ContentTemplate of ContentBlock with ID '{0}' cannot be set after the template has been instantiated.", new object[] { this.ID }));
				}

				_contentTemplate = value;
			}
		}

		/// <summary>
		///		Gets the container control for the ContentTemplate.
		/// </summary>
		[Browsable(false)]
		public TemplateContainerControl ContentTemplateContainer
		{
			get { EnsureChildControls(); return _contentTemplateContainer; }
		}

		/// <summary>
		///		Gets a <see cref="T:ControlCollection" /> object that represents the child controls
		///		in a <see cref="T:CompositeControl" />.
		/// </summary>
		/// <returns>
		///		A <see cref="T:ControlCollection" /> that represents the child controls in the
		///		<see cref="T:CompositeControl" />.
		/// </returns>
		[Browsable(false)]
		public override ControlCollection Controls
		{
			get { EnsureChildControls(); return base.Controls; }
		}

		/// <summary>
		///		
		/// </summary>
		[Bindable(true)]
		[Category("Layout")]
		[DefaultValue(FloatStyle.None)]
		[Description("Float")]
		public virtual FloatStyle Float
		{
			get { return (FloatStyle)(ViewState["Float"] ?? FloatStyle.None); }
			set { ViewState["Float"] = value; }
		}

		/// <summary>
		///		
		/// </summary>
		[Bindable(true)]
		[Category("Layout")]
		[DefaultValue(typeof(Unit), null)]
		[Description("The height of the HeaderTemplate section.")]
		public virtual Unit HeaderHeight
		{
			get { return (Unit)(ViewState["HeaderHeight"] ?? Unit.Empty); }
			set { ViewState["HeaderHeight"] = value; }
		}

		/// <summary>
		///		
		/// </summary>
		[Category("Appearance")]
		[CssClassProperty]
		[DefaultValue("")]
		[Description("HeaderCellCssClass")]
		public virtual string HeaderCellCssClass
		{
			get { return (string)(ViewState["HeaderCellCssClass"] ?? string.Empty); }
			set { ViewState["HeaderCellCssClass"] = value; }
		}

		/// <summary>
		///		
		/// </summary>
		[Category("Appearance")]
		[CssClassProperty]
		[DefaultValue("")]
		[Description("HeaderRowCssClass")]
		public virtual string HeaderRowCssClass
		{
			get { return (string)(ViewState["HeaderRowCssClass"] ?? string.Empty); }
			set { ViewState["HeaderRowCssClass"] = value; }
		}

		/// <summary>
		///		Gets or sets the <see cref="T:ITemplate"/> that defines how the header section of
		///		the <see cref="T:ContentBlock"/> control is displayed.
		///	</summary>
		/// <returns>
		///		A <see cref="T:ITemplate"/> that defines how the header section of the
		///		<see cref="T:ContentBlock"/> control is displayed. The default value is null.
		///	</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:ContentBlock.HeaderTemplate"/>
		///		property of the <see cref="T:ContentBlock" /> control is set after an instance of
		///		the template is created or after the header template container is created.
		///	</exception>
		[TemplateContainer(typeof(TemplateContainerControl))]
		[TemplateInstance(TemplateInstance.Single)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[Browsable(false)]
		[DefaultValue((string)null)]
		[Description("ContentBlock_HeaderTemplate")]
		public ITemplate HeaderTemplate
		{
			get { return _headerTemplate; }

			set
			{
				if (!base.DesignMode && _headerTemplate != null && ((Control)_headerTemplate).Parent != null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The HeaderTemplate of ContentBlock with ID '{0}' cannot be set after the template has been instantiated.", new object[] { this.ID }));
				}

				_headerTemplate = value;
			}
		}

		/// <summary>
		///		Gets the container control for the HeaderTemplate.
		/// </summary>
		[Browsable(false)]
		public TemplateContainerControl HeaderTemplateContainer
		{
			get { EnsureChildControls(); return _headerTemplateContainer; }
		}

		#endregion

		#region Private Properties

		private RowControlCollection ChildControls
		{
			get { return (base.Controls as RowControlCollection); }
		}

		#endregion

		#region ICompositeControlDesignerAccessor Members

		void ICompositeControlDesignerAccessor.RecreateChildControls()
		{
			RecreateChildControls();
		}

		#endregion

		#region IPostBackEventHandler Members

		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			RaisePostBackEvent(eventArgument);
		}

		#endregion

		#region Protected Methods

		/// <summary>
		///		Returns the collection of all controls that are contained in the
		///		<see cref="T:ContentBlock"/> control.
		///	</summary>
		/// <returns>
		///		A <see cref="T:ControlCollection" /> object that consists of all controls that
		///		define the content of the <see cref="T:ContentBlock"/> control.
		///	</returns>
		protected sealed override ControlCollection CreateControlCollection()
		{
			return new RowControlCollection(this);
		}

		/// <summary>
		///		Raises events for the <see cref="T:ContentBlock"/> control when a form is posted
		///		back to the server.
		/// </summary>
		/// <param name="argument">A <see cref="T:String"/> that represents the argument for the event.</param>
		protected virtual void RaisePostBackEvent(string argument)
		{
			//base.ValidateEvent(this.UniqueID, argument);

			//if (base._adapter != null)
			//{
			//    IPostBackEventHandler handler = base._adapter as IPostBackEventHandler;

			//    if (handler != null)
			//    {
			//        handler.RaisePostBackEvent(argument);
			//    }
			//}
		}

		/// <summary>
		///		Recreates the child controls n a control derived from <see cref="T:ContentBlock"/>.
		/// </summary>
		protected virtual void RecreateChildControls()
		{
			ChildControls.AllowClear = true;
			base.ChildControlsCreated = false;
			ChildControls.AllowClear = false;

			EnsureChildControls();
		}

		#endregion

		#region Override Events

		/// <summary>
		///		Called by the ASP.NET page framework to notify server controls that use
		///		composition-based implementation to create any child controls they contain in
		///		preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			TableCell cell;
			Table table;
			TableRow row;


			if (DesignMode)
			{
				ChildControls.ClearInternal();
			}

			ChildControls.AddInternal(_headerRow = new TableRow());

			_headerRow.Cells.Add(cell = new TableCell());

			cell.Controls.Add(table = new Table());
			table.Width = Unit.Percentage(100);

			table.Rows.Add(row = new TableRow());

			row.Cells.Add(_collapseImageCell = new TableCell());
			_collapseImageCell.Style["width"] = "0";
			_collapseImageCell.Visible = false;

			_collapseImageCell.Controls.Add(_collapseStateImage = new HtmlGenericControl("div"));
			_collapseStateImage.ID = "CollapseStateImage";

			// Create header template container.  Will fail if called before OnInit().
			row.Cells.Add(_headerTemplateContainer = new TemplateContainerControl());
			_headerTemplateContainer.ID = "Header";

			if (_headerTemplate != null)
			{
				_headerTemplate.InstantiateIn(_headerTemplateContainer);
			}

			ChildControls.AddInternal(_contentRow = new TableRow());

			// Create content template container.  Will fail if called before OnInit().
			_contentRow.Cells.Add(_contentTemplateContainer = new TemplateContainerControl());
			_contentTemplateContainer.ID = "Content";

			if (_contentTemplate != null)
			{
				_contentTemplate.InstantiateIn(_contentTemplateContainer);
			}
		}

		/// <summary>
		///		Gets a reference to a collection of properties that define the appearance of a
		///		<see cref="T:Table"/> control.
		/// </summary>
		/// <returns>
		///		A reference to the <see cref="T:Style"/> object that contains the properties that
		///		define the appearance of the <see cref="T:Table"/> control.
		/// </returns>
		protected override Style CreateControlStyle()
		{
			return new TableStyle(this.ViewState);
		}

		/// <summary>
		///		Binds a data source to the <see cref="T:ControlBlock" /> and all its child
		///		controls.
		///	</summary>
		public override void DataBind()
		{
			OnDataBinding(EventArgs.Empty);
			EnsureChildControls();
			DataBindChildren();
		}

		/// <summary>
		///		Raises the <see cref="E:Control.Init"/> event.
		///	</summary>
		/// <param name="e">An <see cref="T:EventArgs" /> object that contains the event data.</param>
		/// <exception cref="T:InvalidOperationException">
		///		The <see cref="P:ContentBlock.HeaderTemplate"/> property is being defined when the
		///		<see cref="P:ContentBlock.HeaderTemplateContainer" /> property has already been
		///		created.
		///		-or-
		///		The <see cref="P:ContentBlock.ContentTemplate"/> property is being defined when the
		///		<see cref="P:ContentBlock.ContentTemplateContainer" /> property has already been
		///		created.
		///	</exception>
		protected override void OnInit(EventArgs e)
		{
			EnsureChildControls();

			base.OnInit(e);
		}

		#endregion

		#region Render Methods

		/// <summary>
		///		Adds HTML attributes and styles that need to be rendered to the specified
		///		<see cref="T:HtmlTextWriter"/>.
		/// </summary>
		/// <param name="writer">The output stream that renders HTML content to the client.</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
		}

		/// <summary>
		///		Raises the <see cref="E:Control.PreRender" /> event.
		///	</summary>
		/// <param name="e">An <see cref="T:EventArgs" /> object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			const string ContainerCssId = "osc-ContentBlock-css";


			if (!base.DesignMode)
			{
				// Only add the stylesheet if we haven't already added it (e.g. if we have multiple Containers on a page)
				if (Page.Header.FindControl(ContainerCssId) == null)
				{
					// Add style sheet to parent page 
					string cssUrl = Page.ClientScript.GetWebResourceUrl(typeof(ContentBlock), "openSourceC.FrameworkLibrary.Web.Resources.ContentBlock.css");
					HtmlLink cssLink = new HtmlLink();
					cssLink.ID = ContainerCssId;
					cssLink.Href = cssUrl;
					cssLink.Attributes.Add("rel", "stylesheet");
					cssLink.Attributes.Add("type", "text/css");
					Page.Header.Controls.Add(cssLink);
				}

				// Add JavaScript file to parent page
				Page.ClientScript.RegisterClientScriptResource(typeof(ContentBlock), "openSourceC.FrameworkLibrary.Web.Resources.ContentBlock.js");
			}

			if (!Width.IsEmpty)
			{
				//base.Style["width"] = Width.ToString();
				base.Style["margin"] = "auto auto";
			}

			base.Style["float"] = Float.ToString().ToLower();
			base.Style["clear"] = Clear.ToString().ToLower();

			if (!string.IsNullOrEmpty(HeaderRowCssClass)) _headerRow.Attributes["class"] = HeaderRowCssClass;
			if (!HeaderHeight.IsEmpty) _headerRow.Style["height"] = HeaderHeight.ToString();

			_collapseImageCell.Visible = (CollapseMode != CollapseModeState.Disabled);

			if (_collapseImageCell.Visible)
			{
				// Show the collapse state image.
				if (!string.IsNullOrEmpty(CollapseImageCellCssClass)) _collapseImageCell.Attributes["class"] = CollapseImageCellCssClass;
				_collapseStateImage.Attributes["class"] = (CollapseMode == CollapseModeState.Collapsed ? "osc-ContentBlock-col" : "osc-ContentBlock-exp");

				_headerRow.Attributes["onclick"] = string.Format("oscContentBlock.toggleContent('{0}', '{1}');", _contentRow.ClientID, _collapseStateImage.ClientID);
				_headerRow.Style["cursor"] = "hand";
			}

			if (!string.IsNullOrEmpty(HeaderCellCssClass)) _headerTemplateContainer.Attributes["class"] = HeaderCellCssClass;

			if (CollapseMode == CollapseModeState.Collapsed) _contentRow.Style["display"] = "none";
			if (!string.IsNullOrEmpty(ContentRowCssClass)) _contentRow.Attributes["class"] = ContentRowCssClass;
			if (!ContentHeight.IsEmpty) _contentRow.Style["height"] = ContentHeight.ToString();
			if (!string.IsNullOrEmpty(ContentCellCssClass)) _contentTemplateContainer.Attributes["class"] = ContentCellCssClass;

			base.OnPreRender(e);
		}

		/// <summary>
		///		Writes the <see cref="T:CompositeControl" /> content to the specified
		///		<see cref="T:HtmlTextWriter" /> object, for display on the client.
		/// </summary>
		/// <param name="writer">An <see cref="T:HtmlTextWriter" /> that represents the output
		///		stream to render HTML content on the client.</param>
		protected override void Render(HtmlTextWriter writer)
		{
			if (base.DesignMode)
			{
				this.EnsureChildControls();
			}

			base.Render(writer);
		}

		/// <summary>Renders the rows in the table control to the specified writer.</summary>
		/// <param name="writer">An <see cref="T:System.Web.UI.HtmlTextWriter"></see> that represents the output stream to render HTML content on the client. </param>
		protected override void RenderContents(HtmlTextWriter writer)
		{
			foreach (TableRow row in ChildControls)
			{
				row.RenderControl(writer);
			}
		}

		#endregion

		#region Protected Child Classes

		/// <summary>
		///		Summary description for RowControlCollection.
		/// </summary>
		protected class RowControlCollection : ControlCollection
		{
			internal bool AllowClear;


			/// <summary>
			///		Class constructor.
			/// </summary>
			/// <param name="owner"></param>
			public RowControlCollection(Control owner) : base(owner) { }

			/// <summary>
			///		
			/// </summary>
			/// <param name="child"></param>
			public override void Add(Control child)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The Controls property of ContentBlock with ID '{0}' (Add) cannot be modified directly. To change the contents of the ContentBlock modify the child controls of the ContentTemplateContainer property.", new object[] { base.Owner.ID }));
			}

			/// <summary>
			///		
			/// </summary>
			/// <param name="index"></param>
			/// <param name="child"></param>
			public override void AddAt(int index, Control child)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The Controls property of ContentBlock with ID '{0}' (AddAt) cannot be modified directly. To change the contents of the ContentBlock modify the child controls of the ContentTemplateContainer property.", new object[] { base.Owner.ID }));
			}

			/// <summary>
			///		
			/// </summary>
			/// <param name="row"></param>
			internal void AddInternal(TableRow row)
			{
				base.Add(row);
			}

			/// <summary>
			///		
			/// </summary>
			public override void Clear()
			{
				if (AllowClear)
				{
					base.Clear();
				}
				else
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The Controls property of ContentBlock with ID '{0}' (Clear) cannot be modified directly. To change the contents of the ContentBlock modify the child controls of the ContentTemplateContainer property.", new object[] { base.Owner.ID }));
				}
			}

			/// <summary>
			///		
			/// </summary>
			internal void ClearInternal()
			{
				try
				{
					AllowClear = true;
					base.Clear();
				}
				finally
				{
					AllowClear = false;
				}
			}

			/// <summary>
			///		
			/// </summary>
			/// <param name="value"></param>
			public override void Remove(Control value)
			{
				if (!AllowClear)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The Controls property of ContentBlock with ID '{0}' (Remove) cannot be modified directly. To change the contents of the ContentBlock modify the child controls of the ContentTemplateContainer property.", new object[] { base.Owner.ID }));
				}

				base.Remove(value);
			}

			/// <summary>
			///		
			/// </summary>
			/// <param name="value"></param>
			internal void RemoveInternal(Control value)
			{
				try
				{
					AllowClear = true;
					base.Remove(value);
				}
				finally
				{
					AllowClear = false;
				}
			}

			/// <summary>
			///		
			/// </summary>
			/// <param name="index"></param>
			public override void RemoveAt(int index)
			{
				if (!AllowClear)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The Controls property of ContentBlock with ID '{0}' (RemoveAt) cannot be modified directly. To change the contents of the ContentBlock modify the child controls of the ContentTemplateContainer property.", new object[] { base.Owner.ID }));
				}

				base.RemoveAt(index);
			}
		}

		#endregion
	}

	/// <summary>
	///		
	/// </summary>
	[ToolboxItem(false)]
	public class TemplateContainerControl : TableCell, INamingContainer
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="T:TemplateContainerControl" /> class
		///		with default values.
		/// </summary>
		public TemplateContainerControl() : base() { }
	}
}
