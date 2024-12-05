namespace Syncfusion.Maui.Toolkit.BottomSheet
{
    /// <summary>
    /// Specifies the current display state of the SfBottomSheet control.
    /// </summary>
    /// <remarks>
    /// This enum is used to represent the various possible states of the bottom sheet,
    /// allowing for precise control and state management of the UI component.
    /// </remarks>
    public enum BottomSheetState 
    {
        /// <summary>
        /// Indicates that the bottom sheet is fully expanded to cover the entire screen.
        /// </summary> 
        FullExpanded,

        /// <summary>
        /// Represents the state where the bottom sheet is expanded to cover approximately half of the screen.
        /// </summary>
        HalfExpanded,

        /// <summary>
        /// Denotes that the bottom sheet is in its minimized or collapsed state, typically showing only a small portion or header.
        /// </summary>
        Collapsed,

        /// <summary>
        /// Signifies that the bottom sheet is completely hidden from view.
        /// </summary>
        Hidden
    }

    /// <summary>
    /// Defines the allowable states for the SfBottomSheet control.
    /// </summary>
    /// <remarks>
    /// This enum is used to configure the permitted states of the bottom sheet,
    /// enabling developers to restrict or allow specific display modes.
    /// </remarks>
    public enum BottomSheetAllowedState
    {
        /// <summary>
        /// Configures the bottom sheet to only allow full screen expansion.
        /// When set, the bottom sheet can only be fully expanded or hidden.
        /// </summary> 
        FullExpanded,

        /// <summary>
        /// Restricts the bottom sheet to only permit half screen expansion.
        /// When set, the bottom sheet can only be half expanded or hidden.
        /// </summary>
        HalfExpanded,

        /// <summary>
        /// Allows the bottom sheet to be displayed in both full screen and half screen modes.
        /// This option provides the most flexibility, permitting all possible states.
        /// </summary>
        All
    }

	/// <summary>
	/// Defines the content width mode for the SfBottomSheet control.
	/// </summary>
	public enum BottomSheetContentWidthMode
	{
		/// <summary>
		/// The BottomSheet will span the full width of the  parent container.
		/// </summary>
		Full,

		/// <summary>
		/// The BottomSheet will use a custom width value, centered if not full width.
		/// </summary>
		Custom
	}
}
