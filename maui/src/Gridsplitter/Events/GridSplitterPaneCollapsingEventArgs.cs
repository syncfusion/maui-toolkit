using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Provides data for the <see cref="SfGridSplitter.Collapsing"/> event.
    /// </summary>
    /// <remarks>
    /// This event argument is raised when a pane is about to be collapsed (hidden).
    /// The event includes the index of the pane being collapsed and a reference to the pane itself.
    /// Handlers can set Cancel to <c>true</c> to prevent the collapse operation from occurring.
    /// </remarks>
    public class GridSplitterPaneCollapsingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridSplitterPaneCollapsingEventArgs"/> class.
        /// </summary>
        /// <param name="index">
        /// The zero-based indexes of the panes involved in the collapse operation, including
        /// the pane being collapsed and its adjacent pane.
        /// </param>
        /// <param name="pane">
        /// The <see cref="SplitterPane"/> instances involved in the collapse operation,
        /// including the pane being collapsed and its adjacent pane.
        /// </param>
        public GridSplitterPaneCollapsingEventArgs(int[] index, SplitterPane[] pane)
        {
            Indexes = index;
            Panes = pane;
            Cancel = false;
        }

        /// <summary>
        /// Gets the zero-based indexes of the panes involved in the collapse operation.
        /// </summary>
        /// <remarks>
        /// These indexes correspond to the positions of the affected panes in the
        /// <see cref="SfGridSplitter.SplitterPanes"/> collection, including the pane
        /// being collapsed and its adjacent pane.
        /// </remarks>
        public int[] Indexes { get; }

        /// <summary>
        /// Gets the <see cref="SplitterPane"/> instances involved in the collapse operation.
        /// </summary>
        /// <remarks>
        /// Contains the affected panes, including the pane being collapsed and its adjacent pane.
        /// </remarks>
        public SplitterPane[] Panes { get; }

    }
}
