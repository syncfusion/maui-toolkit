namespace Syncfusion.Maui.Toolkit.ProgressBar
{
    /// <summary>
    /// Represents information about the arcs that rendered in progress bar.
    /// </summary>
    internal struct CircularProgressBarArcInfo
    {
        /// <summary>
        /// Gets or sets arc bounds top left position value of arc.
        /// </summary>
        internal PointF TopLeft { get; set; }

        /// <summary>
        /// Gets or sets arc bounds bottom right position value of arc.
        /// </summary>
        internal PointF BottomRight { get; set; }

        /// <summary>
        /// Gets or sets arc start angle. 
        /// </summary>
        internal float StartAngle { get; set; }

        /// <summary>
        /// Gets or sets arc end angle.
        /// </summary>
        internal float EndAngle { get; set; }

        /// <summary>
        /// Gets or sets arc path instance, that holds shapes of arc.
        /// </summary>
        internal PathF ArcPath { get; set; }

        /// <summary>
        /// Gets or sets paint, that represents arc fill color. 
        /// </summary>
        internal Paint FillPaint { get; set; }

        /// <summary>
        /// Gets or sets inner arc start radius.
        /// </summary>
        internal double InnerStartRadius { get; set; }

        /// <summary>
        /// Gets or sets inner arc end radius. 
        /// </summary>
        internal double InnerEndRadius { get; set; }
    }
}
