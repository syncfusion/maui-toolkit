using System.Collections.Specialized;
using Globalization = System.Globalization;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which holds the information of looping panel.
    /// </summary>
    internal partial class CustomSnapLayout
    {
        #region Fields

        /// <summary>
        /// The calendar view info.
        /// </summary>
        ICalendar _calendarInfo;

        /// <summary>
        /// Holds the current calendar view and it used to pass the value for view changed
        /// event previous view argument. Its value updated when ever visible dates calculated.
        /// </summary>
        CalendarView _currentCalendarView;

        /// <summary>
        /// Visible dates collection.
        /// </summary>
        List<DateTime> _visibleDates = new List<DateTime>();

        /// <summary>
        /// The current visible date collections.
        /// </summary>
        List<DateTime> _currentViewVisibleDates = new List<DateTime>();

        /// <summary>
        /// The previous visible date collections.
        /// </summary>
        List<DateTime> _previousViewVisibleDates = new List<DateTime>();

        /// <summary>
        /// The next visible date collections.
        /// </summary>
        List<DateTime> _nextViewVisibleDates = new List<DateTime>();

        /// <summary>
        /// Dictionary which hold the disabled dates only for the current visible dates by removing next or previous view disabled dates.
        /// It hold the current view disabled dates by removing the key values which are not equal to the current visible dates.
        /// If dictionary is not used then the disabled dates is calculated repeatedly for same visible dates while moving/running action on swiping.
        /// </summary>
        Dictionary<List<DateTime>, List<DateTime>> _disabledDatesDictionary = new Dictionary<List<DateTime>, List<DateTime>>();

        /// <summary>
        /// Dictionary which hold the special dates only for the current visible dates by removing next or previous view special dates.
        /// It hold the current view special dates by removing the key values which are not equal to the current visible dates.
        /// If dictionary is not used then the special dates is calculated repeatedly for same visible dates while moving/running action on swiping.
        /// </summary>
        Dictionary<List<DateTime>, List<CalendarIconDetails>> _specialDatesDictionary = new Dictionary<List<DateTime>, List<CalendarIconDetails>>();

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to update selected date in each layout, this will update the selected date for current view.
        /// </summary>
        internal void UpdateSelection()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as ICalendarView)?.UpdateSelectionValue();
            }
        }

        /// <summary>
        /// To update the view when the tapped event is triggered
        /// </summary>
        /// <param name="oldValue">The old value of the view</param>
        /// <param name="newValue">The new value of the view</param>
        internal void UpdateViewChange(CalendarView oldValue, CalendarView newValue)
        {
            // Swiping is performed when switch the view, currentChildIndex is changed. So, need to update the currentChildIndex as 1
            _currentChildIndex = 1;

            // If the oldValue is month view need to clear the month view children
            // Then add the year view(newValue) children and then update the visible dates change
            if (oldValue == CalendarView.Month)
            {
                Children.Clear();
            }

            // If the newValue is month view need to clear the year view(oldValue) children
            // Then add the month view children and then update the visible dates change
            else if (newValue == CalendarView.Month)
            {
                Children.Clear();
            }
            else
            {
                // If the oldValue is year view children and newValue is also year view children
                // no need to clear the children just update the visible dates change for the year view panel
                CalculateVisibleDateOnView();
            }

            // While switching the views the current child index any of the following one. 0,1,2.
            // The default current child index is 1. While switching year to month or month to year we need to set the current child index=1.
            // To rearrange the current view based on the current child index. So, we need to triggered measure and arrange method manually.
#if __ANDROID__
            this.TriggerInvalidateMeasure();
#elif __IOS__ || __MACCATALYST__
            InvalidateMeasure();
            this.TriggerInvalidateMeasure();
#else
            InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Method to update selected date range in each layout, this will update the selected date range for current view.
        /// </summary>
        internal void UpdateSelectionRange()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as ICalendarView)?.UpdateSelectedRangeValue();
            }
        }

        /// <summary>
        /// Method to update selected multi date ranges in each layout, this will update the selected multi date ranges for current view.
        /// </summary>
        internal void UpdateMultiRangeSelectionValue()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as ICalendarView)?.UpdateSelectedMultiRangesValue();
            }
        }

        /// <summary>
        /// Method to update selected dates based on the action.
        /// </summary>
        /// <param name="e">The collection changed event arguments.</param>
        internal void UpdateSelectedDatesOnAction(NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as ICalendarView)?.UpdateSelectedDatesOnAction(e);
            }
        }

        /// <summary>
        /// Method to update view when leading and trailing dates enabled or disabled.
        /// </summary>
        internal void UpdateShowLeadingDates()
        {
            CreateOrResetSpecialAndDisableDates();
            for (int i = 0; i < Children.Count; i++)
            {
                //// Need to update the cell template when the show leading and trailing dates is changed.
                (Children[i] as ICalendarView)?.UpdateTemplateViews(i == _currentChildIndex);
            }
        }

        /// <summary>
        /// Create or reset the special and disable dates for the visible dates.
        /// </summary>
        internal void CreateOrResetSpecialAndDisableDates()
        {
            _disabledDatesDictionary.Clear();
            _specialDatesDictionary.Clear();
            UpdateSpecialAndDisableDates(_visibleDates);
        }

        /// <summary>
        /// Create or reset the disable dates for the visible dates.
        /// </summary>
        internal void CreateOrResetDisableDates()
        {
            _disabledDatesDictionary.Clear();
            List<DateTime>? disabledDates = UpdateDisabledDates(_visibleDates);
            //// Return null means the dictionary already have a visible dates.
            if (disabledDates == null)
            {
                return;
            }

            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as ICalendarView)?.UpdateDisableAndSpecialDateChange(disabledDates, null, _visibleDates);
                (Children[i] as MonthViewLayout)?.UpdateMonthViewSemanticNode(i == _currentChildIndex);
                (Children[i] as YearView)?.InvalidateSemanticsNode(i == _currentChildIndex);
            }
        }

        /// <summary>
        /// Create or reset the special dates for the visible dates.
        /// </summary>
        internal void CreateOrResetSpecialDates()
        {
            _specialDatesDictionary.Clear();
            List<CalendarIconDetails>? specialDates = UpdateSpecialDates(_visibleDates);
            //// Return null means the dictionary already have a visible dates.
            if (specialDates == null)
            {
                return;
            }

            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as ICalendarView)?.UpdateDisableAndSpecialDateChange(null, specialDates, _visibleDates);
                (Children[i] as MonthViewLayout)?.UpdateMonthViewSemanticNode(i == _currentChildIndex);
            }
        }

        /// <summary>
        /// Method to update the selected dates.
        /// </summary>
        internal void UpdateSelectedDates()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as ICalendarView)?.UpdateSelectedDates();
            }
        }

        /// <summary>
        /// Method to update the moth view header view draw.
        /// </summary>
        internal void InvalidateMonthHeaderDraw()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as MonthViewLayout)?.InvalidateDrawMonthHeaderView();
            }
        }

        /// <summary>
        /// Method to update the month view layout.
        /// </summary>
        internal void InvalidateMonthViewLayout()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as MonthViewLayout)?.InvalidateMonthLayout();
            }
        }

        /// <summary>
        /// Method to update the month header while height or navigation direction changes.
        /// </summary>
        internal void UpdateMonthViewHeader()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as MonthViewLayout)?.AddOrRemoveViewHeader(i == _currentChildIndex);
            }
        }

        /// <summary>
        /// Method to update calendar view.
        /// </summary>
        internal void InvalidateViewCellsDraw()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as ICalendarView)?.InvalidateViewCells();
            }
        }

        /// <summary>
        /// Method to update calendar view and hover view.
        /// </summary>
        internal void InvalidateViewDraw()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as ICalendarView)?.InvalidateView();
            }
        }

        /// <summary>
        /// Method to update the template views.
        /// </summary>
        internal void UpdateTemplateViews()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as ICalendarView)?.UpdateTemplateViews(i == _currentChildIndex);
            }
        }

        /// <summary>
        /// Invalidate the month view.
        /// </summary>
        internal void InvalidateMonthViewMeasure()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as MonthViewLayout)?.InvalidateMonthViewMeasure();
            }
        }

        /// <summary>
        /// Update the views before the today date while enable past dates changes.
        /// </summary>
        internal void UpdateEnablePastDates()
        {
            DateTime? date = CalendarViewHelper.GetStartDate(DateTime.Now.Date, _calendarInfo.View, _calendarInfo.Identifier);
            DateTime todayDate = date != null ? date.Value : DateTime.Now.Date;
            int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
            bool isMonthView = _calendarInfo.View == CalendarView.Month && numberOfWeeks == 6;
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarInfo.Identifier.ToString());
            DateTime minDate = cultureCalendar.MinSupportedDateTime;
            int minYear = cultureCalendar.GetYear(minDate);
            int minMonth = cultureCalendar.GetMonth(minDate);
            for (int i = 0; i < Children.Count; i++)
            {
                ICalendarView child = (ICalendarView)Children[i];
                List<DateTime> dates = GetVisibleDatesForView(i);
                DateTime visibleStartDate = dates[0];
                //// Does not need to calculate the visible end date for year view while show trailing and leading dates disabled
                //// because the year view does not have trailing and leading dates.
                if (isMonthView && !_calendarInfo.ShowTrailingAndLeadingDates)
                {
                    DateTime visibleDate = dates[dates.Count / 2];
                    int year = cultureCalendar.GetYear(visibleDate);
                    int month = cultureCalendar.GetMonth(visibleDate);
                    visibleStartDate = minYear == year && minMonth == month ? minDate : new DateTime(year, month, 1, cultureCalendar);
                }

                //// Update the view while the view have before today date.
                if (todayDate.Date > visibleStartDate.Date)
                {
                    child.InvalidateView();
                    (Children[i] as MonthViewLayout)?.UpdateMonthViewSemanticNode(i == _currentChildIndex);
                    (Children[i] as YearView)?.InvalidateSemanticsNode(i == _currentChildIndex);
                }
            }
        }

        /// <summary>
        /// Method to update the today date text style and background.
        /// </summary>
        internal void UpdateTodayCellStyle()
        {
            DateTime? date = CalendarViewHelper.GetStartDate(DateTime.Now.Date, _calendarInfo.View, _calendarInfo.Identifier);
            DateTime todayDate = date != null ? date.Value : DateTime.Now.Date;
            int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
            bool isMonthView = _calendarInfo.View == CalendarView.Month && numberOfWeeks == 6;
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarInfo.Identifier.ToString());
            DateTime minDate = cultureCalendar.MinSupportedDateTime;
            int minYear = cultureCalendar.GetYear(minDate);
            int minMonth = cultureCalendar.GetMonth(minDate);
            for (int i = 0; i < Children.Count; i++)
            {
                ICalendarView child = (ICalendarView)Children[i];
                List<DateTime> dates = GetVisibleDatesForView(i);
                DateTime visibleStartDate = dates[0];
                DateTime visibleEndDate = dates[dates.Count - 1];
                //// Does not need to calculate the visible end date for year view while show trailing and leading dates disabled
                //// because the year view does not have trailing and leading dates.
                if (_calendarInfo.View != CalendarView.Year && !_calendarInfo.ShowTrailingAndLeadingDates)
                {
                    if (isMonthView)
                    {
                        DateTime visibleDate = dates[dates.Count / 2];
                        int year = cultureCalendar.GetYear(visibleDate);
                        int month = cultureCalendar.GetMonth(visibleDate);
                        visibleStartDate = minYear == year && minMonth == month ? minDate : new DateTime(year, month, 1, cultureCalendar);
                    }

                    //// Calculate the view end date other than month view with number of weeks specified.
                    //// if the visible start date is 2020 then the visible end date is assigned to 2029 on decade view.
                    visibleEndDate = _calendarInfo.View == CalendarView.Month && numberOfWeeks != 6 ? visibleEndDate : CalendarViewHelper.GetViewLastDate(_calendarInfo.View, visibleStartDate, _calendarInfo.Identifier);
                }

                //// Update the view while the view have today date.
                if (CalendarViewHelper.IsDateWithinDateRange(todayDate, visibleStartDate, visibleEndDate))
                {
                    child.InvalidateViewCells();
                }
            }
        }

        /// <summary>
        /// Method to update the range text style and background.
        /// </summary>
        internal void UpdateInBetweenRangeStyle()
        {
            //// Skip the view update while the selection mode other than range or selected range is empty.
            if (_calendarInfo.SelectionMode != CalendarSelectionMode.Range || _calendarInfo.SelectedDateRange == null || (_calendarInfo.SelectedDateRange.StartDate == null && _calendarInfo.SelectedDateRange.EndDate == null))
            {
                return;
            }

            //// Skip the view update while the allow view navigation is enabled and the view is not month view.
            //// Because other than month view, views show selection when the allow view navigation disabled.
            if (_calendarInfo.View != CalendarView.Month && _calendarInfo.AllowViewNavigation)
            {
                return;
            }

            DateTime? rangeStartDate = CalendarViewHelper.GetStartDate(_calendarInfo.SelectedDateRange.StartDate, _calendarInfo.View, _calendarInfo.Identifier);
            DateTime? rangeEndDate = CalendarViewHelper.GetStartDate(_calendarInfo.SelectedDateRange.EndDate, _calendarInfo.View, _calendarInfo.Identifier);
            if (rangeStartDate?.Date > rangeEndDate?.Date)
            {
                DateTime? temp = rangeStartDate;
                rangeStartDate = rangeEndDate;
                rangeEndDate = temp;
            }

            int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
            bool isMonthView = _calendarInfo.View == CalendarView.Month && numberOfWeeks == 6;
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarInfo.Identifier.ToString());
            DateTime minDate = cultureCalendar.MinSupportedDateTime;
            int minYear = cultureCalendar.GetYear(minDate);
            int minMonth = cultureCalendar.GetMonth(minDate);
            for (int i = 0; i < Children.Count; i++)
            {
                ICalendarView child = (ICalendarView)Children[i];
                List<DateTime> dates = GetVisibleDatesForView(i);
                DateTime visibleStartDate = dates[0];
                DateTime visibleEndDate = dates[dates.Count - 1];
                //// Does not need to calculate the visible end date for year view while show trailing and leading dates disabled
                //// because the year view does not have trailing and leading dates.
                if (_calendarInfo.View != CalendarView.Year && !_calendarInfo.ShowTrailingAndLeadingDates)
                {
                    if (isMonthView)
                    {
                        DateTime visibleDate = dates[dates.Count / 2];
                        int year = cultureCalendar.GetYear(visibleDate);
                        int month = cultureCalendar.GetMonth(visibleDate);
                        visibleStartDate = minYear == year && minMonth == month ? minDate : new DateTime(year, month, 1, cultureCalendar);
                    }

                    //// Calculate the view end date other than month view with number of weeks specified.
                    //// if the visible start date is 2020 then the visible end date is assigned to 2029 on decade view.
                    visibleEndDate = _calendarInfo.View == CalendarView.Month && numberOfWeeks != 6 ? visibleEndDate : CalendarViewHelper.GetViewLastDate(_calendarInfo.View, visibleStartDate, _calendarInfo.Identifier);
                }

                //// Update the view while the view have range dates.
                //// View update the UI while the view have range start date or range end date or the view have in-between dates.
                if ((rangeStartDate != null && CalendarViewHelper.IsDateWithinDateRange(rangeStartDate.Value, visibleStartDate, visibleEndDate)) ||
                    (rangeEndDate != null && CalendarViewHelper.IsDateWithinDateRange(rangeEndDate.Value, visibleStartDate, visibleEndDate)) ||
                    (rangeStartDate != null && rangeEndDate != null && CalendarViewHelper.IsDateWithinDateRange(visibleStartDate, rangeStartDate.Value, rangeEndDate.Value)))
                {
                    child.InvalidateViewCells();
                }
            }
        }

        /// <summary>
        /// Method to update the selection text style and background.
        /// </summary>
        internal void UpdateSelectedCellStyle()
        {
            //// Skip the view update while the allow view navigation is enabled and the view is not month view.
            //// Because other than month view, views show selection when the allow view navigation disabled.
            if (_calendarInfo.View != CalendarView.Month && _calendarInfo.AllowViewNavigation)
            {
                return;
            }

            int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
            bool isMonthView = _calendarInfo.View == CalendarView.Month && numberOfWeeks == 6;
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarInfo.Identifier.ToString());
            DateTime minDate = cultureCalendar.MinSupportedDateTime;
            int minYear = cultureCalendar.GetYear(minDate);
            int minMonth = cultureCalendar.GetMonth(minDate);
            switch (_calendarInfo.SelectionMode)
            {
                case CalendarSelectionMode.Single:
                    {
                        if (_calendarInfo.SelectedDate == null)
                        {
                            return;
                        }

                        DateTime? selectedDate = CalendarViewHelper.GetStartDate(_calendarInfo.SelectedDate, _calendarInfo.View, _calendarInfo.Identifier);

                        for (int i = 0; i < Children.Count; i++)
                        {
                            ICalendarView child = (ICalendarView)Children[i];
                            List<DateTime> dates = GetVisibleDatesForView(i);
                            DateTime visibleStartDate = dates[0];
                            DateTime visibleEndDate = dates[dates.Count - 1];
                            //// Does not need to calculate the visible end date for year view while show trailing and leading dates disabled
                            //// because the year view does not have trailing and leading dates.
                            if (_calendarInfo.View != CalendarView.Year && !_calendarInfo.ShowTrailingAndLeadingDates)
                            {
                                if (isMonthView)
                                {
                                    DateTime visibleDate = dates[dates.Count / 2];
                                    int year = cultureCalendar.GetYear(visibleDate);
                                    int month = cultureCalendar.GetMonth(visibleDate);
                                    visibleStartDate = minYear == year && minMonth == month ? minDate : new DateTime(year, month, 1, cultureCalendar);
                                }

                                //// Calculate the view end date other than month view with number of weeks specified.
                                //// if the visible start date is 2020 then the visible end date is assigned to 2029 on decade view.
                                visibleEndDate = _calendarInfo.View == CalendarView.Month && numberOfWeeks != 6 ? visibleEndDate : CalendarViewHelper.GetViewLastDate(_calendarInfo.View, visibleStartDate, _calendarInfo.Identifier);
                            }

                            if (selectedDate != null && CalendarViewHelper.IsDateWithinDateRange(selectedDate.Value, visibleStartDate, visibleEndDate))
                            {
                                child.InvalidateViewCells();
                            }
                        }

                        break;
                    }

                case CalendarSelectionMode.Multiple:
                    {
                        if (_calendarInfo.SelectedDates.Count == 0)
                        {
                            return;
                        }

                        for (int i = 0; i < Children.Count; i++)
                        {
                            ICalendarView child = (ICalendarView)Children[i];
                            List<DateTime> dates = GetVisibleDatesForView(i);
                            DateTime visibleStartDate = dates[0];
                            DateTime visibleEndDate = dates[dates.Count - 1];
                            //// Does not need to calculate the visible end date for year view while show trailing and leading dates disabled
                            //// because the year view does not have trailing and leading dates.
                            if (_calendarInfo.View != CalendarView.Year && !_calendarInfo.ShowTrailingAndLeadingDates)
                            {
                                if (isMonthView)
                                {
                                    DateTime visibleDate = dates[dates.Count / 2];
                                    int year = cultureCalendar.GetYear(visibleDate);
                                    int month = cultureCalendar.GetMonth(visibleDate);
                                    visibleStartDate = minYear == year && minMonth == month ? minDate : new DateTime(year, month, 1, cultureCalendar);
                                }

                                //// Calculate the view end date other than month view with number of weeks specified.
                                //// if the visible start date is 2020 then the visible end date is assigned to 2029 on decade view.
                                visibleEndDate = _calendarInfo.View == CalendarView.Month && numberOfWeeks != 6 ? visibleEndDate : CalendarViewHelper.GetViewLastDate(_calendarInfo.View, visibleStartDate, _calendarInfo.Identifier);
                            }

                            for (int j = 0; j < _calendarInfo.SelectedDates.Count; j++)
                            {
                                DateTime? selectedDate = CalendarViewHelper.GetStartDate(_calendarInfo.SelectedDates[j], _calendarInfo.View, _calendarInfo.Identifier);
                                if (selectedDate != null && CalendarViewHelper.IsDateWithinDateRange(selectedDate.Value, visibleStartDate, visibleEndDate))
                                {
                                    child.InvalidateViewCells();
                                    break;
                                }
                            }
                        }

                        break;
                    }

                case CalendarSelectionMode.Range:
                    if (_calendarInfo.SelectionMode != CalendarSelectionMode.Range || _calendarInfo.SelectedDateRange == null || (_calendarInfo.SelectedDateRange.StartDate == null && _calendarInfo.SelectedDateRange.EndDate == null))
                    {
                        return;
                    }

                    DateTime? rangeStartDate = CalendarViewHelper.GetStartDate(_calendarInfo.SelectedDateRange.StartDate, _calendarInfo.View, _calendarInfo.Identifier);
                    DateTime? rangeEndDate = CalendarViewHelper.GetStartDate(_calendarInfo.SelectedDateRange.EndDate, _calendarInfo.View, _calendarInfo.Identifier);
                    for (int i = 0; i < Children.Count; i++)
                    {
                        ICalendarView child = (ICalendarView)Children[i];
                        List<DateTime> dates = GetVisibleDatesForView(i);
                        DateTime visibleStartDate = dates[0];
                        //// Does not need to calculate the visible end date for year view while show trailing and leading dates disabled
                        //// because the year view does not have trailing and leading dates.
                        DateTime visibleEndDate = dates[dates.Count - 1];
                        if (_calendarInfo.View != CalendarView.Year && !_calendarInfo.ShowTrailingAndLeadingDates)
                        {
                            if (isMonthView)
                            {
                                DateTime visibleDate = dates[dates.Count / 2];
                                int year = cultureCalendar.GetYear(visibleDate);
                                int month = cultureCalendar.GetMonth(visibleDate);
                                visibleStartDate = minYear == year && minMonth == month ? minDate : new DateTime(year, month, 1, cultureCalendar);
                            }

                            //// Calculate the view end date other than month view with number of weeks specified.
                            //// if the visible start date is 2020 then the visible end date is assigned to 2029 on decade view.
                            visibleEndDate = _calendarInfo.View == CalendarView.Month && numberOfWeeks != 6 ? visibleEndDate : CalendarViewHelper.GetViewLastDate(_calendarInfo.View, visibleStartDate, _calendarInfo.Identifier);
                        }

                        //// Update the view while the view have range start and end dates because the
                        //// range start and end dates are apply the selection text style.
                        if ((rangeStartDate != null && CalendarViewHelper.IsDateWithinDateRange(rangeStartDate.Value, visibleStartDate, visibleEndDate)) ||
                            (rangeEndDate != null && CalendarViewHelper.IsDateWithinDateRange(rangeEndDate.Value, visibleStartDate, visibleEndDate)))
                        {
                            child.InvalidateViewCells();
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Method to update the visible dates of the view.
        /// </summary>
        internal void UpdateVisibleDateOnView()
        {
            CalculateVisibleDateOnView();
            //// In android platform some time's the InvalidateMeasure doesn't trigger the layout measure.
            //// So the view doesn't renderer properly. Hence calling measure and arrange directly without InvalidateMeasure.
#if __ANDROID__
            this.TriggerInvalidateMeasure();
#else
            InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Method to update the minimum and maximum date change
        /// </summary>
        /// <param name="currentMinMaxDate">The current minimum and maximum date.</param>
        /// <param name="oldMinMaxDate">The old minimum and maximum date.</param>
        internal void UpdateMinMaxDateChange(DateTime currentMinMaxDate, DateTime oldMinMaxDate)
        {
            DateTime displayDate = CalendarViewHelper.GetValidDisplayDate(_calendarInfo);
            if (!CalendarViewHelper.IsSameDisplayDateView(_calendarInfo, _visibleDates, _calendarInfo.DisplayDate, displayDate))
            {
                UpdateVisibleDateOnView();
            }
            else
            {
                DateTime? previousDate = CalendarViewHelper.GetStartDate(oldMinMaxDate, _calendarInfo.View, _calendarInfo.Identifier);
                DateTime? currentDate = CalendarViewHelper.GetStartDate(currentMinMaxDate, _calendarInfo.View, _calendarInfo.Identifier);
                if (previousDate?.Date > currentDate?.Date)
                {
                    DateTime? temp = previousDate;
                    previousDate = currentDate;
                    currentDate = temp;
                }

                int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
                bool isMonthView = _calendarInfo.View == CalendarView.Month && numberOfWeeks == 6;
                Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarInfo.Identifier.ToString());
                DateTime minDate = cultureCalendar.MinSupportedDateTime;
                int minYear = cultureCalendar.GetYear(minDate);
                int minMonth = cultureCalendar.GetMonth(minDate);
                for (int i = 0; i < Children.Count; i++)
                {
                    ICalendarView child = (ICalendarView)Children[i];
                    List<DateTime> dates = GetVisibleDatesForView(i);
                    DateTime visibleStartDate = dates[0];
                    DateTime visibleEndDate = dates[dates.Count - 1];
                    //// Does not need to calculate the visible end date for year view while show trailing and leading dates disabled
                    //// because the year view does not have trailing and leading dates.
                    if (_calendarInfo.View != CalendarView.Year && !_calendarInfo.ShowTrailingAndLeadingDates)
                    {
                        if (isMonthView)
                        {
                            DateTime visibleDate = dates[dates.Count / 2];
                            int year = cultureCalendar.GetYear(visibleDate);
                            int month = cultureCalendar.GetMonth(visibleDate);
                            visibleStartDate = minYear == year && minMonth == month ? minDate : new DateTime(year, month, 1, cultureCalendar);
                        }

                        //// Calculate the view end date other than month view with number of weeks specified.
                        //// if the visible start date is 2020 then the visible end date is assigned to 2029 on decade view.
                        visibleEndDate = _calendarInfo.View == CalendarView.Month && numberOfWeeks != 6 ? visibleEndDate : CalendarViewHelper.GetViewLastDate(_calendarInfo.View, visibleStartDate, _calendarInfo.Identifier);
                    }

                    //// Update the view while the view is inbetween previous min or max date and current min or max date.
                    //// View update the UI while the view have previous min or max date or current min or max date or the view have in-between previous and current min or max date.
                    if ((previousDate != null && CalendarViewHelper.IsDateWithinDateRange(previousDate.Value, visibleStartDate, visibleEndDate)) ||
                        (currentDate != null && CalendarViewHelper.IsDateWithinDateRange(currentDate.Value, visibleStartDate, visibleEndDate)) ||
                        (previousDate != null && currentDate != null && CalendarViewHelper.IsDateWithinDateRange(visibleStartDate, previousDate.Value, currentDate.Value)))
                    {
                        child.InvalidateView();
                        (Children[i] as MonthViewLayout)?.UpdateMonthViewSemanticNode(i == _currentChildIndex);
                        (Children[i] as YearView)?.InvalidateSemanticsNode(i == _currentChildIndex);
                    }
                }
            }
        }

        /// <summary>
        /// Method to update the month view and view header semantic node.
        /// </summary>
        internal void UpdateSemanticNodes()
        {
            if (Children.Count == 0)
            {
                return;
            }

            for (int i = 0; i < ChildCount; i++)
            {
                (Children[i] as MonthViewLayout)?.UpdateViewHeaderSemanticNode(i == _currentChildIndex);
                (Children[i] as MonthViewLayout)?.UpdateMonthViewSemanticNode(i == _currentChildIndex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///  Method to create or reset for current previous next view visible dates.
        /// </summary>
        /// <param name="displayDate">Current display date.</param>
        void CreateOrResetVisibleDates(DateTime displayDate)
        {
            //// This method triggered while create the view or reset the view.
            _currentChildIndex = 1;
            CalendarView oldCalendarView = _currentCalendarView;
            _currentCalendarView = _calendarInfo.View;

            // Get display date based on user defined display date.
            DateTime currentDate = displayDate.Date;

            // Get start date of previous view.
            DateTime previousDate = GetPreviousViewStartDate(currentDate);

            // Get start date of next view.
            DateTime nextDate = GetNextViewStartDate(currentDate);

            // Assign current view visible dates.
            _currentViewVisibleDates = GetVisibleDates(currentDate);

            if (_calendarInfo.IsRTLLayout && _calendarInfo.NavigationDirection == CalendarNavigationDirection.Horizontal)
            {
                _nextViewVisibleDates = GetVisibleDates(previousDate);
                _previousViewVisibleDates = GetVisibleDates(nextDate);
            }
            else
            {
                _nextViewVisibleDates = GetVisibleDates(nextDate);
                _previousViewVisibleDates = GetVisibleDates(previousDate);
            }

            List<DateTime> previousVisibleDates = _visibleDates;
            //// Assign visible collection.
            _visibleDates = _currentViewVisibleDates;
            _calendarInfo.UpdateVisibleDates(_visibleDates, previousVisibleDates, oldCalendarView);
        }

        /// <summary>
        /// Calculate the visible dates and update the visible dates to the view.
        /// </summary>
        void CalculateVisibleDateOnView()
        {
            // Need to calculate the visible dates based on the valid display date. Then update the view based on newly generated visible dates collections.
            DateTime displayDate = CalendarViewHelper.GetValidDisplayDate(_calendarInfo);
            CreateOrResetVisibleDates(displayDate);
            for (int i = 0; i < Children.Count; i++)
            {
                List<DateTime> visibleDates = GetVisibleDatesForView(i);
                (Children[i] as ICalendarView)?.UpdateVisibleDatesChange(visibleDates, i == _currentChildIndex, this);
                //// Condition to update the disabled date only for the current view.
                if (_visibleDates == visibleDates)
                {
                    //// Updating disabled dates based on visible dates change in the calendar view panel.
                    CreateOrResetSpecialAndDisableDates();
                }

                (Children[i] as MonthViewLayout)?.UpdateMonthViewSemanticNode(i == _currentChildIndex);
                (Children[i] as YearView)?.InvalidateSemanticsNode(i == _currentChildIndex);
            }
        }

        /// <summary>
        /// Method to update the visible dates when the view is changed
        /// </summary>
        /// <param name="isNextView">Bool instance to check the view changes</param>
        void UpdateVisibleDatesOnViewUpdate(bool isNextView = false)
        {
            List<DateTime> previousVisibleDates = _visibleDates;
            if (isNextView)
            {
                if (_currentChildIndex == 0)
                {
                    _visibleDates = _currentViewVisibleDates;
                }
                else if (_currentChildIndex == 1)
                {
                    _visibleDates = _nextViewVisibleDates;
                }
                else
                {
                    _visibleDates = _previousViewVisibleDates;
                }
            }
            else
            {
                if (_currentChildIndex == 0)
                {
                    _visibleDates = _nextViewVisibleDates;
                }
                else if (_currentChildIndex == 1)
                {
                    _visibleDates = _previousViewVisibleDates;
                }
                else
                {
                    _visibleDates = _currentViewVisibleDates;
                }
            }

            _calendarInfo.UpdateVisibleDates(_visibleDates, previousVisibleDates, _calendarInfo.View);

            //// Here the previous or next view disabled dates in the dictionary are removed by checking whether the key is not equal to the visible dates.
            var keys = _disabledDatesDictionary.Keys;
            foreach (var key in keys)
            {
                if (_visibleDates != key)
                {
                    _disabledDatesDictionary.Remove(key);
                }
            }

            //// Here the previous or next view special dates in the dictionary are removed by checking whether the key is not equal to the visible dates.
            var specialDayKeys = _specialDatesDictionary.Keys;
            foreach (var key in specialDayKeys)
            {
                if (_visibleDates != key)
                {
                    _specialDatesDictionary.Remove(key);
                }
            }
        }

        /// <summary>
        /// Method to update the next view visible dates.
        /// </summary>
        void UpdateNextViewVisibleDates()
        {
            DateTime currentDate = _visibleDates[0];
            int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
            if (_calendarInfo.View == CalendarView.Month && numberOfWeeks == 6)
            {
                currentDate = _visibleDates[_visibleDates.Count / 2];
            }

            // The flow direction is RTL then need to calculate the previous view visible dates.
            DateTime nextViewDate = _calendarInfo.IsRTLLayout && _calendarInfo.NavigationDirection == CalendarNavigationDirection.Horizontal ? GetPreviousViewStartDate(currentDate) : GetNextViewStartDate(currentDate);

            // Get next view visible dates after forward swipe or touch is performed.
            List<DateTime> dates = GetVisibleDates(nextViewDate);

            // Need to update the calculated visible dates to views based on current child index. current child index value holds the index value of visible view before the navigation.
            // Custom snap layout hold 3 children and its index values are 0(previousDates), 1(currentDates) and 2(next Dates)
            // If the view navigation to index 2 from index 1 and current child index value is 1 then update the index 1(previous dates) view.
            // Else if the view navigation to index 0 from index 2 and current child index value is 1 then update the index 2(current dates) view.
            // Else if the view navigation to index 0 from index 1 and current child index value is 0 then update the index 1(next dates) view.
            if (_currentChildIndex == 0)
            {
                _nextViewVisibleDates = dates;
            }
            else if (_currentChildIndex == 1)
            {
                _previousViewVisibleDates = dates;
            }
            else
            {
                _currentViewVisibleDates = dates;
            }
        }

        /// <summary>
        /// Method to update the previous view visible dates.
        /// </summary>
        void UpdatePreviousViewVisibleDates()
        {
            DateTime currentDate = _visibleDates[0];
            int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
            if (_calendarInfo.View == CalendarView.Month && numberOfWeeks == 6)
            {
                currentDate = _visibleDates[_visibleDates.Count / 2];
            }

            // The flow direction is RTL then need to calculate the next view visible dates.
            DateTime previousViewDate = _calendarInfo.IsRTLLayout && _calendarInfo.NavigationDirection == CalendarNavigationDirection.Horizontal ? GetNextViewStartDate(currentDate) : GetPreviousViewStartDate(currentDate);

            // Get previous view visible dates after backward swipe or touch is performed.
            List<DateTime> dates = GetVisibleDates(previousViewDate);

            // Need to update the calculated visible dates to views based on current child index. current child index value holds the index value of visible view before the navigation.
            // Custom snap layout hold 3 children and its index values are 0(previousDates), 1(currentDates) and 2(next Dates)
            // If the view navigation to index 1 from index 2 and current child index value is 2 then update the index 0(previous dates) view.
            // Else if the view navigation to index 0 from index 1 and current child index value is 1 then update the index 2(next dates) view.
            // Else if the view navigation to index 2 from index 0 and current child index value is 0 then update the index 1(current dates) view.
            if (_currentChildIndex == 0)
            {
                _currentViewVisibleDates = dates;
            }
            else if (_currentChildIndex == 1)
            {
                _nextViewVisibleDates = dates;
            }
            else
            {
                _previousViewVisibleDates = dates;
            }
        }

        List<DateTime> GetVisibleDatesForView(int index)
        {
            if (index == 0)
            {
                return _previousViewVisibleDates;
            }
            else if (index == 1)
            {
                return _currentViewVisibleDates;
            }
            else
            {
                return _nextViewVisibleDates;
            }
        }

        /// <summary>
        /// Method to get previous or next view visible dates.
        /// </summary>
        /// <param name="isNextView">Is next view.</param>
        /// <returns>The visible date collection.</returns>
        /// Here the visible dates already calculated based on the flow direction so it is not considered.
        List<DateTime> GetNextViewVisibleDate(bool isNextView)
        {
            if (isNextView)
            {
                if (_currentChildIndex == 0)
                {
                    return _currentViewVisibleDates;
                }
                else if (_currentChildIndex == 1)
                {
                    return _nextViewVisibleDates;
                }

                return _previousViewVisibleDates;
            }
            else
            {
                if (_currentChildIndex == 0)
                {
                    return _nextViewVisibleDates;
                }
                else if (_currentChildIndex == 1)
                {
                    return _previousViewVisibleDates;
                }

                return _currentViewVisibleDates;
            }
        }

        /// <summary>
        /// Method to update visible dates changes to previous or next views after the swipe animation is completed.
        /// </summary>
        /// <param name="isPreviousAnimation">Specifies whether the view update is for previous or next view.</param>
        /// <param name="previousCurrentIndex">The old current view index.</param>
        void UpdateCalendarViewVisibleDates(bool isPreviousAnimation, int previousCurrentIndex)
        {
            if (Children.Count != ChildCount)
            {
                return;
            }

            int currentIndex;
            if (isPreviousAnimation)
            {
                //// To calculate the next view index based on the previousCurrentIndex by adding offset(1) value
                //// to update the visible dates on previous navigation(current view to previous view)
                //// Example : The view renders its children as previous view(May), current view(June), next view(July)
                //// The view navigated from June to May, so it needs to update the July dates to April and place
                //// the April month before May month.
                currentIndex = (previousCurrentIndex + 1) % ChildCount;
            }
            else
            {
                //// To calculate the previous view index based on the previousCurrentIndex by subtract offset(1) value
                //// to update the visible dates on next navigation(current view to next view)
                //// Example : The view renders its children as previous view(May), current view(June), next view(July)
                //// The view navigated from June to July, so it needs to update the May dates to August and place
                //// the August month after July month.
                currentIndex = (ChildCount + previousCurrentIndex - 1) % ChildCount;
            }

            UpdateVisibleDatesOnChange(currentIndex);
        }

        /// <summary>
        /// Update the view when the View is changed
        /// </summary>
        /// <param name="currentIndex">The index of current view</param>
        void UpdateVisibleDatesOnChange(int currentIndex)
        {
            if (Children.Count == 0)
            {
                return;
            }

            (Children[currentIndex] as ICalendarView)?.UpdateVisibleDatesChange(GetVisibleDatesForView(currentIndex), currentIndex == _currentChildIndex, this);
            for (int i = 0; i < ChildCount; i++)
            {
                (Children[i] as YearView)?.InvalidateSemanticsNode(i == _currentChildIndex);
            }

            UpdateSemanticNodes();
        }

        /// <summary>
        /// Add the Children to the Custom snap layout
        /// </summary>
        void AddChildren()
        {
            CreateOrResetSpecialAndDisableDates();
            for (int i = 0; i < ChildCount; i++)
            {
                List<DateTime> visibleDates = GetVisibleDatesForView(i);
                List<DateTime> disableDates = _disabledDatesDictionary.ContainsKey(visibleDates) ? _disabledDatesDictionary[visibleDates] : new List<DateTime>();
                List<CalendarIconDetails> specialDates = _specialDatesDictionary.ContainsKey(visibleDates) ? _specialDatesDictionary[visibleDates] : new List<CalendarIconDetails>();
                if (_calendarInfo.View == CalendarView.Month)
                {
                    MonthViewLayout monthViewLayout = new MonthViewLayout(_calendarInfo, visibleDates, _calendarInfo.SelectedDate, disableDates, specialDates, i == _currentChildIndex);
                    Add(monthViewLayout);
                }
                else
                {
                    YearView yearView = new YearView(_calendarInfo, visibleDates, disableDates, i == _currentChildIndex);
                    Add(yearView);
                    (Children[i] as YearView)?.InvalidateSemanticsNode(i == _currentChildIndex);
                }
            }
        }

        /// <summary>
        /// Return the visible dates collection based on visible date.
        /// </summary>
        /// <param name="date">The current visible date.</param>
        /// <returns>The visible dates collection based on visible date.</returns>
        List<DateTime> GetVisibleDates(DateTime date)
        {
            Globalization.Calendar calendar = CalendarViewHelper.GetCalendar(_calendarInfo.Identifier.ToString());
            _startYearDateTime = calendar.MinSupportedDateTime.Date;
            _endYearDateTime = calendar.MaxSupportedDateTime.Date;
            int startRangeYear = calendar.GetYear(_startYearDateTime);
            int startRangeMonth = calendar.GetMonth(_startYearDateTime);
            int startRangeDay = calendar.GetDayOfMonth(_startYearDateTime);
            int endRangeYear = calendar.GetYear(_endYearDateTime);
            int endRangeMonth = calendar.GetMonth(_endYearDateTime);
            int year = calendar.GetYear(date);
            List<DateTime> visibleDates = new List<DateTime>();
            switch (_calendarInfo.View)
            {
                //// Return the visible collection based on visible dates count.
                case CalendarView.Month:
                    //// Example number of weeks = 2, visibleDatesCount = 14, date= 9999,12,20
                    int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
                    int visibleDatesCount = numberOfWeeks * 7;

                    // Get the current date based on the first day of week basis.
                    // From example currentDate = 12/14/9999.
                    DateTime currentDate = GetFirstDayOfWeek(visibleDatesCount, date, calendar);

                    // The visibleMaxDate = 9999,12,31.AddDays(-14) = 9999,12,17.
                    DateTime visibleMaxDate = _endYearDateTime.AddDays(-visibleDatesCount);
                    if (currentDate == _startYearDateTime)
                    {
                        int value = CalendarViewHelper.GetFirstDayOfWeekDifference(currentDate, _calendarInfo.MonthView.FirstDayOfWeek, _calendarInfo.Identifier);
                        visibleDatesCount = visibleDatesCount - value;
                    }
                    //// The visible visibleMaxDate(9999,12,17) lesser than currentDate(9999,12,14). Condition True.
                    else if (visibleMaxDate < currentDate)
                    {
                        //// To calculate the visible date count based on the current date of day. The difference between the end year date time and current date is the visible
                        //// EndYearDateTie(9999,12,31) - 9999,12,14 + 1 = 16(visibleDatesCount).
                        visibleDatesCount = (_endYearDateTime - currentDate.Date).Days + 1;
                    }

                    for (int i = 0; i < visibleDatesCount; i++)
                    {
                        DateTime visibleDate = calendar.AddDays(currentDate, i);
                        visibleDates.Add(visibleDate);
                    }

                    return visibleDates;

                // Return the every month of first day for visible view years.
                case CalendarView.Year:

                    // Get the current year of first month of first day.
                    DateTime firstMonth = startRangeYear == year ? _startYearDateTime : new DateTime(year, 1, 1, calendar);
                    int currentFirstMonth = calendar.GetMonth(firstMonth);
                    for (int i = 0; i < 12; i++)
                    {
                        if (year == endRangeYear && currentFirstMonth + i > endRangeMonth)
                        {
                            continue;
                        }
                        //// To restrict the date while month goes beyond the minimum supported month.
                        //// Example : For Persian calendar, the minimum supported month is 10. So the month value is 10 to 12.
                        else if (year == startRangeYear && currentFirstMonth + i > 12)
                        {
                            continue;
                        }

                        DateTime months = calendar.AddMonths(firstMonth, i);
                        visibleDates.Add(months);
                    }

                    return visibleDates;

                // Return the every current year of decade years.
                case CalendarView.Decade:

                    // Get the starting decade by using current year. Decade view holds the 10 years.
                    // By dividing the current year by 10 and multiply by 10 we can get the starting year of decade view.
                    // Example: Current year is 2022. It is divided by 10 then the value is 202. Then multiply by 10 then the value is 2020. So the first decade year is 2020.
                    DateTime startYear = year / 10 * 10 <= startRangeYear ? _startYearDateTime : new DateTime(year / 10 * 10, 1, 1, calendar);

                    for (int i = 0; i < 12; i++)
                    {
                        // To restrict the date when it goes beyond the range
                        // Example: if the year goes beyond 9999, no need to add years
                        if (calendar.GetYear(startYear) + i > endRangeYear)
                        {
                            continue;
                        }

                        DateTime years = calendar.AddYears(startYear, i);
                        visibleDates.Add(years);
                    }

                    return visibleDates;

                // Return the every decade year of century years.
                case CalendarView.Century:

                    // Get the starting century by using current year. Century view holds the 100 years.
                    // By dividing the current year by 100 and multiply by 100 we can get the starting year of century view.
                    // Example: Current year is 2022. It is divided by 100 then the value is 20. Then multiply by 100 then the value is 2000. So the first century year is 2000.
                    DateTime firstCentury = year / 100 * 100 <= startRangeYear ? _startYearDateTime : new DateTime(year / 100 * 100, 1, 1, calendar);

                    for (int i = 0; i < 12; i++)
                    {
                        int addedYear = i * 10;

                        // To restrict the date when it goes beyond the range
                        // Example: if the year goes beyond 9999, no need to add years
                        if (calendar.GetYear(firstCentury) + addedYear > endRangeYear)
                        {
                            continue;
                        }

                        // By adding addedYear we can get the continuous century view years.
                        DateTime years = calendar.AddYears(firstCentury, addedYear);

                        // If below condition is used to subtract one year from the first century
                        // Example : The first century year starts from 0001. The year is multiplied by 10 results 0011, but want to render as (0001-0010)
                        // That's why subtract one year from the first century. It applicable only within the 0099 years
                        if (calendar.GetYear(firstCentury) < startRangeYear + 100)
                        {
                            int yearValue = calendar.GetYear(years);
                            years = (yearValue != startRangeYear) ? calendar.AddYears(years, -yearValue % 10) : years;
                        }

                        visibleDates.Add(years);
                    }

                    return visibleDates;
            }

            return visibleDates;
        }

        /// <summary>
        /// Return the date time value based on date and first day of week value.
        /// This method applicable only for month view.
        /// </summary>
        /// <param name="visibleDatesCount">The visible dates count value.</param>
        /// <param name="date">The current date.</param>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Other views return the same date</returns>
        DateTime GetFirstDayOfWeek(int visibleDatesCount, DateTime date, Globalization.Calendar calendarIdentifier)
        {
            // Default the day of week is 7.
            int dayOfWeek = 7;

            // The condition become true While view is not a month view.
            if ((visibleDatesCount % dayOfWeek) != 0)
            {
                return date;
            }

            DateTime currentDate = date;
            DateTime minDate = calendarIdentifier.MinSupportedDateTime;
            int minYear = calendarIdentifier.GetYear(minDate);
            int minMonth = calendarIdentifier.GetMonth(minDate);

            int year = calendarIdentifier.GetYear(date);
            int month = calendarIdentifier.GetMonth(date);

            // If the number of weeks 6 then need to render entire month.So that we need to assign the start date(1) to current date value.
            if (visibleDatesCount == 42)
            {
                currentDate = minYear == year && minMonth == month ? minDate : new DateTime(year, month, 1, calendarIdentifier);
            }

            // Get difference between current day of week and day of week.
            // Example: Assume current date is 25 may 2022. First day of week is Sunday(1).The default day of week is 7.
            // Current date(25th) of day of week is 4.
            // As per calculation value = -4(currentDate(25) of day of week)+1(First day of week(Sunday(1))) -7(Default day of week) = -10(value)
            int value = (-(int)calendarIdentifier.GetDayOfWeek(currentDate)) + ((int)_calendarInfo.MonthView.FirstDayOfWeek - dayOfWeek);

            // Condition become false while value is lesser than 7.
            // From example: Math.Abs(-10 >= 7) condition true.
            if (Math.Abs(value) >= dayOfWeek)
            {
                // From example: -10(value) += 7(Day of week). The answer is -3.
                // The actual difference between first day of week and current date of week = -3.
                value += dayOfWeek;
            }

            //// This is used to get the first day of week based on start year date time and value.
            //// From above example visibleMinDate = 0001,01,01.Adddays(3) = 0001,01,04.
            DateTime visibleMinDate = _startYearDateTime.AddDays(Math.Abs(value));
            //// The visibleMinDate(0001,01,04) greater than currentDate(2022,05,25) condition false.
            if (visibleMinDate.Date > currentDate.Date)
            {
                return _startYearDateTime;
            }

            // Add value to the current date to get the first day of week.
            // From example: 25(currentDate).AddDays(-3(value)). Finally We get first day of week(22).
            return calendarIdentifier.AddDays(currentDate, value);
        }

        /// <summary>
        /// Return the previous view visible start date based on current view date and it calendar view.
        /// </summary>
        /// <param name="date">The current view visible start date.</param>
        /// <returns>The previous view visible start date.</returns>
        DateTime GetPreviousViewStartDate(DateTime date)
        {
            Globalization.Calendar calendar = CalendarViewHelper.GetCalendar(_calendarInfo.Identifier.ToString());
            _startYearDateTime = calendar.MinSupportedDateTime.Date;
            _endYearDateTime = calendar.MaxSupportedDateTime.Date;
            int startRangeYear = calendar.GetYear(_startYearDateTime);
            int startRangeMonth = calendar.GetMonth(_startYearDateTime);
            int year = calendar.GetYear(date);
            int month = calendar.GetMonth(date);

            switch (_calendarInfo.View)
            {
                case CalendarView.Month:

                    int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
                    if (numberOfWeeks == 6)
                    {
                        return year == startRangeYear && month == startRangeMonth ? _startYearDateTime : calendar.AddMonths(new DateTime(year, month, 1, calendar).Date, -1);
                    }

                    //// The number of weeks not equal to 6.
                    //// The number of days based on the days per week.
                    double numberOfDays = numberOfWeeks * 7;
                    //// The difference between the current date and the minDate.
                    //// Example here the date = 0001,01,02.
                    //// difference = 0001,01,02- 0001,01,01 = 1 days.
                    int difference = (date.Date - _startYearDateTime.Date).Days;
                    //// The difference(1) is lesser that numberOfDays(14)
                    if (difference < numberOfDays)
                    {
                        return _startYearDateTime;
                    }
                    else
                    {
                        return calendar.AddDays(date, (int)-numberOfDays);
                    }

                case CalendarView.Year:
                case CalendarView.Decade:
                case CalendarView.Century:
                    //// Get the offset value based on the view
                    int offset = _calendarInfo.View.GetOffset();

                    //// To check the year is before or equal to minimum supported year.
                    if ((year / offset * offset) - offset <= startRangeYear)
                    {
                        return _startYearDateTime;
                    }

                    //// To convert other culture to Gregorian
                    return new DateTime(year - offset, 1, 1, calendar);
            }

            return date;
        }

        /// <summary>
        /// Return the next view visible start date based on current view date and it calendar view.
        /// </summary>
        /// <param name="date">The current view visible start date.</param>
        /// <returns>The next view visible start date.</returns>
        DateTime GetNextViewStartDate(DateTime date)
        {
            Globalization.Calendar calendar = CalendarViewHelper.GetCalendar(_calendarInfo.Identifier.ToString());
            _startYearDateTime = calendar.MinSupportedDateTime.Date;
            _endYearDateTime = calendar.MaxSupportedDateTime.Date;
            int endRangeYear = calendar.GetYear(_endYearDateTime);
            int endRangeMonth = calendar.GetMonth(_endYearDateTime);

            int year = calendar.GetYear(date);
            int month = calendar.GetMonth(date);
            switch (_calendarInfo.View)
            {
                case CalendarView.Month:

                    int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
                    if (numberOfWeeks == 6)
                    {
                        //// While reaching the end year(9999) and month(12) then return the 1st date of the current date. Or else need to return the next month of the first date.
                        return year == endRangeYear && month == endRangeMonth ? new DateTime(year, month, 1, calendar) : calendar.AddMonths(new DateTime(year, month, 1, calendar).Date, 1);
                    }

                    //// The number of weeks not equal to 6.
                    //// The number of days based on the days per week.
                    double numberOfDays = numberOfWeeks * 7;
                    //// The difference between the current date and the max date.
                    //// Example here the date = 28/12/9999.
                    //// difference = 31,12,9999 - 28,12,9999 = 3 days.
                    int difference = (_endYearDateTime - date.Date).Days;
                    //// The difference(3) is lesser that numberOfDays(14)
                    if (difference < numberOfDays)
                    {
                        return _endYearDateTime;
                    }
                    else
                    {
                        return calendar.AddDays(date, (int)numberOfDays);
                    }

                case CalendarView.Year:
                case CalendarView.Decade:
                case CalendarView.Century:
                    //// Get the offset value based on the view
                    int offset = _calendarInfo.View.GetOffset();
                    if ((year / offset * offset) + offset >= endRangeYear)
                    {
                        return _endYearDateTime;
                    }

                    return new DateTime(year + offset, 1, 1, calendar);
            }

            return date;
        }

        /// <summary>
        /// Method to update special date collection and disabled date collection for current visible dates.
        /// </summary>
        /// <param name="visibleDates">The visible dates collection.</param>
        void UpdateSpecialAndDisableDates(List<DateTime> visibleDates)
        {
            if (visibleDates.Count == 0)
            {
                return;
            }
            
            List<DateTime>? disabledDates = UpdateDisabledDates(visibleDates);
            List<CalendarIconDetails>? specialDates = UpdateSpecialDates(visibleDates);
            //// Disable dates null means the disabled dates are already updated for current visible dates.
            //// Special dates null means the special dates are already updated for current visible dates.
            if (disabledDates == null && specialDates == null)
            {
                return;
            }

            if (disabledDates == null)
            {
                disabledDates = _disabledDatesDictionary[visibleDates];
            }
            else if (specialDates == null)
            {
                specialDates = _specialDatesDictionary.Count == 0 ? new List<CalendarIconDetails>() : _specialDatesDictionary[visibleDates];
            }

            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as ICalendarView)?.UpdateDisableAndSpecialDateChange(disabledDates, specialDates, visibleDates);
                (Children[i] as MonthViewLayout)?.UpdateMonthViewSemanticNode(i == _currentChildIndex);
                (Children[i] as YearView)?.InvalidateSemanticsNode(i == _currentChildIndex);
            }
        }

        /// <summary>
        /// Method to get disabled date collection.
        /// </summary>
        /// <param name="visibleDates">The visible dates collection.</param>
        List<DateTime>? UpdateDisabledDates(List<DateTime> visibleDates)
        {
            if (_disabledDatesDictionary.ContainsKey(visibleDates))
            {
                return null;
            }

            List<DateTime> disabledDates = new List<DateTime>();
            DateTime currentViewDate = _calendarInfo.View == CalendarView.Month ? visibleDates[visibleDates.Count / 2] : visibleDates[0];
            int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
            //// Boolean to check whether the views are having leading and trailing dates.
            bool isViewWithLeadingAndTrailingDates = (_calendarInfo.View == CalendarView.Month && numberOfWeeks == 6) || _calendarInfo.View == CalendarView.Decade || _calendarInfo.View == CalendarView.Century;
            //// Boolean to check whether the leading and trailing dates for the views are disabled.
            bool isLeadingAndTrailingDatesDisabled = isViewWithLeadingAndTrailingDates && !_calendarInfo.ShowTrailingAndLeadingDates;
            for (int i = 0; i < visibleDates.Count; i++)
            {
                DateTime date = visibleDates[i];
                //// Restricting the SelectableDayPredicate triggering for 42 dates even ShowLeadingAndTrailingDates is False.
                //// Also restricting the SelectableDayPredicate triggering for 12 dates in decade and century view even ShowLeadingAndTrailingDates is False.
                //// Restricting is not necessary for year view because it does not have leading and trailing dates.
                if (isLeadingAndTrailingDatesDisabled)
                {
                    if (!CalendarViewHelper.IsLeadingAndTrailingDate(date, currentViewDate, _calendarInfo.View, _calendarInfo.Identifier) && !_calendarInfo.IsSelectableDayPredicate(date))
                    {
                        disabledDates.Add(date);
                    }
                }
                else
                {
                    if (!_calendarInfo.IsSelectableDayPredicate(date))
                    {
                        disabledDates.Add(date);
                    }
                }
            }

            _disabledDatesDictionary.Add(visibleDates, disabledDates);
            return disabledDates;
        }

        /// <summary>
        /// Method to get special date collection.
        /// </summary>
        /// <param name="visibleDates">The visible dates collection.</param>
        List<CalendarIconDetails>? UpdateSpecialDates(List<DateTime> visibleDates)
        {
            if (_calendarInfo.View != CalendarView.Month || _specialDatesDictionary.ContainsKey(visibleDates))
            {
                return null;
            }

            List<CalendarIconDetails> specialDates = new List<CalendarIconDetails>();
            DateTime currentViewDate = visibleDates[visibleDates.Count / 2];
            int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
            //// Boolean to check whether the leading and trailing dates for the views are disabled.
            bool isLeadingAndTrailingDatesDisabled = numberOfWeeks == 6 && !_calendarInfo.ShowTrailingAndLeadingDates;
            for (int i = 0; i < visibleDates.Count; i++)
            {
                DateTime date = visibleDates[i];
                //// Restricting the SpecialDayPredicate triggering for 42 dates even ShowLeadingAndTrailingDates is False.
                if (isLeadingAndTrailingDatesDisabled)
                {
                    if (!CalendarViewHelper.IsLeadingAndTrailingDate(date, currentViewDate, _calendarInfo.View, _calendarInfo.Identifier))
                    {
                        CalendarIconDetails? details = _calendarInfo.IsSpecialDayPredicate(date);
                        if (details != null)
                        {
                            details.Parent = _calendarInfo.MonthView;
                            details.Date = date.Date;
                            specialDates.Add(details);
                        }
                    }
                }
                else
                {
                    CalendarIconDetails? details = _calendarInfo.IsSpecialDayPredicate(date);
                    if (details != null)
                    {
                        details.Date = date.Date;
                        specialDates.Add(details);
                    }
                }
            }

            _specialDatesDictionary.Add(visibleDates, specialDates);
            return specialDates;
        }

        #endregion
    }
}