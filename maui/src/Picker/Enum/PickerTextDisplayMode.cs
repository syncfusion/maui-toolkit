namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Defines the text display mode for the picker.
    /// </summary>
    public enum PickerTextDisplayMode
    {
        /// <summary>
        /// Represents the picker items in default mode.
        /// </summary>
        /// <remarks>
        /// The default font size for the picker Text is 14, and the text color is white.
        /// The default text color for unselected items is black.
        /// </remarks>
        Default,

        /// <summary>
        /// Represents the picker items in fade mode.
        /// </summary>
        /// <remarks>
        /// The default text display color will fade from the selected item.
        /// </remarks>
        Fade,

        /// <summary>
        /// Represents the picker items in shrink mode.
        /// </summary>
        /// <remarks>
        /// The text font size will be reduced from the selected item, while the text color remains the same.
        /// </remarks>
        Shrink,

        /// <summary>
        /// Represents the picker items in fade and shrink mode.
        /// </summary>
        /// <remarks>
        /// The text display color will fade, and the text font size will be reduced from the selected item.
        /// </remarks>
        FadeAndShrink,
    }
}