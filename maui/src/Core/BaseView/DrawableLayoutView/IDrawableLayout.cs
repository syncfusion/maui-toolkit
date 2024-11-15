namespace Syncfusion.Maui.Toolkit
{
    /// <summary>
    /// Represents a layout that can be drawn and supports absolute positioning.
    /// </summary>
    public interface IDrawableLayout : IDrawable, IAbsoluteLayout
    {
        /// <summary>
        /// Invalidates the drawable, causing it to be redrawn.
        /// </summary>
        void InvalidateDrawable();

        /// <summary>
        /// Gets or sets the drawing order of the drawable elements.
        /// </summary>
        DrawingOrder DrawingOrder { get; set; }
    }
}
