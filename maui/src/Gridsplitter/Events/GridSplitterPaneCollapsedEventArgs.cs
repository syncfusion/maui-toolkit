namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Provides data for the <see cref="SfGridSplitter.Collapsed"/> event.
    /// </summary>
    /// <remarks>
    /// This event argument is raised after a pane has been successfully collapsed (hidden).
    /// The event includes the index of the collapsed pane and a reference to the pane itself.
    /// This event marks the completion of a collapse operation and can be used to trigger dependent UI updates.
    /// </remarks>
    public class GridSplitterPaneCollapsedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridSplitterPaneCollapsedEventArgs"/> class.
        /// </summary>
        /// <param name="index">
        /// An array containing the zero-based indexes of the affected panes. The first value
        /// represents the collapsed pane, and the second value represents its adjacent pane.
        /// </param>
        /// <param name="pane">
        /// An array containing the affected <see cref="SplitterPane"/> objects. The first item
        /// is the collapsed pane, and the second item is its adjacent pane.
        /// </param>
        public GridSplitterPaneCollapsedEventArgs(int[] index, SplitterPane[] pane)
        {
            Indexes = index;
            Panes = pane;
        }

        /// <summary>
        /// Gets the zero-based indexes of the panes involved in the collapse operation.
        /// </summary>
        /// <remarks>
        /// These indexes correspond to the positions of the affected panes in the
        /// <see cref="SfGridSplitter.SplitterPanes"/> collection, including the
        /// collapsed pane and its adjacent pane.
        /// </remarks>
        public int[] Indexes { get; }

        /// <summary>
        /// Gets the <see cref="SplitterPane"/> instances involved in the collapse operation.
        /// </summary>
        /// <remarks>
        /// Contains the affected panes, including the collapsed pane and its adjacent pane.
        /// </remarks>
         public SplitterPane[] Panes { get; }

    }
}
