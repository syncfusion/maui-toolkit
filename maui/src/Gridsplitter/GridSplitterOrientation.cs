using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Specifies the orientation of the SfGridSplitter layout.
    /// </summary>
    public enum GridSplitterOrientation
    {
        /// <summary>
        /// Panes are arranged side-by-side, left-to-right (column-style).
        /// Separators are vertical lines. Resizing changes the widths of adjacent panes.
        /// This is the default and the layout shown in the reference UI.
        /// </summary>
        Horizontal ,

        /// <summary>
        /// Panes are stacked top-to-bottom (row-style).
        /// Separators are horizontal lines. Resizing changes the heights of adjacent panes.
        /// </summary>
        Vertical 
    }
}
