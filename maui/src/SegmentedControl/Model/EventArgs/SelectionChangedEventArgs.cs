using System;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
    /// <summary>
    /// Provides data for the event when the selection is changed in the segmented control.
    /// </summary>
    public class SelectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the index of the previously selected item before the selection change.
        /// </summary>
        public int? OldIndex { get; internal set; }

        /// <summary>
        /// Gets the index of the newly selected item after the selection change.
        /// </summary>
        public int? NewIndex { get; internal set; }

#nullable disable
        /// <summary>
        /// Gets the previously selected item before the selection change.
        /// </summary>
        public SfSegmentItem OldValue { get; internal set; }

        /// <summary>
        /// Gets the newly selected item after the selection change.
        /// </summary>
        public SfSegmentItem NewValue { get; internal set; }
#nullable enable
    }
}