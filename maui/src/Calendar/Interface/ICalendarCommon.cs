using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Interface that holds the common properties
    /// </summary>
    internal interface ICalendarCommon : IHeaderCommon
    {
        /// <summary>
        /// Gets the selection shape.
        /// </summary>
        CalendarSelectionShape SelectionShape { get; }

        /// <summary>
        /// Gets the display date of the calendar.
        /// </summary>
        DateTime DisplayDate { get; }

        /// <summary>
        /// Gets a value indicating whether to enable past dates or not.
        /// </summary>
        bool EnablePastDates { get; }

        /// <summary>
        /// Gets a value indicating whether to enable swipe selection or not.
        /// </summary>
        bool EnableSwipeSelection { get; }

        /// <summary>
        /// Gets a value indicating whether the dates out of range.
        /// </summary>
        bool ShowOutOfRangeDates { get; }

        /// <summary>
        /// Gets the selected date of the calendar.
        /// </summary>
        DateTime? SelectedDate { get; }

        /// <summary>
        /// Gets the selection background.
        /// </summary>
        Brush? SelectionBackground { get; }

        /// <summary>
        /// Gets the range selection color.
        /// </summary>
        Brush? SelectedRangeColor { get; }

        /// <summary>
        /// Gets the hover color.
        /// </summary>
        Brush? HoverColor { get; }

        /// <summary>
        /// Gets a value indicating whether to displays the leading and trailing dates of the SfCalendar.
        /// </summary>
        bool ShowTrailingAndLeadingDates { get; }

        /// <summary>
        /// Gets a value indicating whether the selection can be removed while interaction.
        /// </summary>
        bool CanToggleDaySelection { get; }

        /// <summary>
        /// Gets a selection mode of the calendar
        /// </summary>
        CalendarSelectionMode SelectionMode { get; }

        /// <summary>
        /// Gets the selected dates of the calendar view.
        /// </summary>
        ObservableCollection<DateTime> SelectedDates { get; }

        /// <summary>
        /// Gets the selected date range.
        /// </summary>
        CalendarDateRange? SelectedDateRange { get; }

        /// <summary>
        /// Gets the selected multi date range.
        /// </summary>
        ObservableCollection<CalendarDateRange>? SelectedDateRanges { get; }

        /// <summary>
        /// Gets the start range selection background.
        /// </summary>
        Brush StartRangeSelectionBackground { get; }

        /// <summary>
        /// Gets the end range selection background.
        /// </summary>
        Brush EndRangeSelectionBackground { get; }

        /// <summary>
        /// Gets a direction of the range selection.
        /// </summary>
        CalendarRangeSelectionDirection RangeSelectionDirection { get; }

        /// <summary>
        /// Gets a month view header template.
        /// </summary>
        DataTemplate MonthViewHeaderTemplate { get; }

        /// <summary>
        /// Gets a custom template view for single selection.
        /// </summary>
        DataTemplate SelectionCellTemplate { get; }

        /// <summary>
        /// Gets a value to display the number of weeks in calendar month view.
        /// </summary>
        int NumberOfVisibleWeeks { get; }
        /// <summary>
        /// Gets a calendar mode.
        /// </summary>
        CalendarMode Mode { get; }
    }
}