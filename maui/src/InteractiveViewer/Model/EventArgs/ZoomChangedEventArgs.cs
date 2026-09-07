namespace Syncfusion.Maui.Toolkit.InteractiveViewer
{
    /// <summary>
    /// Provides data for the <see cref="SfInteractiveViewer.ZoomFactorChanged"/> event, including the previous and current zoom factors.
    /// </summary>
    public class ZoomFactorChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomFactorChangedEventArgs"/> class with the specified previous and current zoom factors.
        /// </summary>
        /// <param name="oldZoomFactor">The zoom factor before the zoom operation occurred. </param>
        /// <param name="newZoomFactor">The zoom factor after the zoom operation occurred. </param>
        public ZoomFactorChangedEventArgs(double oldZoomFactor, double newZoomFactor)
        {
            OldZoomFactor = oldZoomFactor;
            NewZoomFactor = newZoomFactor;
        }

        /// <summary>
        /// Gets the zoom factor before the zoom operation.
        /// </summary>
        public double OldZoomFactor { get; internal set; }

        /// <summary>
        /// Gets the zoom factor after the zoom operation.
        /// </summary>
        public double NewZoomFactor { get; internal set; }
    }
}