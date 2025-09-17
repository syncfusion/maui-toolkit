using System;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Provides data for the <see cref="SfSunburstChart.SelectionChanging"/> event.
    /// </summary>
    /// <remarks>
    /// <para>The SunburstSelectionChangingEventArgs class provides information about the selection change that is about to occur in the sunburst chart. This event is triggered before the selection is applied, allowing you to cancel the selection change if needed.</para>
    /// 
    /// <para>The event provides details about both the previously selected segment and the new segment being selected, as well as the ability to cancel the selection change.</para>
    /// 
    /// <example>
    /// <code>
    /// <![CDATA[
    /// sunburstChart.SelectionChanging += OnSelectionChanging;
    /// 
    /// private void OnSelectionChanging(object sender, SunburstSelectionChangingEventArgs args)
    /// {
    ///     var oldSegment = args.OldSegment;
    ///     var newSegment = args.NewSegment;
    ///     
    ///     // Cancel selection change, if needed.
    ///     args.Cancel = true;
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// 
    /// <para><b>Properties:</b></para>
    /// <list type="bullet">
    ///   <item>
    ///     <term><see cref="OldSegment"/></term>
    ///     <description>The segment that was previously selected before the selection change is initiated.</description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="NewSegment"/></term>
    ///     <description>The segment that is about to be selected or deselected.</description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="Cancel"/></term>
    ///     <description>Gets or sets a value indicating whether the selection change should be canceled.</description>
    ///   </item>
    ///   
    /// </list>
    /// </remarks>
    public class SunburstSelectionChangingEventArgs : EventArgs
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SunburstSelectionChangingEventArgs"/> class.
        /// </summary>
        /// <param name="oldSegment">The previously selected segment.</param>
        /// <param name="newSegment">The segment that is being selected.</param>
        public SunburstSelectionChangingEventArgs(SunburstSegment? oldSegment, SunburstSegment newSegment)
        {
            OldSegment = oldSegment;
            NewSegment = newSegment;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the segment that was previously selected before the current selection change.
        /// </summary>
        /// <value>
        /// The previously selected segment, or <c>null</c> if no segment was previously selected or this is the first selection.
        /// </value>
        public SunburstSegment? OldSegment
        {
            get; internal set;
        }

        /// <summary>
        /// Gets the segment that is being selected.
        /// </summary>
        /// <value>
        /// The segment that is about to be selected.
        /// </value>
        public SunburstSegment NewSegment
        {
            get; internal set;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the selection change should be canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> to cancel the selection change; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// private void OnSelectionChanging(object sender, SunburstSelectionChangingEventArgs args)
        /// {
        ///     // Cancel selection for specific categories
        ///     // Access category data through the Item property safely
        ///     string? category = "";
        ///     if (args.NewSegment?.Item is List<object?> items && items.Count >= 2)
        ///     {
        ///         category = items[0]?.ToString() ?? "";
        ///     }
        ///     
        ///     if (category == "RestrictedCategory")
        ///     {
        ///         args.Cancel = true;
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        public bool Cancel { get; set; }

        #endregion
    }
}
