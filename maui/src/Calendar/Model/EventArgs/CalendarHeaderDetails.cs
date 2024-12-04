namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which holds the details of the calendar header view to be set as binding context of <see cref="SfCalendar.HeaderTemplate"/>
    /// </summary>
    public class CalendarHeaderDetails
    {
        #region Properties

        /// <summary>
        /// Gets the start date of calendar visible dates.
        /// </summary>
        public DateTime StartDateRange { get; internal set; }

        /// <summary>
        /// Gets the end date of calendar visible dates.
        /// </summary>
        public DateTime EndDateRange { get; internal set; }

        /// <summary>
        /// Gets the header text based on header text format.
        /// </summary>
        public string Text { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the calendar views.
        /// </summary>
        public CalendarView View { get; internal set; }

        #endregion
    }
}