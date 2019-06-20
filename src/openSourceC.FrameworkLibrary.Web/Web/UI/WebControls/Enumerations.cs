using System;

namespace openSourceC.FrameworkLibrary.Web.UI.WebControls
{
	/// <summary>
	///		Amount of background noise to add to rendered image
	/// </summary>
	public enum BackgroundNoiseLevel
	{
		/// <summary></summary>
		None,
		/// <summary></summary>
		Low,
		/// <summary></summary>
		Medium,
		/// <summary></summary>
		High,
		/// <summary></summary>
		Extreme
	}

	/// <summary>
	///		
	/// </summary>
	public enum CacheType
	{
		/// <summary></summary>
		HttpRuntime,
		/// <summary></summary>
		Session
	}

	/// <summary>
	///		
	/// </summary>
	public enum CaptchaLayout
	{
		/// <summary></summary>
		Horizontal,
		/// <summary></summary>
		Vertical
	}

	/// <summary>
	///		Specifies the sides where floating objects are not accepted.
	/// </summary>
	public enum ClearStyle
	{
		/// <summary>Floating objects are allowed on both sides.</summary>
		None,
		/// <summary>Object is moved below any floating object on the left side.</summary>
		Left,
		/// <summary>Object is moved below any floating object on the right side.</summary>
		Right,
		/// <summary>Object is moved below any floating object</summary>
		Both
	}

	/// <summary>
	///		Specifies the state of the collapsable object.
	/// </summary>
	public enum CollapseModeState
	{
		/// <summary>The object is not collapsable.</summary>
		Disabled,
		/// <summary>The object is expanded.</summary>
		Expanded,
		/// <summary>The object is collapsed.</summary>
		Collapsed
	}

	/// <summary>
	///		Specifies which side of the object that text will flow.
	/// 
	///		<para>
	///			<b>div</b> and <b>span</b> objects must have a width set for the <b>float</b>
	///			attribute to render. In Microsoft Internet Explorer 5, <b>div</b> and <b>span</b>
	///			objects are assigned a width by default and will render if a width is not specified.
	///		</para>
	/// </summary>
	public enum FloatStyle
	{
		/// <summary>Object displays where it appears in the text.</summary>
		None,
		/// <summary>Text flows to the right of the object.</summary>
		Left,
		/// <summary>Text flows to the left of the object.</summary>
		Right
	}

	/// <summary>
	///		Amount of random font warping to apply to rendered text
	/// </summary>
	public enum FontWarpFactor
	{
		/// <summary></summary>
		None,
		/// <summary></summary>
		Low,
		/// <summary></summary>
		Medium,
		/// <summary></summary>
		High,
		/// <summary></summary>
		Extreme
	}

	/// <summary>
	///		Amount of curved line noise to add to rendered image
	/// </summary>
	public enum LineNoiseLevel
	{
		/// <summary></summary>
		None,
		/// <summary></summary>
		Low,
		/// <summary></summary>
		Medium,
		/// <summary></summary>
		High,
		/// <summary></summary>
		Extreme
	}
}
