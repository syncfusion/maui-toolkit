namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Provides data for the <see cref="SfGridSplitter.ResizeStopped"/> event.
    /// </summary>
    /// <remarks>
    /// This event argument is raised when a user stops dragging a separator (pointer released or resize canceled).
    /// The event includes the index of the pane that was resized and a reference to the pane itself.
    /// This event marks the end of a resize gesture and can be used to commit final layout changes or trigger dependent updates.
    /// </remarks>
    public class GridSplitterResizeStoppedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridSplitterResizeStoppedEventArgs"/> class.
        /// </summary>
        /// <param name="index">
        /// An array containing the zero-based indexes of the affected panes. The first value
        /// represents the primary pane that was resized, and the second value represents its adjacent pane.
        /// </param>
        /// <param name="pane">
        /// An array containing the affected <see cref="SplitterPane"/> instances. The first item
        /// is the primary pane that was resized, and the second item is its adjacent pane.
        /// </param>
        public GridSplitterResizeStoppedEventArgs(int[] index, SplitterPane[] pane)
        {
            Indexes = index;
            Panes = pane;
        }

        /// <summary>
        /// Gets the zero-based indexes of the affected panes.
        /// </summary>
        /// <remarks>
        /// The first index represents the primary pane that was resized, and the second index
        /// represents its adjacent pane in the <see cref="SfGridSplitter.SplitterPanes"/> collection.
        /// </remarks>
        public int[] Indexes { get; }

        /// <summary>
        /// Gets the affected <see cref="SplitterPane"/> instances.
        /// </summary>
        /// <remarks>
        /// The first pane is the primary pane that was resized, and the second pane is its adjacent pane.
        /// </remarks>
        public SplitterPane[] Panes { get; }
    }
}
