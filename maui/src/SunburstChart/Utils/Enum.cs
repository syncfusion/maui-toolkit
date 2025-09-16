using System;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Represents SunburstLabelRotationMode. This enum is used for data label rotation modes.
    /// </summary>
    public enum SunburstLabelRotationMode
    {
        /// <summary>
        /// Indicates that the data label is rotated.
        /// </summary>
        Angle,

        /// <summary>
        /// Indicates that the data label is displayed horizontally.
        /// </summary>
        Normal
    }

    /// <summary>
    /// Represents SunburstLabelOverflowMode. This enum is used for data label visibility modes.
    /// </summary>
    public enum SunburstLabelOverflowMode
    {
        /// <summary>
        /// Data label visibility mode as Trim.
        /// </summary>
        Trim,

        /// <summary>
        /// Data label visibility mode as Hide.
        /// </summary>
        Hide
    }

    /// <summary>
    /// Specifies the type of selection behavior in the sunburst chart.
    /// </summary>
    public enum SunburstSelectionType
    {
        /// <summary>
        /// Selects only the tapped segment.
        /// </summary>
        Single,

        /// <summary>
        /// Selects the tapped segment and all its child segments.
        /// </summary>
        Child,

        /// <summary>
        /// Selects the entire group of the tapped segment in the hierarchy.
        /// </summary>
        Group,

        /// <summary>
        /// Selects the parent of the tapped segment in the hierarchy.
        /// </summary>
        Parent
    }

    /// <summary>
    /// Specifies how selected segments are visually highlighted in the sunburst chart.
    /// </summary>
    /// <remarks>
    /// <para><b>Enum Values:</b></para>
    /// <list type="bullet">
    ///   <item>
    ///     <description><b>HighlightByBrush (1):</b> Highlights selected segments by changing their fill color using the <see cref="SunburstSelectionSettings.Fill"/> property.</description>
    ///   </item>
    ///   <item>
    ///     <description><b>HighlightByOpacity (2):</b> Highlights selected segments by adjusting their opacity using the <see cref="SunburstSelectionSettings.Opacity"/> property.</description>
    ///   </item>
    ///   <item>
    ///     <description><b>HighlightByStroke (4):</b> Highlights selected segments by modifying their stroke color and width using the <see cref="SunburstSelectionSettings.Stroke"/> and <see cref="SunburstSelectionSettings.StrokeWidth"/> properties.</description>
    ///   </item>
    /// </list>
    ///
    /// <para><b>Usage Example:</b></para>
    /// <code><![CDATA[
    /// // Apply multiple display modes using bitwise OR
    /// sunburstChart.SelectionSettings.DisplayMode = SunburstSelectionDisplayMode.HighlightByBrush | SunburstSelectionDisplayMode.HighlightByStroke;
    /// ]]></code>
    /// </remarks>
    [Flags]
    public enum SunburstSelectionDisplayMode
    {
        /// <summary>
        /// Highlights selected segments by changing their fill color.
        /// </summary>
        HighlightByBrush = 1 << 0,   // 1

        /// <summary>
        /// Highlights selected segments by changing their opacity.
        /// </summary>
        HighlightByOpacity = 1 << 1, // 2

        /// <summary>
        /// Highlights selected segments by changing their stroke color and width.
        /// </summary>
        HighlightByStroke = 1 << 2   // 4
    }
}
