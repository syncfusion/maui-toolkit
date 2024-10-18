
namespace Syncfusion.Maui.Toolkit
{
    /// <summary>
    /// Represents the position of the tooltip label.
    /// </summary>
    internal enum TooltipPosition
    {
        /// <summary>
        /// Positioning the tooltip based on available size. By default, it will position on top.
        /// </summary>
        Auto,

        /// <summary>
        /// Positioning the tooltip at left of target rect.
        /// </summary>
        Left,

        /// <summary>
        /// Positioning the tooltip at top of target rect.
        /// </summary>
        Top,

        /// <summary>
        /// Positioning the tooltip at right of target rect.
        /// </summary>
        Right,

        /// <summary>
        /// Positioning the tooltip at bottom of target rect.
        /// </summary>
        Bottom,
    }

    /// <summary>
    /// Represents the shape of tooltip UI.
    /// </summary>
    internal enum TooltipDrawType
    {
        /// <summary>
        /// Default UI for tooltip.
        /// </summary>
        Default,

        /// <summary>
        /// Paddle UI for tooltip.
        /// </summary>
        Paddle,
    }
}
