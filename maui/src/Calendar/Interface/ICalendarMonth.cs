namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Interface that holds the properties of Calendar Month view
    /// </summary>
    internal interface ICalendarMonth : ICalendarCommon
    {
        /// <summary>
        /// Gets the settings for the calendar month view.
        /// </summary>
        CalendarMonthView MonthView { get; }
    }
}