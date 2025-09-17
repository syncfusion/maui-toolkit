using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Represents a single item (node) in the sunburst chart hierarchy.
    /// </summary>
    internal class SunburstItem
    {
		#region Internal Properties

        /// <summary>
        /// Gets or sets the slice index of the chart.
        /// </summary>
        internal int SliceIndex { get; set; }

        /// <summary>
        /// Gets or sets the start angle of the segment.
        /// </summary>
        internal double ArcStart { get; set; }

        /// <summary>
        /// Gets or sets the end angle of the segment.
        /// </summary>
        internal double ArcEnd { get; set; }

        /// <summary>
        /// Gets or sets the middle angle of the segment.
        /// </summary>
        internal double ArcMid { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SunburstHierarchicalLevel.GroupMemberPath"/> value.
        /// </summary>
        internal string? Key { get; set; }

        /// <summary>
        /// Gets or sets sum of the <see cref="SunburstHierarchicalLevel.GroupMemberPath"/> value field values.
        /// </summary>
        internal double KeyValue { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SunburstHierarchicalLevel.GroupMemberPath"/> child items source.
        /// </summary>
        internal ObservableCollection<object>? Values { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SunburstHierarchicalLevel.GroupMemberPath"/> child items.
        /// </summary>
        internal List<SunburstItem>? ChildItems { get; set; }

        #endregion
    }
}
