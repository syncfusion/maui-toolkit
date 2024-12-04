namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Defines the Selected range element of Month and Year View panel
    /// </summary>
    internal enum SelectedRangeStatus
    {
        /// <summary>
        /// The string value used to detect the calendar selected range having only start date value while the draw method triggered.
        /// While triggered get a range selection status and range selection draw method.
        /// </summary>
        SelectedRange,

        /// <summary>
        /// The string value denotes the range selection status of the date value is start range. This value assigned while the date value is start date of the selected range and selected range have different and valid start and end date.
        /// This value used to draw the selection on range selection.
        /// </summary>
        StartRange,

        /// <summary>
        /// The string value denotes the range selection status of the date value is end range. This value assigned while the date value is end date of the selected range and selected range have different and valid start and end date.
        /// This value used to draw the selection on range selection.
        /// </summary>
        EndRange,

        /// <summary>
        /// The string value denotes the range selection status of the date value is inBetween range. This value assigned while the date value is with in the start date and end date of the selected range and selected range have different and valid start and end date.
        /// This value used to draw the selection on range selection.
        /// </summary>
        InBetweenRange,

        /// <summary>
        /// The HoverStartDate is used to detect the hovered date is greater than end range selection or not.
        /// It is applicable for the range selection direction is forward, both and none.
        /// Example: Range(Sep-10 to Sep-15). The current hovered date is Sep-16. Then the Sep-15 is end range selection applied. Then need to draw the dash line for the end range.
        /// </summary>
        HoverStartDate,

        /// <summary>
        /// The HoverEndDate is used to detect the hovered date is lesser than start range selection or not.
        /// It is applicable for the range selection direction is backward, both and none.
        /// Example: Range(Sep-10 to Sep-15). The current hovered date is Sep-09. Then the Sep-10 is start range selection applied. Then need to draw the dash line for the start range.
        /// </summary>
        HoverEndDate,
    }
}