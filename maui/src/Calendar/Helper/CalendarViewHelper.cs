using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Localization;
using Globalization = System.Globalization;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which contains calendar view helper.
    /// </summary>
    internal static class CalendarViewHelper
    {
        #region Internal Methods

        /// <summary>
        /// Get the display date based on the min and max date.
        /// </summary>
        /// <param name="calendarInfo">Get the min and max date value.</param>
        /// <returns>Returns display date.</returns>
        internal static DateTime GetValidDisplayDate(ICalendar calendarInfo)
        {
            // To get the calendar instance based on the calendar identifier.
            Globalization.Calendar cultureCalendar = GetCalendar(calendarInfo.Identifier.ToString());
            DateTime startDateRange = cultureCalendar.MinSupportedDateTime.Date;
            DateTime endDateRange = cultureCalendar.MaxSupportedDateTime.Date;
            DateTime minDate = startDateRange;
            DateTime maxDate = endDateRange;

            //// The minimum date must between the startDateRange and endDateRange based on the calendar identifier.
            if (IsSupportedDate(calendarInfo.MinimumDate, startDateRange, endDateRange))
            {
                minDate = calendarInfo.MinimumDate.Date;
            }

            //// The maximum date must between the startDateRange and endDateRange based on the calendar identifier.
            if (IsSupportedDate(calendarInfo.MaximumDate, startDateRange, endDateRange))
            {
                maxDate = calendarInfo.MaximumDate.Date;
            }

            if (calendarInfo.DisplayDate < minDate)
            {
                return minDate;
            }
            else if (calendarInfo.DisplayDate > maxDate)
            {
                return maxDate;
            }

            return calendarInfo.DisplayDate;
        }

        /// <summary>
        /// Method to convert brush to color.
        /// </summary>
        /// <param name="brush">The brush to convert.</param>
        /// <returns>Returns the color value.</returns>
        internal static Color ToColor(this Brush brush)
        {
            Paint paint = brush;
            return paint.ToColor() ?? Colors.Transparent;
        }

        /// <summary>
        /// Method to get number of weeks in view.
        /// </summary>
        /// <param name="monthView">The number of weeks.</param>
        /// <returns>Returns the value for the number of weeks.</returns>
        internal static int GetNumberOfWeeks(this CalendarMonthView monthView)
        {
            if (monthView.NumberOfVisibleWeeks > 6)
            {
                return 6;
            }
            else if (monthView.NumberOfVisibleWeeks < 1)
            {
                return 1;
            }

            return monthView.NumberOfVisibleWeeks;
        }

        /// <summary>
        /// Gets the RTL value true or false.
        /// </summary>
        /// <param name="view">The type of view.</param>
        /// <param name="calendarIdentifier">The calendar Identifier.</param>
        /// <returns>Return true or false</returns>
        internal static bool IsRTL(this View view, CalendarIdentifier calendarIdentifier)
        {
            if (IsGregorianCalendar(calendarIdentifier))
            {
                //// Flow direction property only added in VisualElement class only, so that we return while the object type is not visual element.
                //// And Effective flow direction is flag type so that we add & RightToLeft.
                if (!(view is IVisualElementController))
                {
                    return false;
                }

                return ((view as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft;
            }
            else
            {
                //// Hijri, Persian and UmAlQura calendars are typically rendered in RTL directions.
                //// These calendars are used primarily in countries where the dominant language is written from RightToLeft, such as Arabic.
                if (calendarIdentifier == CalendarIdentifier.Hijri || calendarIdentifier == CalendarIdentifier.Persian || calendarIdentifier == CalendarIdentifier.UmAlQura)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Return the selection background based on the selection mode.
        /// </summary>
        /// <param name="selectionBackground">Selection background for the calendar.</param>
        /// <param name="mode">Selection mode for the calendar.</param>
        /// <returns>The selection background value.</returns>
        internal static Brush GetSelectionBackground(Brush? selectionBackground, CalendarSelectionMode mode)
        {
            if (selectionBackground != null)
            {
                return selectionBackground;
            }

            switch (mode)
            {
                case CalendarSelectionMode.Single:
                case CalendarSelectionMode.Multiple:
                    return CalendarColors.GetPrimaryBrush();
                case CalendarSelectionMode.Range:
                case CalendarSelectionMode.MultiRange:
                    return (Brush)Color.FromArgb("#1f6750a4");
            }

            return CalendarColors.GetPrimaryBrush();
        }

        /// <summary>
        /// Method to draw text inside month cells.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="dateText">Text to draw inside cell.</param>
        /// <param name="textStyle">Text style for text.</param>
        /// <param name="rect">The draw rectangle.</param>
        /// <param name="horizontalAlignment">Horizontal alignment of the text.</param>
        /// <param name="verticalAlignment">Vertical alignment of the text.</param>
        internal static void DrawText(ICanvas canvas, string dateText, ITextElement textStyle, Rect rect, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            if (rect.Height <= 0 || rect.Width <= 0)
            {
                return;
            }

            canvas.DrawText(dateText, rect, horizontalAlignment, verticalAlignment, textStyle);
        }

        /// <summary>
        /// Method to check whether the date is in the date collection.
        /// </summary>
        /// <param name="dateTime">The date used to check whether it is in collection.</param>
        /// <param name="dateCollection">The date collection.</param>
        /// <returns>Returns true if date is in the collection, false if date is not in the collection.</returns>
        internal static bool IsDateInDateCollection(DateTime dateTime, IList<DateTime> dateCollection)
        {
            foreach (DateTime date in dateCollection)
            {
                if (date.Date == dateTime.Date)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Triggers the layout measure and arrange operations.
        /// </summary>
        /// <param name="layout">The calendar layout.</param>
        internal static void TriggerInvalidateMeasure(this CalendarLayout layout)
        {
            layout.LayoutMeasure(layout.Width, layout.Height);
            layout.LayoutArrangeChildren(new Rect(0, 0, layout.Width, layout.Height));
        }

        /// <summary>
        /// Trigger the layout measure and arrange operations.
        /// </summary>
        /// <param name="layout">The snap layout or custom scroll layout.</param>
        internal static void TriggerInvalidateMeasure(this SnapLayout layout)
        {
            layout.LayoutMeasure(layout.Width, layout.Height);
            layout.LayoutArrangeChildren(new Rect(0, 0, layout.Width, layout.Height));
        }

        /// <summary>
        /// Method to get the value of handle the dates based on the calendar views.
        /// </summary>
        /// <param name="view">The calendar view.</param>
        /// <returns>Returns the value based on the calendar views.</returns>
        internal static int GetOffset(this CalendarView view)
        {
            switch (view)
            {
                case CalendarView.Month:
                    break;

                // The Year view hold the 1 year.
                case CalendarView.Year:
                    return 1;

                // The Decade view holds the 10 years.
                case CalendarView.Decade:
                    return 10;

                // The Century view holds the 100 years.
                case CalendarView.Century:
                    return 100;
            }

            return 0;
        }

        /// <summary>
        /// Method to check the next arrow will be enable or disable.
        /// </summary>
        /// <param name="visibleDates">The visible dates.</param>
        /// <param name="view">The calendar view.</param>
        /// <param name="numberOfVisibleWeeks">The number of weeks in view.</param>
        /// <param name="maxDate">The maximum date time.</param>
        /// <param name="calendarIdentifier">The calendar Identifier.</param>
        /// <returns>The next date valid navigation.</returns>
        internal static bool IsValidNextDatesNavigation(List<DateTime> visibleDates, CalendarView view, int numberOfVisibleWeeks, DateTime maxDate, CalendarIdentifier calendarIdentifier)
        {
            // To get the calendar instance based on the calendar Identifier
            Globalization.Calendar cultureCalendar = GetCalendar(calendarIdentifier.ToString());

            // Convert the maximum date to year, month on culture basis.
            // Need to check the maximum date to present max value, if the max value is given convert that date on culture basis
            // or else need to get the max date with the calendar instance for the specified culture.
            DateTime startDateRange = cultureCalendar.MinSupportedDateTime.Date;
            DateTime endDateRange = cultureCalendar.MaxSupportedDateTime.Date;
            DateTime maxDisplayDate = endDateRange;

            //// If the max date is greater than the max supported date, then the max date is set to max supported date.
            if (IsSupportedDate(maxDate, startDateRange, endDateRange))
            {
                maxDisplayDate = maxDate.Date;
            }

            int maxYear = cultureCalendar.GetYear(maxDisplayDate);
            int maxMonth = cultureCalendar.GetMonth(maxDisplayDate);

            switch (view)
            {
                case CalendarView.Month:
                    {
                        if (numberOfVisibleWeeks == 6)
                        {
                            DateTime currentDate = visibleDates[visibleDates.Count / 2];
                            int currentYear = cultureCalendar.GetYear(currentDate);
                            int currentMonth = cultureCalendar.GetMonth(currentDate);
                            if ((currentMonth >= maxMonth && currentYear == maxYear) || currentYear > maxYear)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            DateTime currentDate = visibleDates[visibleDates.Count - 1];
                            if (maxDisplayDate <= currentDate.Date)
                            {
                                return false;
                            }
                        }

                        break;
                    }

                case CalendarView.Year:
                case CalendarView.Decade:
                case CalendarView.Century:
                    {
                        DateTime currentDate = visibleDates[0];
                        int currentYear = cultureCalendar.GetYear(currentDate.Date);
                        int offset = view.GetOffset();

                        // Example for Year View: Assume current year is 2022. The maximum year is 2022.
                        // The offset value is 1 while the view is year view.
                        // Calculation: (2022 / 1 * 1) + 1 > (2022) => 2023 > 2022 => The condition is true.
                        // It returns false, it means reaches the maximum date and it restricts the next view navigation.
                        if ((currentYear / offset * offset) + offset > (maxYear / offset * offset))
                        {
                            return false;
                        }

                        break;
                    }
            }

            return true;
        }

        /// <summary>
        /// Method to check the previous arrow will be enable or disable.
        /// </summary>
        /// <param name="visibleDates">The visible dates.</param>
        /// <param name="view">The calendar view.</param>
        /// <param name="numberOfVisibleWeeks">The number of weeks in view.</param>
        /// <param name="minDate">The minimum date time.</param>
        /// <param name="calendarIdentifier">The calendar Identifier.</param>
        /// <returns>The previous date valid navigation.</returns>
        internal static bool IsValidPreviousDatesNavigation(List<DateTime> visibleDates, CalendarView view, int numberOfVisibleWeeks, DateTime minDate, CalendarIdentifier calendarIdentifier)
        {
            // To get the calendar instance based on the calendar identifier
            Globalization.Calendar cultureCalendar = GetCalendar(calendarIdentifier.ToString());

            DateTime startDateRange = cultureCalendar.MinSupportedDateTime.Date;
            DateTime endDateRange = cultureCalendar.MaxSupportedDateTime.Date;
            DateTime minDisplayDate = startDateRange;

            // Convert the minimum date to year, month on culture basis.
            // Need to check the minimum date to present min value, if the min value is given convert that date on culture basis
            // or else need to get the min date with the calendar instance for the specified culture.
            if (IsSupportedDate(minDate, startDateRange, endDateRange))
            {
                minDisplayDate = minDate.Date;
            }

            int minYear = cultureCalendar.GetYear(minDisplayDate);
            int minMonth = cultureCalendar.GetMonth(minDisplayDate);

            switch (view)
            {
                case CalendarView.Month:
                    {
                        if (numberOfVisibleWeeks == 6)
                        {
                            DateTime currentDate = visibleDates[visibleDates.Count / 2];
                            int currentYear = cultureCalendar.GetYear(currentDate);
                            int currentMonth = cultureCalendar.GetMonth(currentDate);
                            if ((currentMonth <= minMonth && currentYear == minYear) || currentYear < minYear)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            DateTime currentDate = visibleDates[0];
                            if (minDisplayDate >= currentDate.Date)
                            {
                                return false;
                            }
                        }

                        break;
                    }

                case CalendarView.Year:
                case CalendarView.Decade:
                case CalendarView.Century:
                    {
                        DateTime currentDate = visibleDates[0];
                        int currentYear = cultureCalendar.GetYear(currentDate);
                        int offset = view.GetOffset();

                        // Example for Year View: Assume current year is 2022. The minimum year is 2022.
                        // The offset value is 1 while the view is year view.
                        // Calculation: (2022 / 1 * 1) - 1 < (2022) => 2021 < 2022 => The condition is true.
                        // It returns false, it means reaches the minimum date and it restricts the previous view navigation.
                        if ((currentYear / offset * offset) - offset < (minYear / offset * offset))
                        {
                            return false;
                        }

                        break;
                    }
            }

            return true;
        }

        /// <summary>
        /// Method to get week start date based on ISO standard.
        /// </summary>
        /// <param name="visibleDates">The visible dates collection.</param>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <param name="dayOfWeek">The first day of week</param>
        /// <returns>The week start date.</returns>
        internal static DateTime GetWeekStartDate(List<DateTime> visibleDates, CalendarIdentifier calendarIdentifier, DayOfWeek dayOfWeek)
        {
            Globalization.Calendar cultureCalendar = GetCalendar(calendarIdentifier.ToString());
            foreach (DateTime dateTime in visibleDates)
            {
                //// We have adjust mid day of week based on first day of week.
                //// Passing mid day instead of start day returns the correct week number.
                if (cultureCalendar.GetDayOfWeek(dateTime) == GetMidDayOfWeek(dayOfWeek))
                {
                    return dateTime;
                }
            }

            return visibleDates[0];
        }

        /// <summary>
        /// Method to get week end date based on ISO standard.
        /// </summary>
        /// <param name="calendarIdentifier">The Calendar identifier.</param>
        /// <param name="dateTime">The date time</param>
        /// <param name="dayOfWeek">The first day of week</param>
        /// <returns>Returns the week number.</returns>
        internal static int GetWeekNumber(CalendarIdentifier calendarIdentifier, DateTime dateTime, DayOfWeek dayOfWeek)
        {
            int weekNumber;

            // If the type is not gregorian, need to get the week number based on the specified calendar identifier.
            if (!IsGregorianCalendar(calendarIdentifier))
            {
                weekNumber = GetCalendar(calendarIdentifier.ToString()).GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, dayOfWeek);
            }
            else
            {
                weekNumber = GetWeekNumberOfYear(dateTime);
            }

            return weekNumber;
        }

        /// <summary>
        /// Get the week number based on the given date.
        /// </summary>
        /// <param name="date">The date value.</param>
        /// <returns>The week number.</returns>
        internal static int GetWeekNumberOfYear(DateTime date)
        {
            int weekNumber = (date.DayOfYear - GetWeekday(date.DayOfWeek) + 10) / 7;
            if (weekNumber < 1)
            {
                //// If the week number is equals 0, it means that the given date belongs to the preceding (week-based) year.
                return GetWeeksInYear(date.Year - 1);
            }
            else
            {
                return weekNumber;
            }
        }

        /// <summary>
        /// Method to check the Min and Max dates for the Disabled date.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="view">The calendar view.</param>
        /// <param name="enablePastDate">The enable pass date true or false.</param>
        /// <param name="minDate">The min date.</param>
        /// <param name="maxDate">The max date.</param>
        /// <param name="selectionMode">The calendar selection mode.</param>
        /// <param name="selectionDirection">The calendar range selection direction.</param>
        /// <param name="selectedDateRange">The calendar date range.</param>
        /// <param name="allowViewNavigation">Defines the selection enabled for year, decade and century views.</param>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>It returns the date is disabled date or not.</returns>
        internal static bool IsDisabledDate(DateTime dateTime, CalendarView view, bool enablePastDate, DateTime minDate, DateTime maxDate, CalendarSelectionMode selectionMode, CalendarRangeSelectionDirection selectionDirection, CalendarDateRange? selectedDateRange, bool allowViewNavigation, CalendarIdentifier calendarIdentifier)
        {
            // To get the calendar instance based on the calendar identifier.
            Globalization.Calendar cultureCalendar = GetCalendar(calendarIdentifier.ToString());

            DateTime startDateRange = cultureCalendar.MinSupportedDateTime.Date;
            DateTime endDateRange = cultureCalendar.MaxSupportedDateTime.Date;
            DateTime maxDisplayDate = endDateRange;
            DateTime minDisplayDate = startDateRange;

            // Convert the minimum date to year, month on culture basis.
            // Need to check the minimum date to present min value, if the min value is given convert that date on culture basis
            // or else need to get the min date with the calendar instance for the specified culture.
            if (startDateRange <= minDate.Date && endDateRange >= minDate.Date)
            {
                minDisplayDate = minDate.Date;
            }

            // Convert the maximum date to year, month on culture basis.
            // Need to check the maximum date to present max value, if the max value is given convert that date on culture basis
            // or else need to get the max date with the calendar instance for the specified culture.
            if (startDateRange <= maxDate.Date && endDateRange >= maxDate.Date)
            {
                maxDisplayDate = maxDate.Date;
            }

            int maxYear = cultureCalendar.GetYear(maxDisplayDate);
            int maxMonth = cultureCalendar.GetMonth(maxDisplayDate);
            int minYear = cultureCalendar.GetYear(minDisplayDate);
            int minMonth = cultureCalendar.GetMonth(minDisplayDate);

            // Get the year and month for the dateTime corresponding to the calendar identifier.
            int year = cultureCalendar.GetYear(dateTime);
            int month = cultureCalendar.GetMonth(dateTime);
            int currentYear = cultureCalendar.GetYear(DateTime.Now);
            int currentMonth = cultureCalendar.GetMonth(DateTime.Now);

            bool isDisabledDate = false;
            switch (view)
            {
                case CalendarView.Month:
                    {
                        bool isEnablePastDate = !enablePastDate && dateTime.Date < DateTime.Now.Date;
                        isDisabledDate = (minDisplayDate > dateTime.Date) || (maxDisplayDate < dateTime.Date) || isEnablePastDate;
                        break;
                    }

                case CalendarView.Year:
                    {
                        bool isBeforeMinDate = ((minYear == year) && (minMonth > month)) || (minYear > year);
                        bool isAfterMaxDate = ((maxYear == year) && (maxMonth < month)) || (maxYear < year);
                        bool isEnablePastDate = !enablePastDate && ((month < currentMonth && currentYear == year) || (currentYear > year));
                        isDisabledDate = isBeforeMinDate || isAfterMaxDate || isEnablePastDate;
                        break;
                    }

                case CalendarView.Decade:
                    {
                        bool isEnablePastDate = !enablePastDate && currentYear > year;
                        isDisabledDate = (minYear > year) || (maxYear < year) || isEnablePastDate;
                        break;
                    }

                case CalendarView.Century:
                    {
                        bool isEnablePastDate = !enablePastDate && (currentYear / 10 > year / 10);
                        isDisabledDate = (minYear / 10 > year / 10) || (maxYear / 10 < year / 10) || isEnablePastDate;
                        break;
                    }
            }

            if (selectionMode != CalendarSelectionMode.Range || isDisabledDate || (view != CalendarView.Month && allowViewNavigation))
            {
                return isDisabledDate;
            }

            //// Check the DateTime is disabled date for the Range selection mode based on selection direction
            if (selectedDateRange == null || (selectedDateRange.StartDate == null && selectedDateRange.EndDate == null))
            {
                return false;
            }

            switch (selectionDirection)
            {
                case CalendarRangeSelectionDirection.Default:
                case CalendarRangeSelectionDirection.Both:
                    break;
                case CalendarRangeSelectionDirection.Forward:
                    return selectedDateRange.StartDate.IsGreaterDate(view, dateTime, calendarIdentifier);
                case CalendarRangeSelectionDirection.Backward:
                    {
                        DateTime? endDate = selectedDateRange.EndDate;
                        if (selectedDateRange.EndDate == null)
                        {
                            endDate = selectedDateRange.StartDate;
                        }

                        return IsGreaterDate(dateTime, view, endDate, calendarIdentifier);
                    }

                case CalendarRangeSelectionDirection.None:
                    {
                        if (selectedDateRange?.StartDate != null && selectedDateRange?.EndDate != null && !IsSameDate(view, selectedDateRange.StartDate, selectedDateRange.EndDate, calendarIdentifier))
                        {
                            return selectedDateRange.StartDate.IsGreaterDate(view, dateTime, calendarIdentifier) || IsGreaterDate(dateTime, view, selectedDateRange.EndDate, calendarIdentifier);
                        }

                        break;
                    }
            }

            return false;
        }

        /// <summary>
        /// Method to get week number width.
        /// </summary>
        /// <param name="monthView">The Show week number.</param>
        /// <param name="monthViewWidth">The width of the month view.</param>
        /// <returns>The Week number width.</returns>
        internal static float GetWeekNumberWidth(CalendarMonthView monthView, float monthViewWidth)
        {
            return monthView.ShowWeekNumber ? monthViewWidth / 7 * 0.5f : 0;
        }

        /// <summary>
        /// Method to find the is leading date
        /// </summary>
        /// <param name="dateTime">The interacted date</param>
        /// <param name="currentViewDate">The current view date</param>
        /// <param name="view">View of the calendar</param>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Returns true or false</returns>
        internal static bool IsLeadingAndTrailingDate(DateTime dateTime, DateTime currentViewDate, CalendarView view, CalendarIdentifier calendarIdentifier)
        {
            // To get the calendar instance based on the calendar identifier.
            Globalization.Calendar cultureCalendar = GetCalendar(calendarIdentifier.ToString());

            // Get year and month value for the dateTime corresponding to the specified calendar identifier.
            int year = cultureCalendar.GetYear(dateTime);
            int month = cultureCalendar.GetMonth(dateTime);
            int currentYear = cultureCalendar.GetYear(currentViewDate);
            int currentMonth = cultureCalendar.GetMonth(currentViewDate);

            switch (view)
            {
                case CalendarView.Month:
                    return month != currentMonth;
                case CalendarView.Year:
                    return false;
                case CalendarView.Decade:
                    return year / 10 != currentYear / 10;
                case CalendarView.Century:
                    return year / 100 != currentYear / 100;
            }

            return false;
        }

        /// <summary>
        /// Method to check today date, selected date and the toggle date
        /// </summary>
        /// <param name="view">The calendar view</param>
        /// <param name="interactedDate">The today date or selected date</param>
        /// <param name="dateTime">The dateTime</param>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Returns true or false</returns>
        internal static bool IsSameDate(CalendarView view, DateTime? interactedDate, DateTime? dateTime, CalendarIdentifier calendarIdentifier)
        {
            if (interactedDate == dateTime)
            {
                return true;
            }

            if (interactedDate == null || dateTime == null)
            {
                return false;
            }

            // To get the calendar instance based on the calendar identifier.
            Globalization.Calendar cultureCalendar = GetCalendar(calendarIdentifier.ToString());
            DateTime minSupportedDate = cultureCalendar.MinSupportedDateTime;
            DateTime maxSupportedDate = cultureCalendar.MaxSupportedDateTime;

            // Return false when the date is not in the supported range.
            if (!IsSupportedDate(dateTime.Value, minSupportedDate, maxSupportedDate) || !IsSupportedDate(interactedDate.Value, minSupportedDate, maxSupportedDate))
            {
                return false;
            }

            // Get year and month value for the dateTime corresponding to the specified calendar identifier.
            int year = cultureCalendar.GetYear(dateTime.Value);
            int month = cultureCalendar.GetMonth(dateTime.Value);
            int interactedYear = cultureCalendar.GetYear(interactedDate.Value);
            int interactedMonth = cultureCalendar.GetMonth(interactedDate.Value);

            switch (view)
            {
                case CalendarView.Month:
                    return dateTime.Value.Date == interactedDate.Value.Date;
                case CalendarView.Year:
                    return month == interactedMonth && year == interactedYear;
                case CalendarView.Decade:
                    return year == interactedYear;
                case CalendarView.Century:
                    return year / 10 == interactedYear / 10;
            }

            return false;
        }

        /// <summary>
        /// Return true when the date is greater than and equal to the min date.
        /// </summary>
        /// <param name="date">Date value.</param>
        /// <param name="minDate">Min date value.</param>
        /// <returns>Return true, while date is greater than and equal to min date value.</returns>
        internal static bool IsAfterMinDate(DateTime date, DateTime minDate)
        {
            return date.Date >= minDate.Date;
        }

        /// <summary>
        /// Return true when the date is lesser than and equal to the max date.
        /// </summary>
        /// <param name="date">Date value.</param>
        /// <param name="maxDate">Max date value.</param>
        /// <returns>Return true, while the date is lesser than and equal to the max date.</returns>
        internal static bool IsBeforeMaxDate(DateTime date, DateTime maxDate)
        {
            return date.Date <= maxDate.Date;
        }

        /// <summary>
        /// Return true when the date is in the supported range(between min and max date).
        /// </summary>
        /// <param name="date">Date value.</param>
        /// <param name="minDate">Min date value.</param>
        /// <param name="maxDate">Max date value.</param>
        /// <returns>Return true, while the date is in the supported range(between min and max date).</returns>
        internal static bool IsSupportedDate(DateTime date, DateTime minDate, DateTime maxDate)
        {
            return IsAfterMinDate(date, minDate) && IsBeforeMaxDate(date, maxDate);
        }

        /// <summary>
        /// Method to get the hover color from selection color.
        /// </summary>
        /// <param name="hoveringBackground">The hovering background.</param>
        /// <param name="cellTemplate">The month or year cell template.</param>
        /// <returns>Returns hovering color.</returns>
        internal static Color GetMouseHoverColor(Brush? hoveringBackground, DataTemplate? cellTemplate)
        {
            if (hoveringBackground?.ToColor() == Colors.Transparent)
            {
                if (cellTemplate != null)
                {
                    return Colors.Transparent;
                }

                return Colors.Gray.WithAlpha(0.1f);
            }

            if (hoveringBackground != null)
            {
                return hoveringBackground.ToColor().WithAlpha(0.1f);
            }

            return Color.FromArgb("#735EAB").WithAlpha(0.1f);
        }

        /// <summary>
        /// Method to get the hover dash line color from selection color.
        /// </summary>
        /// <param name="hoveringBackground">The hovering background.</param>
        /// <returns>Returns hovering dash line color.</returns>
        internal static Color GetMouseHoverDashlineColor(Brush? hoveringBackground)
        {
            if (hoveringBackground == Brush.Transparent)
            {
                return Colors.Gray.WithAlpha(0.25f);
            }

            if (hoveringBackground != null)
            {
                return hoveringBackground.ToColor().WithAlpha(0.25f);
            }

            return Color.FromArgb("#735EAB").WithAlpha(0.25f);
        }

        /// <summary>
        /// Method to restrict the display date changed. The method return true means the display date is present in the current view so no need to update the display date.
        /// Example:Assume display date = 2022/08/05 the display date value changed to 2022/08/10. Previous display date of month, year and newly assigned display date of month and year is same.
        /// Then it returns true. Other wise return false.
        /// </summary>
        /// <param name="calendar">The calendar information.</param>
        /// <param name="visibleDates">The visible dates</param>
        /// <param name="previousDisplayDate">The previous display date</param>
        /// <param name="displayDate">The current displayDate.</param>
        /// <returns>Returns the true or false.</returns>
        internal static bool IsSameDisplayDateView(ICalendar calendar, List<DateTime> visibleDates, DateTime previousDisplayDate, DateTime displayDate)
        {
            // To get the calendar instance based on the calendar identifier.
            Globalization.Calendar cultureCalendar = GetCalendar(calendar.Identifier.ToString());

            DateTime startDateRange = cultureCalendar.MinSupportedDateTime.Date;
            DateTime endDateRange = cultureCalendar.MaxSupportedDateTime.Date;
            int previousYear = previousDisplayDate.Year;
            int previousMonth = previousDisplayDate.Month;
            int currentYear = displayDate.Year;
            int currentMonth = displayDate.Month;

            if (IsSupportedDate(previousDisplayDate, startDateRange, endDateRange))
            {
                previousYear = cultureCalendar.GetYear(previousDisplayDate);
                previousMonth = cultureCalendar.GetMonth(previousDisplayDate);
            }

            if (IsSupportedDate(displayDate, startDateRange, endDateRange))
            {
                currentYear = cultureCalendar.GetYear(displayDate);
                currentMonth = cultureCalendar.GetMonth(displayDate);
            }

            switch (calendar.View)
            {
                case CalendarView.Month:
                    if (calendar.MonthView.GetNumberOfWeeks() == 6)
                    {
                        return previousMonth == currentMonth && previousYear == currentYear;
                    }
                    else
                    {
                        return visibleDates[0].Date <= displayDate.Date && displayDate.Date <= visibleDates[visibleDates.Count - 1].Date;
                    }

                case CalendarView.Year:
                    return previousYear == currentYear;

                case CalendarView.Decade:
                    return previousYear / 10 == currentYear / 10;

                case CalendarView.Century:
                    return previousYear / 100 == currentYear / 100;
            }

            return false;
        }

        /// <summary>
        /// Return the date time value based on date and first day of week value.
        /// This method applicable only for month view.
        /// </summary>
        /// <param name="visibleDatesCount">The visible dates count value.</param>
        /// <param name="date">The current date.</param>
        /// <param name="firstDayOfWeek">The calendar first day of week.</param>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Other views return the same date</returns>
        internal static DateTime GetFirstDayOfWeek(int visibleDatesCount, DateTime date, DayOfWeek firstDayOfWeek, CalendarIdentifier calendarIdentifier)
        {
            // Default the day of week is 7.
            int dayOfWeek = 7;

            // The condition become true While view is not a month view.
            if (visibleDatesCount % dayOfWeek != 0)
            {
                return date;
            }

            DateTime currentDate = date;
            Globalization.Calendar calendar = GetCalendar(calendarIdentifier.ToString());
            int currentYear = calendar.GetYear(currentDate);
            int currentMonth = calendar.GetMonth(currentDate);
            DateTime minDate = calendar.MinSupportedDateTime;
            int minYear = calendar.GetYear(minDate);
            int minMonth = calendar.GetMonth(minDate);

            // If the number of weeks 6 then need to render entire month.So that we need to assign the start date(1) to current date value.
            if (visibleDatesCount == 42)
            {
                currentDate = minYear == currentYear && minMonth == currentMonth ? minDate : new DateTime(currentYear, currentMonth, 1, calendar);
            }

            // Get difference between current day of week and day of week.
            // Example: Assume current date is 25 may 2022. First day of week is Sunday(1).The default day of week is 7.
            // Current date(25th) of day of week is 4.
            // As per calculation value = -4(currentDate(25) of day of week)+1(First day of week(Sunday(1))) -7(Default day of week) = -10(value)
            int value = -(int)calendar.GetDayOfWeek(currentDate) + (int)firstDayOfWeek - dayOfWeek;

            // Condition become false while value is lesser than 7.
            // From example: Math.Abs(-10 >= 7) condition true.
            if (Math.Abs(value) >= dayOfWeek)
            {
                // From example: -10(value) += 7(Day of week). The answer is -3.
                // The actual difference between first day of week and current date of week = -3.
                value += dayOfWeek;
            }

            // Add value to the current date to get the first day of week.
            // From example: 25(currentDate).AddDays(-3(value)). Finally We get first day of week(22).
            currentDate = calendar.AddDays(currentDate, value);
            return currentDate;
        }

        /// <summary>
        /// Method to calculate date text
        /// </summary>
        /// <param name="dateTime">The year cell date time</param>
        /// <param name="calendar">The calendar view.</param>
        /// <param name="isAccessibility">The accessibility is enabled or not.</param>
        /// <returns>Returns the date time</returns>
        internal static string GetYearCellText(DateTime dateTime, ICalendarYear calendar, bool isAccessibility = false)
        {
            // To get the calendar instance based on the calendar identifier.
            Globalization.Calendar cultureCalendar = GetCalendar(calendar.Identifier.ToString());

            // Get the year based on the calendar identifier.
            int year = cultureCalendar.GetYear(dateTime);

            // Convert the minimum date to year on culture basis.
            // Need to check the minimum and maximum date to present min and max value, if the min and max value is given convert that date on culture basis
            // or else need to get the min and max date with the calendar instance for the specified culture.
            int startDateRange = cultureCalendar.GetYear(cultureCalendar.MinSupportedDateTime);
            int endDateRange = cultureCalendar.GetYear(cultureCalendar.MaxSupportedDateTime);
            CultureInfo cultureInfo = GetCurrentUICultureInfo(calendar.Identifier);
            if (calendar.View == CalendarView.Year)
            {
                if (isAccessibility)
                {
                    return dateTime.ToString("MMMM/yyyy", cultureInfo);
                }

                return dateTime.ToString(calendar.YearView.MonthFormat, cultureInfo);
            }
            else if (calendar.View == CalendarView.Decade)
            {
                return dateTime.ToString("yyyy", cultureInfo);
            }
            else
            {
                if (year == startDateRange)
                {
                    //// To get the century date range by adding 8 to the dateTime while the year is 1.
                    //// because it need to show the text as 0001 - 0009.
                    if (isAccessibility)
                    {
                        return dateTime.ToString("yyyy", cultureInfo) + " to " + cultureCalendar.AddYears(dateTime, (startDateRange / 10 * 10) + 10 - startDateRange - 1).ToString("yyyy", cultureInfo);
                    }

                    return dateTime.ToString("yyyy", cultureInfo) + " - " + cultureCalendar.AddYears(dateTime, (startDateRange / 10 * 10) + 10 - startDateRange - 1).ToString("yyyy", cultureInfo);
                }
                //// If the added year is greater than the end date range need to add the remaining date within the end range.
                else if ((year + 9) > endDateRange)
                {
                    int addedYear = endDateRange - year;
                    if (isAccessibility)
                    {
                        return dateTime.ToString("yyyy", cultureInfo) + " to " + cultureCalendar.AddYears(dateTime, addedYear).ToString("yyyy", cultureInfo);
                    }

                    return dateTime.ToString("yyyy", cultureInfo) + " - " + cultureCalendar.AddYears(dateTime, addedYear).ToString("yyyy", cultureInfo);
                }

                if (isAccessibility)
                {
                    return dateTime.ToString("yyyy", cultureInfo) + " to " + cultureCalendar.AddYears(dateTime, 9).ToString("yyyy", cultureInfo);
                }

                //// To get the century date range by adding 9 to the dateTime
                return dateTime.ToString("yyyy", cultureInfo) + " - " + cultureCalendar.AddYears(dateTime, 9).ToString("yyyy", cultureInfo);
            }
        }

        /// <summary>
        /// Method to get the date on touch interaction in month view.
        /// </summary>
        /// <param name="point">The interacted point.</param>
        /// <param name="width">The width of the view.</param>
        /// <param name="height">The height of the view.</param>
        /// <param name="calendar">The calendar info.</param>
        /// <param name="visibleDates">The visible dates collection.</param>
        /// <returns>Returns the interacted date.</returns>
        internal static DateTime? GetMonthDateFromPosition(Point point, double width, double height, ICalendarMonth calendar, List<DateTime> visibleDates)
        {
            int daysPerWeek = 7;
            double weekNumberWidth = GetWeekNumberWidth(calendar.MonthView, (float)width);
            double monthViewWidth = width - weekNumberWidth;
            double monthViewHeight = height;
            bool isRTL = calendar.IsRTLLayout;
            //// xPosition is used to find the tap point in the x-axis.
            //// Example- Total width = 100 and weekNumberWidth = 20
            //// monthViewWidth = Total width - weekNumberWidth = 100 - 20 = 80
            //// LTR- tapPoint.X = 10, tapPoint.X - weekNumberWidth = 10 - 20 = -10
            //// RTL- tapPoint.X = 90, monthViewWidth - tapPoint.X = 80 - 90 = -10
            //// Tapped rows and columns cannot be calculated when the tap point is not in the calendar cell element.
            double xPosition = isRTL ? monthViewWidth - point.X : point.X - weekNumberWidth;
            double yPosition = point.Y;
            if (xPosition < 0 || yPosition < 0)
            {
                return null;
            }

            int numberOfWeeks = calendar.MonthView.GetNumberOfWeeks();
            int row = (int)(yPosition / (monthViewHeight / numberOfWeeks));
            int column = (int)(xPosition / (monthViewWidth / daysPerWeek));
            if (row >= numberOfWeeks || column >= daysPerWeek)
            {
                return null;
            }

            int dateIndex = (row * daysPerWeek) + column;
            int visibleDatesCount = visibleDates.Count;
            //// The visible dates count not a multiple value of 7 and number of weeks then need to render the date of day of week based on the first day of week basis.
            //// Example: Assume display date is 0001,01,01 first day of week is Tuesday(Enum Value = 2) and number of weeks is 2.
            //// From above scenario the visible dates contains from 0001,1,1 to 0001,1,8.
            //// The visible dates[0] contains 0001,1,1 date and first day of week is Monday.
            //// Here condition is true while visible date count is 8. Calculation 8 % 7 != 0.
            if (visibleDatesCount % 7 != 0)
            {
                DateTime currentDate = visibleDates.First();
                int startIndex = GetFirstDayOfWeekDifference(currentDate, calendar.MonthView.FirstDayOfWeek, calendar.Identifier);
                dateIndex = dateIndex - startIndex;
            }

            //// If tapped empty cell does not need to trigger the tapped event.
            //// Condition true from example -2 < 0.
            //// It means if interacted with the empty cell return the null value.
            if (dateIndex < 0 || visibleDatesCount <= dateIndex)
            {
                return null;
            }

            DateTime date = visibleDates[dateIndex];
            return date;
        }

        /// <summary>
        /// Method to get the date on touch interaction in year view.
        /// </summary>
        /// <param name="point">The interacted point.</param>
        /// <param name="width">The width of the view.</param>
        /// <param name="height">The height of the view.</param>
        /// <param name="calendar">The calendar info.</param>
        /// <param name="visibleDates">The visible dates collection.</param>
        /// <returns>Returns the interacted date.</returns>
        internal static DateTime? GetYearDateFromPosition(Point point, double width, double height, ICalendarYear calendar, List<DateTime> visibleDates)
        {
            int rowCount = 4;
            int columnCount = 3;
            bool isRTL = calendar.IsRTLLayout;
            double xPosition = isRTL ? width - point.X : point.X;
            double yPosition = point.Y;
            int tappedRow = (int)(yPosition / (height / rowCount));
            int tappedColumn = (int)(xPosition / (width / columnCount));
            if (tappedRow >= rowCount || tappedColumn >= columnCount)
            {
                return null;
            }

            int tappedDateIndex = (tappedRow * columnCount) + tappedColumn;
            //// Return null while interact with empty cells shown on year view(reaching max date - 9999 year).
            if (tappedDateIndex >= visibleDates.Count && visibleDates.Count < rowCount * columnCount)
            {
                return null;
            }

            DateTime tappedDate = tappedDateIndex < visibleDates.Count ? visibleDates[tappedDateIndex] : visibleDates[visibleDates.Count - 1];
            return tappedDate;
        }

        /// <summary>
        /// Method to check whether the interaction is enabled for the date.
        /// </summary>
        /// <param name="date">The interacted date.</param>
        /// <param name="disabledDates">The disabled dates collection.</param>
        /// <param name="visibleDates">The visible dates collection.</param>
        /// <param name="calendar">The calendar info.</param>
        /// <param name="numberOfWeeks">The number of weeks in view.</param>
        /// <returns>Returns true if interaction enabled, false if interaction disabled.</returns>
        internal static bool IsInteractableDate(DateTime date, List<DateTime> disabledDates, List<DateTime> visibleDates, ICalendarCommon calendar, int numberOfWeeks)
        {
            //// If the interacted date is disabled date then the date must be non interactable.
            if (IsDisabledDate(date, calendar.View, calendar.EnablePastDates, calendar.MinimumDate, calendar.MaximumDate, calendar.SelectionMode, calendar.RangeSelectionDirection, calendar.SelectedDateRange, calendar.AllowViewNavigation, calendar.Identifier))
            {
                return false;
            }

            DateTime currentViewDate = calendar.View == CalendarView.Month ? visibleDates[visibleDates.Count / 2] : visibleDates[0];
            if (calendar.View == CalendarView.Month)
            {
                //// If number of weeks is equal to 6 and if tapped date is leading or trailing date and show leading and trailing dates is false then the date must be non interactable.
                if (numberOfWeeks == 6 && !calendar.ShowTrailingAndLeadingDates && IsLeadingAndTrailingDate(date, currentViewDate, calendar.View, calendar.Identifier))
                {
                    return false;
                }
            }
            else
            {
                if (!calendar.ShowTrailingAndLeadingDates && IsLeadingAndTrailingDate(date, currentViewDate, calendar.View, calendar.Identifier))
                {
                    return false;
                }
            }

            //// If the interacted date is Blackout date then the date must be non interactable.
            if (IsDateInDateCollection(date, disabledDates))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method to find the date is greater date on the basis of view
        /// </summary>
        /// <param name="interactedDate">The interacted date</param>
        /// <param name="calendarView">The calendar view</param>
        /// <param name="date">The date time to compare</param>
        /// <param name="calendarIdentifier">Calendar identifier value.</param>
        /// <returns>Returns true when the interacted date is greater than the dateTime</returns>
        internal static bool IsGreaterDate(this DateTime? interactedDate, CalendarView calendarView, DateTime? date, CalendarIdentifier calendarIdentifier)
        {
            if (date == null || interactedDate == null)
            {
                return false;
            }

            // To get the calendar instance based on the calendar identifier.
            Globalization.Calendar cultureCalendar = GetCalendar(calendarIdentifier.ToString());
            DateTime minSupportedDate = cultureCalendar.MinSupportedDateTime;
            DateTime maxSupportedDate = cultureCalendar.MaxSupportedDateTime;

            // Return date comparison when the date is not in the supported range.
            if (!IsSupportedDate(date.Value, minSupportedDate, maxSupportedDate) || !IsSupportedDate(interactedDate.Value, minSupportedDate, maxSupportedDate))
            {
                return date.Value.Date < interactedDate.Value.Date;
            }

            // Get the year and month based on the calendar identifier.
            int interactedYear = cultureCalendar.GetYear(interactedDate.Value);
            int interactedMonth = cultureCalendar.GetMonth(interactedDate.Value);
            int year = cultureCalendar.GetYear(date.Value);
            int month = cultureCalendar.GetMonth(date.Value);

            switch (calendarView)
            {
                case CalendarView.Month:
                    return date.Value.Date < interactedDate.Value.Date;

                case CalendarView.Year:
                    return year < interactedYear || (year == interactedYear && month < interactedMonth);

                case CalendarView.Decade:
                    return year < interactedYear;

                case CalendarView.Century:
                    return year / 10 < interactedYear / 10;
            }

            return false;
        }

        /// <summary>
        /// Method to get the date is inBetween selected range
        /// </summary>
        /// <param name="view">The calendar view</param>
        /// <param name="selectedDateRange">The selected range of the calendar</param>
        /// <param name="dateTime">The date time</param>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Returns true when the dateTime is inBetween range</returns>
        internal static bool IsInBetweenSelectedRange(CalendarView view, CalendarDateRange? selectedDateRange, DateTime? dateTime, CalendarIdentifier calendarIdentifier)
        {
            if (selectedDateRange?.StartDate == null || selectedDateRange?.EndDate == null || dateTime == null)
            {
                return false;
            }

            // To check the dateTime is in between the selected start and end date range based on the view.
            return dateTime.IsGreaterDate(view, selectedDateRange.StartDate, calendarIdentifier) && selectedDateRange.EndDate.IsGreaterDate(view, dateTime, calendarIdentifier);
        }

        /// <summary>
        /// Method to check the ranges are same.
        /// </summary>
        /// <param name="view">The calendar view</param>
        /// <param name="previousRange">The previous date range</param>
        /// <param name="currentRange">The current date of the calendar</param>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Returns true when the ranges are same</returns>
        internal static bool IsSameRange(CalendarView view, CalendarDateRange? previousRange, CalendarDateRange? currentRange, CalendarIdentifier calendarIdentifier)
        {
            if (previousRange == currentRange)
            {
                return true;
            }

            if (previousRange == null || currentRange == null)
            {
                return false;
            }

            DateTime? previousStartDate = previousRange.StartDate;
            DateTime? previousEndDate = previousRange.EndDate;

            //// If the previous range end Date is null, need to assign the previous range start date as the end date
            if (previousEndDate == null)
            {
                previousEndDate = previousStartDate;
            }

            DateTime? currentStartDate = currentRange.StartDate;
            DateTime? currentEndDate = currentRange.EndDate;

            //// If the current range end Date is null, need to assign the current range start date as the end date
            if (currentEndDate == null)
            {
                currentEndDate = currentStartDate;
            }

            // The old range selection and new range selection are equal then no need to update the UI.
            // Same Year for previous start(Jan1 2022) and current start(Jan1 2022 or Feb1 2022)
            // Same Year for previous end(Jan1 2022 or Feb1 2023) and current end(Jan1 2022 or Feb1 2023)
            // Diff Year for previous start and current start but Same previous start(Jan1 2022) and current end(Jan15 2022)
            // Diff Year for previous end and current end but Same previous end(Feb15 2023) and current start(Feb1 2023)
            return (IsSameDate(view, previousStartDate, currentStartDate, calendarIdentifier) && IsSameDate(view, previousEndDate, currentEndDate, calendarIdentifier)) ||
                (IsSameDate(view, previousStartDate, currentEndDate, calendarIdentifier) && IsSameDate(view, previousEndDate, currentStartDate, calendarIdentifier));
        }

        /// <summary>
        /// Method to check the date is in between the first and last date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="firstDate">The first date.</param>
        /// <param name="lastDate">The last date.</param>
        /// <returns>Returns true while the date in between first and last date.</returns>
        internal static bool IsDateWithinDateRange(DateTime? date, DateTime? firstDate, DateTime? lastDate)
        {
            return date?.Date >= firstDate?.Date && date?.Date <= lastDate?.Date;
        }

        /// <summary>
        /// Method to get first date of the date value.
        /// </summary>
        /// <param name="dateTime">The date value.</param>
        /// <param name="calendarView">The calendar view.</param>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Returns the start date of the date.</returns>
        internal static DateTime? GetStartDate(DateTime? dateTime, CalendarView calendarView, CalendarIdentifier calendarIdentifier)
        {
            if (dateTime == null)
            {
                return null;
            }

            // To get the calendar instance based on the calendar identifier.
            Globalization.Calendar cultureCalendar = GetCalendar(calendarIdentifier.ToString());

            DateTime startDateTime = cultureCalendar.MinSupportedDateTime;
            DateTime endDateTime = cultureCalendar.MaxSupportedDateTime;
            if (!IsSupportedDate(dateTime.Value, startDateTime, endDateTime))
            {
                return dateTime;
            }

            //// Get the year, month and day based on the calendar identifier.
            int startRangeYear = cultureCalendar.GetYear(startDateTime);
            int startRangeMonth = cultureCalendar.GetMonth(startDateTime);

            int year = cultureCalendar.GetYear(dateTime.Value);
            int month = cultureCalendar.GetMonth(dateTime.Value);

            // If the date time year lesser than the start range year, need to start form the start date.
            if (year <= startRangeYear)
            {
                dateTime = startDateTime;
            }

            switch (calendarView)
            {
                case CalendarView.Month:
                    return dateTime;
                case CalendarView.Year:
                    return startRangeYear == year && startRangeMonth == month ? startDateTime : new DateTime(year, month, 1, cultureCalendar);
                case CalendarView.Decade:
                    return startRangeYear == year ? startDateTime : new DateTime(year, 1, 1, cultureCalendar);
                case CalendarView.Century:
                    //// Return 0001 year start date while the year cell date year lesser than 9.
                    //// because each time we return date divided by 10 but there is no 0th year.
                    if (year < (startRangeYear / 10 * 10) + 10 - 1)
                    {
                        return startDateTime;
                    }
                    else
                    {
                        return new DateTime(year / 10 * 10, 1, 1, cultureCalendar);
                    }
            }

            return dateTime;
        }

        /// <summary>
        /// Method to get the view last date of the date based on the Calendar view
        /// </summary>
        /// <param name="view">The calendar view</param>
        /// <param name="date">The tapped date through interaction</param>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Returns the last date of view</returns>
        internal static DateTime? GetLastDate(CalendarView view, DateTime? date, CalendarIdentifier calendarIdentifier)
        {
            if (date == null)
            {
                return null;
            }

            // To get the calendar instance based on the calendar identifier.
            Globalization.Calendar cultureCalendar = GetCalendar(calendarIdentifier.ToString());
            DateTime endDateTime = cultureCalendar.MaxSupportedDateTime;
            DateTime startDateTime = cultureCalendar.MinSupportedDateTime.Date;

            if (!IsSupportedDate(date.Value, startDateTime, endDateTime))
            {
                return date;
            }

            int maxDateYear = cultureCalendar.GetYear(endDateTime);
            int maxDateMonth = cultureCalendar.GetMonth(endDateTime);

            // Get the year based on the calendar identifier.
            int year = cultureCalendar.GetYear(date.Value);
            int month = cultureCalendar.GetMonth(date.Value);

            int minYear = cultureCalendar.GetYear(startDateTime);

            switch (view)
            {
                case CalendarView.Month:
                    return date;

                case CalendarView.Year:
                    return maxDateYear == year && maxDateMonth == month ? endDateTime : new DateTime(year, month, cultureCalendar.GetDaysInMonth(year, month), cultureCalendar);

                case CalendarView.Decade:
                    return maxDateYear == year ? endDateTime : new DateTime(year, month, cultureCalendar.GetDaysInMonth(year, month), cultureCalendar);

                case CalendarView.Century:
                    {
                        //// If the year is min year(0001) then need to return 0009 year end date by adding 8 year.
                        if (year == minYear)
                        {
                            int newDateYear = cultureCalendar.GetYear(cultureCalendar.AddYears(date.Value, (minYear / 10 * 10) + 10 - minYear - 1));
                            return new DateTime(newDateYear, 12, cultureCalendar.GetDaysInMonth(newDateYear, 12), cultureCalendar);
                        }

                        //// If the year is max year then need to return max date.
                        //// 9 refers the end date in century cell. Century cell contains 10 years span.
                        if (year + 9 >= maxDateYear)
                        {
                            return endDateTime.Date;
                        }

                        date = new DateTime(date.Value.Year / 10 * 10, 1, 1);
                        int newYear = cultureCalendar.GetYear(cultureCalendar.AddYears(date.Value, 9));
                        return new DateTime(newYear, 12, cultureCalendar.GetDaysInMonth(newYear, 12), cultureCalendar);
                    }
            }

            return null;
        }

        /// <summary>
        /// Method to get the visible last date of view based on calendar view without leading dates.
        /// </summary>
        /// <param name="view">The calendar view.</param>
        /// <param name="date">The date value.</param>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Returns the last date of view</returns>
        internal static DateTime GetViewLastDate(CalendarView view, DateTime date, CalendarIdentifier calendarIdentifier)
        {
            // To get the calendar instance based on the calendar identifier.
            Globalization.Calendar cultureCalendar = GetCalendar(calendarIdentifier.ToString());

            // Get the year based on the calendar identifier.
            DateTime maxDate = cultureCalendar.MaxSupportedDateTime;
            int maxYear = cultureCalendar.GetYear(maxDate);
            int year = cultureCalendar.GetYear(date);
            int month = cultureCalendar.GetMonth(date);

            switch (view)
            {
                case CalendarView.Month:
                    return new DateTime(year, month, cultureCalendar.GetDaysInMonth(year, month), cultureCalendar);

                case CalendarView.Year:
                    return maxYear == year ? maxDate : new DateTime(year, 12, 1, cultureCalendar);

                case CalendarView.Decade:
                    {
                        int lastDateYear = (year / 10 * 10) + 10 - 1;
                        if (lastDateYear >= maxYear)
                        {
                            return new DateTime(maxYear, 1, 1, cultureCalendar);
                        }

                        return new DateTime(lastDateYear, 1, 1, cultureCalendar);
                    }

                case CalendarView.Century:
                    {
                        int lastDateYear = (year / 100 * 100) + 100 - 1;
                        if (lastDateYear >= maxYear)
                        {
                            return new DateTime(maxYear, 1, 1, cultureCalendar);
                        }

                        return new DateTime(lastDateYear, 1, 1, cultureCalendar);
                    }
            }

            return date;
        }

        /// <summary>
        /// Method to get the different between the current date of day of week and first day of week property day of week.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <param name="dayOfWeek">The first day of week day of week.</param>
        /// <param name="calendarIdentifier">The calendar Identifier.</param>
        /// <returns>It returns different between the day of weeks.</returns>
        internal static int GetFirstDayOfWeekDifference(DateTime currentDate, DayOfWeek dayOfWeek, CalendarIdentifier calendarIdentifier)
        {
            Globalization.Calendar cultureCalendar = GetCalendar(calendarIdentifier.ToString());

            // Get difference between currentDate of day of week and day of week.
            // currentDate.DayOfWeek = Monday(1). And first day of Week = Tuesday(2).
            // value = -1+ (2-7) = -6(value).
            int value = (-(int)cultureCalendar.GetDayOfWeek(currentDate)) + ((int)dayOfWeek - 7);

            // Condition become false while value is lesser than 7.
            // From example: Math.Abs(-6 >= 7) condition false.
            // To get the valid value by subtracting 7(days per week).
            if (Math.Abs(value) >= 7)
            {
                value += 7;
                return Math.Abs(value);
            }

            return Math.Abs(value);
        }

        /// <summary>
        /// Method to trim the text based on available width.
        /// </summary>
        /// <param name="text">The text to trim.</param>
        /// <param name="width">The available width.</param>
        /// <param name="textStyle">The text style.</param>
        /// <returns>Returns the text for the available width.</returns>
        internal static string TrimText(string text, float width, CalendarTextStyle textStyle)
        {
            Size textSize = text.Measure(textStyle);
            if (textSize.Width < width)
            {
                return text;
            }

            string textTrim = text;
            while ((int)textSize.Width + 1 > width)
            {
                textTrim = textTrim.Substring(0, textTrim.Length - 1);
                textSize = (textTrim + "..").Measure((float)textStyle.FontSize, textStyle.FontAttributes, textStyle.FontFamily);
            }

            return textTrim + "..";
        }

        /// <summary>
        /// Method to get the header height.
        /// </summary>
        /// <param name="calendarHeaderView">The calendar header view</param>
        /// <returns>Returns the header height.</returns>
        internal static double GetHeaderHeight(this CalendarHeaderView calendarHeaderView)
        {
            if (calendarHeaderView.Height > 0)
            {
                return calendarHeaderView.Height;
            }

            return 0;
        }

        /// <summary>
        /// Method to get the footer height.
        /// </summary>
        /// <param name="calendarFooterView">The calendar footer view.</param>
        /// <returns>Returns the footer height.</returns>
        internal static double GetFooterHeight(this CalendarFooterView calendarFooterView)
        {
            if (calendarFooterView.Height > 0)
            {
                return calendarFooterView.Height;
            }

            return 0;
        }

        /// <summary>
        /// Method to get the view header height.
        /// </summary>
        /// <param name="calendarMonthHeaderView">The calendar month header view</param>
        /// <returns>Returns the view header height.</returns>
        internal static double GetViewHeaderHeight(this CalendarMonthHeaderView calendarMonthHeaderView)
        {
            if (calendarMonthHeaderView.Height > 0)
            {
                return calendarMonthHeaderView.Height;
            }

            return 0;
        }

        /// <summary>
        /// Method to return always true when the number of visible weeks value not equal to 6.
        /// </summary>
        /// <param name="calendarViewInfo">To access the number of visible weeks and showLeadingandtrailingdates</param>
        /// <returns>Boolean number of visible weeks value not equal to 6.</returns>
        internal static bool ShowTrailingLeadingDates(ICalendar calendarViewInfo)
        {
            int numberOfVisibleWeeks = GetNumberOfWeeks(calendarViewInfo.MonthView);
            if (numberOfVisibleWeeks == 6)
            {
                return calendarViewInfo.ShowTrailingAndLeadingDates;
            }

            return true;
        }

        /// <summary>
        /// Method to create the template views
        /// </summary>
        /// <param name="template">The template parameter</param>
        /// <param name="details">The binding context details.</param>
        /// <returns>Returns the view from the view template.</returns>
        internal static View CreateTemplateView(DataTemplate template, object? details)
        {
            View view;
            var content = template.CreateContent();

            //// Inside a data template we can have view, layout and ViewCell directly.
            //// If the template has view cell as it direct content then we cannot cast it to view.
            //// All layouts, controls and views are base type of view but ViewCell is not type of view,
            //// hence we need to get the view from view cell.
#if NET9_0
            if (content is ViewCell)
            {
                view = ((ViewCell)content).View;
            }
            else
            {
                view = (View)content;
            }
#elif NET10_0
            view = (View)content;
#endif

            if (view.BindingContext == null && details != null)
            {
                view.BindingContext = details;
            }

            return view;
        }

        /// <summary>
        /// To get the current culture for the calendar.
        /// </summary>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Returns the current culture.</returns>
        internal static CultureInfo GetCurrentUICultureInfo(CalendarIdentifier calendarIdentifier)
        {
            if (IsGregorianCalendar(calendarIdentifier))
            {
                return SfCalendarResources.CultureInfo;
            }

            //// If the calendar identifier is not gregorian then need to get the specific culture for the calendar.
            return GetCultureInfo(calendarIdentifier.ToString());
        }

        /// <summary>
        /// Method to identify the default calendar is Gregorian or not.
        /// </summary>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Returns true if the calendar identifier is gregorian.</returns>
        internal static bool IsGregorianCalendar(CalendarIdentifier calendarIdentifier)
        {
            if (calendarIdentifier == CalendarIdentifier.Gregorian)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to get culture info based on calendar identifier.
        /// </summary>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Returns the specific culture for the calendar.</returns>
        internal static CultureInfo GetCultureInfo(string calendarIdentifier)
        {
            var language = GetLanguage(calendarIdentifier);

            //// If the language is not null or empty then need to get the specific culture with the language for the calendar.
            if (string.IsNullOrEmpty(language))
            {
                return CultureInfo.CurrentUICulture;
            }
#if NET9_0_OR_GREATER && (MACCATALYST || IOS)
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(language);
            if (calendarIdentifier == "Hijri")
            {
                cultureInfo.DateTimeFormat.Calendar = GetCalendar(calendarIdentifier);
            }
#else
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(language);
            cultureInfo.DateTimeFormat.Calendar = GetCalendar(calendarIdentifier);
#endif
            return cultureInfo;
        }

        /// <summary>
        /// Method to get language string for the specified calendar identifier.
        /// </summary>
        /// <param name="calendarIdentifier">The calendar identifier specified.</param>
        /// <returns>Returns the respective language string for the calendar identifier.</returns>
        internal static string GetLanguage(string calendarIdentifier)
        {
            switch (calendarIdentifier)
            {
                case "Hebrew":
                    return "he-IL";
                case "Hijri":
                case "UmAlQura":
                    return "ar-SA";
                case "Korean":
                    return "ko-KR";
                case "Persian":
                    return "fa";
                case "Taiwan":
                    return "zh-TW";
                case "ThaiBuddhist":
                    return "th-TH";
                case "Japanese":
                    return "ja-JP";
            }

            return string.Empty;
        }

		/// <summary>
		/// Get calendar instance using its calendar identifier.
		/// </summary>
		/// <param name="calendarIdentifier">The name of the calendar.</param>
		/// <returns>A calendar instance.</returns>
		internal static Globalization.Calendar GetCalendar(string calendarIdentifier)
		{
			switch (calendarIdentifier)
			{

				case "Gregorian":

					return new GregorianCalendar();

				case "Hijri":

					return new HijriCalendar();

				case "Persian":

					return new PersianCalendar();

				case "ThaiBuddhist":

					return new ThaiBuddhistCalendar();

				case "Taiwan":

					return new TaiwanCalendar();

				case "UmAlQura":

					return new UmAlQuraCalendar();

				case "Korean":

					return new KoreanCalendar();

				default:

					// If calendar identifier is specified wrongly, then default calendar will be used.

					return CultureInfo.CurrentUICulture.DateTimeFormat.Calendar;

			}
		}

        /// <summary>
        /// Method to get the non valid week(first row without monday) while reaching the min date of the month.
        /// </summary>
        /// <param name="cultureCalendar">The culture calendar.</param>
        /// <param name="startDate">The week start date.</param>
        /// <param name="firstDayOfWeek">The first day of the week.</param>
        /// <param name="daysPerWeek">The days per week value.</param>
        /// <returns>Returns true when the first row contains monday.</returns>
        internal static bool IsInValidWeekNumberWeek(Globalization.Calendar cultureCalendar, DateTime startDate, DayOfWeek firstDayOfWeek, int daysPerWeek)
        {
            // Get the day of week for the startDateWeek.
            DayOfWeek startDateWeek = cultureCalendar.GetDayOfWeek(startDate);
            //// If the start date of the week is monday, then no need to check for the non valid week.
            if (startDateWeek == DayOfWeek.Monday)
            {
                return true;
            }

            //// To get the number of available days in the first row on the basis of first day of the week.
            //// Example: startDateWeek is (7/18/0622) and startDate DayOfWeek is Thursday(4) in hijri Identiifer.
            //// Assume if the FirstDayOfWeek is Monday, then the first row contains 4 days.
            //// daysPerWeek(7) - (int)startDateWeek(4) + (int)firstDayOfWeek(1) = 4. So, The available value is 4.
            int numberOfAvailableDays = daysPerWeek - (int)startDateWeek + (int)firstDayOfWeek;
            numberOfAvailableDays = numberOfAvailableDays > daysPerWeek ? numberOfAvailableDays - daysPerWeek : numberOfAvailableDays;
            for (int i = 0; i < numberOfAvailableDays; i++)
            {
                if (cultureCalendar.GetDayOfWeek(startDate.AddDays(i)) == DayOfWeek.Monday)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Method to check the two date ranges are same in the collection.
        /// </summary>
        /// <param name="view">The calendar view.</param>
        /// <param name="previousRanges">The previous selected range.</param>
        /// <param name="currentRanges">The current selected ranges.</param>
        /// <param name="calendarIdentifier">The calendar identifier.</param>
        /// <returns>Returns true if the ranges same.</returns>
        internal static bool IsSameDateRanges(CalendarView view, ObservableCollection<CalendarDateRange>? previousRanges, ObservableCollection<CalendarDateRange>? currentRanges, CalendarIdentifier calendarIdentifier)
        {
            ObservableCollection<CalendarDateRange>? previousCollection = previousRanges?.Count == 0 ? null : previousRanges;
            ObservableCollection<CalendarDateRange>? currentCollection = currentRanges?.Count == 0 ? null : currentRanges;

            //// If the previousCollection and currentCollection are null, then return true.
            if (previousCollection == currentCollection)
            {
                return true;
            }

            //// If any one of the collection is not null or the count of the collection is not same, then it does not have same ranges.
            if (previousCollection == null || currentCollection == null || previousCollection.Count != currentCollection.Count)
            {
                return false;
            }

            for (int i = 0; i < previousCollection.Count; i++)
            {
                //// If any one of the range is not same, then return false.
                if (!IsSameRange(view, previousCollection[i], currentCollection[i], calendarIdentifier))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Method to get the range is inbetween the existing range.
        /// </summary>
        /// <param name="view">The calendar view.</param>
        /// <param name="calendarDateRange">The date range.</param>
        /// <param name="newRange">The new range.</param>
        /// <param name="identifier">The calendar identifier.</param>
        /// <returns>Returns true, if the range is in between the existing range.</returns>
        internal static bool AreRangesIntercept(CalendarView view, CalendarDateRange calendarDateRange, CalendarDateRange newRange, CalendarIdentifier identifier)
        {
            DateTime? newRangeStartDate = newRange.StartDate;
            DateTime? newRangeEndDate = newRange.EndDate == null ? newRangeStartDate : newRange.EndDate;

            if (newRangeEndDate < newRangeStartDate)
            {
                DateTime? dateTime = newRangeEndDate;
                newRangeEndDate = newRangeStartDate;
                newRangeStartDate = dateTime;
            }

            DateTime? startDate = calendarDateRange.StartDate;
            DateTime? endDate = calendarDateRange.EndDate == null ? startDate : calendarDateRange.EndDate;

            if (endDate < startDate)
            {
                DateTime? dateTime = endDate;
                endDate = startDate;
                startDate = dateTime;
            }

            //// If the newRangeStartDate is in between the existing range, then return true. The below examples satisfies the condition.
            //// (start date - end date), (newRangeStartDate - newRangeEndDate)
            //// 1.(10/08/2023 - 20/08/2023), (10/08/2023 - 12/08/2023)
            //// 2.(20/08/2023 - 10/08/2023), (12/08/2023 - 09/08/2023)
            //// 3.(10/08/2023 - 20/08/2023), (05/08/2023 - 10/08/2023)
            //// 4.(10/08/2023 - 20/08/2023), (20/08/2023 - 22/08/2023)
            //// 5.(10/08/2023 - 20/08/2023), (05/08/2023 - 25/08/2023)
            //// 6.(05/08/2023 - 25/08/2023), (10/08/2023 - 20/08/2023)
            if ((IsSameDate(view, startDate, newRangeStartDate, identifier) || IsSameDate(view, endDate, newRangeStartDate, identifier) || (newRangeStartDate.IsGreaterDate(view, startDate, identifier) && endDate.IsGreaterDate(view, newRangeStartDate, identifier)))
                || (IsSameDate(view, startDate, newRangeEndDate, identifier) || IsSameDate(view, endDate, newRangeEndDate, identifier) || (newRangeEndDate.IsGreaterDate(view, startDate, identifier) && endDate.IsGreaterDate(view, newRangeEndDate, identifier)))
                || (startDate.IsGreaterDate(view, newRangeStartDate, identifier) && newRangeEndDate.IsGreaterDate(view, endDate, identifier)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to check the date is in between the range.
        /// </summary>
        /// <param name="view">The calendar view.</param>
        /// <param name="dateRange">The calendar date range.</param>
        /// <param name="dateTime">The date time.</param>
        /// <param name="identifier">The calendar identifier.</param>
        /// <returns>Returns true, if the date is in-between ranges.</returns>
        internal static bool IsDateInBetweenRanges(CalendarView view, CalendarDateRange dateRange, DateTime? dateTime, CalendarIdentifier identifier)
        {
            DateTime? startDate = dateRange.StartDate;
            DateTime? endDate = dateRange.EndDate == null ? startDate : dateRange.EndDate;

            if (startDate > endDate)
            {
                DateTime? date = startDate;
                startDate = endDate;
                endDate = date;
            }

            if (IsSameDate(view, dateTime, startDate, identifier) || IsSameDate(view, dateTime, endDate, identifier) || (dateTime.IsGreaterDate(view, startDate, identifier) && endDate.IsGreaterDate(view, dateTime, identifier)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to clone the selected date ranges.
        /// </summary>
        /// <param name="dateRanges">The date ranges collection.</param>
        /// <returns>Returns the cloned selected date ranges.</returns>
        internal static ObservableCollection<CalendarDateRange> CloneSelectedRanges(ObservableCollection<CalendarDateRange> dateRanges)
        {
            ObservableCollection<CalendarDateRange> clonedDateRanges = new ObservableCollection<CalendarDateRange>();
            foreach (CalendarDateRange range in dateRanges)
            {
                clonedDateRanges.Add(new CalendarDateRange(range.StartDate, range.EndDate));
            }

            return clonedDateRanges;
        }

        /// <summary>
        /// Method to switch the different view modes based on the keyboard keys.
        /// </summary>
        /// <param name="view">The calendar view.</param>
        /// <param name="args">The key event args.</param>
        /// <returns>Returns the calendar view.</returns>
        internal static CalendarView ProcessControlKeyPressed(CalendarView view, KeyEventArgs args)
        {
            if (args.Key == KeyboardKey.Up)
            {
                if (view == CalendarView.Month)
                {
                    view = CalendarView.Year;
                }
                else if (view == CalendarView.Year)
                {
                    view = CalendarView.Decade;
                }
                else if (view == CalendarView.Decade)
                {
                    view = CalendarView.Century;
                }
                else
                {
                    view = CalendarView.Month;
                }
            }
            else if (args.Key == KeyboardKey.Down)
            {
                if (view == CalendarView.Month)
                {
                    view = CalendarView.Century;
                }
                else if (view == CalendarView.Century)
                {
                    view = CalendarView.Decade;
                }
                else if (view == CalendarView.Decade)
                {
                    view = CalendarView.Year;
                }
                else
                {
                    view = CalendarView.Month;
                }
            }

            return view;
        }

        /// <summary>
        /// Method to get the next date for selection on key press in month view.
        /// </summary>
        /// <param name="calendar">The calendar view info.</param>
        /// <param name="args">The key event args.</param>
        /// <param name="oldSelectedDate">The previous selected date.</param>
        /// <returns>The date for selection.</returns>
        internal static DateTime? GeKeyNavigationDate(ICalendar calendar, KeyEventArgs args, DateTime oldSelectedDate)
        {
            // To get the calendar instance based on the calendar identifier.
            Globalization.Calendar cultureCalendar = GetCalendar(calendar.Identifier.ToString());

            DateTime startDateRange = cultureCalendar.MinSupportedDateTime.Date;
            DateTime endDateRange = cultureCalendar.MaxSupportedDateTime.Date;

            int startDateYear = startDateRange.Year;
            int endDateYear = endDateRange.Year;

            int startDateMonth = startDateRange.Month;
            int endDateMonth = endDateRange.Month;

            int startDateDay = startDateRange.Day;
            int endDateDay = endDateRange.Day;

            if (calendar.View == CalendarView.Month)
            {
                switch (args.Key)
                {
                    case KeyboardKey.Right:
                        if (calendar.IsRTLLayout)
                        {
                            if (oldSelectedDate.Year == startDateYear && oldSelectedDate.Month == startDateMonth && oldSelectedDate.Day - 1 < startDateDay)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year == endDateYear && oldSelectedDate.Month == endDateMonth && oldSelectedDate.Day - 1 > endDateDay)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddDays(-1);
                        }
                        else
                        {
                            if (oldSelectedDate.Year == startDateYear && oldSelectedDate.Month == startDateMonth && oldSelectedDate.Day + 1 < startDateDay)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year == endDateYear && oldSelectedDate.Month == endDateMonth && oldSelectedDate.Day + 1 > endDateDay)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddDays(1);
                        }

                    case KeyboardKey.Left:
                        if (calendar.IsRTLLayout)
                        {
                            if (oldSelectedDate.Year == startDateYear && oldSelectedDate.Month == startDateMonth && oldSelectedDate.Day + 1 < startDateDay)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year == endDateYear && oldSelectedDate.Month == endDateMonth && oldSelectedDate.Day + 1 > endDateDay)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddDays(1);
                        }
                        else
                        {
                            if (oldSelectedDate.Year == startDateYear && oldSelectedDate.Month == startDateMonth && oldSelectedDate.Day - 1 < startDateDay)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year == endDateYear && oldSelectedDate.Month == endDateMonth && oldSelectedDate.Day - 1 > endDateDay)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddDays(-1);
                        }

                    case KeyboardKey.Down:
                        if (oldSelectedDate.Year == startDateYear && oldSelectedDate.Month == startDateMonth && oldSelectedDate.Day + 7 < startDateDay)
                        {
                            return startDateRange;
                        }
                        else if (oldSelectedDate.Year == endDateYear && oldSelectedDate.Month == endDateMonth && oldSelectedDate.Day + 7 > endDateDay)
                        {
                            return endDateRange;
                        }

                        return oldSelectedDate.AddDays(7);

                    case KeyboardKey.Up:
                        if (oldSelectedDate.Year == startDateYear && oldSelectedDate.Month == startDateMonth && oldSelectedDate.Day - 7 < startDateDay)
                        {
                            return startDateRange;
                        }
                        else if (oldSelectedDate.Year == endDateYear && oldSelectedDate.Month == endDateMonth && oldSelectedDate.Day - 7 > endDateDay)
                        {
                            return endDateRange;
                        }

                        return oldSelectedDate.AddDays(-7);
                }
            }
            else if (calendar.View == CalendarView.Year)
            {
                if (oldSelectedDate.Year == startDateYear)
                {
                    oldSelectedDate = startDateRange;
                }
                else if (oldSelectedDate.Year == endDateYear)
                {
                    oldSelectedDate = endDateRange;
                }
                else
                {
                    oldSelectedDate = new DateTime(oldSelectedDate.Year, oldSelectedDate.Month, 1).Date;
                }

                switch (args.Key)
                {
                    case KeyboardKey.Right:
                        if (calendar.IsRTLLayout)
                        {
                            if (oldSelectedDate.Year == startDateYear && oldSelectedDate.Month - 1 < startDateMonth)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year == endDateYear && oldSelectedDate.Month - 1 > endDateMonth)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddMonths(-1);
                        }
                        else
                        {
                            if (oldSelectedDate.Year == startDateYear && oldSelectedDate.Month + 1 < startDateMonth)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year == endDateYear && oldSelectedDate.Month + 1 > endDateMonth)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddMonths(1);
                        }

                    case KeyboardKey.Left:
                        if (calendar.IsRTLLayout)
                        {
                            if (oldSelectedDate.Year == startDateYear && oldSelectedDate.Month + 1 < startDateMonth)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year == endDateYear && oldSelectedDate.Month + 1 > endDateMonth)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddMonths(1);
                        }
                        else
                        {
                            if (oldSelectedDate.Year == startDateYear && oldSelectedDate.Month - 1 < startDateMonth)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year == endDateYear && oldSelectedDate.Month - 1 > endDateMonth)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddMonths(-1);
                        }

                    case KeyboardKey.Down:
                        if (oldSelectedDate.Year == startDateYear && oldSelectedDate.Month + 3 < startDateMonth)
                        {
                            return startDateRange;
                        }
                        else if (oldSelectedDate.Year == endDateYear && oldSelectedDate.Month + 3 > endDateMonth)
                        {
                            return endDateRange;
                        }

                        return oldSelectedDate.AddMonths(3);

                    case KeyboardKey.Up:
                        if (oldSelectedDate.Year == startDateYear && oldSelectedDate.Month - 3 < startDateMonth)
                        {
                            return startDateRange;
                        }
                        else if (oldSelectedDate.Year == endDateYear && oldSelectedDate.Month - 3 > endDateMonth)
                        {
                            return endDateRange;
                        }

                        return oldSelectedDate.AddMonths(-3);
                }
            }
            else if (calendar.View == CalendarView.Decade)
            {
                if (oldSelectedDate.Year == startDateYear)
                {
                    oldSelectedDate = startDateRange;
                }
                else if (oldSelectedDate.Year == endDateYear)
                {
                    oldSelectedDate = endDateRange;
                }
                else
                {
                    oldSelectedDate = new DateTime(oldSelectedDate.Year, 1, 1).Date;
                }

                switch (args.Key)
                {
                    case KeyboardKey.Right:
                        if (calendar.IsRTLLayout)
                        {
                            if (oldSelectedDate.Year - 1 < startDateYear)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year - 1 > endDateYear)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddYears(-1);
                        }
                        else
                        {
                            if (oldSelectedDate.Year + 1 < startDateYear)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year + 1 > endDateYear)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddYears(1);
                        }

                    case KeyboardKey.Left:
                        if (calendar.IsRTLLayout)
                        {
                            if (oldSelectedDate.Year + 1 < startDateYear)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year + 1 > endDateYear)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddYears(1);
                        }
                        else
                        {
                            if (oldSelectedDate.Year - 1 < startDateYear)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year - 1 > endDateYear)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddYears(-1);
                        }

                    case KeyboardKey.Down:
                        if (oldSelectedDate.Year + 3 < startDateYear)
                        {
                            return startDateRange;
                        }
                        else if (oldSelectedDate.Year + 3 > endDateYear)
                        {
                            return endDateRange;
                        }

                        return oldSelectedDate.AddYears(3);

                    case KeyboardKey.Up:
                        if (oldSelectedDate.Year - 3 < startDateYear)
                        {
                            return startDateRange;
                        }
                        else if (oldSelectedDate.Year - 3 > endDateYear)
                        {
                            return endDateRange;
                        }

                        return oldSelectedDate.AddYears(-3);
                }
            }
            else if (calendar.View == CalendarView.Century)
            {
                if (oldSelectedDate.Year == startDateYear)
                {
                    oldSelectedDate = startDateRange;
                }
                else if (oldSelectedDate.Year == endDateYear)
                {
                    oldSelectedDate = endDateRange;
                }
                else
                {
                    oldSelectedDate = new DateTime(oldSelectedDate.Year / 10 * 10, 1, 1).Date;
                }

                switch (args.Key)
                {
                    case KeyboardKey.Right:
                        if (calendar.IsRTLLayout)
                        {
                            if (oldSelectedDate.Year - 10 < startDateYear)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year - 10 > endDateYear)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddYears(-10);
                        }
                        else
                        {
                            if (oldSelectedDate.Year + 10 < startDateYear)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year + 10 > endDateYear)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddYears(10);
                        }

                    case KeyboardKey.Left:
                        if (calendar.IsRTLLayout)
                        {
                            if (oldSelectedDate.Year + 10 < startDateYear)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year + 10 > endDateYear)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddYears(10);
                        }
                        else
                        {
                            if (oldSelectedDate.Year - 10 < startDateYear)
                            {
                                return startDateRange;
                            }
                            else if (oldSelectedDate.Year - 10 > endDateYear)
                            {
                                return endDateRange;
                            }

                            return oldSelectedDate.AddYears(-10);
                        }

                    case KeyboardKey.Down:
                        if (oldSelectedDate.Year + 30 < startDateYear)
                        {
                            return startDateRange;
                        }
                        else if (oldSelectedDate.Year + 30 > endDateYear)
                        {
                            return endDateRange;
                        }

                        return oldSelectedDate.AddYears(30);

                    case KeyboardKey.Up:
                        if (oldSelectedDate.Year - 30 < startDateYear)
                        {
                            return startDateRange;
                        }
                        else if (oldSelectedDate.Year - 30 > endDateYear)
                        {
                            return endDateRange;
                        }

                        return oldSelectedDate.AddYears(-30);
                }
            }

            return null;
        }

        /// <summary>
        /// Method to check the current selected date is in the current visible date collection.
        /// </summary>
        /// <param name="calendar">The calendar instance.</param>
        /// <param name="visibleDates">The visible dates.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns>Returns true if the date is in current view.</returns>
        internal static bool IsDateInCurrentVisibleDate(ICalendar calendar, List<DateTime> visibleDates, DateTime selectedDate)
        {
            // To get the calendar instance based on the calendar identifier.
            Globalization.Calendar cultureCalendar = GetCalendar(calendar.Identifier.ToString());

            DateTime startDateRange = cultureCalendar.MinSupportedDateTime.Date;
            DateTime endDateRange = cultureCalendar.MaxSupportedDateTime.Date;

            int selectedYear = selectedDate.Year;

            foreach (DateTime date in visibleDates)
            {
                int visibleYear = date.Year;
                if (IsSupportedDate(date, startDateRange, endDateRange))
                {
                    visibleYear = cultureCalendar.GetYear(date);
                }

                if (IsSupportedDate(selectedDate, startDateRange, endDateRange))
                {
                    selectedYear = cultureCalendar.GetYear(selectedDate);
                }

                switch (calendar.View)
                {
                    case CalendarView.Month:
                        if (date.Date == selectedDate.Date)
                        {
                            return true;
                        }

                        break;

                    case CalendarView.Year:
                        if (visibleYear == selectedYear)
                        {
                            return true;
                        }

                        break;

                    case CalendarView.Decade:
                        if (visibleYear / 10 == selectedYear / 10)
                        {
                            return true;
                        }

                        break;

                    case CalendarView.Century:
                        if (visibleYear / 10 * 10 == selectedYear / 10 * 10)
                        {
                            return true;
                        }

                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// Method to validate and update cell selection on key press.
        /// </summary>
        /// <param name="args">The key event args.</param>
        /// <param name="oldSelectedDate">The previous selected date.</param>
        /// <param name="newDate">The new selected date.</param>
        /// <param name="calendarViewInfo">The calendar info instance.</param>
        /// <param name="visibleDates">The visible dates collection.</param>
        /// <param name="disabledDates">The disabled date collection.</param>
        internal static void ValidateDateOnKeyNavigation(KeyEventArgs args, DateTime? oldSelectedDate, DateTime? newDate, ICalendar calendarViewInfo, List<DateTime> visibleDates, List<DateTime> disabledDates)
        {
            if (newDate == null || oldSelectedDate == null)
            {
                return;
            }

            if ((calendarViewInfo.View == CalendarView.Month && (!visibleDates.Contains(newDate.Value.Date) ||
                (calendarViewInfo.MonthView.NumberOfVisibleWeeks == 6 && oldSelectedDate.Value.Month != newDate.Value.Month && visibleDates[15].Month != newDate.Value.Month)))
                || (calendarViewInfo.View != CalendarView.Month && !IsSameDisplayDateView(calendarViewInfo, visibleDates, oldSelectedDate.Value, newDate.Value)))
            {
                calendarViewInfo.SwipeOnKeyNavigation(args, newDate.Value);
            }
            else if (!IsInteractableDate(newDate.Value.Date, disabledDates, visibleDates, calendarViewInfo, 0))
            {
                if (disabledDates.Contains(newDate.Value.Date))
                {
                    UpdateSelectionOnKeyNavigation(args, newDate.Value, calendarViewInfo, visibleDates, disabledDates);
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (calendarViewInfo.SelectionMode != CalendarSelectionMode.Range)
                {
                    calendarViewInfo.UpdateSelectedDate(newDate.Value);
                }
                else
                {
                    calendarViewInfo.UpdateRangeSelectionOnKeyboardInteraction(newDate.Value);
                }
            }
        }

        /// <summary>
        /// Return the dates out of min and max date text style.
        /// </summary>
        /// <returns>The text style.</returns>
        internal static CalendarTextStyle GetOutOfRangeDatesTextStyle()
        {
            CalendarTextStyle hideTextStyle = new CalendarTextStyle()
            {
                TextColor = Color.FromArgb("#00FFFFFF"),
            };

            return hideTextStyle;
        }

        /// <summary>
        /// Method to get the current month visible dates from the visible dates collection.
        /// </summary>
        /// <param name="visibleDates">The visible dates.</param>
        /// <returns>Returns the current month visible dates.</returns>
        internal static List<DateTime> GetCurrentMonthDates(List<DateTime> visibleDates)
        {
            int currentMonth = visibleDates[visibleDates.Count / 2].Month;
            List<DateTime> currentMonthDates = new List<DateTime>();
            foreach (DateTime date in visibleDates)
            {
                if (date.Month != currentMonth)
                {
                    continue;
                }

                currentMonthDates.Add(date);
            }

            return currentMonthDates;
        }

        /// <summary>
        /// Method to get the month view header dates.
        /// </summary>
        /// <param name="calendarInfo">The calendar view info.</param>
        /// <param name="visibleDates">The current visible dates.</param>
        /// <param name="currentMonth">The current month.</param>
        /// <param name="numberOfVisibleWeeks">The number of visible weeks value.</param>
        /// <returns>The view header date.</returns>
        internal static DateTime GetMonthViewHeaderWeekStartDates(ICalendar calendarInfo, List<DateTime> visibleDates, int currentMonth, int numberOfVisibleWeeks)
        {
            //// The month view header dates are calculated based on month visible dates. If first row contains leading days then second row dates will be considered for month view header dates.
            return visibleDates[0].Month != currentMonth && numberOfVisibleWeeks == 6 ? visibleDates[0].Date.AddDays(7) : visibleDates[0];
        }

		/// <summary>
		/// Creates a selection cell template for a calendar view based on the provided date and template information.
		/// </summary>
		/// <param name="selectedDate">The selected date for the cell template creation.</param>
		/// <param name="selectionCellTemplate">The template for the selection cell, can be a custom DataTemplate or a DataTemplateSelector.</param>
		/// <param name="templateSelectorContext">The context used for template selection (if a DataTemplateSelector is used).</param>
		/// <param name="details">A function to get the details for a specific date.</param>
		/// <param name="rect">The rectangle defining the bounds and position for the view.</param>
		/// <returns>A view representing the selection cell template, or null if the template cannot be created.</returns>
		internal static View? CreateSelectionCellTemplate(DateTime? selectedDate, DataTemplate selectionCellTemplate, BindableObject templateSelectorContext, CalendarCellDetails details, RectF rect)
		{
			// Early exit on nulls
			if (selectedDate == null || details == null || selectionCellTemplate == null)
			{
				return null;
			}

			var template = selectionCellTemplate is DataTemplateSelector selector ? selector.SelectTemplate(details, templateSelectorContext) : selectionCellTemplate;
			var selectionView = CalendarViewHelper.CreateTemplateView(template, details);
			if (selectionView is not View viewResult)
			{
				return null;
			}

			viewResult.WidthRequest = rect.Width;
			viewResult.HeightRequest = rect.Height;

			return viewResult;
		}

#endregion

		#region Private Method

		/// <summary>
		/// Method to update selected date on key navigation.
		/// </summary>
		/// <param name="args">The keyboard event args.</param>
		/// <param name="oldSelectedDate">The old selected date.</param>
		/// <param name="calendarViewInfo">The calendar instance.</param>
		/// <param name="visibleDates">The visible dates collections.</param>
		/// <param name="disabledDates">The disabled dates collections.</param>
		static void UpdateSelectionOnKeyNavigation(KeyEventArgs args, DateTime oldSelectedDate, ICalendar calendarViewInfo, List<DateTime> visibleDates, List<DateTime> disabledDates)
        {
            DateTime? newDate = CalendarViewHelper.GeKeyNavigationDate(calendarViewInfo, args, oldSelectedDate);
            ValidateDateOnKeyNavigation(args, oldSelectedDate, newDate, calendarViewInfo, visibleDates, disabledDates);
        }


        /// <summary>
        /// Get the week number based on leap year.
        /// </summary>
        /// <param name="year">The year value.</param>
        /// <returns>The week number.</returns>
        static int GetWeeksInYear(int year)
        {
            //// The extra days occur in each year which is an integer multiple of 4(except for years evenly divisible by 100, but not by 400)
            if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0))
            {
                //// If the year is leap year it has 53 weeks.
                return 53;
            }
            //// If the year is not leap year it has 52 weeks.
            return 52;
        }
        /// <summary>
        /// Day of week in ISO is represented by an integer from 1 through 7, beginning with Monday and ending with sunday. This matches the underlying values of the DayOfWeek enum, except for Sunday, which needs to be converted.
        /// </summary>
        /// <param name="dayOfWeek">The day Of Week.</param>
        /// <returns>The day of week.</returns>
        static int GetWeekday(DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Sunday ? 7 : (int)dayOfWeek;
        }

        /// <summary>
        /// Method to get mid day of week
        /// </summary>
        /// <param name="dayOfWeek">The first day of week</param>
        /// <returns>Returns the mid day of the week based on the first day of week.</returns>
        static DayOfWeek GetMidDayOfWeek(DayOfWeek dayOfWeek)
        {
            // (int)firstDayOfWeek + 3 gets the mid-day, taking modulo 7 for wrap-around
            int midDay = ((int)dayOfWeek + 3) % 7;
            switch (midDay)
            {
                case 0:
                    return DayOfWeek.Sunday;
                case 1:
                    return DayOfWeek.Monday;
                case 2:
                    return DayOfWeek.Tuesday;
                case 3:
                    return DayOfWeek.Wednesday;
                case 4:
                    return DayOfWeek.Thursday;
                case 5:
                    return DayOfWeek.Friday;
                case 6:
                    return DayOfWeek.Saturday;
                default:
                    return DayOfWeek.Sunday;
            }
        }


        #endregion
    }
}