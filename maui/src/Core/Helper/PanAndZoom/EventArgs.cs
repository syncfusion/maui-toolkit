namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Provides data for the zoom events.
    /// </summary>	
    public class ZoomEventArgs : EventArgs
    {
        internal ZoomEventArgs(double zoomFactor, Point zoomOrigin)
        {
            ZoomOrigin = zoomOrigin;
            ZoomFactor = zoomFactor;
        }

        /// <summary>
        /// Gets the desired origin (or center) of the zoom action.
        /// </summary>
        public Point ZoomOrigin { get; internal set; }

        /// <summary>
        /// Gets the angle of pinch interaction
        /// </summary>
        public double? Angle { get; internal set; }

        /// <summary>
        /// Gets the current zoom factor.
        /// </summary>
        public double ZoomFactor { get; internal set; }
    }
}