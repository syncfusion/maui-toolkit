namespace Syncfusion.Maui.Toolkit
{
    /// <summary>
    /// 
    /// </summary>
    public enum DrawingOrder
    {
        /// <summary>
        /// Draws over the content.
        /// </summary>
        AboveContent,
        /// <summary>
        /// Draws over the content with Drawable view having the touch. This is applicable only for WinUI platform.
        /// </summary>
        AboveContentWithTouch,
        /// <summary>
        /// Draws below the content.
        /// </summary>
        BelowContent,
        /// <summary>
        /// Disables the drawing.
        /// </summary>
        NoDraw
    }
}
