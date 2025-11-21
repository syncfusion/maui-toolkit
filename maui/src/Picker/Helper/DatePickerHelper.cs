using System.Collections.ObjectModel;
using System.Globalization;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents a class which contains date picker helper methods.
    /// </summary>
    internal static class DatePickerHelper
    {
        #region Internal Methods

        /// <summary>
        /// Method to get the day string for the day value based on format.
        /// </summary>
        /// <param name="format">The day format.</param>
        /// <param name="day">The day value.</param>
        /// <returns>Returns the day string for the day value.</returns>
        internal static string GetDayString(string format, int day)
        {
            return format == "d" ? $"{day:0}" : $"{day:00}";
        }

        /// <summary>
        /// Method to get the month string for month value based on format.
        /// </summary>
        /// <param name="format">The month format.</param>
        /// <param name="month">The month value.</param>
        /// <returns>Returns month string based on format.</returns>
        internal static string GetMonthString(string format, int month)
        {
            bool isAbbreviatedMonth = format == "MMM";
            bool isSingleDigitMonth = format == "M";
            if (isAbbreviatedMonth)
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);
            }
            else if (isSingleDigitMonth)
            {
                return $"{month:0}";
            }

            return $"{month:00}";
        }

        /// <summary>
        /// Method to replace the invariant month string with culture based month string.
        /// </summary>
        /// <param name="value">The invariant month string.</param>
        /// <param name="format">The date format string.</param>
        /// <param name="selectedDate">Date need to format.</param>
        /// <returns>Returns the culture based month value.</returns>
        internal static string ReplaceCultureMonthString(string value, string format, DateTime selectedDate)
        {
            if (format.Contains("MMMM", StringComparison.Ordinal))
            {
                string monthName = selectedDate.ToString("MMMM", CultureInfo.InvariantCulture);
                int index = value.IndexOf(monthName);
                if (index != -1)
                {
                    string currentMonthName = selectedDate.ToString("MMMM", CultureInfo.CurrentUICulture);
                    value = value.Replace(monthName, currentMonthName, StringComparison.Ordinal);
                }
            }
            else if (format.Contains("MMM", StringComparison.Ordinal))
            {
                string monthName = selectedDate.ToString("MMM", CultureInfo.InvariantCulture);
                int index = value.IndexOf(monthName);
                if (index != -1)
                {
                    string currentMonthName = selectedDate.ToString("MMM", CultureInfo.CurrentUICulture);
                    value = value.Replace(monthName, currentMonthName, StringComparison.Ordinal);
                }
            }

            return value;
        }

        /// <summary>
        /// Method to replace the invariant culture meridiem string to localization based string.
        /// </summary>
        /// <param name="value">Invariant culture meridiem string value.</param>
        /// <param name="format">Time format.</param>
        /// <returns>Returns localized meridiem string value.</returns>
        internal static string ReplaceCultureMeridiemString(string value, string format)
        {
            if (format.Contains("tt", StringComparison.Ordinal) || format == "t")
            {
                int index = value.IndexOf("AM");
                if (index != -1)
                {
                    string meridiumString = DateTime.Now.Date.AddHours(6).ToString("tt", CultureInfo.CurrentUICulture);
                    value = value.Replace("AM", meridiumString, StringComparison.Ordinal);
                }

                index = value.IndexOf("PM");
                if (index != -1)
                {
                    string meridiumString = DateTime.Now.Date.AddHours(18).ToString("tt", CultureInfo.CurrentUICulture);
                    value = value.Replace("PM", meridiumString, StringComparison.Ordinal);
                }
            }
            else if (format.Contains("t", StringComparison.Ordinal))
            {
                int index = value.IndexOf("A");
                if (index != -1)
                {
                    string meridiumString = DateTime.Now.Date.AddHours(6).ToString(" t", CultureInfo.CurrentUICulture);
                    meridiumString = meridiumString.Substring(1, meridiumString.Length - 1);
                    value = value.Replace("A", meridiumString, StringComparison.Ordinal);
                }

                index = value.IndexOf("P");
                if (index != -1)
                {
                    string meridiumString = DateTime.Now.Date.AddHours(18).ToString(" t", CultureInfo.CurrentUICulture);
                    meridiumString = meridiumString.Substring(1, meridiumString.Length - 1);
                    value = value.Replace("P", meridiumString, StringComparison.Ordinal);
                }
            }

            return value;
        }

        /// <summary>
        /// Method to get the years from minimum to maximum date.
        /// </summary>
        /// <param name="minDate">The min date value.</param>
        /// <param name="maxDate">The max date value.</param>
        /// <param name="yearInterval">The year interval value.</param>
        /// <returns>Returns year collection based on min and max date.</returns>
        internal static ObservableCollection<string> GetYears(DateTime minDate, DateTime maxDate, int yearInterval)
        {
            ObservableCollection<string> years = new ObservableCollection<string>();
            int minimumYear = minDate.Year;
            int maximumYear = maxDate.Year;
            for (int i = minimumYear; i <= maximumYear; i += yearInterval)
            {
                years.Add(i.ToString());
            }

            return years;
        }

        /// <summary>
        /// Method to get the months for the year from min to max date.
        /// </summary>
        /// <param name="format">The month format.</param>
        /// <param name="year">The needed months for the year.</param>
        /// <param name="minDate">The min date value.</param>
        /// <param name="maxDate">The max date value.</param>
        /// <param name="monthInterval">The month interval value.</param>
        /// <returns>Returns month collection for year.</returns>
        internal static ObservableCollection<string> GetMonths(string format, int year, DateTime minDate, DateTime maxDate, int monthInterval)
        {
            ObservableCollection<string> months = new ObservableCollection<string>();
            if (string.IsNullOrEmpty(format))
            {
                return months;
            }

            int minimumMonth = 1;
            int maximumMonth = 12;

            //// Check the selected date year and minimum date year are same.
            if (year == minDate.Year)
            {
                minimumMonth = minDate.Month;
            }

            //// Check the selected date year and maximum date year are same.
            if (year == maxDate.Year)
            {
                maximumMonth = maxDate.Month;
            }

            bool isAbbreviatedMonth = format == "MMM";
            bool isSingleDigitMonth = format == "M";
            for (int i = minimumMonth; i <= maximumMonth; i += monthInterval)
            {
                if (isAbbreviatedMonth)
                {
                    months.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i));
                }
                else
                {
                    string monthValue = isSingleDigitMonth ? $"{i:0}" : $"{i:00}";
                    months.Add(monthValue);
                }
            }

            return months;
        }

        /// <summary>
        /// Method to get the days for the year and month from min to max date.
        /// </summary>
        /// <param name="format">The day format.</param>
        /// <param name="month">The month value.</param>
        /// <param name="year">The year value.</param>
        /// <param name="minDate">The min date value.</param>
        /// <param name="maxDate">The max date value.</param>
        /// <param name="dayInterval">The day interval value.</param>
        /// <returns>Returns the days collection for the specified month and year.</returns>
        internal static ObservableCollection<string> GetDays(string format, int month, int year, DateTime minDate, DateTime maxDate, int dayInterval)
        {
            ObservableCollection<string> days = new ObservableCollection<string>();
            if (string.IsNullOrEmpty(format))
            {
                return days;
            }

            int minimumDate = 1;
            int maximumDate = DateTime.DaysInMonth(year, month);

            //// Check the selected date year with minimum date year and month are same.
            if (year == minDate.Year && month == minDate.Month)
            {
                minimumDate = minDate.Day;
            }

            //// Check the selected date year with maximum date year and month are same.
            if (year == maxDate.Year && month == maxDate.Month)
            {
                maximumDate = maxDate.Day;
            }

            bool isSingleDigitDay = format == "d";
            for (int i = minimumDate; i <= maximumDate; i += dayInterval)
            {
                string dayValue = isSingleDigitDay ? $"{i:0}" : $"{i:00}";
                days.Add(dayValue);
            }

            return days;
        }

        /// <summary>
        /// Method to return the index value for day value inside the days string collection based on format.
        /// </summary>
        /// <param name="format">The day format.</param>
        /// <param name="days">The days collection.</param>
        /// <param name="day">The day value.</param>
        /// <returns>Returns index of the day value. if the day value not placed inside the days collection then return the nearby value.</returns>
        internal static int GetDayIndex(string format, ObservableCollection<string> days, int day)
        {
            if (string.IsNullOrEmpty(format))
            {
                return -1;
            }

            string dayString = GetDayString(format, day);
            int index = days.IndexOf(dayString);
            if (index != -1)
            {
                return index;
            }

            for (int i = 0; i < days.Count; i++)
            {
                string dayItem = days[i];
                if (int.Parse(dayItem) > day)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                index = days.Count - 1;
            }

            return index;
        }

        /// <summary>
        /// Method to return the index value for year value inside the year string collection.
        /// </summary>
        /// <param name="years">The years collection.</param>
        /// <param name="year">The year value.</param>
        /// <returns>Returns index of the year value. if the year value not placed inside the years collection then return the nearby value.</returns>
        internal static int GetYearIndex(ObservableCollection<string> years, int year)
        {
            if (years == null || years.Count == 0)
            {
                return -1;
            }

            string yearString = year.ToString();
            int index = years.IndexOf(yearString);
            if (index != -1)
            {
                return index;
            }

            for (int i = 0; i < years.Count; i++)
            {
                string yearValue = years[i];
                if (int.Parse(yearValue) > year)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                index = years.Count - 1;
            }

            return index;
        }

        /// <summary>
        /// Method to return the index value for month value inside the months string collection based on format.
        /// </summary>
        /// <param name="format">The month format.</param>
        /// <param name="months">The months collection.</param>
        /// <param name="month">The month value.</param>
        /// <returns>Returns index of the month value. if the month value not placed inside the months collection then return the nearby value.</returns>
        internal static int GetMonthIndex(string format, ObservableCollection<string> months, int month)
        {
            string monthString = GetMonthString(format, month);
            int monthIndex = months.IndexOf(monthString);
            if (monthIndex != -1)
            {
                return monthIndex;
            }

            List<string> monthStrings = new List<string>();
            bool isAbbreviatedMonth = format == "MMM";
            if (isAbbreviatedMonth)
            {
                monthStrings = DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames.ToList();
            }

            if (isAbbreviatedMonth)
            {
                for (int i = 0; i < months.Count; i++)
                {
                    string monthItem = months[i];
                    if (monthStrings.IndexOf(monthItem) + 1 > month)
                    {
                        monthIndex = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < months.Count; i++)
                {
                    string monthItem = months[i];
                    if (int.Parse(monthItem) > month)
                    {
                        monthIndex = i;
                        break;
                    }
                }
            }

            if (monthIndex == -1)
            {
                monthIndex = months.Count - 1;
            }

            return monthIndex;
        }

        /// <summary>
        /// Method to return the date order based on picker format. 0 denotes day, 1 denotes month and 2 denotes year.
        /// </summary>
        /// <param name="dayFormat">Holds the day format value based on picker format.</param>
        /// <param name="monthFormat">Holds the month format value based on picker format.</param>
        /// <param name="format">The date picker date format value.</param>
        /// <returns>Returns date order list.</returns>
        internal static List<int> GetFormatStringOrder(out string dayFormat, out string monthFormat, PickerDateFormat format)
        {
            dayFormat = string.Empty;
            monthFormat = string.Empty;
            List<int> formatStringOrder = new List<int>();

            // Get the culture-specific date pattern
            string cultureDateFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
            string cultureFormatString = format.ToString();

            if (format == PickerDateFormat.Default)
            {
                cultureFormatString = string.Empty;
            }

            // Handle predefined formats first
            switch (cultureFormatString)
            {
                case "dd_MM":
                    dayFormat = "dd";
                    monthFormat = "MM";
                    formatStringOrder = new List<int>() { 0, 1 };
                    break;

                case "dd_MM_yyyy":
                    dayFormat = "dd";
                    monthFormat = "MM";
                    formatStringOrder = new List<int>() { 0, 1, 2 };
                    break;

                case "dd_MMM_yyyy":
                    dayFormat = "dd";
                    monthFormat = "MMM";
                    formatStringOrder = new List<int>() { 0, 1, 2 };
                    break;

                case "M_d_yyyy":
                    dayFormat = "d";
                    monthFormat = "M";
                    formatStringOrder = new List<int>() { 1, 0, 2 };
                    break;

                case "MM_dd_yyyy":
                    dayFormat = "dd";
                    monthFormat = "MM";
                    formatStringOrder = new List<int>() { 1, 0, 2 };
                    break;

                case "MM_yyyy":
                    monthFormat = "MM";
                    formatStringOrder = new List<int>() { 1, 2 };
                    break;

                case "MMM_yyyy":
                    monthFormat = "MMM";
                    formatStringOrder = new List<int>() { 1, 2 };
                    break;

                case "yyyy_MM_dd":
                    dayFormat = "dd";
                    monthFormat = "MM";
                    formatStringOrder = new List<int>() { 2, 1, 0 };
                    break;

                default:
                    // If the format is not predefined, dynamically determine the format order
                    formatStringOrder = GetCustomFormatOrder(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern, out dayFormat, out monthFormat);
                    break;
            }

            return formatStringOrder;
        }

        /// <summary>
        /// Method to check both date value are equal.
        /// </summary>
        /// <param name="date">Date value.</param>
        /// <param name="otherDate">The other date value.</param>
        /// <returns>Returns true, while both date values are same.</returns>
        internal static bool IsSameDate(DateTime? date, DateTime? otherDate)
        {
            if (date == null || otherDate == null)
            {
                return false;
            }

            return date.Value.Year == otherDate.Value.Year && date.Value.Month == otherDate.Value.Month && date.Value.Date == otherDate.Value.Date;
        }

        /// <summary>
        /// Method to check both date time value are equal.
        /// </summary>
        /// <param name="date">Date time value.</param>
        /// <param name="otherDate">The other date time value.</param>
        /// <returns>Returns true, while both date time values are same.</returns>
        internal static bool IsSameDateTime(DateTime? date, DateTime? otherDate)
        {
            if (date == null || otherDate == null)
            {
                return date == otherDate;
            }

            return date.Value.Year == otherDate.Value.Year &&
                   date.Value.Month == otherDate.Value.Month &&
                   date.Value.Day == otherDate.Value.Day &&
                   date.Value.Hour == otherDate.Value.Hour &&
                   date.Value.Minute == otherDate.Value.Minute &&
                   date.Value.Second == otherDate.Value.Second &&
                   date.Value.Millisecond == otherDate.Value.Millisecond;
        }

        /// <summary>
        /// Method to get the valid date based on min and max date.
        /// </summary>
        /// <param name="date">The date value.</param>
        /// <param name="minDate">The min date value.</param>
        /// <param name="maxDate">The max date value.</param>
        /// <returns>Returns the valid date.</returns>
        internal static DateTime? GetValidDate(DateTime? date, DateTime minDate, DateTime maxDate)
        {
            if (date != null)
            {
                if (date.Value.Date < minDate.Date)
                {
                    return minDate.Date;
                }
                else if (date.Value.Date > maxDate.Date)
                {
                    return maxDate.Date;
                }
            }

            return date;
        }

        /// <summary>
        /// Method to get the valid date time based on min and max date.
        /// </summary>
        /// <param name="date">The date time value.</param>
        /// <param name="minDate">The min date time value.</param>
        /// <param name="maxDate">The max date time value.</param>
        /// <returns>Returns the valid date time.</returns>
        internal static DateTime GetValidDateTime(DateTime? date, DateTime minDate, DateTime maxDate)
        {
            DateTime validDate = date ?? DateTime.MinValue;

            if (validDate < minDate)
            {
                return minDate;
            }
            else if (validDate > maxDate)
            {
                return maxDate;
            }

            return validDate;
        }

        /// <summary>
        /// Method to get the valid max date.
        /// </summary>
        /// <param name="minDate">The min date value.</param>
        /// <param name="maxDate">The max date value.</param>
        /// <returns>Returns the valid max date</returns>
        internal static DateTime GetValidMaxDate(DateTime minDate, DateTime maxDate)
        {
            if (maxDate < minDate)
            {
                return minDate;
            }

            return maxDate;
        }

        /// <summary>
        /// Method to check the current time is black out datetime or not.
        /// </summary>
        /// <param name="blackOutDateTime">An black out date time value.</param>
        /// <param name="currentDateTime">Current selected datetime value.</param>
        /// <param name="isTimeSpanAtZero">Checks whether we need to update time header only.</param>
        /// <returns>Returns true or false based on blackout datetime.</returns>
        internal static bool IsBlackoutDateTime(DateTime blackOutDateTime, DateTime? currentDateTime, out bool isTimeSpanAtZero)
        {
            isTimeSpanAtZero = false;

            if (currentDateTime != null)
            {
                if (blackOutDateTime.Date == currentDateTime.Value.Date)
                {
                    if (blackOutDateTime.TimeOfDay == TimeSpan.Zero)
                    {
                        isTimeSpanAtZero = true;
                        return true;
                    }
                    else if (blackOutDateTime.Hour == currentDateTime.Value.Hour && blackOutDateTime.Minute == currentDateTime.Value.Minute)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Method to check the current time is black out date or not.
        /// </summary>
        /// <param name="isDateOnlyComparison">Determines calculation compares date only.</param>
        /// <param name="currentValue">Current selected value.</param>
        /// <param name="blackOutDate">An black out date.</param>
        /// <param name="selectedDate">Selected date value.</param>
        /// <returns>Returns true or false based on blackout date.</returns>
        internal static bool IsBlackoutDate(bool isDateOnlyComparison, string currentValue, DateTime blackOutDate, DateTime selectedDate)
        {
            if (isDateOnlyComparison)
            {
                return blackOutDate.Date == selectedDate.Date;
            }
            else
            {
                return blackOutDate.Year == selectedDate.Year && blackOutDate.Month == selectedDate.Month && blackOutDate.Day == int.Parse(currentValue);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to return the date order based on culture. 0 denotes day, 1 denotes month and 2 denotes year.
        /// </summary>
        /// <param name="cultureDateFormat">culture based date format</param>
        /// <param name="dayFormat">culture based day format</param>
        /// <param name="monthFormat">culture based month format</param>
        /// <returns>Returns date order list</returns>
        static List<int> GetCustomFormatOrder(string cultureDateFormat, out string dayFormat, out string monthFormat)
        {
            dayFormat = string.Empty;
            monthFormat = string.Empty;
            List<int> formatOrder = new List<int>();

            // Extract parts of the format and map them to positions (0, 1, 2)
            string[] formatParts = cultureDateFormat.Split(new char[] { '/', '-', '.', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < formatParts.Length; i++)
            {
                if (formatParts[i].CompareTo("d") == 0 || formatParts[i].CompareTo("dd") == 0)
                {
                    dayFormat = formatParts[i];
                    formatOrder.Add(0);
                }
                else if (formatParts[i].CompareTo("M") == 0 || formatParts[i].CompareTo("MM") == 0)
                {
                    monthFormat = formatParts[i];
                    formatOrder.Add(1);
                }
                else if (formatParts[i].CompareTo("yy") == 0 || formatParts[i].CompareTo("yyyy") == 0)
                {
                    formatOrder.Add(2);
                }
            }

            // Distinct Method is used to store only unique values in the list.
            formatOrder = formatOrder.Distinct().ToList();
            return formatOrder;
        }

        #endregion
    }
}