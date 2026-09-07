using System.ComponentModel;
namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Provides data for the <see cref="SfGridSplitter.ResizeStarted"/> event.
    /// </summary>
    /// <remarks>
    /// This event argument is raised when a user begins dragging a separator to resize adjacent panes.
    /// The event includes the index of the pane being resized and a reference to the pane itself.
    /// Handlers can set Cancel to <c>true</c> to prevent the resize operation from starting.
    /// </remarks>
    public class GridSplitterResizeStartedEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridSplitterResizeStartedEventArgs"/> class.
        /// </summary>
        /// <param name="index">
        /// An array containing the zero-based indexes of the affected panes. The first value
        /// represents the primary pane being resized, and the second value represents its adjacent pane.
        /// </param>
        /// <param name="pane">
        /// An array containing the affected <see cref="SplitterPane"/> instances. The first item
        /// is the primary pane being resized, and the second item is its adjacent pane.
        /// </param>
        public GridSplitterResizeStartedEventArgs(int[] index, SplitterPane[] pane)
        {
            Indexes = index;
            Panes = pane;
            Cancel = false;
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
