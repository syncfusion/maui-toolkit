using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Interface that holds the properties to render the calendar view.
    /// </summary>
    internal interface ICalendar : ICalendarYear, ICalendarMonth, IInteractionInfo, ICalendarCommon
    {
        /// <summary>
        /// Triggered when the visible dates value changed.
        /// </summary>
        /// <param name="visibleDates">Current view visible dates.</param>
        /// <param name="previousVisibleDates">The previous visible dates.</param>
        /// <param name="previousCalendarView">The previous calendar view.</param>
        void UpdateVisibleDates(List<DateTime> visibleDates, List<DateTime> previousVisibleDates, CalendarView previousCalendarView);

        /// <summary>
        /// Triggers the selected date predicate for enable/disable the date.
        /// </summary>
        /// <param name="date">The date value.</param>
        /// <returns>The date is enabled or not.</returns>
        bool IsSelectableDayPredicate(DateTime date);

        /// <summary>
        /// Triggers the special date predicate for enable/disable the date.
        /// </summary>
        /// <param name="date">The date value.</param>
        /// <returns>The calendar special day icon details.</returns>
        CalendarIconDetails? IsSpecialDayPredicate(DateTime date);
    }

    /// <summary>
    /// Interface that holds the properties related to calendar interaction.
    /// </summary>
    internal interface IInteractionInfo
    {
        /// <summary>
        /// Triggers the tapped, double tapped or long press event based on it argument values.
        /// </summary>
        /// <param name="isTapped">Defines the calendar view interaction is tap.</param>
        /// <param name="tapCount">Holds the tap count value.</param>
        /// <param name="selectedDate">Interacted date vale.</param>
        /// <param name="element">Interacted element.</param>
        void TriggerCalendarInteractionEvent(bool isTapped, int tapCount, DateTime selectedDate, CalendarElement element);

        /// <summary>
        /// Method to update the selected date while interaction.
        /// </summary>
        /// <param name="tappedDate">The interacted date.</param>
        void UpdateSelectedDate(DateTime? tappedDate);

        /// <summary>
        /// Method to update the new range selection while the enable swipe selection true.
        /// </summary>
        /// <param name="newRange">The current selected range through interaction.</param>
        /// <param name="isNewRange">To identify, need to create new instance for the selected range or not.</param>
        void UpdateSwipeSelection(CalendarDateRange newRange, bool isNewRange = false);

        /// <summary>
        /// Method to update the selected date for keyboard interaction.
        /// </summary>
        /// <param name="selectedDate">The selected date value.</param>
        void UpdateRangeSelectionOnKeyboardInteraction(DateTime? selectedDate);

        /// <summary>
        /// Process keyboard interaction.
        /// </summary>
        /// <param name="args">The key board event args.</param>
        void ProcessKeyDown(KeyEventArgs args);

        /// <summary>
        /// Invokes view swipe and continue selection on keyboard navigation.
        /// </summary>
        /// <param name="args">The key board event args.</param>
        /// <param name="selectedDateTime">The selected date time on key press.</param>
        void SwipeOnKeyNavigation(KeyEventArgs args, DateTime selectedDateTime);

        /// <summary>
        /// Process the control key event args.
        /// </summary>
        /// <param name="args">The event args value.</param>
        void ProcessOnControlKeyPress(KeyEventArgs args);
    }
}