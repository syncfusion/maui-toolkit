namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
    /// <summary>
    /// Describes the possible values for the position of the selection strip in <see cref="SfSegmentedControl"/> View.
    /// The selection indicator is used to indicate the selected index in the <see cref="SfSegmentedControl"/> View.
    /// </summary>
    public enum SelectionIndicatorPlacement
    {
        /// <summary>
        /// Specifies the fill region for the selection indicator in <see cref="SfSegmentedControl"/> View.
        /// </summary>
        Fill,

        /// <summary>
        /// Specifies the border to cover the outer region of the selected item in <see cref="SfSegmentedControl"/> View.
        /// </summary>
        Border,

        /// <summary>
        /// Specifies the top location for the selection indicator in <see cref="SfSegmentedControl"/> View.
        /// </summary>
        TopBorder,

        /// <summary>
        /// Specifies the bottom location for the selection indicator in <see cref="SfSegmentedControl"/> View.
        /// </summary>
        BottomBorder
    }
}