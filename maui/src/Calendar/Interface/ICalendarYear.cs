namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Interface that holds the properties of year view.
    /// </summary>
    internal interface ICalendarYear : ICalendarCommon, IInteractionInfo
    {
        /// <summary>
        /// Gets the settings for the calendar year view.
        /// </summary>
        CalendarYearView YearView { get; }
    }
}