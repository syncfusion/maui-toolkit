namespace Syncfusion.Maui.Toolkit.InteractiveViewer
{
    /// <summary>
    /// Provides data for the <see cref="SfInteractiveViewer.ScrollChanged"/> event, including the current pan direction and zoom factor.
    /// </summary>
    public class InteractiveScrollChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveScrollChangedEventArgs"/> class.
        /// </summary>
        /// <param name="panAxis">Specifies the directions in which panning is currently allowed.</param>
        /// <param name="zoomFactor">Specifies the current zoom factor applied to the content.</param>
        public InteractiveScrollChangedEventArgs(PanAxis panAxis, double zoomFactor)
        {
            PanAxis = panAxis;
            ZoomFactor = zoomFactor;
        }

        /// <summary>
        /// Gets the directions in which panning is currently allowed.
        /// </summary>
        public PanAxis PanAxis { get; internal set; }

        /// <summary>
        /// Gets the current zoom factor applied to the content.
        /// </summary>
        public double ZoomFactor { get; internal set; }
    }
}