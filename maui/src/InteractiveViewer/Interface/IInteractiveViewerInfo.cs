namespace Syncfusion.Maui.Toolkit.InteractiveViewer
{
    /// <summary>
    /// Represents the properties and settings for content hosted in a Interactive Viewer.
    /// </summary>
    internal interface IInteractiveViewerInfo : IInteractiveZoomInfo
    {
        /// <summary>
        /// Gets the view displayed within the Interactive Viewer.
        /// </summary>
        View? Content { get; }

        /// <summary>
        /// Resets the zoom and pan values of the Interactive Viewer to their default values.
        /// </summary>
        void ResetZoomAndPan();
    }

    /// <summary>
    /// Provides functionality for managing zoom and scroll operations.
    /// </summary>
    internal interface IInteractiveZoomInfo
    {
        /// <summary>
        /// Method to update the content size.
        /// </summary>
        /// <param name="contentSize">The content size.</param>
        /// <param name="isInitialZoom">Indicates whether the content size is updated for the first zoom operation.</param>
        void UpdateContentSize(Size contentSize, bool isInitialZoom);

        /// <summary>
        /// Scrolls to the specified offsets.
        /// </summary>
        /// <param name="xOffset">The horizontal offset.</param>
        /// <param name="yOffset">The vertical offset.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ScrollToAsync(double xOffset, double yOffset);

        /// <summary>
        /// Gets the viewport size.
        /// </summary>
        /// <returns>The viewport size.</returns>
        Size GetViewportSize();

        /// <summary>
        /// Gets the scroll view size.
        /// </summary>
        /// <returns>The scroll view size.</returns>
        Size GetScrollViewDesiredSize();

        /// <summary>
        /// Gets the horizontal scroll offset.
        /// </summary>
        /// <returns>The horizontal scroll offset.</returns>
        double GetScrollX();

        /// <summary>
        /// Gets the vertical scroll offset.
        /// </summary>
        /// <returns>The vertical scroll offset.</returns>
        double GetScrollY();
    }
}