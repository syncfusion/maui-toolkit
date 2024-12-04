using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Interface that holds the properties to render calendar footer.
    /// </summary>
    internal interface IFooterView : ICalendarCommon
    {
        /// <summary>
        /// Gets the settings of the calendar footer view.
        /// </summary>
        CalendarFooterView FooterView { get; }

        /// <summary>
        /// Method to update after the confirm button clicked.
        /// </summary>
        void OnConfirmButtonClicked();

        /// <summary>
        /// Method to update after the cancel button clicked.
        /// </summary>
        /// <param name="selectedDate">The selected date.</param>
        /// <param name="selectedDates">The selected dates.</param>
        /// <param name="calendarDateRange">The calendar date range.</param>
        void OnCancelButtonClicked(DateTime? selectedDate, ObservableCollection<DateTime> selectedDates, CalendarDateRange? calendarDateRange);

        /// <summary>
        /// Method to update after the today button clicked.
        /// </summary>
        void OnTodayButtonClicked();
    }
}
