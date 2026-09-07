namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Provides data for the <see cref="SfGridSplitter.Resizing"/> event.
    /// </summary>
    /// <remarks>
    /// This event argument is raised continuously as a user drags a separator to resize adjacent panes.
    /// The event is throttled to approximately 60 FPS (every ~16 ms) to avoid performance degradation.
    /// To cancel a resize operation, set
    /// Cancel to <c>true</c> in the
    /// <see cref="SfGridSplitter.ResizeStarted"/> event handler BEFORE the drag begins mutating
    /// the layout. The <see cref="SfGridSplitter.Resizing"/> event is purely informational and
    /// no longer carries a <c>Cancel</c> flag - cancellation must be signalled in
    /// <see cref="SfGridSplitter.ResizeStarted"/>.
    /// </remarks>
    public class GridSplitterResizingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridSplitterResizingEventArgs"/> class.
        /// </summary>
        /// <param name="index">
        /// An array containing the zero-based indexes of the affected panes. The first value
        /// represents the primary pane being resized, and the second value represents its adjacent pane.
        /// </param>
        /// <param name="pane">
        /// An array containing the affected <see cref="SplitterPane"/> instances. The first item
        /// is the primary pane being resized, and the second item is its adjacent pane.
        /// </param>
        public GridSplitterResizingEventArgs(int[] index, SplitterPane[] pane)
        {
            Indexes = index;
            Panes = pane;
        }

        /// <summary>
        /// Gets the zero-based indexes of the affected panes.
        /// </summary>
        /// <remarks>
        /// The first index represents the primary pane being resized, and the second index
        /// represents its adjacent pane in the <see cref="SfGridSplitter.SplitterPanes"/> collection.
        /// </remarks>
        public int[] Indexes { get; }

        /// <summary>
        /// Gets the affected <see cref="SplitterPane"/> instances.
        /// </summary>
        /// <remarks>
        /// The first pane is the primary pane being resized, and the second pane is its adjacent pane.
        /// </remarks>
        public SplitterPane[] Panes { get; }
    }
}
