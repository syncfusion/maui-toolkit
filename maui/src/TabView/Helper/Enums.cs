namespace Syncfusion.Maui.Toolkit.TabView
{
	#region TabBarPlacement

	/// <summary>
	/// Describes the possible values for the position of header layout in <see cref="SfTabView"/> control.
	/// </summary>
	public enum TabBarPlacement
	{
		/// <summary>
		/// Specifies the location below the content region of <see cref="SfTabView"/> control.
		/// </summary>
		Bottom,

		/// <summary>
		/// Specifies the location above the content region of <see cref="SfTabView"/> control.
		/// </summary>
		Top,
	}

	#endregion

	#region TabWidthMode

	/// <summary>
	/// Describes the possible values for tab width mode.
	/// </summary>
	public enum TabWidthMode
	{

		/// <summary>
		/// Specifies that all tab header widths share the available header space.
		/// </summary>
		Default,

		/// <summary>
		/// Specifies that the tab header width will be set based on the text size and image of each tab.
		/// </summary>
		SizeToContent,
	}

	#endregion

	#region TabIndicatorPlacement

	/// <summary>
	/// Describes the possible values for selection indicator placement.
	/// </summary>
	public enum TabIndicatorPlacement
	{
		/// <summary>
		/// Specifies the location below the content region of the tab item header.
		/// </summary>
		Bottom,

		/// <summary>
		/// Specifies the full content region of the tab item header.
		/// </summary>
		Fill,

		/// <summary>
		/// Specifies the location above the content region of the tab item header.
		/// </summary>
		Top,
	}

	#endregion

	#region TabImagePosition

	/// <summary>
	/// Describes the possible values for the tab item's image position in the header layout.
	/// </summary>
	public enum TabImagePosition
	{
		/// <summary>
		/// Specifies the location below the tab item's header.
		/// </summary>
		Bottom,

		/// <summary>
		/// Specifies the location left to the tab item's header.
		/// </summary>
		Left,

		/// <summary>
		/// Specifies the location right to the tab item's header.
		/// </summary>
		Right,

		/// <summary>
		/// Specifies the location above the tab item's header.
		/// </summary>
		Top,
	}

	#endregion

	#region TabHeaderAlignment

	/// <summary> 
	/// Describes the possible values for the header position of SfTabView. 
	/// </summary>  
	public enum TabHeaderAlignment
	{
		/// <summary> 
		/// Specifies start position for the tab header. 
		/// </summary>   
		Start,

		/// <summary>
		/// Specifies center position for the tab header.
		/// </summary>
		Center,

		/// <summary>
		/// Specifies end position for the tab header.
		/// </summary>
		End
	}

	#endregion

	#region TabBarDisplayMode

	/// <summary>
	/// Describes the possible values for the display types in the header layout.
	/// </summary>
	public enum TabBarDisplayMode
	{
		/// <summary>
		/// Specifies the image type for header.
		/// </summary>
		Image,

		/// <summary>
		/// Specifies the text type for header.
		/// </summary>
		Text,

		/// <summary>
		/// Specifies both image and text type for header.
		/// </summary>  
		Default,
	}

	#endregion

	#region IndicatorWidthMode

	/// <summary>
	/// Describes the possible values for tab indicator width.
	/// </summary>
	public enum IndicatorWidthMode
	{
		/// <summary> 
		/// Specifies that the selection indicator width will be set based on the header’s content width of the tab view.
		/// </summary>  
		Fit,

		/// <summary> 
		/// Specifies that the selection indicator width will be set based on the header width value of the tab view.
		/// </summary> 
		Stretch,
	}

	#endregion

	#region CenterButtonDisplatMode

	/// <summary>
	/// Specifies the possible values for the display type of center button in TabView. 
	/// </summary>
	public enum CenterButtonDisplayMode
	{
		/// <summary>
		/// Specifies the image types for center button in Tab View.
		/// </summary>  
		Image,

		/// <summary>
		/// Specifies the text types for center button in Tab View.
		/// </summary>
		Text,

		/// <summary>
		/// Specifies both image as well as text for center button in Tab View.
		/// </summary>
		ImageWithText
	}

	#endregion

	#region ArrowType

	/// <summary>
	/// Specifies the direction of arrows used, either backward or forward.
	/// </summary>
	internal enum ArrowType
	{
		/// <summary>
		/// Specifies the backward direction for arrows.
		/// </summary>
		Backward,

		/// <summary>
		/// Specifies the forward direction for arrows.
		/// </summary>
		Forward,
	}
	#endregion
}