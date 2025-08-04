using System.Collections.Specialized;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Interface that holds the common methods that needs to render month and year view.
    /// </summary>
    internal interface ICalendarView
    {
		/// <summary>
		/// Method to update when visible dates change.
		/// </summary>
		/// <param name="visibleDates">The visible dates collection.</param>
		/// <param name="isCurrentView">Checks whether the view is current view or not.</param>
		/// <param name="customSnapLayout">Gets the month view instance for current canvas.</param>
		void UpdateVisibleDatesChange(List<DateTime> visibleDates, bool isCurrentView, CustomSnapLayout customSnapLayout);

		/// <summary>
		/// Method to update selected date on visible date change.
		/// </summary>
		void UpdateSelectionValue();

        /// <summary>
        /// Method to update the selected dates.
        /// </summary>
        void UpdateSelectedDates();

        /// <summary>
        /// Method to update the selected dates by action.
        /// </summary>
        /// <param name="e">The collection changed event args.</param>
        void UpdateSelectedDatesOnAction(NotifyCollectionChangedEventArgs e);

        /// <summary>
        /// Method to update when special and disabled dates on view changed.
        /// </summary>
        /// <param name="disabledDates">The disabled dates for this view.</param>
        /// <param name="specialDatesDetails">The special dates for this view.</param>
        /// <param name="visibleDates">The visible dates collection.</param>
        void UpdateDisableAndSpecialDateChange(List<DateTime>? disabledDates, List<CalendarIconDetails>? specialDatesDetails, List<DateTime> visibleDates);

        /// <summary>
        /// Method to invalidate view cells and hover view.
        /// </summary>
        void InvalidateView();

        /// <summary>
        /// Method to invalidate view cells.
        /// </summary>
        void InvalidateViewCells();

        /// <summary>
        /// Method to update template views.
        /// </summary>
        /// <param name="isCurrentView">Defines the view is current visible view.</param>
        void UpdateTemplateViews(bool isCurrentView);

        /// <summary>
        /// Method to update the selected range
        /// </summary>
        void UpdateSelectedRangeValue();

        /// <summary>
        /// Method to update the Selected Range while Swiping
        /// </summary>
        /// <param name="status">The pan gesture status.</param>
        /// <param name="point">The pan gesture touch point.</param>
        void HandleSwipeRangeSelection(GestureStatus status, Point point);

        /// <summary>
        /// Method to update the selected multi ranges.
        /// </summary>
        void UpdateSelectedMultiRangesValue();

        /// <summary>
        /// Method to invoke on key up action.
        /// </summary>
        /// <param name="args">The key event args.</param>
        /// <param name="oldSelectedDate">Previous selected date time.</param>
        /// <param name="isAfterSwipe">Key down triggers after view swipe.</param>
        void OnKeyDown(KeyEventArgs args, DateTime oldSelectedDate, bool isAfterSwipe);

        /// <summary>
        /// Method to update the private variable for the range selection on keyboard interaction.
        /// </summary>
        /// <param name="previousSelectedDate">The previous selected date.</param>
        void UpdatePreviousSelectedDateOnRangeSelection(DateTime previousSelectedDate);

        /// <summary>
        /// Method to update the focus while view is changed on keyboard interaction.
        /// </summary>
        void SetFocusOnViewChanged();
    }
}
