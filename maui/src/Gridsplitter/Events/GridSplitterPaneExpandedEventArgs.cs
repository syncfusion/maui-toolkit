namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Provides data for the <see cref="SfGridSplitter.Expanded"/> event.
    /// </summary>
    /// <remarks>
    /// This event argument is raised after a pane has been successfully expanded (shown after being collapsed).
    /// The event includes the index of the expanded pane and a reference to the pane itself.
    /// This event marks the completion of an expand operation and can be used to trigger dependent UI updates.
    /// </remarks>
    public class GridSplitterPaneExpandedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridSplitterPaneExpandedEventArgs"/> class.
        /// </summary>
        /// <param name="index">
        /// The zero-based indexes of the panes involved in the expand operation, including
        /// the expanded pane and its adjacent pane.
        /// </param>
        /// <param name="pane">
        /// The <see cref="SplitterPane"/> instances involved in the expand operation,
        /// including the expanded pane and its adjacent pane.
        /// </param>
        public GridSplitterPaneExpandedEventArgs(int[] index, SplitterPane[] pane)
        {
            Indexes = index;
            Panes = pane;
        }

        /// <summary>
        /// Gets the zero-based indexes of the panes involved in the expand operation.
        /// </summary>
        /// <remarks>
        /// These indexes correspond to the positions of the affected panes in the
        /// <see cref="SfGridSplitter.SplitterPanes"/> collection, including the
        /// expanded pane and its adjacent pane.
        /// </remarks>
        public int[] Indexes { get; }

        /// <summary>
        /// Gets the <see cref="SplitterPane"/> instances involved in the expand operation.
        /// </summary>
        /// <remarks>
        /// Contains the affected panes, including the expanded pane and its adjacent pane.
        /// </remarks>
        public SplitterPane[] Panes { get; }
    }
}
