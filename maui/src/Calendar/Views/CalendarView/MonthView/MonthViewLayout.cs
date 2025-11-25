using System.Collections.Specialized;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which contains arrangement logics for month view and week number view.
    /// </summary>
    internal class MonthViewLayout : CalendarLayout, ICalendarView
    {
        #region Fields

        /// <summary>
        /// Holds the month view header details.
        /// </summary>
        MonthViewHeader? _monthViewHeader;

        /// <summary>
        /// The calendar view info.
        /// </summary>
        readonly ICalendar _calendarViewInfo;

        /// <summary>
        /// The calendar view visible dates.
        /// </summary>
        List<DateTime> _visibleDates;

        /// <summary>
        /// View that hold the month cells.
        /// </summary>
        readonly MonthView _monthView;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthViewLayout"/> class.
        /// </summary>
        /// <param name="calendarView">The calendar view.</param>
        /// <param name="visibleDates">The visible dates for the view.</param>
        /// <param name="selectedDate">The selected date for the view.</param>
        /// <param name="disabledDates">The disabled dates for the view.</param>
        /// <param name="specialDates">The special dates for the view.</param>
        /// <param name="isCurrentView">Defines whether the view is current view or not.</param>
        internal MonthViewLayout(ICalendar calendarView, List<DateTime> visibleDates, DateTime? selectedDate, List<DateTime> disabledDates, List<CalendarIconDetails> specialDates, bool isCurrentView)
        {
            _calendarViewInfo = calendarView;
            _visibleDates = visibleDates;
            _monthView = new MonthView(_calendarViewInfo, visibleDates, selectedDate, disabledDates, specialDates, isCurrentView);
            Add(_monthView);
            AddOrRemoveViewHeader(isCurrentView);
#if IOS

#if NET10_0
            this.SafeAreaEdges = SafeAreaEdges.None;
#else
            IgnoreSafeArea = true;
#endif

#endif
		}

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to invalidate the month view.
        /// </summary>
        internal void InvalidateMonthViewMeasure()
        {
            _monthView.InvalidateViewMeasure();
        }

        /// <summary>
        /// Method to add or remove the month view header from month layout.
        /// </summary>
        /// <param name="isCurrentView">The view is current view or not.</param>
        internal void AddOrRemoveViewHeader(bool isCurrentView)
        {
            if (_calendarViewInfo.NavigationDirection == CalendarNavigationDirection.Horizontal)
            {
                if (_monthViewHeader != null && _calendarViewInfo.MonthView.HeaderView.Height <= 0)
                {
                    Remove(_monthViewHeader);
                    _monthViewHeader = null;
                }
                else if (_monthViewHeader == null && _calendarViewInfo.MonthView.HeaderView.Height > 0)
                {
                    _monthViewHeader = new MonthViewHeader(_calendarViewInfo, _visibleDates, isCurrentView);
                    Add(_monthViewHeader);
                }
            }
            else
            {
                if (_monthViewHeader == null)
                {
                    return;
                }

                Remove(_monthViewHeader);
                _monthViewHeader = null;
            }

            InvalidateMonthLayout();
        }

        /// <summary>
        /// Method to update the month header view draw.
        /// </summary>
        internal void InvalidateDrawMonthHeaderView()
        {
            _monthViewHeader?.InvalidateDrawMonthHeaderView();
        }

        /// <summary>
        /// Method to update invalidate the month layout.
        /// </summary>
        internal void InvalidateMonthLayout()
        {
            //// In android platform some time's the InvalidateMeasure doesn't trigger the layout measure.So the view doesn't renderer properly. Hence calling measure and arrange directly without InvalidateMeasure.
#if __ANDROID__
            this.TriggerInvalidateMeasure();
#else
            InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Method to update the month view semantic node.
        /// </summary>
        /// <param name="isCurrentView">The view is current view or not.</param>
        internal void UpdateMonthViewSemanticNode(bool isCurrentView)
        {
            _monthView.InvalidateSemanticsNode(isCurrentView);
        }

        /// <summary>
        /// Method to update the month view header semantic node.
        /// </summary>
        /// <param name="isCurrentView">The view is current view or not.</param>
        internal void UpdateViewHeaderSemanticNode(bool isCurrentView)
        {
            _monthViewHeader?.InvalidateSemanticsNode(isCurrentView);
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>The layout size.</returns>
        internal override Size LayoutArrangeChildren(Rect bounds)
        {
            double monthViewHeaderHeight = _calendarViewInfo.NavigationDirection == CalendarNavigationDirection.Horizontal ? _calendarViewInfo.MonthView.HeaderView.GetViewHeaderHeight() : 0;
            foreach (var child in Children)
            {
                if (child is MonthViewHeader)
                {
                    child.Arrange(new Rect(0, 0, bounds.Width, monthViewHeaderHeight));
                }
                else if (child == _monthView)
                {
                    child.Arrange(new Rect(0, monthViewHeaderHeight, bounds.Width, bounds.Height - monthViewHeaderHeight));
                }
            }

            return bounds.Size;
        }

        /// <summary>
        /// Method used to measure the children based on width and height value.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>The maximum size of the layout.</returns>
        internal override Size LayoutMeasure(double widthConstraint, double heightConstraint)
        {
            double monthViewHeaderHeight = _calendarViewInfo.NavigationDirection == CalendarNavigationDirection.Horizontal ? _calendarViewInfo.MonthView.HeaderView.GetViewHeaderHeight() : 0;
            foreach (View view in Children)
            {
                if (view is MonthViewHeader)
                {
                    view.Measure(widthConstraint, monthViewHeaderHeight);
                }
                else if (view == _monthView)
                {
                    _monthView.Measure(widthConstraint, heightConstraint - monthViewHeaderHeight);
                }
            }

            return new Size(widthConstraint, heightConstraint);
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// Method to update the focus while view is changed.
        /// </summary>
        void ICalendarView.SetFocusOnViewChanged()
        {
            _monthView.SetFocusOnViewChanged();
        }

		/// <summary>
		/// Method to update when visible dates changed.
		/// </summary>
		/// <param name="visibleDates">The visible dates.</param>
		/// <param name="isCurrentView">Defines whether the view is current view or not.</param>
		/// <param name="customSnapLayout">Gets the month view instance for current canvas.</param>
		void ICalendarView.UpdateVisibleDatesChange(List<DateTime> visibleDates, bool isCurrentView, CustomSnapLayout customSnapLayout)
		{
            if (_visibleDates == visibleDates)
            {
                return;
            }

            _visibleDates = visibleDates;
            _monthView.UpdateVisibleDatesChange(visibleDates, isCurrentView, customSnapLayout);
            _monthViewHeader?.UpdateVisibleDatesChange(visibleDates);
        }

        /// <summary>
        /// Method to update selected dates based on the action.
        /// </summary>
        /// <param name="e">The collection changed even arguments.</param>
        void ICalendarView.UpdateSelectedDatesOnAction(NotifyCollectionChangedEventArgs e)
        {
            _monthView.UpdateSelectedDatesOnAction(e);
        }

        /// <summary>
        /// Method to update the selected dates.
        /// </summary>
        void ICalendarView.UpdateSelectedDates()
        {
            _monthView.UpdateSelectedDates();
        }

        /// <summary>
        /// Method to update selected date on visible date change.
        /// </summary>
        void ICalendarView.UpdateSelectionValue()
        {
            _monthView.UpdateSelectionValue();
        }

        /// <summary>
        /// Method to update when special and disabled dates on view changed.
        /// </summary>
        /// <param name="disabledDates">The disabled dates for this view.</param>
        /// <param name="specialDatesDetails">The special dates for this view.</param>
        /// <param name="visibleDates">The visible dates collection.</param>
        void ICalendarView.UpdateDisableAndSpecialDateChange(List<DateTime>? disabledDates, List<CalendarIconDetails>? specialDatesDetails, List<DateTime> visibleDates)
        {
            if (_visibleDates != visibleDates)
            {
                return;
            }

            _monthView.UpdateDisableAndSpecialDateChange(disabledDates, specialDatesDetails);
        }

        /// <summary>
        /// Method to update the range selection.
        /// </summary>
        void ICalendarView.UpdateSelectedRangeValue()
        {
            _monthView.UpdateRangeSelection();
        }

        /// <summary>
        /// Method to update the multi range selection.
        /// </summary>
        void ICalendarView.UpdateSelectedMultiRangesValue()
        {
            _monthView.UpdateMultiRangeSelection();
        }

        /// <summary>
        /// Method to update the range selection while the enable swipe selection true.
        /// </summary>
        /// <param name="status">The pan gesture status.</param>
        /// <param name="point">The pan gesture touch point.</param>
        void ICalendarView.HandleSwipeRangeSelection(GestureStatus status, Point point)
        {
            bool isHorizontal = _calendarViewInfo.NavigationDirection == CalendarNavigationDirection.Horizontal;

            // If the navigation is vertical then the view header is children of calendar. So no need to considered view header height and touch point considered the view header height.
            double viewHeaderHeight = isHorizontal ? _calendarViewInfo.MonthView.HeaderView.GetViewHeaderHeight() : 0;
            point.Y = point.Y - viewHeaderHeight;

            // The point.Y is lesser than 0 then we know user interacted with the view header so skip the swipe range selection calculations.
            if (point.Y < 0)
            {
                return;
            }

            _monthView.HandleSwipeRangeSelection(status, point);
        }

        /// <summary>
        /// Method to invalidate view.
        /// </summary>
        void ICalendarView.InvalidateView()
        {
            _monthView.InvalidateView();
        }

        /// <summary>
        /// Method to invalidate view cells.
        /// </summary>
        void ICalendarView.InvalidateViewCells()
        {
            _monthView.InvalidateMonthView();
        }

        /// <summary>
        /// Method to update template views.
        /// </summary>
        /// <param name="isCurrentView">Defines the view is current visible view.</param>
        void ICalendarView.UpdateTemplateViews(bool isCurrentView)
        {
            _monthView.UpdateTemplateViews(isCurrentView);
        }

        /// <summary>
        /// Invokes on keyboard key press event.
        /// </summary>
        /// <param name="args">The keyboard event args.</param>
        /// <param name="selectedDate">Previous selected date time. In after swipe scenario it holds the current view selected date.</param>
        /// <param name="isAfterSwipe">Key down triggers after view swipe.</param>
        void ICalendarView.OnKeyDown(KeyEventArgs args, DateTime selectedDate, bool isAfterSwipe)
        {
            if (!isAfterSwipe)
            {
                _monthView.UpdateSelectionWhileKeyNavigation(args, selectedDate);
            }
            else
            {
                _monthView.UpdateKeyNavigation(args, selectedDate, selectedDate);
            }
        }

        /// <summary>
        /// Method to update the private variable for the keyboard interaction.
        /// </summary>
        /// <param name="previousSelectedDate">The previous selected date.</param>
        void ICalendarView.UpdatePreviousSelectedDateOnRangeSelection(DateTime previousSelectedDate)
        {
            _monthView.UpdatePreviousSelectedDateOnRangeSelection(previousSelectedDate);
        }

        #endregion
    }
}