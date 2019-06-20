using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;

using openSourceC.FrameworkLibrary.Web.UI.WebControls;

namespace openSourceC.FrameworkLibrary.Web.UI.Design.WebControls
{
	/// <summary>
	///		Summary description for ContentBlockDesigner.
	/// </summary>
	public class ContentBlockDesigner : CompositeControlDesigner
	{
		private const string HeaderTemplateName = "Header Template";
		private const string HeaderTemplatePropertyName = "HeaderTemplate";

		private const string ContentTemplateName = "Content Template";
		private const string ContentTemplatePropertyName = "ContentTemplate";

		private ContentBlock _currentObject;

		//private PropertyDescriptor _headerTemplatePropertyDescriptor;
		//private PropertyDescriptor _contentTemplatePropertyDescriptor;

		//private TemplateGroupCollection _templateGroupCollection = null;
		//private TemplateDefinition _headerTemplateDefinition;
		//private TemplateDefinition _contentTemplateDefinition;


		#region Initialization

		/// <summary>
		///		Initializes the specified component.
		///	</summary>
		/// <param name="component">A <see cref="T:ContentBlock" /> component that implements the
		///		<see cref="T:IComponent" /> interface.</param>
		public override void Initialize(IComponent component)
		{
			// Initialize the base
			base.Initialize(component);

			_currentObject = (ContentBlock)component;

			//// Turn on template editing
			//SetViewFlags(ViewFlags.TemplateEditing, true);
		}

		#endregion

		#region Public Override Properties

		/// <summary>
		///		
		/// </summary>
		public override bool AllowResize
		{
			get { return !InTemplateMode; }
		}

		///// <summary>
		///// 
		///// </summary>
		//public override TemplateGroupCollection TemplateGroups
		//{
		//    get
		//    {
		//        if (_templateGroupCollection == null)
		//        {
		//            TemplateGroup tempGroup;


		//            // Get the base collection
		//            _templateGroupCollection = base.TemplateGroups;

		//            // Create a TemplateGroup
		//            tempGroup = new TemplateGroup("Templates");

		//            tempGroup.AddTemplateDefinition(HeaderTemplateDefinition);
		//            tempGroup.AddTemplateDefinition(ContentTemplateDefinition);

		//            _templateGroupCollection.Add(tempGroup);
		//        }

		//        return _templateGroupCollection;
		//    }
		//}

		#endregion

		#region Private Properties

		//private TemplateDefinition ContentTemplateDefinition
		//{
		//    get
		//    {
		//        if (_contentTemplateDefinition == null)
		//        {
		//            _contentTemplateDefinition = new TemplateDefinition(this, ContentTemplateName, base.Component, ContentTemplatePropertyName, false);
		//        }

		//        return _contentTemplateDefinition;
		//    }
		//}

		//private PropertyDescriptor ContentTemplatePropertyDescriptor
		//{
		//    get
		//    {
		//        if (_contentTemplatePropertyDescriptor == null)
		//        {
		//            _contentTemplatePropertyDescriptor = TypeDescriptor.GetProperties(base.Component)[ContentTemplatePropertyName];
		//        }

		//        return _contentTemplatePropertyDescriptor;
		//    }
		//}

		//private TemplateDefinition HeaderTemplateDefinition
		//{
		//    get
		//    {
		//        if (_headerTemplateDefinition == null)
		//        {
		//            _headerTemplateDefinition = new TemplateDefinition(this, HeaderTemplateName, base.Component, HeaderTemplatePropertyName, false);
		//        }

		//        return _headerTemplateDefinition;
		//    }
		//}

		//private PropertyDescriptor HeaderTemplatePropertyDescriptor
		//{
		//    get
		//    {
		//        if (_headerTemplatePropertyDescriptor == null)
		//        {
		//            _headerTemplatePropertyDescriptor = TypeDescriptor.GetProperties(base.Component)[HeaderTemplatePropertyName];
		//        }

		//        return _headerTemplatePropertyDescriptor;
		//    }
		//}

		#endregion

		#region Public Override Methods

		/// <summary>
		///		Creates the child controls of this control.
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			// Header
			//((TemplateContainerControl)_currentObject.Controls[0]).Attributes[DesignerRegion.DesignerRegionAttributeName] = "0";
			_currentObject.HeaderTemplateContainer.Attributes[DesignerRegion.DesignerRegionAttributeName] = "0";

			// Content
			//((TemplateContainerControl)_currentObject.Controls[1]).Attributes[DesignerRegion.DesignerRegionAttributeName] = "1";
			_currentObject.ContentTemplateContainer.Attributes[DesignerRegion.DesignerRegionAttributeName] = "1";
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="regions"></param>
		/// <returns></returns>
		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			// Create an editable region for the HeaderTemplate and add it to the regions.
			EditableDesignerRegion headerRegion = new EditableDesignerRegion(this, HeaderTemplateName, false);
			regions.Add(headerRegion);

			// Create an editable region for the ContentTemplate and add it to the regions.
			EditableDesignerRegion contentRegion = new EditableDesignerRegion(this, ContentTemplateName, false);
			regions.Add(contentRegion);

			//// Set the highlight for the selected region
			//regions[myControl.CurrentView].Highlight = true;

			// Use the base class to render the markup
			return base.GetDesignTimeHtml();
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="region"></param>
		/// <returns></returns>
		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}

			IDesignerHost host = (IDesignerHost)Component.Site.GetService(typeof(IDesignerHost));

			if (host != null)
			{
				ITemplate template = null;

				switch (region.Name)
				{
					case ContentTemplateName:
						template = _currentObject.ContentTemplate;
						break;

					case HeaderTemplateName:
						template = _currentObject.HeaderTemplate;
						break;
				}

				if (template != null)
				{
					return ControlPersister.PersistTemplate(template, host);
				}
			}

			return string.Empty;
		}

		/// <summary>
		///		Retrieves the HTML markup that provides information about the specified exception.
		/// </summary>
		/// <param name="e">The exception that occurred.</param>
		/// <returns>
		///		The design-time HTML markup for the specified exception.
		/// </returns>
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return CreateErrorDesignTimeHtml(Format.Exception(e), e);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClick(DesignerRegionMouseEventArgs e)
		{
			if (e.Region == null)
				return;

			//// If the clicked region is not a header, return
			//if (e.Region.Name.IndexOf("Header") != 0)
			//    return;

			//// Switch the current view if required
			//if (e.Region.Name.Substring(6, 1) != myControl.CurrentView.ToString())
			//{
			//    Container.CurrentView = int.Parse(e.Region.Name.Substring(6, 1));
			//    base.UpdateDesignTimeHtml();
			//}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="region"></param>
		/// <param name="content"></param>
		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}

			if (content == null)
			{
				return;
			}

			IDesignerHost host = (IDesignerHost)Component.Site.GetService(typeof(IDesignerHost));

			if (host != null)
			{
				ITemplate template = ControlParser.ParseTemplate(host, content);

				switch (region.Name)
				{
					case ContentTemplateName:
						_currentObject.ContentTemplate = template;
						break;

					case HeaderTemplateName:
						_currentObject.HeaderTemplate = template;
						break;
				}
			}
		}

		#endregion

		#region Private Classes

		//private class ContainerDesignerRegion : TemplatedEditableDesignerRegion
		//{
		//    public ContainerDesignerRegion(TemplateDefinition definition)
		//        : base(definition)
		//    {
		//        base.EnsureSize = false;
		//        base.IsSingleInstanceTemplate = true;
		//        base.Selectable = true;
		//        base.ServerControlsOnly = false;
		//    }
		//}

		#endregion
	}
}
