using System.ComponentModel;
namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Provides data for the <see cref="SfGridSplitter.Expanding"/> event.
    /// </summary>
    /// <remarks>
    /// This event argument is raised when a pane is about to be expanded (shown after being collapsed).
    /// The event includes the index of the pane being expanded and a reference to the pane itself.
    /// Handlers can set Cancel to <c>true</c> to prevent the expand operation from occurring.
    /// </remarks>
    public class GridSplitterPaneExpandingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridSplitterPaneExpandingEventArgs"/> class.
        /// </summary>
        /// <param name="index">
        /// An array containing the zero-based indexes of the affected panes. The first value
        /// represents the pane being expanded, and the second value represents its adjacent pane.
        /// </param>
        /// <param name="pane">
        /// An array containing the affected <see cref="SplitterPane"/> instances. The first item
        /// is the pane being expanded, and the second item is its adjacent pane.
        /// </param>
        public GridSplitterPaneExpandingEventArgs(int[] index, SplitterPane[] pane)
        {
            Indexes = index;
            Panes = pane;
            Cancel = false;
        }

        /// <summary>
        /// Gets the zero-based indexes of the affected panes.
        /// </summary>
        /// <remarks>
        /// The first index represents the pane being expanded, and the second index represents
        /// its adjacent pane in the <see cref="SfGridSplitter.SplitterPanes"/> collection.
        /// </remarks>
        public int[] Indexes { get; }

        /// <summary>
        /// Gets the affected <see cref="SplitterPane"/> instances.
        /// </summary>
        /// <remarks>
        /// The first pane is the pane being expanded, and the second pane is its adjacent pane.
        /// </remarks>
        public SplitterPane[] Panes { get; }

    }
}
