using System;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// The <see cref="SunburstSelectionChangedEventArgs"/> class contains information about the selection change that occurs in a <see cref="SfSunburstChart"/>.
    /// </summary>
    /// <remarks>
    /// <para>This event argument provides access to the segment that was previously selected, the segment that is newly selected or deselected, and a flag indicating whether the segment is currently selected.</para>
    ///
    /// <para><b>Usage Example:</b></para>
    /// <code><![CDATA[
    /// sunburstChart.SelectionChanged += OnSelectionChanged;
    /// 
    /// void OnSelectionChanged(object sender, SunburstSelectionChangedEventArgs e)
    /// {
    ///     if (e.IsSelected)
    ///     {
    ///         Debug.WriteLine($"Selected Index: {e.NewSegment.Index}");
    ///     }
    ///     else
    ///     {
    ///         Debug.WriteLine($"Old selected Index: {e.OldSegment.Index}");
    ///     }
    /// }
    /// ]]></code>
    ///
    /// <para><b>Properties:</b></para>
    /// <list type="bullet">
    ///   <item>
    ///     <term><see cref="OldSegment"/></term>
    ///     <description>The segment that was previously selected before the current selection change.</description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="NewSegment"/></term>
    ///     <description>The segment that is newly selected or deselected as a result of the interaction.</description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="IsSelected"/></term>
    ///     <description>Indicates whether the <see cref="NewSegment"/> is selected or deselected.</description>
    ///   </item>
    /// </list>
    /// </remarks>

    public class SunburstSelectionChangedEventArgs : EventArgs
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SunburstSelectionChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldSegment">The previously selected segment.</param>
        /// <param name="newSegment">The newly selected segment.</param>
        /// <param name="isSelected">Whether the segment is selected or deselected.</param>
        public SunburstSelectionChangedEventArgs(SunburstSegment? oldSegment, SunburstSegment newSegment, bool isSelected)
        {
            OldSegment = oldSegment;
            NewSegment = newSegment;
            IsSelected = isSelected;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the segment that was previously selected before the current selection change.
        /// </summary>
        /// <value>
        /// The previously selected segment.
        /// </value>
        public SunburstSegment? OldSegment
        {
            get; internal set;
        }

        /// <summary>
        /// Gets the segment that is newly selected or deselected.
        /// </summary>
        /// <value>
        /// The newly selected or deselected segment.
        /// </value>
        public SunburstSegment NewSegment
        {
            get; internal set;
        }

        /// <summary>
        /// Gets a value indicating whether the segment was selected (<c>true</c>) or deselected (<c>false</c>).
        /// </summary>
        public bool IsSelected
        {
            get; internal set;
        }

        #endregion
    }
}
