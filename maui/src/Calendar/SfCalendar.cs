using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Localization;
using Syncfusion.Maui.Toolkit.Popup;
using Syncfusion.Maui.Toolkit.Themes;
using Globalization = System.Globalization;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SfCalendar"/> class that represents a control,
    /// used to display and select one or more dates with in specified date range.
    /// </summary>
    public partial class SfCalendar : SfView, ICalendar, IHeader, IFooterView, IParentThemeElement
    {
        #region Fields

        /// <summary>
        /// The minimum height for the calendar UI.
        /// </summary>
        readonly double _minHeight = 400;

        /// <summary>
        /// The minimum Width for the calendar UI.
        /// </summary>
        readonly double _minWidth = 350;

        /// <summary>
        /// Holds the current available size of the calendar.
        /// </summary>
        Size _availableSize = Size.Zero;

        /// <summary>
        /// The custom gesture like sliding, fling and handling children gestures and performing parent gestures in looping panel.
        /// </summary>
        CustomSnapLayout? _customScrollLayout;

        /// <summary>
        /// Holds the header layout, custom snap layout and allowed view pop up.
        /// </summary>
        readonly CalendarVerticalStackLayout _layout;

        /// <summary>
        /// The main header layout which contains date, navigation arrows.
        /// </summary>
        HeaderLayout? _headerLayout;

        /// <summary>
        /// The main footer layout which contains today, ok and cancel buttons.
        /// </summary>
        FooterLayout? _footerLayout;

        /// <summary>
        /// Holds the month view header while the view is month view.
        /// </summary>
        MonthViewHeader? _monthViewHeader;

        /// <summary>
        /// The visible dates that will be rendered on the calendar views.
        /// </summary>
        List<DateTime> _visibleDates = new List<DateTime>();

        /// <summary>
        /// The current view display date.
        /// </summary>
        DateTime _displayDate;

        /// <summary>
        /// The selected dates private variable collection holds the duplicate for the calendar selected dates property.
        /// Here we are using private variable to get the old values for selection changed event handler.
        /// In observable collection old values can be obtained in add, remove and replace action,
        /// but for reset action we can't get old values in the NotifyCollectionChangedEventArgs.
        /// </summary>
        ObservableCollection<DateTime> _selectedDates;

        /// <summary>
        /// The selected date ranges private variable collection holds the duplicate for the calendar selected date ranges property.
        /// Here we are using private variable to get the old values for selection changed event handler.
        /// In observable collection old values can be obtained in add, remove and replace action,
        /// but for reset action we can't get old values in the NotifyCollectionChangedEventArgs.
        /// </summary>
        ObservableCollection<CalendarDateRange> _selectedDateRanges;

        /// <summary>
        /// Specifies the view is in RTL flow direction or not.
        /// </summary>
        bool _isRTLLayout;

        /// <summary>
        /// This private variable to update the last selected date of the year view in keyboard interaction.
        /// </summary>
        DateTime? _previousSelectedDate;

        /// <summary>
        /// This proxy is used to avoid the memory leks while wire the text style property changed events.
        /// </summary>
        readonly SfCalendarProxy _proxy;

        /// <summary>
        /// The SfPopup view.
        /// </summary>
        SfPopup? _popup;

        /// <summary>
        /// Boolean to get the previous open state of the calendar in the dialog mode.
        /// This value is used to maintain the previous state of the calendar while changing the visibility of the calendar in the dialog mode.
        /// While changing the IsOpen property of the calendar dynamically in the Visibility change the property change will not trigger sometimes.
        /// So that we are updated the previous state of the calendar when the IsOpen property changed.
        /// </summary>
        bool _isCalendarPreviouslyOpened = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfCalendar"/> class.
        /// </summary>
        public SfCalendar()
        {
			SfCalendarResources.InitializeDefaultResource("Syncfusion.Maui.Toolkit.Calendar.Resources.SfCalendar", typeof(SfCalendar));
			ThemeElement.InitializeThemeResources(this, "SfCalendarTheme");
#if IOS
			IgnoreSafeArea = true;
#endif
			_proxy = new(this);
            DrawingOrder = DrawingOrder.AboveContent;
            BackgroundColor = CalendarBackground;
            _layout = new CalendarVerticalStackLayout(HeaderView.Height, FooterView.ShowActionButtons || FooterView.ShowTodayButton, FooterView.Height);
#if IOS
			_layout.IgnoreSafeArea = true;
#endif
			Add(_layout);
            _displayDate = DisplayDate;
            _selectedDates = new ObservableCollection<DateTime>();
            _selectedDateRanges = new ObservableCollection<CalendarDateRange>();
            WireEvents();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of week in calendar month view.
        /// </summary>
        int IHeader.NumberOfVisibleWeeks => MonthView.GetNumberOfWeeks();

        /// <summary>
        /// Gets the visible Appointments that will render based on view date ranges.
        /// </summary>
        List<DateTime> IHeader.VisibleDates => _visibleDates;

        /// <summary>
        /// Gets a value indicating whether the layout is RTL or not.
        /// </summary>
        bool IHeaderCommon.IsRTLLayout => _isRTLLayout;

        #endregion

        #region Public Methods

        /// <summary>
        /// Move to previous view which displays previous view dates.
        /// </summary>
        public void Backward()
        {
            if (_customScrollLayout == null)
            {
                return;
            }

            int numberOfWeeks = MonthView.GetNumberOfWeeks();
            if (!CalendarViewHelper.IsValidPreviousDatesNavigation(_visibleDates, View, numberOfWeeks, MinimumDate, Identifier))
            {
                return;
            }

            if (NavigationDirection == CalendarNavigationDirection.Horizontal)
            {
                if (_isRTLLayout)
                {
                    _customScrollLayout.AnimateMoveToNextView();
                }
                else
                {
                    _customScrollLayout.AnimateMoveToPreviousView();
                }
            }
            else
            {
                _customScrollLayout.AnimateMoveToPreviousView();
            }
        }

        /// <summary>
        /// Move to next view which displays next view dates.
        /// </summary>
        public void Forward()
        {
            if (_customScrollLayout == null)
            {
                return;
            }

            int numberOfWeeks = MonthView.GetNumberOfWeeks();
            if (!CalendarViewHelper.IsValidNextDatesNavigation(_visibleDates, View, numberOfWeeks, MaximumDate, Identifier))
            {
                return;
            }

            if (NavigationDirection == CalendarNavigationDirection.Horizontal)
            {
                if (_isRTLLayout)
                {
                    _customScrollLayout.AnimateMoveToPreviousView();
                }
                else
                {
                    _customScrollLayout.AnimateMoveToNextView();
                }
            }
            else
            {
                _customScrollLayout.AnimateMoveToNextView();
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to invoke the closed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The event args.</param>
        internal void InvokeClosedEvent(object sender, EventArgs eventArgs)
        {
            CalendarPopupClosed?.Invoke(sender, eventArgs);
        }

        /// <summary>
        /// Method to invoke the closing event.
        /// </summary>
        /// <param name="sender">The calendar instance.</param>
        /// <param name="eventArgs">The event arguments.</param>
        internal void InvokeClosingEvent(object sender, CancelEventArgs eventArgs)
        {
            CalendarPopupClosing?.Invoke(sender, eventArgs);
        }

        /// <summary>
        /// Method to invoke the opening event.
        /// </summary>
        /// <param name="sender">The calendar instance.</param>
        /// <param name="eventArgs">The cancel event args.</param>
        internal void InvokeOpeningEvent(object sender, CancelEventArgs eventArgs)
        {
            CalendarPopupOpening?.Invoke(sender, eventArgs);
        }

        /// <summary>
        /// Method to invoke the opened event.
        /// </summary>
        /// <param name="sender">The calendar instance.</param>
        /// <param name="eventArgs">The event arguments.</param>
        internal void InvokeOpenedEvent(object sender, EventArgs eventArgs)
        {
            CalendarPopupOpened?.Invoke(sender, eventArgs);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Update Visible dates for the calendar Header views.
        /// </summary>
        /// <param name="visibleDates">The current view visible dates.</param>
        /// <param name="previousVisibleDates">The previous visible dates.</param>
        /// <param name="previousCalendarView">The previous calendar view.</param>
        void ICalendar.UpdateVisibleDates(List<DateTime> visibleDates, List<DateTime> previousVisibleDates, CalendarView previousCalendarView)
        {
            // Check if the current visible dates list instance is the same as the provided list to avoid unnecessary updates
            if (ReferenceEquals(_visibleDates, visibleDates))
            {
                return;
            }

            _visibleDates = visibleDates;
            UpdateDisplayDateOnNavigation();

            // Update navigation arrows and month view based on new visible dates
            _headerLayout?.UpdateVisibleDates();
            _monthViewHeader?.UpdateVisibleDatesChange(visibleDates);
            OnCalendarViewChanged(previousVisibleDates, previousCalendarView, ShowTrailingAndLeadingDates);
        }

        /// <summary>
        /// Method to check whether the cell is selectable or not in the calendar.
        /// </summary>
        /// <param name="date">The date time.</param>
        /// <returns>True if the cell is selectable, else false.</returns>
        bool ICalendar.IsSelectableDayPredicate(DateTime date)
        {
            return SelectableDayPredicate == null ? true : SelectableDayPredicate.Invoke(date);
        }

        /// <summary>
        /// Method to process the keyboard interaction.
        /// </summary>
        /// <param name="args">The key event args.</param>
        void IInteractionInfo.ProcessKeyDown(KeyEventArgs args)
        {
            _customScrollLayout?.ProcessOnKeyDown(args);
        }

        /// <summary>
        /// Method to check whether the date is special day or not in the calendar.
        /// </summary>
        /// <param name="date">The date time.</param>
        /// <returns>The calendar special day icon details.</returns>
        CalendarIconDetails? ICalendar.IsSpecialDayPredicate(DateTime date)
        {
            return MonthView.SpecialDayPredicate?.Invoke(date);
        }

        /// <summary>
        /// Method to trigger the tapped, double tapped or long pressed event.
        /// </summary>
        /// <param name="isTapped">Defines the calendar view interaction is tap.</param>
        /// <param name="tapCount">Holds the tap count value.</param>
        /// <param name="tappedDate">Interacted date vale.</param>
        /// <param name="element">Interacted element.</param>
        void IInteractionInfo.TriggerCalendarInteractionEvent(bool isTapped, int tapCount, DateTime tappedDate, CalendarElement element)
        {
            int weekNumber = -1;
            if (element == CalendarElement.WeekNumber)
            {
                int daysToAdd = 0;

                // Check if the given date is not Monday
                if (tappedDate.DayOfWeek != DayOfWeek.Monday)
                {
                    // Calculate the number of days to add to get to the next monday to get week number.
                    daysToAdd = ((int)DayOfWeek.Monday - (int)tappedDate.DayOfWeek + 7) % 7;
                }

                weekNumber = CalendarViewHelper.GetWeekNumber(Identifier, tappedDate.AddDays(daysToAdd));
            }

            if (isTapped)
            {
                if (tapCount == 1)
                {
                    OnTappedCallBack(new CalendarTappedEventArgs() { Date = tappedDate, Element = element, WeekNumber = weekNumber });
                }
                else if (tapCount == 2)
                {
                    var calendarDoubleTappedEventArgs = new CalendarDoubleTappedEventArgs() { Date = tappedDate, Element = element, WeekNumber = weekNumber };
                    if (DoubleTapped != null)
                    {
                        DoubleTapped?.Invoke(this, calendarDoubleTappedEventArgs);
                    }

                    if (DoubleTappedCommand != null && DoubleTappedCommand.CanExecute(calendarDoubleTappedEventArgs))
                    {
                        DoubleTappedCommand.Execute(calendarDoubleTappedEventArgs);
                    }
                }
            }
            else
            {
                var calendarLongpressedEventArgs = new CalendarLongPressedEventArgs() { Date = tappedDate, Element = element, WeekNumber = weekNumber };
                if (LongPressed != null)
                {
                    LongPressed?.Invoke(this, calendarLongpressedEventArgs);
                }

                if (LongPressedCommand != null && LongPressedCommand.CanExecute(calendarLongpressedEventArgs))
                {
                    LongPressedCommand.Execute(calendarLongpressedEventArgs);
                }
            }
        }

        /// <summary>
        /// Method to update the selected date based on the selection mode while interaction.
        /// </summary>
        /// <param name="tappedDate">The interacted date.</param>
        void IInteractionInfo.UpdateSelectedDate(DateTime? tappedDate)
        {
            switch (SelectionMode)
            {
                case CalendarSelectionMode.Single:
                    // The selected and tapped date is checked when both the value is not null
                    if (SelectedDate != null && tappedDate != null && SelectedDate.Value.Date == tappedDate.Value.Date)
                    {
                        return;
                    }

                    SelectedDate = tappedDate;
                    break;
                case CalendarSelectionMode.Range:
                    UpdateRangeSelection(tappedDate);
                    break;
                case CalendarSelectionMode.Multiple:
                    UpdateMultipleSelection(tappedDate);
                    break;
                case CalendarSelectionMode.MultiRange:
                    if (tappedDate == null)
                    {
                        return;
                    }

                    UpdateMultiRangeSelection(tappedDate.Value);
                    break;
            }
        }

        /// <summary>
        /// Method to trigger the calendar popup closed.
        /// </summary>
        /// <param name="e">The event args.</param>
        void ICalendar.OnPopupClosed(EventArgs e)
        {
            InvokeClosedEvent(this, e);
        }

        /// <summary>
        /// Method to trigger the popup closing.
        /// </summary>
        /// <param name="e">The cancel event args.</param>
        void ICalendar.OnPopupClosing(CancelEventArgs e)
        {
            InvokeClosingEvent(this, e);
        }

        /// <summary>
        /// Method to trigger the popup opening.
        /// </summary>
        /// <param name="e">The cancel event args.</param>
        void ICalendar.OnPopupOpening(CancelEventArgs e)
        {
            InvokeOpeningEvent(this, e);
        }

        /// <summary>
        /// Method to trigger the calendar opened.
        /// </summary>
        /// <param name="e">The event args.</param>
        void ICalendar.OnPopupOpened(EventArgs e)
        {
            InvokeOpenedEvent(this, e);
        }

        /// <summary>
        /// Method to update the multiple selection.
        /// </summary>
        /// <param name="tappedDate">The tapped or pan date.</param>
        void UpdateMultipleSelection(DateTime? tappedDate)
        {
            if (tappedDate == null)
            {
                return;
            }

            bool isDateInCollection = false;
            for (int i = 0; i < SelectedDates.Count; i++)
            {
                DateTime date = SelectedDates[i];
                if (CalendarViewHelper.IsSameDate(View, tappedDate, date, Identifier))
                {
                    SelectedDates.RemoveAt(i);
                    isDateInCollection = true;
                    break;
                }
            }

            if (!isDateInCollection)
            {
                SelectedDates.Add((DateTime)tappedDate);
            }
        }

        /// <summary>
        /// Method to update the selected range based for the keyboard selection.
        /// </summary>
        /// <param name="selectedDate">The selected date value.</param>
        void IInteractionInfo.UpdateRangeSelectionOnKeyboardInteraction(DateTime? selectedDate)
        {
            if (selectedDate == null)
            {
                return;
            }

            switch (RangeSelectionDirection)
            {
                case CalendarRangeSelectionDirection.Default:
                case CalendarRangeSelectionDirection.Both:
                    UpdateRangeSelectionOnKeyboard(selectedDate.Value);
                    break;

                case CalendarRangeSelectionDirection.Forward:
                    UpdateForwardRangeSelection(selectedDate.Value);
                    break;

                case CalendarRangeSelectionDirection.Backward:
                    UpdateBackwardRangeSelection(selectedDate.Value);
                    break;

                case CalendarRangeSelectionDirection.None:
                    UpdateNoneRangeSelection(selectedDate.Value);
                    break;
            }

            _customScrollLayout?.UpdateLastSelectedDate(_previousSelectedDate);
        }

        /// <summary>
        /// Method to update the new range selection.
        /// </summary>
        /// <param name="newRange">The updated selected range through interaction.</param>
        /// <param name="isNewRange">To identify, The swipe is started and Need to create new instance for the selected range or not .</param>
        void IInteractionInfo.UpdateSwipeSelection(CalendarDateRange newRange, bool isNewRange)
        {
            if (SelectionMode == CalendarSelectionMode.MultiRange)
            {
                UpdateMultiRangeSwipeSelection(newRange, isNewRange);
                return;
            }

            // The following scenarios need to create instance for selected range.
            // 1.The selected date range is null.
            // 2.The selected date range start and end are null.
            // 3.The isNewRange is true when the swipe is started.
            if (SelectedDateRange == null || (SelectedDateRange.StartDate == null && SelectedDateRange.EndDate == null) || isNewRange)
            {
                SelectedDateRange = newRange;
            }
            //// If the selected date range is not null, need to check the start and end date then updated the selected date range
            else
            {
                UpdateExistingRange(newRange);
            }
        }

        /// <summary>
        /// Method to update the existing range selection.
        /// </summary>
        /// <param name="newRange">The updated selected range through interaction.</param>
        void UpdateExistingRange(CalendarDateRange newRange)
        {
            DateTime? startDate = SelectedDateRange.StartDate?.Date;
            DateTime? endDate = SelectedDateRange.EndDate?.Date;
            DateTime? newRangeStartDate = newRange.StartDate?.Date;
            DateTime? newRangeEndDate = newRange.EndDate?.Date;
            bool isSameStartDate = CalendarViewHelper.IsSameDate(View, startDate, newRangeStartDate, Identifier);
            bool isSameEndDate = CalendarViewHelper.IsSameDate(View, endDate, newRangeEndDate, Identifier);

            // Need to update the end date. While the interacted date is greater than start date.
            // Example: Start or initially interacted date is Sept-1 and end date is Sept-10. The interacted date is Sept-10 to Sept-11 then need to update the end date only.
            if (isSameStartDate && !isSameEndDate)
            {
                SelectedDateRange.EndDate = CalendarViewHelper.GetLastDate(View, newRange.EndDate, Identifier);
            }

            // Need to update the start date. While the interacted date is lesser than start date.
            // Example: Swipe start from Sept-10 then interacted to Sept-09 so the range become Sept-09 to Sept-10. Then again interacted to Sept-08 then need to update the start date.
            else if (isSameEndDate && !isSameStartDate)
            {
                SelectedDateRange.StartDate = newRange.StartDate;
            }

            // Need to update new range when the interacted date is lesser than start date so,its needs swap the start and end date values.
            else
            {
                newRange.EndDate = CalendarViewHelper.GetLastDate(View, newRange.EndDate, Identifier);
                SelectedDateRange = newRange;
            }
        }

        /// <summary>
        /// Triggers the animation to forward the calendar view.
        /// </summary>
        void IHeader.AnimateMoveToNextView()
        {
            _customScrollLayout?.AnimateMoveToNextView();
        }

        /// <summary>
        /// Triggers the animation to backward the calendar view.
        /// </summary>
        void IHeader.AnimateMoveToPreviousView()
        {
            _customScrollLayout?.AnimateMoveToPreviousView();
        }

        /// <summary>
        /// Method triggers when click the confirm button.
        /// </summary>
        void IFooterView.OnConfirmButtonClicked()
        {
            if (_footerLayout == null)
            {
                return;
            }

            switch (SelectionMode)
            {
                case CalendarSelectionMode.Single:
                    var singleSelection = new CalendarSubmittedEventArgs() { Value = SelectedDate };
                    if (ActionButtonClicked != null)
                    {
                        ActionButtonClicked?.Invoke(this, singleSelection);
                    }

                    if (AcceptCommand != null && AcceptCommand.CanExecute(singleSelection))
                    {
                        AcceptCommand.Execute(singleSelection);
                    }

                    break;
                case CalendarSelectionMode.Multiple:
                    ReadOnlyObservableCollection<DateTime> selectedDates = new ReadOnlyObservableCollection<DateTime>(SelectedDates);
                    var multipleDatesSelection = new CalendarSubmittedEventArgs() { Value = SelectedDates };
                    if (ActionButtonClicked != null)
                    {
                        ActionButtonClicked?.Invoke(this, multipleDatesSelection);
                    }

                    if (AcceptCommand != null && AcceptCommand.CanExecute(multipleDatesSelection))
                    {
                        AcceptCommand.Execute(multipleDatesSelection);
                    }

                    break;
                case CalendarSelectionMode.Range:
                    CalendarDateRange? dateRange = SelectedDateRange == null ? null : new CalendarDateRange(SelectedDateRange.StartDate, SelectedDateRange.EndDate);
                    var rangeSelection = new CalendarSubmittedEventArgs() { Value = SelectedDateRange };
                    if (ActionButtonClicked != null)
                    {
                        ActionButtonClicked?.Invoke(this, rangeSelection);
                    }

                    if (AcceptCommand != null && AcceptCommand.CanExecute(rangeSelection))
                    {
                        AcceptCommand.Execute(rangeSelection);
                    }

                    break;
            }
        }

        /// <summary>
        /// Method trigger when cancel button click action.
        /// </summary>
        /// <param name="selectedDate">The selected date.</param>
        /// <param name="selectedDates">The selected dates.</param>
        /// <param name="calendarDateRange">The calendar date range.</param>
        void IFooterView.OnCancelButtonClicked(DateTime? selectedDate, ObservableCollection<DateTime> selectedDates, CalendarDateRange? calendarDateRange)
        {
            if (_footerLayout == null)
            {
                return;
            }

            switch (SelectionMode)
            {
                case CalendarSelectionMode.Single:
                    SelectedDate = selectedDate;
                    break;
                case CalendarSelectionMode.Multiple:
                    SelectedDates = new ObservableCollection<DateTime>(selectedDates);
                    break;
                case CalendarSelectionMode.Range:
#nullable disable
                    SelectedDateRange = calendarDateRange == null ? null : new CalendarDateRange(calendarDateRange.StartDate, calendarDateRange.EndDate);
#nullable enable
                    break;
            }

            ActionButtonCanceled?.Invoke(null, new EventArgs { });
            if (DeclineCommand != null && DeclineCommand.CanExecute(new EventArgs { }))
            {
                DeclineCommand.Execute(new EventArgs { });
            }
        }

        /// <summary>
        /// Method trigger when today button click action.
        /// </summary>
        void IFooterView.OnTodayButtonClicked()
        {
            DisplayDate = DateTime.Now.Date;
        }

        /// <summary>
        /// Invokes view swipe and continue seletion on keyboard navigation.
        /// </summary>
        /// <param name="args">The key borad event args.</param>
        /// <param name="selectedDateTime">The selected date time on key press.</param>
        void IInteractionInfo.SwipeOnKeyNavigation(KeyEventArgs args, DateTime selectedDateTime)
        {
            IHeader navigationInfo = this;
            bool isRTLLayout = navigationInfo.IsRTLLayout;
            if (((args.Key == KeyboardKey.Left || args.Key == KeyboardKey.Up) && !isRTLLayout) ||
                    ((args.Key == KeyboardKey.Right || args.Key == KeyboardKey.Up) && isRTLLayout))
            {
                Backward();
            }
            else if (((args.Key == KeyboardKey.Right || args.Key == KeyboardKey.Down) && !isRTLLayout) ||
                ((args.Key == KeyboardKey.Left || args.Key == KeyboardKey.Down) && isRTLLayout))
            {
                Forward();
            }

            _customScrollLayout?.UpdateSelectionOnKeyNavigation(args, selectedDateTime);
        }

        /// <summary>
        /// Method to process the control key press on key navigation.
        /// </summary>
        /// <param name="args">The key event args.</param>
        void IInteractionInfo.ProcessOnControlKeyPress(KeyEventArgs args)
        {
            KeyboardKey arrowKey = args.Key;
            switch (arrowKey)
            {
                case KeyboardKey.Up:
                case KeyboardKey.Down:
                    //// Have to navigate to the other view only when the allow view navigation is false.
                    if (!AllowViewNavigation)
                    {
                        return;
                    }

                    View = CalendarViewHelper.ProcessControlKeyPressed(View, args);
                    break;

                case KeyboardKey.Left:
                    Backward();
                    break;

                case KeyboardKey.Right:
                    Forward();
                    break;
            }
        }

        /// <summary>
        /// Method to invoke view changed event.
        /// </summary>
        /// <param name="previousVisibleDates">The previous visible dates collection.</param>
        /// <param name="previousCalendarView">The previous calendar view.</param>
        /// <param name="previousShowLeadingDatesValue">Defines whether the previous view has leading and trailing dates.</param>
        void OnCalendarViewChanged(List<DateTime> previousVisibleDates, CalendarView previousCalendarView, bool previousShowLeadingDatesValue)
        {
            if (ViewChanged == null && ViewChangedCommand == null)
            {
                return;
            }

            ReadOnlyCollection<DateTime> newVisibleDates = GetCalendarVisibleDates(_visibleDates, View, ShowTrailingAndLeadingDates);
            ReadOnlyCollection<DateTime> oldVisibleDates = GetCalendarVisibleDates(previousVisibleDates, previousCalendarView, previousShowLeadingDatesValue);
            CalendarViewChangedEventArgs eventArgs = new CalendarViewChangedEventArgs()
            {
                NewView = View,
                OldView = previousCalendarView,
                OldVisibleDates = oldVisibleDates,
                NewVisibleDates = newVisibleDates,
            };

            ViewChanged?.Invoke(this, eventArgs);
            if (ViewChangedCommand != null && ViewChangedCommand.CanExecute(eventArgs))
            {
                ViewChangedCommand.Execute(eventArgs);
            }
        }

        /// <summary>
        /// The Method to get the current view visible dates.
        /// </summary>
        /// <param name="visibleDates">The visible dates.</param>
        /// <param name="view">The current scheduler view.</param>
        /// <param name="isLeadingAndTrailingDatesEnabled">Defines whether the leading and trailing dates enabled.</param>
        /// <returns>The current view visible dates.</returns>
        ReadOnlyCollection<DateTime> GetCalendarVisibleDates(List<DateTime> visibleDates, CalendarView view, bool isLeadingAndTrailingDatesEnabled)
        {
            if (view != CalendarView.Year && !isLeadingAndTrailingDatesEnabled && visibleDates.Count > 0)
            {
                //// Does not need to handle the trailing and leading dates while the month view does not show 6 rows.
                if (view == CalendarView.Month && MonthView.GetNumberOfWeeks() != 6)
                {
                    return visibleDates.AsReadOnly();
                }

                DateTime currentViewDate = view == CalendarView.Month ? visibleDates[visibleDates.Count / 2] : visibleDates[0];
                List<DateTime> currentViewDates = new List<DateTime>();
                foreach (DateTime date in visibleDates)
                {
                    if (CalendarViewHelper.IsLeadingAndTrailingDate(date, currentViewDate, view, Identifier))
                    {
                        continue;
                    }

                    //// Add only the current month or decade or century dates.
                    currentViewDates.Add(date);
                }

                return currentViewDates.AsReadOnly();
            }
            else
            {
                return visibleDates.AsReadOnly();
            }
        }

        /// <summary>
        /// Method to invoke events on tap interaction on calendar elements.
        /// </summary>
        /// <param name="calendarTappedEventArgs">The tap event arguments that contains tap element details.</param>
        void OnTappedCallBack(CalendarTappedEventArgs calendarTappedEventArgs)
        {
            // Invoke the tapped event
            Tapped?.Invoke(this, calendarTappedEventArgs);
            if (TappedCommand != null && TappedCommand.CanExecute(calendarTappedEventArgs))
            {
                TappedCommand.Execute(calendarTappedEventArgs);
            }

            // Returns when the view navigation is false, when the view is month view or tapped date is null
            if (!AllowViewNavigation)
            {
                return;
            }

            if (calendarTappedEventArgs.Element == CalendarElement.CalendarCell)
            {
                if (View != CalendarView.Month)
                {
                    // Update Display date when the Allow view navigation is true
                    DisplayDate = calendarTappedEventArgs.Date;
                }

                // Switch case to check the condition for Calendar view to navigate.
                switch (View)
                {
                    case CalendarView.Century:
                        View = CalendarView.Decade;
                        break;
                    case CalendarView.Decade:
                        View = CalendarView.Year;
                        break;
                    case CalendarView.Year:
                        View = CalendarView.Month;
                        break;
                    case CalendarView.Month:
                        NavigateToAdjacentDates(calendarTappedEventArgs);
                        break;
                }
            }
            else if (calendarTappedEventArgs.Element == CalendarElement.Header)
            {
                // Switch case to check the condition for Calendar view to navigate.
                switch (View)
                {
                    case CalendarView.Month:
                        View = CalendarView.Year;
                        break;
                    case CalendarView.Year:
                        View = CalendarView.Decade;
                        break;
                    case CalendarView.Decade:
                        View = CalendarView.Century;
                        break;
                }
            }
        }

        /// <summary>
        /// Method to Navigate previous/next month
        /// </summary>
        void NavigateToAdjacentDates(CalendarTappedEventArgs calendarTappedEventArgs)
        {
            if (NavigateToAdjacentMonth && MonthView.NumberOfVisibleWeeks == 6 && SelectionMode == CalendarSelectionMode.Single)
            {
                // Logic to handle navigation specifically based on tapped date and current month.
                DateTime displayDateInCurrentCulture = ConvertToCurrentCulture(_displayDate);
                DateTime tappedDateInCurrentCulture = ConvertToCurrentCulture(calendarTappedEventArgs.Date, true);

                if (tappedDateInCurrentCulture.Month != displayDateInCurrentCulture.Month)
                {
                    if (CalendarViewHelper.IsGreaterDate(tappedDateInCurrentCulture, View, displayDateInCurrentCulture, Identifier))
                    {
                        Forward();
                    }
                    else
                    {
                        Backward();
                    }
                }
            }
        }

        /// <summary>
        /// Method to convert the current culture.
        /// </summary>
        /// <param name="date">The date time.</param>
        /// <param name="adjustDay">Tapped date.</param>
        /// <returns>The current culture.</returns>
        DateTime ConvertToCurrentCulture(DateTime date, bool adjustDay = false)
        {
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(Identifier.ToString());
            int day = 0;
            if (adjustDay)
            {
                day = cultureCalendar.GetMonth(date) == 2 ? 1 : cultureCalendar.GetDayOfMonth(date);

                //// Converts the tapped day into 1st day of the month since date time does not have 31 day on certain months.
                if (Identifier == CalendarIdentifier.Persian)
                {
                    day = day == 31 ? 1 : day;
                }
            }
            else
            {
                //// Need to Convert if it exceeds 28 since Hijri and UmAlQura calendars does not have 29 days in
                if (Identifier == CalendarIdentifier.Hijri || Identifier == CalendarIdentifier.UmAlQura)
                {
                    day = cultureCalendar.GetMonth(date) == 2 && cultureCalendar.GetDayOfMonth(date) >= 29 ? 28 : cultureCalendar.GetDayOfMonth(date);
                }
                else
                {
                    day = cultureCalendar.GetDayOfMonth(date);
                }
            }

            return new DateTime(cultureCalendar.GetYear(date), cultureCalendar.GetMonth(date), day);
        }

        /// <summary>
        /// Method to add layout children.
        /// </summary>
        void AddChildren()
        {
            _customScrollLayout = new CustomSnapLayout(this);
            DateTime displayDate = CalendarViewHelper.GetValidDisplayDate(this);
            _visibleDates = new List<DateTime>() { displayDate };
            AddOrRemoveHeaderLayout();
            AddOrRemoveFooterLayout();
            _layout.UpdateHeaderHeight(HeaderView.GetHeaderHeight());
            _layout.UpdateFooterHeight(FooterView.GetFooterHeight());
            AddOrRemoveMonthHeaderView();
            _layout.Add(_customScrollLayout);
        }

        /// <summary>
        /// Method add or remove the calendar header from calendar.
        /// </summary>
        void AddOrRemoveHeaderLayout()
        {
            if (HeaderView.Height > 0 && _headerLayout == null)
            {
                _headerLayout = new HeaderLayout(this);
                _layout.Add(_headerLayout);
            }
            else if (_headerLayout != null && HeaderView.Height <= 0)
            {
                _layout.Remove(_headerLayout);
                _headerLayout = null;
            }
        }

        /// <summary>
        /// Method to add or remove footer layout.
        /// </summary>
        void AddOrRemoveFooterLayout()
        {
            if (FooterView.ShowActionButtons || FooterView.ShowTodayButton)
            {
                if (_footerLayout == null)
                {
                    _footerLayout = new FooterLayout(this);
                    _layout.Add(_footerLayout);
                }
                else
                {
                    _footerLayout.AddOrRemoveFooterButtons();
                }
            }
            else
            {
                if (_footerLayout == null)
                {
                    return;
                }

                _layout.Remove(_footerLayout);
                _footerLayout.AddOrRemoveFooterButtons();
                if (_footerLayout.Handler != null && _footerLayout.Handler.PlatformView != null)
                {
                    _footerLayout.Handler.DisconnectHandler();
                }

                _footerLayout = null;
            }

            //// In Android, dynamically add the today button while action button shown and
            //// dynamically add action button while today button shown does not trigger the measure and layout.
#if ANDROID
            _layout?.TriggerInvalidateMeasure();
#endif
            _layout?.UpdateActionButtonHeight(FooterView.ShowActionButtons || FooterView.ShowTodayButton);
        }

        /// <summary>
        /// Method add or remove the month view header from calendar.
        /// </summary>
        void AddOrRemoveMonthHeaderView()
        {
            if (NavigationDirection == CalendarNavigationDirection.Vertical && View == CalendarView.Month)
            {
                //// Initially the visible dates collection is null while the navigation direction is vertical. So need to calculate visible dates collection in temporary.
                List<DateTime> visibleDates = new List<DateTime>();
                if (visibleDates.Count == 0)
                {
                    DateTime firstDisplayDate = CalendarViewHelper.GetFirstDayOfWeek(7, DisplayDate, MonthView.FirstDayOfWeek, Identifier);
                    for (int i = 0; i < 7; i++)
                    {
                        DateTime dateTime = firstDisplayDate.AddDays(i);
                        visibleDates.Add(dateTime);
                    }
                }
                else
                {
                    visibleDates = _visibleDates;
                }

                if (MonthView.HeaderView.Height > 0 && _monthViewHeader == null)
                {
                    _monthViewHeader = new MonthViewHeader(this, visibleDates, true);
                    _layout.Add(_monthViewHeader);
                    _layout.UpdateViewHeaderHeight(MonthView.HeaderView.GetViewHeaderHeight());
                }
                else if (_monthViewHeader != null && MonthView.HeaderView.Height <= 0)
                {
                    _layout.Remove(_monthViewHeader);
                    _monthViewHeader = null;
                    _layout.UpdateViewHeaderHeight(MonthView.HeaderView.GetViewHeaderHeight());
                }
            }
            else
            {
                if (_monthViewHeader == null)
                {
                    return;
                }

                _layout.Remove(_monthViewHeader);
                _monthViewHeader = null;

                //// To update the view header height as 0, while navigation direction is Horizontal and the view is other than month view.
                _layout.UpdateViewHeaderHeight(0);
            }
        }

        /// <summary>
        /// Method to update the display date while the navigation action occur.
        /// </summary>
        void UpdateDisplayDateOnNavigation()
        {
            //// Consider current view visible dates of first date as display date.
            DateTime date = _visibleDates[0];
            int numberOfWeeks = MonthView.GetNumberOfWeeks();
            if (View == CalendarView.Month && numberOfWeeks == 6)
            {
                Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(Identifier.ToString());
                DateTime minDate = cultureCalendar.MinSupportedDateTime;
                int minYear = cultureCalendar.GetYear(minDate);
                int minMonth = cultureCalendar.GetMonth(minDate);
                DateTime displayDateMonth = _visibleDates[_visibleDates.Count / 2];
                int year = cultureCalendar.GetYear(displayDateMonth);
                int month = cultureCalendar.GetMonth(displayDateMonth);
                date = minYear == year && minMonth == month ? minDate : new DateTime(year, month, 1, cultureCalendar);
            }

            if (date < MinimumDate)
            {
                date = MinimumDate;
            }

            // The display date is present in current month or year or decade or century dates means no need to update the display date.
            if (CalendarViewHelper.IsSameDisplayDateView(this, _visibleDates, _displayDate, date))
            {
                if (View == CalendarView.Month && numberOfWeeks != 6)
                {
                    date = _visibleDates.Contains(DateTime.Now.Date) ? DateTime.Now.Date : _visibleDates[0];
                }
                else
                {
                    return;
                }
            }

            _displayDate = date;
            DisplayDate = _displayDate;
        }

        /// <summary>
        /// Method to update the calendar date range.
        /// </summary>
        /// <param name="selectedDate">The selected date.</param>
        void UpdateSelectedDateRange(DateTime selectedDate)
        {
            DateTime? startDate = SelectedDateRange?.StartDate?.Date;
            DateTime? endDate = SelectedDateRange?.EndDate?.Date;
            bool isSameStartAndEndDate = CalendarViewHelper.IsSameDate(View, startDate, endDate, Identifier);

            // Need to create a new instance for selected range below mentioned scenario's.
            // 1.The selected range is null.
            // 2.The start and end date are not equal and isSameStartAndEndDate is false.
            // 3.The isSameStartAndEndDate is true.
            // 4.The start and end date are null.
            // The selected range is valid range(The range have different valid start and end date).
            // Example: Valid range Sept-10 to Sept-15. Invalid range Sept-10 to null or Sept-10 to Sept-10.
            if (SelectedDateRange == null || (startDate == null && endDate == null) || (startDate != null && endDate != null && !isSameStartAndEndDate))
            {
                CalendarDateRange rangeSelection = new CalendarDateRange(selectedDate, null);
                SelectedDateRange = rangeSelection;
            }
            else
            {
                // The start date and tapped date is equal then no need update the end date. Because of there is no change in UI.
                // Example: Start date is Sept-1 then tapped date also Sept-1 no need to update the start or end date.
                if (CalendarViewHelper.IsSameDate(View, startDate, selectedDate, Identifier))
                {
                    return;
                }

                // Need to create new range when the interacted date is lesser than start date because,its needs swap the start and end date values.
                if (startDate.IsGreaterDate(View, selectedDate, Identifier))
                {
                    //// It returns the endRangeDate. Example : date(2022, 11, 09)
                    //// For Month view - returns (2022, 11, 09) , Year view - returns (2022, 11, 30)
                    //// For Decade view - returns (2022, 12, 31) , Century view - returns (2029, 12, 31)
                    DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, startDate, Identifier);
                    CalendarDateRange selectionRange = new CalendarDateRange(selectedDate, endRangeDate);
                    SelectedDateRange = selectionRange;
                }
                //// If the start date is smaller than tapped date, need to update the end date
                else
                {
                    DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, selectedDate, Identifier);
                    SelectedDateRange.EndDate = endRangeDate;
                }
            }
        }

        /// <summary>
        /// Method to update the calendar date ranges.
        /// </summary>
        /// <param name="selectedDate">The selected date.</param>
        void UpdateMultiRangeSelection(DateTime selectedDate)
        {
            //// If the SelectedDateRanges collections is null, then create a new instance and add the selected date as a Start date.
            if (SelectedDateRanges == null)
            {
                SelectedDateRanges = new ObservableCollection<CalendarDateRange> { new CalendarDateRange(selectedDate, null) };
            }
            //// If the SelectedDateRanges collections is empty, then add the selected date to the collection.
            else if (SelectedDateRanges.Count == 0)
            {
                SelectedDateRanges.Add(new CalendarDateRange(selectedDate, null));
            }
            //// If the SelectedDateRanges collections is not null or empty, then check the selected date is present in between any of the ranges.
            //// If the selected date is present in between any of the ranges, then remove the range from the collection.
            else
            {
                //// If the selected date is not present in between any of the ranges, then add the selected date to the collection.
                CalendarDateRange? lastRangeInCollection = SelectedDateRanges[SelectedDateRanges.Count - 1];

                if (lastRangeInCollection.EndDate != null)
                {
                    for (int i = SelectedDateRanges.Count - 1; i >= 0; i--)
                    {
                        CalendarDateRange range = SelectedDateRanges[i];
                        if (CalendarViewHelper.IsDateInBetweenRanges(View, range, selectedDate, Identifier))
                        {
                            SelectedDateRanges.RemoveAt(i);
                            return;
                        }
                    }

                    SelectedDateRanges.Add(new CalendarDateRange(selectedDate, null));
                    return;
                }

                //// If the last range has start date and end date is null, need to update the end date.
                if (CalendarViewHelper.IsSameDate(View, selectedDate, lastRangeInCollection.StartDate, Identifier))
                {
                    return;
                }

                for (int i = SelectedDateRanges.Count - 2; i >= 0; i--)
                {
                    CalendarDateRange range = SelectedDateRanges[i];
                    if (CalendarViewHelper.AreRangesIntercept(View, range, new CalendarDateRange(lastRangeInCollection.StartDate, selectedDate), Identifier))
                    {
                        SelectedDateRanges.RemoveAt(i);
                    }
                }

                // Need to create new range when the interacted date is lesser than start date because,its needs swap the start and end date values.
                if (lastRangeInCollection.StartDate.IsGreaterDate(View, selectedDate, Identifier))
                {
                    //// It returns the endRangeDate. Example : date(2022, 11, 09)
                    //// For Month view - returns (2022, 11, 09) , Year view - returns (2022, 11, 30)
                    //// For Decade view - returns (2022, 12, 31) , Century view - returns (2029, 12, 31)
                    DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, lastRangeInCollection.StartDate, Identifier);
                    SelectedDateRanges[SelectedDateRanges.Count - 1] = new CalendarDateRange(selectedDate, endRangeDate);
                }
                //// If the start date is smaller than tapped date, need to update the end date
                else
                {
                    SelectedDateRanges[SelectedDateRanges.Count - 1].EndDate = CalendarViewHelper.GetLastDate(View, selectedDate, Identifier);
                }
            }
        }

        /// <summary>
        /// Method to update the selected date ranges in collection.
        /// </summary>
        /// <param name="newRange">The updated selected range through interaction.</param>
        /// <param name="isNewRange">To identify, The swipe is started and need to create new instance for the selected range or not.</param>
        void UpdateMultiRangeSwipeSelection(CalendarDateRange newRange, bool isNewRange)
        {
            //// If the SelectedDateRanges collections is null, then create a new instance and add the new range.
            if (SelectedDateRanges == null)
            {
                SelectedDateRanges = new ObservableCollection<CalendarDateRange> { newRange };
            }
            //// If the SelectedDateRanges collections is empty, then add the new range to the collection.
            else if (SelectedDateRanges.Count == 0)
            {
                SelectedDateRanges.Add(newRange);
            }
            //// If the SelectedDateRanges collections is not null or empty or not a new range, then check the new range is present in between any of the ranges.
            //// If the new range is present in between any of the ranges, then remove the range from the collection.
            else
            {
                int lastIndex = isNewRange ? SelectedDateRanges.Count - 1 : SelectedDateRanges.Count - 2;
                for (int i = lastIndex; i >= 0; i--)
                {
                    CalendarDateRange range = SelectedDateRanges[i];
                    if (CalendarViewHelper.AreRangesIntercept(View, range, newRange, Identifier))
                    {
                        SelectedDateRanges.RemoveAt(i);
                    }
                }

                if (isNewRange)
                {
                    SelectedDateRanges.Add(newRange);
                    return;
                }

                //// If the selected date is not present in between any of the ranges, then add the selected date to the collection.
                CalendarDateRange? lastRangeInCollection = SelectedDateRanges[SelectedDateRanges.Count - 1];

                DateTime? lastRangeStartDate = lastRangeInCollection.StartDate?.Date;
                DateTime? lastRangeEndDate = lastRangeInCollection.EndDate?.Date;
                DateTime? newRangeStartDate = newRange.StartDate?.Date;
                DateTime? newRangeEndDate = newRange.EndDate?.Date;
                bool isSameStartDate = CalendarViewHelper.IsSameDate(View, lastRangeStartDate, newRangeStartDate, Identifier);
                bool isSameEndDate = CalendarViewHelper.IsSameDate(View, lastRangeEndDate, newRangeEndDate, Identifier);

                //// Need to update the end date. While the interacted date is greater than the start date.
                //// Example: Start or initially interacted date is Sept-1 and the end date is Sept-10. The interacted date is Sept-10 to Sept-11 then need to update the end date only.
                if (isSameStartDate && !isSameEndDate)
                {
                    SelectedDateRanges[SelectedDateRanges.Count - 1].EndDate = CalendarViewHelper.GetLastDate(View, newRange.EndDate, Identifier);
                }
                //// Need to update the start date. While the interacted date is lesser than the start date.
                //// Example: Swipe starts from Sept-10 then interacted to Sept-09 so the range becomes Sept-09 to Sept-10. Then again interacted to Sept-08 then need to update the start date.
                else if (isSameEndDate && !isSameStartDate)
                {
                    SelectedDateRanges[SelectedDateRanges.Count - 1].StartDate = newRange.StartDate;
                }
                //// Need to update new range end date when the interacted date is lesser than start date so,its needs swap the start and end date values.
                else
                {
                    newRange.EndDate = CalendarViewHelper.GetLastDate(View, newRange.EndDate, Identifier);
                    SelectedDateRanges[SelectedDateRanges.Count - 1] = newRange;
                }
            }
        }

        /// <summary>
        /// Wire the property changed events for calendar properties.
        /// </summary>
        void WireEvents()
        {
            if (SelectedDates != null)
            {
                SelectedDates.CollectionChanged += _proxy.OnSelectedDatesCollectionChanged;
            }

            if (SelectedDateRange != null)
            {
                SelectedDateRange.CalendarPropertyChanged += OnSelectedRangePropertyChanged;
            }

            if (SelectedDateRanges != null)
            {
                SelectedDateRanges.CollectionChanged += OnSelectedDateRangesCollectionChanged;
            }

            if (HeaderView != null)
            {
                HeaderView.CalendarPropertyChanged += OnHeaderSettingsPropertyChanged;
                if (HeaderView.TextStyle != null)
                {
                    HeaderView.TextStyle.PropertyChanged += OnHeaderTextStylePropertyChanged;
                }
            }

            WireMonthViewEvents(MonthView);
            WireYearViewEvents(YearView);
        }

        /// <summary>
        /// Method to wire the events for year view properties.
        /// </summary>
        /// <param name="yearViewStyle">The calendar year view style.</param>
        void WireYearViewEvents(CalendarYearView yearViewStyle)
        {
            if (yearViewStyle == null)
            {
                return;
            }

            SetInheritedBindingContext(yearViewStyle, BindingContext);
            yearViewStyle.CalendarPropertyChanged += OnYearViewSettingsPropertyChanged;
            if (yearViewStyle.TextStyle != null)
            {
                SetInheritedBindingContext(yearViewStyle.TextStyle, BindingContext);
                yearViewStyle.TextStyle.PropertyChanged += OnYearCellTextStylePropertyChanged;
            }

            if (yearViewStyle.DisabledDatesTextStyle != null)
            {
                SetInheritedBindingContext(yearViewStyle.DisabledDatesTextStyle, BindingContext);
                yearViewStyle.DisabledDatesTextStyle.PropertyChanged += OnYearCellTextStylePropertyChanged;
            }

            if (yearViewStyle.TodayTextStyle != null)
            {
                SetInheritedBindingContext(yearViewStyle.TodayTextStyle, BindingContext);
                yearViewStyle.TodayTextStyle.PropertyChanged += OnTodayYearCellStylePropertyChanged;
            }

            if (yearViewStyle.LeadingDatesTextStyle != null)
            {
                SetInheritedBindingContext(yearViewStyle.LeadingDatesTextStyle, BindingContext);
                yearViewStyle.LeadingDatesTextStyle.PropertyChanged += OnLeadingYearCellTextStyleSettingsChanged;
            }

            if (yearViewStyle.RangeTextStyle != null)
            {
                SetInheritedBindingContext(yearViewStyle.RangeTextStyle, BindingContext);
                yearViewStyle.RangeTextStyle.PropertyChanged += OnYearCellRangeTextStylePropertyChanged;
            }

            if (yearViewStyle.SelectionTextStyle != null)
            {
                SetInheritedBindingContext(yearViewStyle.SelectionTextStyle, BindingContext);
                yearViewStyle.SelectionTextStyle.PropertyChanged += OnYearCellSelectionTextStylePropertyChanged;
            }
        }

        /// <summary>
        /// Method to remove the property changed events for year view properties.
        /// </summary>
        /// <param name="yearViewStyle">The calendar year view style.</param>
        void UnWireYearViewEvents(CalendarYearView yearViewStyle)
        {
            if (yearViewStyle == null)
            {
                return;
            }

            yearViewStyle.BindingContext = null;
            yearViewStyle.CalendarPropertyChanged -= OnYearViewSettingsPropertyChanged;
            if (yearViewStyle.TextStyle != null)
            {
                yearViewStyle.TextStyle.BindingContext = null;
                yearViewStyle.TextStyle.PropertyChanged -= OnYearCellTextStylePropertyChanged;
            }

            if (yearViewStyle.DisabledDatesTextStyle != null)
            {
                yearViewStyle.DisabledDatesTextStyle.BindingContext = null;
                yearViewStyle.DisabledDatesTextStyle.PropertyChanged -= OnYearCellTextStylePropertyChanged;
            }

            if (yearViewStyle.TodayTextStyle != null)
            {
                yearViewStyle.TodayTextStyle.BindingContext = null;
                yearViewStyle.TodayTextStyle.PropertyChanged -= OnTodayYearCellStylePropertyChanged;
            }

            if (yearViewStyle.LeadingDatesTextStyle != null)
            {
                yearViewStyle.LeadingDatesTextStyle.BindingContext = null;
                yearViewStyle.LeadingDatesTextStyle.PropertyChanged -= OnLeadingYearCellTextStyleSettingsChanged;
            }

            if (yearViewStyle.RangeTextStyle != null)
            {
                yearViewStyle.RangeTextStyle.BindingContext = null;
                yearViewStyle.RangeTextStyle.PropertyChanged -= OnYearCellRangeTextStylePropertyChanged;
            }

            if (yearViewStyle.SelectionTextStyle != null)
            {
                yearViewStyle.SelectionTextStyle.BindingContext = null;
                yearViewStyle.SelectionTextStyle.PropertyChanged -= OnYearCellSelectionTextStylePropertyChanged;
            }
        }

        /// <summary>
        /// Removed the property changed events.
        /// </summary>
        void UnwireEvents()
        {
            if (SelectedDates != null)
            {
                SelectedDates.CollectionChanged -= _proxy.OnSelectedDatesCollectionChanged;
            }

            if (SelectedDateRange != null)
            {
                SelectedDateRange.CalendarPropertyChanged -= OnSelectedRangePropertyChanged;
            }

            if (HeaderView != null)
            {
                HeaderView.CalendarPropertyChanged -= OnHeaderSettingsPropertyChanged;
                if (HeaderView.TextStyle != null)
                {
                    HeaderView.TextStyle.PropertyChanged -= OnHeaderTextStylePropertyChanged;
                }
            }

            UnWireMonthViewEvents(MonthView);
            UnWireYearViewEvents(YearView);
        }

        /// <summary>
        /// Method to remove the property changed events for calendar month view properties.
        /// </summary>
        /// <param name="calendarMonthView">The calendar month view.</param>
        void UnWireMonthViewEvents(CalendarMonthView calendarMonthView)
        {
            if (calendarMonthView == null)
            {
                return;
            }

            calendarMonthView.BindingContext = null;
            calendarMonthView.CalendarPropertyChanged -= OnMonthViewPropertyChanged;
            if (calendarMonthView.WeekNumberStyle != null)
            {
                calendarMonthView.WeekNumberStyle.BindingContext = null;
                calendarMonthView.WeekNumberStyle.CalendarPropertyChanged -= OnWeekNumberStylePropertyChanged;
                if (calendarMonthView.WeekNumberStyle.TextStyle != null)
                {
                    calendarMonthView.WeekNumberStyle.TextStyle.BindingContext = null;
                    calendarMonthView.WeekNumberStyle.TextStyle.PropertyChanged -= OnWeekNumberTextStyleChanged;
                }
            }

            if (calendarMonthView.HeaderView != null)
            {
                calendarMonthView.HeaderView.BindingContext = null;
                calendarMonthView.HeaderView.CalendarPropertyChanged -= OnMonthHeaderViewPropertyChanged;
                if (calendarMonthView.HeaderView.TextStyle != null)
                {
                    calendarMonthView.HeaderView.TextStyle.BindingContext = null;
                    calendarMonthView.HeaderView.TextStyle.PropertyChanged -= OnMonthHeaderViewTextStylePropertyChanged;
                }
            }

            if (calendarMonthView.TextStyle != null)
            {
                calendarMonthView.TextStyle.BindingContext = null;
                calendarMonthView.TextStyle.PropertyChanged -= OnMonthViewTextStyleChanged;
            }

            if (calendarMonthView.TrailingLeadingDatesTextStyle != null)
            {
                calendarMonthView.TrailingLeadingDatesTextStyle.BindingContext = null;
                calendarMonthView.TrailingLeadingDatesTextStyle.PropertyChanged -= OnMonthTrailingLeadingDatesTextStyleChanged;
            }

            if (calendarMonthView.TodayTextStyle != null)
            {
                calendarMonthView.TodayTextStyle.BindingContext = null;
                calendarMonthView.TodayTextStyle.PropertyChanged -= OnMonthViewTodayStylePropertyChanged;
            }

            if (calendarMonthView.DisabledDatesTextStyle != null)
            {
                calendarMonthView.DisabledDatesTextStyle.BindingContext = null;
                calendarMonthView.DisabledDatesTextStyle.PropertyChanged -= OnMonthDisabledDatesTextStylePropertyChanged;
            }

            if (calendarMonthView.WeekendDatesTextStyle != null)
            {
                calendarMonthView.WeekendDatesTextStyle.BindingContext = null;
                calendarMonthView.WeekendDatesTextStyle.PropertyChanged -= OnMonthWeekendsDatesTextStylePropertyChanged;
            }

            if (calendarMonthView.SpecialDatesTextStyle != null)
            {
                calendarMonthView.SpecialDatesTextStyle.BindingContext = null;
                calendarMonthView.SpecialDatesTextStyle.PropertyChanged -= OnSpecialDatesStylePropertyChanged;
            }

            if (calendarMonthView.RangeTextStyle != null)
            {
                calendarMonthView.RangeTextStyle.BindingContext = null;
                calendarMonthView.RangeTextStyle.PropertyChanged -= OnMonthViewRangeTextStylePropertyChanged;
            }

            if (calendarMonthView.SelectionTextStyle != null)
            {
                calendarMonthView.SelectionTextStyle.BindingContext = null;
                calendarMonthView.SelectionTextStyle.PropertyChanged -= OnMonthViewSelectionTextStylePropertyChanged;
            }
        }

        /// <summary>
        /// Method to wire the events for month view properties.
        /// </summary>
        /// <param name="calendarMonthView">The calendar month view.</param>
        void WireMonthViewEvents(CalendarMonthView calendarMonthView)
        {
            if (calendarMonthView == null)
            {
                return;
            }

            SetInheritedBindingContext(calendarMonthView, BindingContext);
            calendarMonthView.CalendarPropertyChanged += OnMonthViewPropertyChanged;
            if (calendarMonthView.WeekNumberStyle != null)
            {
                SetInheritedBindingContext(calendarMonthView.WeekNumberStyle, BindingContext);
                calendarMonthView.WeekNumberStyle.CalendarPropertyChanged += OnWeekNumberStylePropertyChanged;
                if (calendarMonthView.WeekNumberStyle.TextStyle != null)
                {
                    SetInheritedBindingContext(calendarMonthView.WeekNumberStyle.TextStyle, BindingContext);
                    calendarMonthView.WeekNumberStyle.TextStyle.PropertyChanged += OnWeekNumberTextStyleChanged;
                }
            }

            if (calendarMonthView.HeaderView != null)
            {
                SetInheritedBindingContext(calendarMonthView.HeaderView, BindingContext);
                calendarMonthView.HeaderView.CalendarPropertyChanged += OnMonthHeaderViewPropertyChanged;
                if (calendarMonthView.HeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(calendarMonthView.HeaderView.TextStyle, BindingContext);
                    calendarMonthView.HeaderView.TextStyle.PropertyChanged += OnMonthHeaderViewTextStylePropertyChanged;
                }
            }

            if (calendarMonthView.TextStyle != null)
            {
                SetInheritedBindingContext(calendarMonthView.TextStyle, BindingContext);
                calendarMonthView.TextStyle.PropertyChanged += OnMonthViewTextStyleChanged;
            }

            if (calendarMonthView.TrailingLeadingDatesTextStyle != null)
            {
                SetInheritedBindingContext(calendarMonthView.TrailingLeadingDatesTextStyle, BindingContext);
                calendarMonthView.TrailingLeadingDatesTextStyle.PropertyChanged += OnMonthTrailingLeadingDatesTextStyleChanged;
            }

            if (calendarMonthView.TodayTextStyle != null)
            {
                SetInheritedBindingContext(calendarMonthView.TodayTextStyle, BindingContext);
                calendarMonthView.TodayTextStyle.PropertyChanged += OnMonthViewTodayStylePropertyChanged;
            }

            if (calendarMonthView.DisabledDatesTextStyle != null)
            {
                SetInheritedBindingContext(calendarMonthView.DisabledDatesTextStyle, BindingContext);
                calendarMonthView.DisabledDatesTextStyle.PropertyChanged += OnMonthDisabledDatesTextStylePropertyChanged;
            }

            if (calendarMonthView.WeekendDatesTextStyle != null)
            {
                SetInheritedBindingContext(calendarMonthView.WeekendDatesTextStyle, BindingContext);
                calendarMonthView.WeekendDatesTextStyle.PropertyChanged += OnMonthWeekendsDatesTextStylePropertyChanged;
            }

            if (calendarMonthView.SpecialDatesTextStyle != null)
            {
                SetInheritedBindingContext(calendarMonthView.SpecialDatesTextStyle, BindingContext);
                calendarMonthView.SpecialDatesTextStyle.PropertyChanged += OnSpecialDatesStylePropertyChanged;
            }

            if (calendarMonthView.RangeTextStyle != null)
            {
                SetInheritedBindingContext(calendarMonthView.RangeTextStyle, BindingContext);
                calendarMonthView.RangeTextStyle.PropertyChanged += OnMonthViewRangeTextStylePropertyChanged;
            }

            if (calendarMonthView.SelectionTextStyle != null)
            {
                SetInheritedBindingContext(calendarMonthView.SelectionTextStyle, BindingContext);
                calendarMonthView.SelectionTextStyle.PropertyChanged += OnMonthViewSelectionTextStylePropertyChanged;
            }
        }

        /// <summary>
        /// Method to update the flow direction.
        /// </summary>
        void UpdateFlowDirection()
        {
            if (_customScrollLayout == null)
            {
                return;
            }

            UpdateLayoutFlowDirection();
            _layout.UpdateFlowDirection();
            //// To update the header text flow direction.
            _headerLayout?.UpdateHeaderTextFlowDirection();
            _footerLayout?.UpdateFooterFlowDirection();
        }

        /// <summary>
        /// Method to update the layout flow direction.
        /// </summary>
        void UpdateLayoutFlowDirection()
        {
            // Set isRTLLayout based on CalendarIdentifier
            switch (Identifier)
            {
                case CalendarIdentifier.Hijri:
                case CalendarIdentifier.Persian:
                case CalendarIdentifier.UmAlQura:
                    _isRTLLayout = true;
                    break;

                case CalendarIdentifier.Korean:
                case CalendarIdentifier.Taiwan:
                case CalendarIdentifier.ThaiBuddhist:
                    _isRTLLayout = false;
                    break;

                case CalendarIdentifier.Gregorian:
                    _isRTLLayout = FlowDirection == FlowDirection.RightToLeft ? true : this.IsRTL(this.Identifier);
                    break;

                default:
                    _isRTLLayout = false;
                    break;
            }
        }

        /// <summary>
        /// Method to update the range selection.
        /// </summary>
        /// <param name="tappedDate">The tapped or pan date.</param>
        void UpdateRangeSelection(DateTime? tappedDate)
        {
            if (tappedDate == null)
            {
                return;
            }

            switch (RangeSelectionDirection)
            {
                case CalendarRangeSelectionDirection.Default:
                    UpdateSelectedDateRange(tappedDate.Value);
                    break;
                case CalendarRangeSelectionDirection.Forward:
                    UpdateForwardRangeSelection(tappedDate.Value);
                    break;
                case CalendarRangeSelectionDirection.Backward:
                    UpdateBackwardRangeSelection(tappedDate.Value);
                    break;
                case CalendarRangeSelectionDirection.Both:
                    UpdateBothRangeSelection(tappedDate.Value);
                    break;
                case CalendarRangeSelectionDirection.None:
                    UpdateNoneRangeSelection(tappedDate.Value);
                    break;
            }
        }

        /// <summary>
        /// Method to update the selected range value through interaction on forward range selection.
        /// </summary>
        /// <param name="date">The interacted date.</param>
        void UpdateForwardRangeSelection(DateTime date)
        {
            DateTime? startDate = SelectedDateRange?.StartDate?.Date;
            DateTime? endDate = SelectedDateRange?.EndDate == null ? startDate : SelectedDateRange?.EndDate?.Date;

            // Need to create a new instance for selected range below mentioned scenario's.
            // 1.The selected range is null.
            // 2.The start is null.
            if (SelectedDateRange == null || startDate == null)
            {
                CalendarDateRange rangeSelection = new CalendarDateRange(date, null);
                SelectedDateRange = rangeSelection;
            }
            else
            {
                // The tapped date equal to end date then no need to update the end date.
                if (CalendarViewHelper.IsSameDate(View, endDate, date, Identifier) || CalendarViewHelper.IsSameDate(View, startDate, date, Identifier))
                {
                    return;
                }

                if (CalendarViewHelper.IsGreaterDate(date, View, startDate, Identifier))
                {
                    DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, date, Identifier);
                    SelectedDateRange.EndDate = endRangeDate;
                }
            }
        }

        /// <summary>
        /// Method to update the selected range value through interaction on backward range selection.
        /// </summary>
        /// <param name="date">The interacted date.</param>
        void UpdateBackwardRangeSelection(DateTime date)
        {
            DateTime? startDate = SelectedDateRange?.StartDate?.Date;
            DateTime? endDate = SelectedDateRange?.EndDate?.Date;

            // Need to create a new instance for selected range below mentioned scenario's.
            // 1.The selected range is null.
            // 2.The start is null.
            if (SelectedDateRange == null || startDate == null)
            {
                CalendarDateRange rangeSelection = new CalendarDateRange(date, null);
                SelectedDateRange = rangeSelection;
            }
            else
            {
                // The tapped date equal to start date then no need to update the start date.
                if (CalendarViewHelper.IsSameDate(View, startDate, date, Identifier) || CalendarViewHelper.IsSameDate(View, endDate, date, Identifier))
                {
                    return;
                }

                // Assume Range Sept-10 to null and tapped date is before start date( Here tapped date is Sept-05).
                // Then considered start as tapped date(Sept-05) and end date is start date(Sept-10).
                if (startDate.IsGreaterDate(View, date, Identifier) && endDate == null)
                {
                    DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, startDate, Identifier);
                    CalendarDateRange rangeSelection = new CalendarDateRange(date, endRangeDate);
                    SelectedDateRange = rangeSelection;
                }
                else if (endDate.IsGreaterDate(View, date, Identifier))
                {
                    SelectedDateRange.StartDate = date;
                }
            }
        }

        /// <summary>
        /// Method to update the selected range value through interaction on both range selection.
        /// </summary>
        /// <param name="date">The interacted date.</param>
        void UpdateBothRangeSelection(DateTime date)
        {
            DateTime? startDate = SelectedDateRange?.StartDate?.Date;
            DateTime? endDate = SelectedDateRange?.EndDate?.Date;

            // Need to create a new instance for selected range below mentioned scenario's.
            // 1.The selected range is null.
            // 2.The start is null.
            if (SelectedDateRange == null || startDate == null)
            {
                CalendarDateRange rangeSelection = new CalendarDateRange(date, null);
                SelectedDateRange = rangeSelection;
            }
            else
            {
                // The tapped date equal to start date then no need to update the start date.
                if (CalendarViewHelper.IsSameDate(View, startDate, date, Identifier) || CalendarViewHelper.IsSameDate(View, endDate, date, Identifier))
                {
                    return;
                }

                // Assume Range Sept-10 to null and tapped date is before start date( Here tapped date is Sept-05).
                // Then considered start as tapped date(Sept-05) and end date is start date(Sept-10).
                if (startDate.IsGreaterDate(View, date, Identifier) && endDate == null)
                {
                    DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, startDate, Identifier);
                    CalendarDateRange rangeSelection = new CalendarDateRange(date, endRangeDate);
                    SelectedDateRange = rangeSelection;
                }

                // Example: range (Nov-10 to Nov-18), The interacted date is Nov-25 then need to update the end date.
                else if (CalendarViewHelper.IsGreaterDate(date, View, endDate, Identifier))
                {
                    DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, date, Identifier);
                    SelectedDateRange.EndDate = endRangeDate;
                }

                // Example: range (Nov-10 to Nov-18), The interacted date is Nov-5 then need to update the start date.
                else if (startDate.IsGreaterDate(View, date, Identifier))
                {
                    SelectedDateRange.StartDate = date;
                }
                else
                {
                    TimeSpan? startDateSpan = date.Date - startDate;
                    TimeSpan? endDateSpan = endDate - date.Date;

                    // The different between the start and tapped date is lesser than different between end and tapped date then need to update the selected range start date value.
                    if (startDateSpan <= endDateSpan)
                    {
                        SelectedDateRange.StartDate = date;
                    }
                    else
                    {
                        DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, date, Identifier);
                        SelectedDateRange.EndDate = endRangeDate;
                    }
                }
            }
        }

        /// <summary>
        /// Method to update the selected range value through interaction on both range selection.
        /// </summary>
        /// <param name="date">The interacted date.</param>
        void UpdateRangeSelectionOnKeyboard(DateTime date)
        {
            DateTime? startDate = SelectedDateRange?.StartDate?.Date;
            DateTime? endDate = SelectedDateRange?.EndDate?.Date;

            // Need to create a new instance for selected range below mentioned scenario's.
            // 1.The selected range is null.
            // 2.The start is null.
            if (SelectedDateRange == null || startDate == null)
            {
                CalendarDateRange rangeSelection = new CalendarDateRange(date, null);
                SelectedDateRange = rangeSelection;
            }
            else
            {
                // The tapped date equal to start date then no need to update the start date.
                if (CalendarViewHelper.IsSameDate(View, startDate, date, Identifier))
                {
                    _previousSelectedDate = date;
                    return;
                }
                else if (CalendarViewHelper.IsSameDate(View, endDate, date, Identifier))
                {
                    _previousSelectedDate = date;
                    return;
                }

                // Assume Range Sept-10 to null and tapped date is before start date( Here tapped date is Sept-05).
                // Then considered start as tapped date(Sept-05) and end date is start date(Sept-10).
                if (startDate.IsGreaterDate(View, date, Identifier) && endDate == null)
                {
                    DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, startDate, Identifier);
                    CalendarDateRange rangeSelection = new CalendarDateRange(date, endRangeDate);
                    SelectedDateRange = rangeSelection;
                    _previousSelectedDate = date;
                }

                // Example: range (Nov-10 to Nov-18), The interacted date is Nov-25 then need to update the end date.
                else if (CalendarViewHelper.IsGreaterDate(date, View, endDate, Identifier))
                {
                    DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, date, Identifier);
                    SelectedDateRange.EndDate = endRangeDate;
                }

                // Example: range (Nov-10 to Nov-18), The interacted date is Nov-5 then need to update the start date.
                else if (startDate.IsGreaterDate(View, date, Identifier))
                {
                    SelectedDateRange.StartDate = date;
                    _previousSelectedDate = date;
                }
                else
                {
                    TimeSpan? startDateSpan = date.Date - startDate;
                    TimeSpan? endDateSpan = endDate - date.Date;

                    // The different between the start and tapped date is lesser than different between end and tapped date then need to update the selected range start date value.
                    if (startDateSpan <= endDateSpan)
                    {
                        SelectedDateRange.StartDate = date;
                        _previousSelectedDate = date;
                    }
                    else
                    {
                        DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, date, Identifier);
                        SelectedDateRange.EndDate = endRangeDate;
                    }
                }
            }
        }

        /// <summary>
        /// Method to update the selected range value through interaction on none range selection.
        /// </summary>
        /// <param name="date">The interacted date.</param>
        void UpdateNoneRangeSelection(DateTime date)
        {
            DateTime? startDate = SelectedDateRange?.StartDate?.Date;
            DateTime? endDate = SelectedDateRange?.EndDate?.Date;

            // Need to create a new instance for selected range below mentioned scenario's.
            // 1.The selected range is null.
            // 2.The start is null.
            if (SelectedDateRange == null || startDate == null)
            {
                CalendarDateRange rangeSelection = new CalendarDateRange(date, null);
                SelectedDateRange = rangeSelection;
            }
            else
            {
                // The selected range is not a valid range(range (Nov-10 to null) or(Nov-10 to Nov-10) )) then need to update the interacted date.
                // The selected range valid range (Nov-10 to Nov-15) the start and end are not null and not equal then no need to update the interacted date.
                // Example: Range(Nov-10 to null) interacted date is Nov-10. If the interacted date is same as start date then no need to update the selected range.
                if ((startDate != null && endDate != null && !CalendarViewHelper.IsSameDate(View, startDate, endDate, Identifier)) || CalendarViewHelper.IsSameDate(View, startDate, date, Identifier))
                {
                    return;
                }

                //// If the startDate is greater than the date need to update the tapped date as start date and start date as end date.
                if (startDate.IsGreaterDate(View, date, Identifier))
                {
                    DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, startDate, Identifier);
                    CalendarDateRange rangeSelection = new CalendarDateRange(date, endRangeDate);
                    SelectedDateRange = rangeSelection;
                }
                else
                {
                    DateTime? endRangeDate = CalendarViewHelper.GetLastDate(View, date, Identifier);
                    SelectedDateRange.EndDate = endRangeDate;
                }
            }
        }

        /// <summary>
        /// Method for add calendar to popup.
        /// </summary>
        void AddCalendarToPopup()
        {
            if (_popup == null)
            {
                _popup = new SfPopup();
                _popup.ShowHeader = false;
                _popup.ShowFooter = false;
                _popup.PopupStyle.CornerRadius = CornerRadius;
                _popup.Opened += OnPopupOpened;
                _popup.Opening += OnPopupOpening;
                _popup.Closed += OnPopupClosed;
                _popup.Closing += OnPopupClosing;
                Add(_popup);
            }

            if (_layout == null)
            {
                return;
            }

            _layout.BackgroundColor = BackgroundColor;
            _popup.ContentTemplate = new DataTemplate(() =>
            {
                return _layout;
            });
            UpdatePopUpSize();
        }

        /// <summary>
        /// Method to show the popup.
        /// </summary>
        void ShowPopup()
        {
            if (_popup == null)
            {
                return;
            }

            if (Mode == CalendarMode.RelativeDialog)
            {
                if (RelativeView != null)
                {
                    ShowRelativeToView(RelativeView, RelativePosition);
                }
                else
                {
                    ShowRelativeToView(this, RelativePosition);
                }
            }
            else
            {
                _popup.IsOpen = true;
            }
        }

        /// <summary>
        /// Method to update the popup size.
        /// </summary>
        void UpdatePopUpSize()
        {
            if (_popup == null)
            {
                return;
            }

            // Static values used from design specifications provided by the UX team in Figma.
            _popup.WidthRequest = 300;
#if WINDOWS || MACCATALYST
            if (View == CalendarView.Month)
            {
                _popup.HeightRequest = 350 + HeaderView.Height + MonthView.HeaderView.Height + FooterView.Height;
            }
            else
            {
                _popup.HeightRequest = 350 + HeaderView.Height + FooterView.Height;
            }
#elif ANDROID || IOS
            if (View == CalendarView.Month)
            {
                _popup.HeightRequest = 250 + HeaderView.Height + MonthView.HeaderView.Height + FooterView.Height;
            }
            else
            {
                _popup.HeightRequest = 250 + HeaderView.Height + FooterView.Height;
            }
#endif
        }

        /// <summary>
        /// Method to close the calendar popup.
        /// </summary>
        void CloseCalendarPopup()
        {
            if (_popup == null)
            {
                return;
            }

            _popup.IsOpen = false;
            _popup.ContentTemplate = new DataTemplate(() =>
            {
                return null;
            });
        }

        /// <summary>
        /// Method to show the calendar popup based on the relative view and relative position.
        /// </summary>
        /// <param name="relativeView">The relative view.</param>
        /// <param name="relativePosition">The relative position.</param>
        void ShowRelativeToView(View relativeView, CalendarRelativePosition relativePosition)
        {
            if (_popup == null)
            {
                return;
            }

            switch (relativePosition)
            {
                case CalendarRelativePosition.AlignTop:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignTop);
                    break;

                case CalendarRelativePosition.AlignBottom:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignBottom);
                    break;

                case CalendarRelativePosition.AlignTopLeft:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignTopLeft);
                    break;

                case CalendarRelativePosition.AlignTopRight:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignTopRight);
                    break;

                case CalendarRelativePosition.AlignBottomLeft:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignBottomLeft);
                    break;

                case CalendarRelativePosition.AlignBottomRight:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignBottomRight);
                    break;

                case CalendarRelativePosition.AlignToLeftOf:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignToLeftOf);
                    break;

                case CalendarRelativePosition.AlignToRightOf:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignToRightOf);
                    break;
            }
        }

        /// <summary>
        /// Method to reset the calendar popup.
        /// </summary>
        void ResetPopup()
        {
            if (_popup == null)
            {
                return;
            }

            _popup.IsOpen = false;
            _popup.Opened -= OnPopupOpened;
            _popup.Opening -= OnPopupOpening;
            _popup.Closed -= OnPopupClosed;
            _popup.Closing -= OnPopupClosing;
            Remove(_popup);
            _popup = null;
        }

        /// <summary>
        /// Method to raise the popup closing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The cancel event args.</param>
        void OnPopupClosing(object? sender, CancelEventArgs e)
        {
            e.Cancel = RaisePopupClosingEvent();
        }

        /// <summary>
        /// Method raises while the popup event closing.
        /// </summary>
        /// <returns>Returns whether to cancel closing of the popup.</returns>
        bool RaisePopupClosingEvent()
        {
            if (CalendarPopupClosing != null)
            {
                CancelEventArgs popupClosingEventArgs = new CancelEventArgs();
                InvokeClosingEvent(this, popupClosingEventArgs);
                return popupClosingEventArgs.Cancel;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method triggered while the popup closed.
        /// </summary>
        /// <param name="sender">The popup instance.</param>
        /// <param name="e">Closed event argument.</param>
        void OnPopupClosed(object? sender, EventArgs e)
        {
            IsOpen = false;
            InvokeClosedEvent(this, e);
        }

        /// <summary>
        /// Method triggered while the popup opened.
        /// </summary>
        /// <param name="sender">The popup instance.</param>
        /// <param name="e">Opened event argument.</param>
        void OnPopupOpened(object? sender, EventArgs e)
        {
            InvokeOpenedEvent(this, e);
        }

        /// <summary>
        /// Method triggered while the popup opening.
        /// </summary>
        /// <param name="sender">The popup instance.</param>
        /// <param name="e">Opened event argument.</param>
        void OnPopupOpening(object? sender, CancelEventArgs e)
        {
            e.Cancel = RaisePopupOpeningEvent();
        }

        /// <summary>
        /// Method raise while the popup opening.
        /// </summary>
        /// <returns>The bool value.</returns>
        bool RaisePopupOpeningEvent()
        {
            if (CalendarPopupOpening != null)
            {
                CancelEventArgs popupOpeningEventArgs = new CancelEventArgs();
                InvokeOpeningEvent(this, popupOpeningEventArgs);
                return popupOpeningEventArgs.Cancel;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Measures the desired size of the view within the given constraints.
        /// </summary>
        /// <param name="widthConstraint">The width limit for measuring the view.</param>
        /// <param name="heightConstraint">The height limit for measuring the view.</param>
        /// <returns>The desired size of the view.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            double width = double.IsFinite(widthConstraint) ? widthConstraint : _minWidth;
            double height = double.IsFinite(heightConstraint) ? heightConstraint : _minHeight;
#if __ANDROID__ || __IOS__ || __MACCATALYST__
            //// Restrict the repeated layout update while the
            //// available size and new measured size is same.
            if (_availableSize.Width == width && _availableSize.Height == height)
            {
                return _availableSize;
            }
#else
            //// Restrict the children generation and layout update while the
            //// available size is zero and new measured size is also zero.
            if (_availableSize == Size.Zero && width == 0 && height == 0)
            {
                return _availableSize;
            }
#endif

            if (_layout.Children.Count == 0)
            {
                AddChildren();
            }

            _availableSize = new Size(width, height);

            if (Mode != CalendarMode.Default)
            {
                return Size.Zero;
            }

            //// This function call to the MeasureContent method of the base class which is used to measure the size of the layout.
            return base.MeasureContent(width, height);
        }

        /// <summary>
        /// Method to clip the content of the calendar view.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="dirtyRect">The dirty rectangle.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            if (CornerRadius == 0)
            {
                return;
            }

            //// Set the clip to the calendar view
            //// The clip is used to ensure that the content within the sfcalendar stays within the boundaries of the rounded rectangle shape,
            //// The content of the rounded corners will be cropped or hidden by the clip, meaning they are not visible in view.
            RoundRectangleGeometry currentClip;
            RectF rectangle = new RectF(0, 0, dirtyRect.Width, dirtyRect.Height);
            //// In windows platform, the RTl and LTR layout is automatically handled by the framework.
#if WINDOWS
            currentClip = new RoundRectangleGeometry(CornerRadius, rectangle);
#else
            if (_isRTLLayout)
            {
                currentClip = new RoundRectangleGeometry(new CornerRadius(CornerRadius.TopRight, CornerRadius.TopLeft, CornerRadius.BottomRight, CornerRadius.BottomLeft), rectangle);
            }
            else
            {
                currentClip = new RoundRectangleGeometry(CornerRadius, rectangle);
            }
#endif
            RoundRectangleGeometry? previousClip = null;
            if (Clip != null && Clip is RoundRectangleGeometry)
            {
                previousClip = (RoundRectangleGeometry)Clip;
            }

            //// Here we are comparing the previous clip with the current clip value.
            //// Because, if the previous and current clip value of the property is same, then we don't need to update the clip.
            if (previousClip == null || previousClip.CornerRadius != currentClip.CornerRadius || previousClip.Rect != currentClip.Rect)
            {
                Clip = currentClip;
            }
        }

        /// <summary>
        /// Method calls when the size for the view is allocated.
        /// </summary>
        /// <param name="width">The width of the view.</param>
        /// <param name="height">The height of the view.</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
#if ANDROID
            //// In Android calling the clip at the draw, cause the layout is not properly update in the view. Since, we have set clipping at the onsize allocated.
            if (width > 0 && height > 0)
            {
                //// Set the clip to the calendar view
                //// The clip is used to ensure that the content within the sfcalendar stays within the boundaries of the rounded rectangle shape,
                //// The content of the rounded corners will be cropped or hidden by the clip, meaning they are not visible in view.
                RoundRectangleGeometry currentClip;
                RectF rectangle = new RectF(0, 0, (float)width, (float)height);
                if (_isRTLLayout)
                {
                    currentClip = new RoundRectangleGeometry(new CornerRadius(CornerRadius.TopRight, CornerRadius.TopLeft, CornerRadius.BottomRight, CornerRadius.BottomLeft), rectangle);
                }
                else
                {
                    currentClip = new RoundRectangleGeometry(CornerRadius, rectangle);
                }

                RoundRectangleGeometry? previousClip = null;
                if (Clip != null && Clip is RoundRectangleGeometry)
                {
                    previousClip = (RoundRectangleGeometry)Clip;
                }

                //// Here we are comparing the previous clip with the current clip value.
                //// Because, if the previous and current clip value of the property is same, then we don't need to update the clip.
                if (previousClip == null || previousClip.CornerRadius != currentClip.CornerRadius || previousClip.Rect != currentClip.Rect)
                {
                    Clip = currentClip;
                }
            }
#endif
        }

        #endregion

        #region Internal Property Changed Method

        /// <summary>
        /// Method to update the selected dates collection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The notify collection changed event args.</param>
        internal void OnSelectedDatesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ReadOnlyObservableCollection<DateTime> oldSelectedDates = new ReadOnlyObservableCollection<DateTime>(_selectedDates);
            _selectedDates = new ObservableCollection<DateTime>(SelectedDates);
            ReadOnlyObservableCollection<DateTime> newSelectedDates = new ReadOnlyObservableCollection<DateTime>(_selectedDates);
            var selectionChangedEventArgs = new CalendarSelectionChangedEventArgs { NewValue = newSelectedDates, OldValue = oldSelectedDates };

            if (SelectionChanged != null)
            {
                SelectionChanged?.Invoke(this, selectionChangedEventArgs);
            }

            if (SelectionChangedCommand != null && SelectionChangedCommand.CanExecute(selectionChangedEventArgs))
            {
                SelectionChangedCommand.Execute(selectionChangedEventArgs);
            }

            if (_customScrollLayout == null)
            {
                return;
            }

            _customScrollLayout.UpdateSelectedDatesOnAction(e);
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Method to update the selection range property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The calendar property changed events arguments.</param>
        void OnSelectedRangePropertyChanged(object? sender, CalendarPropertyChangedEventArgs e)
        {
            CalendarDateRange calendarDateRange;
            if (e.PropertyName == nameof(SelectedDateRange.StartDate))
            {
                calendarDateRange = new CalendarDateRange((DateTime?)e.OldValue, SelectedDateRange?.EndDate);
            }
            else
            {
                calendarDateRange = new CalendarDateRange(SelectedDateRange?.StartDate, (DateTime?)e.OldValue);
            }

            if (SelectionMode == CalendarSelectionMode.Range)
            {
                var selectionChangedEventArgs = new CalendarSelectionChangedEventArgs { NewValue = SelectedDateRange, OldValue = calendarDateRange };
                if (SelectionChanged != null)
                {
                    SelectionChanged?.Invoke(this, selectionChangedEventArgs);
                }

                if (SelectionChangedCommand != null && SelectionChangedCommand.CanExecute(selectionChangedEventArgs))
                {
                    SelectionChangedCommand.Execute(selectionChangedEventArgs);
                }
            }

            _customScrollLayout?.UpdateSelectionRange();
        }

        /// <summary>
        /// Method to update the selection ranges property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The calendar property changed events arguments.</param>
        void OnSelectedRangesPropertyChanged(object? sender, CalendarPropertyChangedEventArgs e)
        {
            ReadOnlyObservableCollection<CalendarDateRange> oldSelectedRanges = new ReadOnlyObservableCollection<CalendarDateRange>(_selectedDateRanges);
            //// To update the new value for the selectedDateRanges private variable.
            _selectedDateRanges = CalendarViewHelper.CloneSelectedRanges(SelectedDateRanges);

            ReadOnlyObservableCollection<CalendarDateRange> newSelectedRanges = new ReadOnlyObservableCollection<CalendarDateRange>(SelectedDateRanges);

            if (SelectionMode == CalendarSelectionMode.MultiRange)
            {
                var selectionChangedEventArgs = new CalendarSelectionChangedEventArgs { NewValue = newSelectedRanges, OldValue = oldSelectedRanges };
                if (SelectionChanged != null)
                {
                    SelectionChanged?.Invoke(this, selectionChangedEventArgs);
                }

                if (SelectionChangedCommand != null && SelectionChangedCommand.CanExecute(selectionChangedEventArgs))
                {
                    SelectionChangedCommand.Execute(selectionChangedEventArgs);
                }
            }

            _customScrollLayout?.UpdateMultiRangeSelectionValue();
        }

        /// <summary>
        /// Method to update the selected date ranges collection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The collection changed events arguments.</param>
        void OnSelectedDateRangesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ReadOnlyObservableCollection<CalendarDateRange> oldSelectedRanges = new ReadOnlyObservableCollection<CalendarDateRange>(_selectedDateRanges);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    //// To wire the properties in each CalendarDateRange object for the new selected ranges
                    if (e.NewItems != null)
                    {
                        foreach (CalendarDateRange newRange in e.NewItems)
                        {
                            newRange.CalendarPropertyChanged += OnSelectedRangesPropertyChanged;
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    //// To unwire the properties in each CalendarDateRange object for the old selected ranges
                    if (e.OldItems != null)
                    {
                        foreach (CalendarDateRange oldRange in e.OldItems)
                        {
                            oldRange.CalendarPropertyChanged -= OnSelectedRangesPropertyChanged;
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Replace:
                    //// To unwire the properties in each CalendarDateRange object for the old selected ranges
                    if (e.OldItems != null)
                    {
                        foreach (CalendarDateRange oldRange in e.OldItems)
                        {
                            oldRange.CalendarPropertyChanged -= OnSelectedRangesPropertyChanged;
                        }
                    }

                    //// To wire the properties in each CalendarDateRange object for the new selected ranges
                    if (e.NewItems != null)
                    {
                        foreach (CalendarDateRange newRange in e.NewItems)
                        {
                            newRange.CalendarPropertyChanged += OnSelectedRangesPropertyChanged;
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    //// Handle this case where the collection is reset.
                    //// This can happen when the entire collection is replaced or cleared.
                    //// To unwire the properties in each CalendarDateRange object for the old selected ranges
                    foreach (CalendarDateRange oldRange in oldSelectedRanges)
                    {
                        oldRange.CalendarPropertyChanged -= OnSelectedRangesPropertyChanged;
                    }

                    //// To wire the properties in each CalendarDateRange object for the new selected ranges
                    foreach (CalendarDateRange newRange in SelectedDateRanges)
                    {
                        newRange.CalendarPropertyChanged += OnSelectedRangesPropertyChanged;
                    }

                    break;

                default:
                    break;
            }

            _selectedDateRanges = CalendarViewHelper.CloneSelectedRanges(SelectedDateRanges);
            ReadOnlyObservableCollection<CalendarDateRange> newSelectedRanges = new ReadOnlyObservableCollection<CalendarDateRange>(_selectedDateRanges);
            var selectionChangedEventArgs = new CalendarSelectionChangedEventArgs { NewValue = newSelectedRanges, OldValue = oldSelectedRanges };
            if (SelectionChanged != null)
            {
                SelectionChanged?.Invoke(this, selectionChangedEventArgs);
            }

            if (SelectionChangedCommand != null && SelectionChangedCommand.CanExecute(selectionChangedEventArgs))
            {
                SelectionChangedCommand.Execute(selectionChangedEventArgs);
            }

            _customScrollLayout?.UpdateMultiRangeSelectionValue();
        }

        /// <summary>
        /// Method invokes on header settings property changed.
        /// </summary>
        /// <param name="sender">The calendar object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnHeaderSettingsPropertyChanged(object? sender, CalendarPropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CalendarHeaderView.Height))
            {
                //// Check the property changed triggered after the view rendered.
                if (_customScrollLayout == null)
                {
                    return;
                }

                AddOrRemoveHeaderLayout();
                _layout.UpdateHeaderHeight(HeaderView.GetHeaderHeight());
                UpdatePopUpSize();
            }
            else if (e.PropertyName == nameof(CalendarHeaderView.TextFormat))
            {
                if (_headerLayout == null)
                {
                    return;
                }

                _headerLayout.UpdateHeaderText();
            }
            else if (e.PropertyName == nameof(CalendarHeaderView.Background))
            {
                if (_headerLayout == null)
                {
                    return;
                }

                _headerLayout.Background = HeaderView.Background;
            }
            else if (e.PropertyName == nameof(CalendarHeaderView.TextStyle))
            {
                CalendarTextStyle? oldStyle = e.OldValue as CalendarTextStyle;
                if (oldStyle != null)
                {
                    oldStyle.BindingContext = null;
                    oldStyle.PropertyChanged -= OnHeaderTextStylePropertyChanged;
                }

                if (HeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(HeaderView.TextStyle, BindingContext);
                    HeaderView.TextStyle.PropertyChanged += OnHeaderTextStylePropertyChanged;
                }

                if (_headerLayout == null)
                {
                    return;
                }

                _headerLayout.UpdateHeaderTextStyle();
            }
            else if (e.PropertyName == nameof(CalendarHeaderView.ShowNavigationArrows))
            {
                if (_headerLayout == null)
                {
                    return;
                }

                _headerLayout.InvalidateNavigationArrowVisibility();
            }
        }

        /// <summary>
        /// Method invokes on footer settings property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The calendar property event args.</param>
        void OnFooterSettingsPropertyChanged(object? sender, CalendarPropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CalendarFooterView.Background))
            {
                if (_footerLayout == null)
                {
                    return;
                }

                _footerLayout.Background = FooterView.Background;
                _footerLayout.InvalidateDrawable();
                _layout.TriggerInvalidateMeasure();
            }
            else if (e.PropertyName == nameof(CalendarFooterView.Height))
            {
                AddOrRemoveFooterLayout();
                _layout.UpdateFooterHeight(FooterView.GetFooterHeight());
                UpdatePopUpSize();
                if (_availableSize == Size.Zero)
                {
                    return;
                }
            }
            else if (e.PropertyName == nameof(CalendarFooterView.DividerColor))
            {
                if (_footerLayout == null || _availableSize == Size.Zero)
                {
                    return;
                }

                _footerLayout.UpdateSeparatorColor();
            }
            else if (e.PropertyName == nameof(CalendarFooterView.ShowActionButtons))
            {
                AddOrRemoveFooterLayout();
#if ANDROID
                _layout.TriggerInvalidateMeasure();
#endif
            }
            else if (e.PropertyName == nameof(CalendarFooterView.ShowTodayButton))
            {
                AddOrRemoveFooterLayout();
#if ANDROID
                _layout.TriggerInvalidateMeasure();
#endif
            }
            else if (e.PropertyName == nameof(CalendarFooterView.TextStyle))
            {
                CalendarTextStyle? oldStyle = e.OldValue as CalendarTextStyle;
                if (oldStyle != null)
                {
                    oldStyle.PropertyChanged -= OnFooterTextStylePropertyChanged;
                    oldStyle.BindingContext = null;
                }

                if (FooterView.TextStyle != null)
                {
                    SetInheritedBindingContext(FooterView.TextStyle, BindingContext);
                    FooterView.TextStyle.PropertyChanged += OnFooterTextStylePropertyChanged;
                }

                if (_footerLayout == null)
                {
                    return;
                }

                _footerLayout.UpdateButtonTextStyle();
            }
        }

        /// <summary>
        /// Method invokes on footer text style property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void OnFooterTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _footerLayout?.UpdateButtonTextStyle();
        }

        /// <summary>
        /// Method invokes on header text style property changed.
        /// </summary>
        /// <param name="sender">The calendar object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnHeaderTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _headerLayout?.UpdateHeaderTextStyle();
        }

        /// <summary>
        /// Method invokes on month view settings property changed.
        /// </summary>
        /// <param name="sender">The calendar month view object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnMonthViewPropertyChanged(object? sender, CalendarPropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CalendarMonthView.NumberOfVisibleWeeks) || e.PropertyName == nameof(CalendarMonthView.FirstDayOfWeek))
            {
                _customScrollLayout?.UpdateVisibleDateOnView();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.WeekNumberStyle))
            {
                CalendarWeekNumberStyle? oldstyle = e.OldValue as CalendarWeekNumberStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.CalendarPropertyChanged -= OnWeekNumberStylePropertyChanged;
                    if (oldstyle.TextStyle != null)
                    {
                        oldstyle.TextStyle.BindingContext = null;
                        oldstyle.TextStyle.PropertyChanged -= OnWeekNumberTextStyleChanged;
                    }
                }

                SetInheritedBindingContext(MonthView.WeekNumberStyle, BindingContext);
                MonthView.WeekNumberStyle.CalendarPropertyChanged += OnWeekNumberStylePropertyChanged;
                if (MonthView.WeekNumberStyle.TextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.WeekNumberStyle.TextStyle, BindingContext);
                    MonthView.WeekNumberStyle.TextStyle.PropertyChanged += OnWeekNumberTextStyleChanged;
                }

                if (!MonthView.ShowWeekNumber || View != CalendarView.Month || _customScrollLayout == null)
                {
                    return;
                }

                _customScrollLayout.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.TextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnMonthViewTextStyleChanged;
                }

                if (MonthView.TextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.TextStyle, BindingContext);
                    MonthView.TextStyle.PropertyChanged += OnMonthViewTextStyleChanged;
                }

                if (View != CalendarView.Month || _customScrollLayout == null || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.Background))
            {
                if (View != CalendarView.Month || _customScrollLayout == null || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.TrailingLeadingDatesTextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnMonthTrailingLeadingDatesTextStyleChanged;
                }

                if (MonthView.TrailingLeadingDatesTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.TrailingLeadingDatesTextStyle, BindingContext);
                    MonthView.TrailingLeadingDatesTextStyle.PropertyChanged += OnMonthTrailingLeadingDatesTextStyleChanged;
                }

                if (MonthView.GetNumberOfWeeks() != 6 || _customScrollLayout == null || View != CalendarView.Month || !ShowTrailingAndLeadingDates || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(MonthView.TrailingLeadingDatesBackground))
            {
                if (MonthView.GetNumberOfWeeks() != 6 || _customScrollLayout == null || View != CalendarView.Month || !ShowTrailingAndLeadingDates || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.TodayTextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnMonthViewTodayStylePropertyChanged;
                }

                if (MonthView.TodayTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.TodayTextStyle, BindingContext);
                    MonthView.TodayTextStyle.PropertyChanged += OnMonthViewTodayStylePropertyChanged;
                }

                if (View != CalendarView.Month || _customScrollLayout == null || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout.UpdateTodayCellStyle();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.TodayBackground))
            {
                if (View != CalendarView.Month || _customScrollLayout == null || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout.UpdateTodayCellStyle();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.WeekendDatesTextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnMonthWeekendsDatesTextStylePropertyChanged;
                }

                if (MonthView.WeekendDatesTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.WeekendDatesTextStyle, BindingContext);
                    MonthView.WeekendDatesTextStyle.PropertyChanged += OnMonthWeekendsDatesTextStylePropertyChanged;
                }

                if (MonthView.WeekendDays.Count == 0 || _customScrollLayout == null || View != CalendarView.Month || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.WeekendDatesBackground))
            {
                if (MonthView.WeekendDays.Count == 0 || _customScrollLayout == null || View != CalendarView.Month || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.DisabledDatesTextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnMonthDisabledDatesTextStylePropertyChanged;
                }

                if (MonthView.DisabledDatesTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.DisabledDatesTextStyle, BindingContext);
                    MonthView.DisabledDatesTextStyle.PropertyChanged += OnMonthDisabledDatesTextStylePropertyChanged;
                }

                if (View != CalendarView.Month || _customScrollLayout == null || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.DisabledDatesBackground) || e.PropertyName == nameof(CalendarMonthView.SpecialDatesBackground))
            {
                if (View != CalendarView.Month || _customScrollLayout == null || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.SpecialDatesTextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnSpecialDatesStylePropertyChanged;
                }

                if (MonthView.SpecialDatesTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.SpecialDatesTextStyle, BindingContext);
                    MonthView.SpecialDatesTextStyle.PropertyChanged += OnSpecialDatesStylePropertyChanged;
                }

                if (View != CalendarView.Month || _customScrollLayout == null || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.RangeTextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnMonthViewRangeTextStylePropertyChanged;
                }

                if (MonthView.RangeTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.RangeTextStyle, BindingContext);
                    MonthView.RangeTextStyle.PropertyChanged += OnMonthViewRangeTextStylePropertyChanged;
                }

                if (View != CalendarView.Month || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout?.UpdateInBetweenRangeStyle();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.SelectionTextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnMonthViewSelectionTextStylePropertyChanged;
                }

                if (MonthView.SelectionTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.SelectionTextStyle, BindingContext);
                    MonthView.SelectionTextStyle.PropertyChanged += OnMonthViewSelectionTextStylePropertyChanged;
                }

                if (View != CalendarView.Month || MonthView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout?.UpdateSelectedCellStyle();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.HeaderView))
            {
                CalendarMonthHeaderView? oldStyle = e.OldValue as CalendarMonthHeaderView;
                if (oldStyle != null)
                {
                    oldStyle.BindingContext = null;
                    oldStyle.CalendarPropertyChanged -= OnMonthHeaderViewPropertyChanged;
                    if (oldStyle.TextStyle != null)
                    {
                        oldStyle.TextStyle.BindingContext = null;
                        oldStyle.TextStyle.PropertyChanged -= OnMonthHeaderViewTextStylePropertyChanged;
                    }
                }

                SetInheritedBindingContext(MonthView.HeaderView, BindingContext);
                MonthView.HeaderView.CalendarPropertyChanged += OnMonthHeaderViewPropertyChanged;
                if (MonthView.HeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.HeaderView.TextStyle, BindingContext);
                    MonthView.HeaderView.TextStyle.PropertyChanged += OnMonthHeaderViewTextStylePropertyChanged;
                }

                if (View != CalendarView.Month || _customScrollLayout == null)
                {
                    return;
                }

                AddOrRemoveMonthHeaderView();
                if (NavigationDirection == CalendarNavigationDirection.Horizontal)
                {
                    _customScrollLayout.UpdateMonthViewHeader();
                    _customScrollLayout.InvalidateMonthHeaderDraw();
                }
                else
                {
                    _layout.UpdateViewHeaderHeight(MonthView.HeaderView.GetViewHeaderHeight());
                    _monthViewHeader?.InvalidateDrawMonthHeaderView();
                }

                //// While the week number visibility is changed need to update the semantic node both view header and month view. Because of in month view and view header virtual semantic nodes considered the week number visibility.
                _monthViewHeader?.InvalidateSemanticsNode(true);
                _customScrollLayout.UpdateSemanticNodes();
            }
            else if (e.PropertyName == nameof(MonthView.ShowWeekNumber))
            {
                if (View != CalendarView.Month)
                {
                    return;
                }

                //// Need to update the month cell views and hovering view also.
                _customScrollLayout?.InvalidateViewDraw();
                //// Need to measure the template view based on the week number view width.
                if (MonthView.CellTemplate != null)
                {
                    _customScrollLayout?.InvalidateMonthViewMeasure();
                }

                if (NavigationDirection == CalendarNavigationDirection.Horizontal)
                {
                    _customScrollLayout?.InvalidateMonthHeaderDraw();
                }
                else
                {
                    _monthViewHeader?.InvalidateDrawMonthHeaderView();
                }

                //// While the week number visibility is changed need to update the semantic node both view header and month view. Because of in month view and view header virtual semantic nodes considered the week number visibility.
                _monthViewHeader?.InvalidateSemanticsNode(true);
                _customScrollLayout?.UpdateSemanticNodes();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.SpecialDayPredicate))
            {
                if (View != CalendarView.Month)
                {
                    return;
                }

                _customScrollLayout?.CreateOrResetSpecialDates();
            }
            else if (e.PropertyName == nameof(CalendarMonthView.CellTemplate))
            {
                if (View != CalendarView.Month)
                {
                    return;
                }

                _customScrollLayout?.UpdateTemplateViews();
            }
        }

        /// <summary>
        /// Method invokes on year view settings property changed.
        /// </summary>
        /// <param name="sender">The calendar object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnYearViewSettingsPropertyChanged(object? sender, CalendarPropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CalendarYearView.TextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnYearCellTextStylePropertyChanged;
                }

                if (YearView.TextStyle != null)
                {
                    SetInheritedBindingContext(YearView.TextStyle, BindingContext);
                    YearView.TextStyle.PropertyChanged += OnYearCellTextStylePropertyChanged;
                }

                if (View == CalendarView.Month || YearView.CellTemplate != null)
                {
                    return;
                }

                //// Need to redraw the year hover view also because hover view highlight calculated based on year cell text style.
                _customScrollLayout?.InvalidateViewDraw();
            }
            else if (e.PropertyName == nameof(CalendarYearView.Background))
            {
                if (View == CalendarView.Month || YearView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout?.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarYearView.TodayTextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnTodayYearCellStylePropertyChanged;
                }

                if (YearView.TodayTextStyle != null)
                {
                    SetInheritedBindingContext(YearView.TodayTextStyle, BindingContext);
                    YearView.TodayTextStyle.PropertyChanged += OnTodayYearCellStylePropertyChanged;
                }

                if (View == CalendarView.Month || YearView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout?.UpdateTodayCellStyle();
            }
            else if (e.PropertyName == nameof(CalendarYearView.TodayBackground))
            {
                if (View == CalendarView.Month || YearView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout?.UpdateTodayCellStyle();
            }
            else if (e.PropertyName == nameof(CalendarYearView.LeadingDatesTextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnLeadingYearCellTextStyleSettingsChanged;
                }

                if (YearView.LeadingDatesTextStyle != null)
                {
                    SetInheritedBindingContext(YearView.LeadingDatesTextStyle, BindingContext);
                    YearView.LeadingDatesTextStyle.PropertyChanged += OnLeadingYearCellTextStyleSettingsChanged;
                }

                // The leading dates text style not applicable for year view. So no need to update the year view.
                if (View == CalendarView.Year || !ShowTrailingAndLeadingDates || View == CalendarView.Month || YearView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout?.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarYearView.LeadingDatesBackground))
            {
                // The leading dates not applicable for year view. So no need to update the year view.
                if (View == CalendarView.Year || !ShowTrailingAndLeadingDates || View == CalendarView.Month || YearView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout?.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarYearView.DisabledDatesTextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnYearCellTextStylePropertyChanged;
                }

                if (YearView.DisabledDatesTextStyle != null)
                {
                    SetInheritedBindingContext(YearView.DisabledDatesTextStyle, BindingContext);
                    YearView.DisabledDatesTextStyle.PropertyChanged += OnYearCellTextStylePropertyChanged;
                }

                if (View == CalendarView.Month || YearView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout?.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarYearView.DisabledDatesBackground))
            {
                if (View == CalendarView.Month || YearView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout?.InvalidateViewCellsDraw();
            }
            else if (e.PropertyName == nameof(CalendarYearView.RangeTextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnYearCellRangeTextStylePropertyChanged;
                }

                if (YearView.RangeTextStyle != null)
                {
                    SetInheritedBindingContext(YearView.RangeTextStyle, BindingContext);
                    YearView.RangeTextStyle.PropertyChanged += OnYearCellRangeTextStylePropertyChanged;
                }

                if (View == CalendarView.Month || YearView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout?.UpdateInBetweenRangeStyle();
            }
            else if (e.PropertyName == nameof(CalendarYearView.SelectionTextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnYearCellSelectionTextStylePropertyChanged;
                }

                if (YearView.SelectionTextStyle != null)
                {
                    SetInheritedBindingContext(YearView.SelectionTextStyle, BindingContext);
                    YearView.SelectionTextStyle.PropertyChanged += OnYearCellSelectionTextStylePropertyChanged;
                }

                if (View == CalendarView.Month || YearView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout?.UpdateSelectedCellStyle();
            }
            else if (e.PropertyName == nameof(CalendarYearView.MonthFormat))
            {
                if (View != CalendarView.Year || _customScrollLayout == null || YearView.CellTemplate != null)
                {
                    return;
                }

                _customScrollLayout?.InvalidateViewDraw();
            }
            else if (e.PropertyName == nameof(CalendarYearView.CellTemplate))
            {
                if (View == CalendarView.Month)
                {
                    return;
                }

                _customScrollLayout?.UpdateTemplateViews();
            }
        }

        /// <summary>
        /// Method invokes on week number text style property changed.
        /// </summary>
        /// <param name="sender">The calendar week number style object.</param>
        /// <param name="e">The calendar property changed event arguments.</param>
        void OnWeekNumberStylePropertyChanged(object? sender, CalendarPropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CalendarWeekNumberStyle.TextStyle))
            {
                CalendarTextStyle? oldstyle = e.OldValue as CalendarTextStyle;
                if (oldstyle != null)
                {
                    oldstyle.BindingContext = null;
                    oldstyle.PropertyChanged -= OnWeekNumberTextStyleChanged;
                }

                if (MonthView.WeekNumberStyle.TextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.WeekNumberStyle.TextStyle, BindingContext);
                    MonthView.WeekNumberStyle.TextStyle.PropertyChanged += OnWeekNumberTextStyleChanged;
                }
            }

            if (!MonthView.ShowWeekNumber || View != CalendarView.Month || _customScrollLayout == null)
            {
                return;
            }

            _customScrollLayout.InvalidateViewCellsDraw();
        }

        /// <summary>
        /// Method to update the week number text style property changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnWeekNumberTextStyleChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!MonthView.ShowWeekNumber || View != CalendarView.Month || _customScrollLayout == null)
            {
                return;
            }

            _customScrollLayout?.InvalidateViewCellsDraw();
        }

        /// <summary>
        /// Method invokes while the range selection text style changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnMonthViewRangeTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (View != CalendarView.Month || _customScrollLayout == null)
            {
                return;
            }

            _customScrollLayout?.UpdateInBetweenRangeStyle();
        }

        /// <summary>
        /// Method invokes while the month view today style settings property changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnMonthViewTodayStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (View != CalendarView.Month || _customScrollLayout == null)
            {
                return;
            }

            _customScrollLayout?.UpdateTodayCellStyle();
        }

        /// <summary>
        /// Method invokes while the month view disabled dates text style property changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnMonthDisabledDatesTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (View != CalendarView.Month || _customScrollLayout == null)
            {
                return;
            }

            _customScrollLayout.InvalidateViewCellsDraw();
        }

        /// <summary>
        /// Method invokes while the month view week end days text style property changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnMonthWeekendsDatesTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (MonthView.WeekendDays.Count == 0 || _customScrollLayout == null || View != CalendarView.Month)
            {
                return;
            }

            _customScrollLayout.InvalidateViewCellsDraw();
        }

        /// <summary>
        /// Method invokes while the month view text style settings property changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnMonthViewTextStyleChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (View != CalendarView.Month || _customScrollLayout == null)
            {
                return;
            }

            _customScrollLayout.InvalidateViewCellsDraw();
        }

        /// <summary>
        /// Method invokes while the month view special dates text style settings property changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnSpecialDatesStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (View != CalendarView.Month || _customScrollLayout == null)
            {
                return;
            }

            _customScrollLayout.InvalidateViewCellsDraw();
        }

        /// <summary>
        /// Method to update the TrailingLeadingDatesTextStyle changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnMonthTrailingLeadingDatesTextStyleChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (MonthView.GetNumberOfWeeks() != 6 || _customScrollLayout == null || View != CalendarView.Month || !ShowTrailingAndLeadingDates)
            {
                return;
            }

            _customScrollLayout.InvalidateViewCellsDraw();
        }

        /// <summary>
        /// Method invokes while the month view header settings changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnMonthHeaderViewTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (View != CalendarView.Month)
            {
                return;
            }

            if (NavigationDirection == CalendarNavigationDirection.Horizontal)
            {
                _customScrollLayout?.InvalidateMonthHeaderDraw();
            }
            else
            {
                _monthViewHeader?.InvalidateDrawMonthHeaderView();
            }
        }

        /// <summary>
        /// Method invokes while the month view header settings changed.
        /// </summary>
        /// <param name="sender">The calendar header view object.</param>
        /// <param name="e">The calendar property changed event arguments.</param>
        void OnMonthHeaderViewPropertyChanged(object? sender, CalendarPropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MonthView.HeaderView.Height))
            {
                if (_customScrollLayout == null || View != CalendarView.Month)
                {
                    return;
                }

                if (NavigationDirection == CalendarNavigationDirection.Vertical)
                {
                    AddOrRemoveMonthHeaderView();
                    _layout.UpdateViewHeaderHeight(MonthView.HeaderView.GetViewHeaderHeight());
                }
                else
                {
                    _customScrollLayout?.UpdateMonthViewHeader();
                }
            }
            else if (e.PropertyName == nameof(MonthView.HeaderView.TextStyle))
            {
                CalendarTextStyle? oldStyle = e.OldValue as CalendarTextStyle;
                if (oldStyle != null)
                {
                    oldStyle.BindingContext = null;
                    oldStyle.PropertyChanged -= OnMonthHeaderViewTextStylePropertyChanged;
                }

                if (MonthView?.HeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.HeaderView.TextStyle, BindingContext);
                    MonthView.HeaderView.TextStyle.PropertyChanged += OnMonthHeaderViewTextStylePropertyChanged;
                }

                if (_customScrollLayout == null || View != CalendarView.Month)
                {
                    return;
                }

                if (NavigationDirection == CalendarNavigationDirection.Horizontal)
                {
                    _customScrollLayout.InvalidateMonthHeaderDraw();
                }
                else
                {
                    _monthViewHeader?.InvalidateDrawMonthHeaderView();
                }
            }
            else
            {
                if (_customScrollLayout == null || View != CalendarView.Month)
                {
                    return;
                }

                if (NavigationDirection == CalendarNavigationDirection.Horizontal)
                {
                    _customScrollLayout.InvalidateMonthHeaderDraw();
                }
                else
                {
                    _monthViewHeader?.InvalidateDrawMonthHeaderView();
                }
            }
        }

        /// <summary>
        /// Method invokes the year view range text style property changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnYearCellRangeTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_customScrollLayout == null || View == CalendarView.Month)
            {
                return;
            }

            _customScrollLayout.UpdateInBetweenRangeStyle();
        }

        /// <summary>
        /// Method invokes the month view selection text style property changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnMonthViewSelectionTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (View != CalendarView.Month)
            {
                return;
            }

            _customScrollLayout?.UpdateSelectedCellStyle();
        }

        /// <summary>
        /// Method invokes the year view selection text style property changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnYearCellSelectionTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_customScrollLayout == null || View == CalendarView.Month)
            {
                return;
            }

            _customScrollLayout.UpdateSelectedCellStyle();
        }

        /// <summary>
        /// Method invokes on year property changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnYearCellTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_customScrollLayout == null || View == CalendarView.Month)
            {
                return;
            }

            //// Need to redraw the year hover view also because hover view highlight calculated based on year cell text style.
            _customScrollLayout.InvalidateViewDraw();
        }

        /// <summary>
        /// Method invokes on year today text style property changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnTodayYearCellStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_customScrollLayout == null || View == CalendarView.Month)
            {
                return;
            }

            _customScrollLayout.UpdateTodayCellStyle();
        }

        /// <summary>
        /// Method to update the leading date text style changed.
        /// </summary>
        /// <param name="sender">The calendar text style object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnLeadingYearCellTextStyleSettingsChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_customScrollLayout == null || View == CalendarView.Year || !ShowTrailingAndLeadingDates || View == CalendarView.Month)
            {
                return;
            }

            _customScrollLayout.InvalidateViewCellsDraw();
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// This method is declared only in IParentThemeElement
        /// and you need to implement this method only in main control.
        /// </summary>
        /// <returns>ResourceDictionary</returns>
        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfCalendarStyles();
        }

        /// <summary>
        /// This method will be called when users merge a theme dictionary
        /// that contains value for �SyncfusionTheme� dynamic resource key.
        /// </summary>
        /// <param name="oldTheme">Old theme.</param>
        /// <param name="newTheme">New theme.</param>
        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
            SetDynamicResource(RangeSelectionColorProperty, "SfCalendarRangeSelectionColor");
            SetDynamicResource(HoverTextColorProperty, "SfCalendarHoverColor");
            SetDynamicResource(HeaderHoverTextColorProperty, "SfCalendarHeaderHoverTextColor");
            SetDynamicResource(ButtonTextColorProperty, "SfCalendarButtonTextColor");
            SetDynamicResource(CalendarBackgroundProperty, "SfCalendarNormalBackground");
        }

        /// <summary>
        /// This method will be called when a theme dictionary
        /// that contains the value for your control key is merged in application.
        /// </summary>
        /// <param name="oldTheme">The old value.</param>
        /// <param name="newTheme">The new value.</param>
        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        #endregion
    }

    #region Internal Class

    /// <summary>
    /// Represents the class calendar control instance.
    /// </summary>
    internal class SfCalendarProxy
    {
        #region Fields

        /// <summary>
        /// The calendar.
        /// </summary>
        readonly WeakReference<SfCalendar> _calendar;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfCalendarProxy"/> class.
        /// </summary>
        /// <param name="calendar">the calendar.</param>
        internal SfCalendarProxy(SfCalendar calendar)
        {
            _calendar = new(calendar);
        }

        #endregion

        #region Internal Method

        /// <summary>
        /// Method to update the toolbar settings.
        /// </summary>
        /// <param name="sender">The toolbar settings</param>
        /// <param name="e">The event args</param>
        internal void OnSelectedDatesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _calendar.TryGetTarget(out var view);
            view?.OnSelectedDatesCollectionChanged(sender, e);
        }

        #endregion
    }

    #endregion
}