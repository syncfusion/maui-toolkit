namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which holds the details of a cell to be set as binding context of <see cref="CalendarYearView.CellTemplate"/> and <see cref="CalendarMonthView.CellTemplate"/>/>.
    /// </summary>
    public class CalendarCellDetails
    {
        #region Properties

        /// <summary>
        /// Gets the cell date time value.
        /// </summary>
        public DateTime Date { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the date is leading or trailing date <see cref="SfCalendar"/>.
        /// </summary>
        public bool IsTrailingOrLeadingDate { get; internal set; }

        #endregion
    }
}
