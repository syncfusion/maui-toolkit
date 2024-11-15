namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
    /// <summary>
    /// Interface for Line drawing customizations.
    /// </summary>
    public interface ILineDrawing
    {
        /// <summary>
        /// Color for line.
        /// </summary>
        Color Stroke { get; set; }

        /// <summary>
        /// Represents line thickness.
        /// </summary>
        double StrokeWidth { get; set; }

        /// <summary>
        /// Represents line drawing smoothness. 
        /// </summary>
        bool EnableAntiAliasing { get; set; }

        /// <summary>
        /// Represents line brush opacity.
        /// </summary>
        float Opacity { get; set; }

        /// <summary>
        /// Represents line drawing dash array.
        /// </summary>
        DoubleCollection? StrokeDashArray { get; set; }

    }
}
