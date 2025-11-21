using System.Collections.ObjectModel;
using System.Globalization;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents a class which contains time picker helper methods.
    /// </summary>
    internal static class TimePickerHelper
    {
        #region Internal Methods

        /// <summary>
        /// Method to check the time spans are equal are not. It only compares the hour, minute and second values.
        /// </summary>
        /// <param name="time">The time span value.</param>
        /// <param name="other">Other time span value.</param>
        /// <returns>Returns true, if both time span values are same.</returns>
        internal static bool IsSameTimeSpan(TimeSpan? time, TimeSpan? other)
        {
            if (other == null || time == null)
            {
                return false;
            }

            return time.Value.Hours == other.Value.Hours && time.Value.Minutes == other.Value.Minutes && time.Value.Seconds == other.Value.Seconds && time.Value.Milliseconds == other.Value.Milliseconds;
        }

        /// <summary>
        /// Method to get the minutes string collection value.
        /// </summary>
        /// <param name="interval">The interval value.</param>
        /// <param name="hour">The hour value.</param>
        /// <param name="selectedDate">The selected date value used to compare min and max date value.</param>
        /// <param name="minDateValue">The min date value.</param>
        /// <param name="maxDateValue">The max date value.</param>
        /// <returns>Returns seconds or minutes string collection.</returns>
        internal static ObservableCollection<string> GetMinutes(int interval, int hour, DateTime? selectedDate, DateTime? minDateValue, DateTime? maxDateValue)
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();
            int startIndex = 0;
            int endIndex = 59;
            if (minDateValue != null)
            {
                if (selectedDate != null && selectedDate.Value.Date == minDateValue.Value.Date && hour == minDateValue.Value.Hour)
                {
                    startIndex = minDateValue.Value.Minute;
                }
            }

            if (maxDateValue != null)
            {
                if (selectedDate != null && selectedDate.Value.Date == maxDateValue.Value.Date && hour == maxDateValue.Value.Hour)
                {
                    endIndex = maxDateValue.Value.Minute;
                }
            }

            for (int i = startIndex; i <= endIndex; i = i + interval)
            {
                collection.Add($"{i:00}");
            }

            return collection;
        }

        /// <summary>
        /// Method to get the seconds string collection value.
        /// </summary>
        /// <param name="interval">The interval value.</param>
        /// <param name="hour">The hour value.</param>
        /// <param name="minute">The minute value.</param>
        /// <param name="selectedDate">The selected date value used to compare min and max date value.</param>
        /// <param name="minDateValue">The min date value.</param>
        /// <param name="maxDateValue">The max date value.</param>
        /// <returns>Returns seconds or minutes string collection.</returns>
        internal static ObservableCollection<string> GetSeconds(int interval, int hour, int minute, DateTime? selectedDate, DateTime? minDateValue, DateTime? maxDateValue)
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();
            int startIndex = 0;
            int endIndex = 59;

            if (minDateValue != null)
            {
                if (selectedDate != null && selectedDate.Value.Date == minDateValue.Value.Date && hour == minDateValue.Value.Hour && minute == minDateValue.Value.Minute)
                {
                    startIndex = minDateValue.Value.Second;
                }
            }

            if (maxDateValue != null)
            {
                if (selectedDate != null && selectedDate.Value.Date == maxDateValue.Value.Date && hour == maxDateValue.Value.Hour && minute == maxDateValue.Value.Minute)
                {
                    endIndex = maxDateValue.Value.Second;
                }
            }

            for (int i = startIndex; i <= endIndex; i = i + interval)
            {
                collection.Add($"{i:00}");
            }

            return collection;
        }

        /// <summary>
        /// Method to get millisecond string collection value.
        /// </summary>
        /// <param name="interval">The interval value.</param>
        /// <param name="hour">The hour value.</param>
        /// <param name="minute">The minute value.</param>
        /// <param name="second">The second value.</param>
        /// <param name="selectedDate">The selected date value used to compare min and max date value.</param>
        /// <param name="minDateValue">The min date value.</param>
        /// <param name="maxDateValue">The max date value.</param>
        /// <returns>Returns millisecond string collection.</returns>
        internal static ObservableCollection<string> GetMilliseconds(int interval, int hour, int minute, int second, DateTime? selectedDate, DateTime? minDateValue, DateTime? maxDateValue)
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();
            int startIndex = 0;
            int endIndex = 999;

            if (minDateValue != null)
            {
                if (selectedDate != null && selectedDate.Value.Date == minDateValue.Value.Date && hour == minDateValue.Value.Hour && minute == minDateValue.Value.Minute && second == minDateValue.Value.Second)
                {
                    startIndex = minDateValue.Value.Millisecond;
                }
            }

            if (maxDateValue != null)
            {
                if (selectedDate != null && selectedDate.Value.Date == maxDateValue.Value.Date && hour == maxDateValue.Value.Hour && minute == maxDateValue.Value.Minute && second == maxDateValue.Value.Second)
                {
                    endIndex = maxDateValue.Value.Millisecond;
                }
            }

            for (int i = startIndex; i <= endIndex; i = i + interval)
            {
                collection.Add($"{i:000}");
            }

            return collection;
            }

        /// <summary>
        /// Method to get the hours string collection value.
        /// </summary>
        /// <param name="format">The hour format value.</param>
        /// <param name="interval">The hour interval value.</param>
        /// <param name="selectedDate">The selected date value.</param>
        /// <param name="minDate">The min date value.</param>
        /// <param name="maxDate">The max date value.</param>
        /// <returns>Returns hours string collection.</returns>
        internal static ObservableCollection<string> GetHours(string format, int interval, DateTime? selectedDate, DateTime? minDate, DateTime? maxDate)
        {
            ObservableCollection<string> hours = new ObservableCollection<string>();
            if (format == "h")
            {
                int startIndex = 0;
                int endIndex = 11;
                if (minDate != null && selectedDate != null && selectedDate.Value.Date == minDate.Value.Date && (int)(selectedDate.Value.Hour / 12) == (int)(minDate.Value.Hour / 12))
                {
                    startIndex = minDate.Value.Hour % 12;
                }

                if (maxDate != null && selectedDate != null && selectedDate.Value.Date == maxDate.Value.Date && (int)(selectedDate.Value.Hour / 12) == (int)(maxDate.Value.Hour / 12))
                {
                    endIndex = maxDate.Value.Hour % 12;
                }

                for (int i = startIndex; i <= endIndex; i = i + interval)
                {
                    if (i == 0)
                    {
                        continue;
                    }

                    hours.Add(i.ToString());
                }

                if (startIndex == 0)
                {
                    hours.Add("12");
                }
            }
            else if (format == "hh")
            {
                int startIndex = 0;
                int endIndex = 11;
                if (minDate != null && selectedDate != null && selectedDate.Value.Date == minDate.Value.Date && (int)(selectedDate.Value.Hour / 12) == (int)(minDate.Value.Hour / 12))
                {
                    startIndex = minDate.Value.Hour % 12;
                }

                if (maxDate != null && selectedDate != null && selectedDate.Value.Date == maxDate.Value.Date && (int)(selectedDate.Value.Hour / 12) == (int)(maxDate.Value.Hour / 12))
                {
                    endIndex = maxDate.Value.Hour % 12;
                }

                for (int i = startIndex; i <= endIndex; i = i + interval)
                {
                    if (i == 0)
                    {
                        continue;
                    }

                    hours.Add($"{i:00}");
                }

                if (startIndex == 0)
                {
                    hours.Add("12");
                }
            }
            else if (format == "H")
            {
                int startIndex = 0;
                int endIndex = 23;
                if (minDate != null)
                {
                    startIndex = minDate.Value.Hour;
                }

                if (maxDate != null)
                {
                    endIndex = maxDate.Value.Hour;
                }

                for (int i = startIndex; i <= endIndex; i = i + interval)
                {
                    hours.Add(i.ToString());
                }
            }
            else if (format == "HH")
            {
                int startIndex = 0;
                int endIndex = 23;
                if (minDate != null)
                {
                    startIndex = minDate.Value.Hour;
                }

                if (maxDate != null)
                {
                    endIndex = maxDate.Value.Hour;
                }

                for (int i = startIndex; i <= endIndex; i = i + interval)
                {
                    hours.Add($"{i:00}");
                }
            }

            return hours;
        }

        /// <summary>
        /// Method to get the hour text for the hour value based on format.
        /// </summary>
        /// <param name="format">Hour format.</param>
        /// <param name="hour">Hour value.</param>
        /// <returns>Returns hour string based on format.</returns>
        internal static string? GetHourText(string format, int? hour)
        {
            if (format == "h")
            {
                int? time = hour % 12;
                return time == 0 ? "12" : time.ToString();
            }
            else if (format == "hh")
            {
                int? time = hour % 12;
                return time == 0 ? "12" : $"{time:00}";
            }
            else if (format == "H")
            {
                return hour.ToString();
            }

            return $"{hour:00}";
        }

        /// <summary>
        /// Method to get the minute text for the minute value based on format.
        /// </summary>
        /// <param name="minute">Minute value.</param>
        /// <returns>Returns minute string based on format.</returns>
        internal static string GetMinuteOrSecondText(int minute)
        {
            return $"{minute:00}";
        }

        /// <summary>
        /// Method to get the meridiem string collection value.
        /// </summary>
        /// <param name="minDate">The min date value.</param>
        /// <param name="maxDate">The max date value.</param>
        /// <param name="selectedDate">The selected date value.</param>
        /// <returns>Returns meridiem string collection.</returns>
        internal static ObservableCollection<string> GetMeridiem(DateTime? minDate, DateTime? maxDate, DateTime? selectedDate)
        {
            ObservableCollection<string> meridiems = new ObservableCollection<string>();
            if (minDate == null || (selectedDate != null && selectedDate.Value.Date == minDate.Value.Date && minDate.Value.Hour < 12))
            {
                meridiems.Add(CultureInfo.CurrentUICulture.DateTimeFormat.AMDesignator);
            }

            if (maxDate == null || (selectedDate != null && selectedDate.Value.Date == maxDate.Value.Date && maxDate.Value.Hour >= 12))
            {
                meridiems.Add(CultureInfo.CurrentUICulture.DateTimeFormat.PMDesignator);
            }

            return meridiems;
        }

        /// <summary>
        /// Method to get the millisecond text for the millisecond value based on format.
        /// </summary>
        /// <param name="millisecond">millisecond value.</param>
        /// <returns>Returns millisecond string based on format.</returns>
        internal static string GetMillisecondText(int millisecond)
        {
            return $"{millisecond:000}";
        }

        /// <summary>
        /// Method to get the selected index as AM text value.
        /// </summary>
        /// <param name="meridiem">The meridiem collection.</param>
        /// <param name="index">The selected index value.</param>
        /// <returns>Returns true while the selected index have AM text.</returns>
        internal static bool IsAMText(ObservableCollection<string> meridiem, int index)
        {
            if (index > meridiem.Count)
            {
                index = meridiem.Count - 1;
            }

            if (index == -1)
            {
                return true;
            }

            return meridiem[index] == CultureInfo.CurrentUICulture.DateTimeFormat.AMDesignator;
        }

        /// <summary>
        /// Method to get the minute or second index value on collection.
        /// </summary>
        /// <param name="collection">The string collection for minutes or seconds.</param>
        /// <param name="value">The minute or second value.</param>
        /// <returns>Returns index for the value in collection.</returns>
        internal static int GetMinuteOrSecondOrMilliSecondsIndex(ObservableCollection<string> collection, int value)
        {
            if (collection.Count == 0)
            {
                return -1;
            }

            string text = collection[0];
            string dayString = string.Empty;
            if (text.Length > 2)
            {
                dayString = GetMillisecondText(value);
            }
            else
            {
                dayString = GetMinuteOrSecondText(value);
            }

            int index = collection.IndexOf(dayString);
            if (index != -1)
            {
                return index;
            }

            for (int i = 0; i < collection.Count; i++)
            {
                string item = collection[i];
                if (int.Parse(item) > value)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                index = collection.Count - 1;
            }

            return index;
        }

        /// <summary>
        /// Method to get the hour index value based on the format.
        /// </summary>
        /// <param name="format">The hour format.</param>
        /// <param name="hours">The hour collection.</param>
        /// <param name="hour">The hour value.</param>
        /// <returns>Returns the hour index value.</returns>
        internal static int GetHourIndex(string format, ObservableCollection<string> hours, int? hour)
        {
            if (string.IsNullOrEmpty(format))
            {
                return -1;
            }

            string? dayString = GetHourText(format, hour);
            int index;
            if (dayString != null)
            {
                index = hours.IndexOf(dayString);
            }
            else
            {
                index = 0;
            }

            if (index != -1)
            {
                return index;
            }

            int? time = hour;
            if (format == "h" || format == "hh")
            {
                time = hour % 12;
                time = time == 0 ? 12 : time;
            }

            for (int i = 0; i < hours.Count; i++)
            {
                string hourItem = hours[i];
                if (int.Parse(hourItem) > time)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                index = hours.Count - 1;
            }

            return index;
        }

        /// <summary>
        /// Method to return the time order based on picker format. 0 denotes hour, 1 denotes minute, 2 denotes second and 3 denotes meridiem.
        /// </summary>
        /// <param name="hourFormat">Holds the hour format value based on picker format.</param>
        /// <param name="format">The time picker time format.</param>
        /// <returns>Returns time order list.</returns>
        internal static List<int> GetFormatStringOrder(out string hourFormat, PickerTimeFormat format)
        {
            hourFormat = string.Empty;
            List<int> formatStringOrder = new List<int>();
            string cultureFormatString = format.ToString();

            if (format == PickerTimeFormat.Default)
            {
                cultureFormatString = string.Empty;
            }

            // Handle predefined formats first
            switch (cultureFormatString)
            {
                case "H_mm":
                    hourFormat = "H";
                    formatStringOrder = new List<int>() { 0, 1 };
                    break;
                case "HH_mm":
                    hourFormat = "HH";
                    formatStringOrder = new List<int>() { 0, 1 };
                    break;
                case "H_mm_ss":
                    hourFormat = "H";
                    formatStringOrder = new List<int>() { 0, 1, 2 };
                    break;
                case "HH_mm_ss":
                    hourFormat = "HH";
                    formatStringOrder = new List<int>() { 0, 1, 2 };
                    break;
                case "HH_mm_ss_fff":
                    hourFormat = "HH";
                    formatStringOrder = new List<int>() { 0, 1, 2, 4 };
                    break;
                case "h_mm_ss_tt":
                    hourFormat = "h";
                    formatStringOrder = new List<int>() { 0, 1, 2, 3 };
                    break;
                case "hh_mm_ss_tt":
                    hourFormat = "hh";
                    formatStringOrder = new List<int>() { 0, 1, 2, 3 };
                    break;
                case "hh_mm_ss_fff_tt":
                    hourFormat = "hh";
                    formatStringOrder = new List<int>() { 0, 1, 2, 4, 3 };
                    break;
                case "h_mm_tt":
                    hourFormat = "h";
                    formatStringOrder = new List<int>() { 0, 1, 3 };
                    break;
                case "hh_mm_tt":
                    hourFormat = "hh";
                    formatStringOrder = new List<int>() { 0, 1, 3 };
                    break;
                case "hh_tt":
                    hourFormat = "hh";
                    formatStringOrder = new List<int>() { 0, 3 };
                    break;
                case "mm_ss":
                    formatStringOrder = new List<int> { 1, 2 };
                    break;
                case "ss_fff":
                    formatStringOrder = new List<int> { 2, 4 };
                    break;
                case "mm_ss_fff":
                    formatStringOrder = new List<int> { 1, 2, 4 };
                    break;
                default:
                    // If the format is not predefined, dynamically determine the format order
                    formatStringOrder = GetCustomFormatOrder(CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern, out hourFormat);
                    break;
            }

            return formatStringOrder;
        }

        /// <summary>
        /// Method to get the valid max time.
        /// </summary>
        /// <param name="minTime">The min time value.</param>
        /// <param name="maxTime">The max time value.</param>
        /// <returns>Returns the valid max time</returns>
        internal static TimeSpan GetValidMaxTime(TimeSpan minTime, TimeSpan maxTime)
        {
            if (maxTime < minTime)
            {
                return minTime;
            }

            return maxTime;
        }

        /// <summary>
        /// Method to get the valid selected time based on min and max time.
        /// </summary>
        /// <param name="time">The time value.</param>
        /// <param name="minTime">The min time value.</param>
        /// <param name="maxTime">The max time value.</param>
        /// <returns>Returns the valid time.</returns>
        internal static TimeSpan? GetValidSelectedTime(TimeSpan? time, TimeSpan minTime, TimeSpan maxTime)
        {
            if (time != null)
            {
                if (time < minTime)
                {
                    return new TimeSpan(0, minTime.Hours, minTime.Minutes, time.Value.Seconds, time.Value.Milliseconds);
                }
                else if (time > maxTime)
                {
                    return new TimeSpan(0, maxTime.Hours, maxTime.Minutes, time.Value.Seconds, time.Value.Milliseconds);
                }
            }

            return time;
        }

        /// <summary>
        /// Method to check the current time is black out time or not.
        /// </summary>
        /// <param name="blackOutTime">An black out time value.</param>
        /// <param name="currentTime">Current selected time value.</param>
        /// <returns>Returns true or false based on blackout time.</returns>
        internal static bool IsBlackoutTime(TimeSpan blackOutTime, TimeSpan? currentTime)
        {
            if (currentTime != null && blackOutTime.Hours == currentTime.Value.Hours && blackOutTime.Minutes == currentTime.Value.Minutes)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to get custom format order if user sets culture values
        /// </summary>
        /// <param name="cultureTimeFormat">culture based time format</param>
        /// <param name="hourFormat">culture based hour format</param>
        /// <returns>Returns time order list</returns>
        static List<int> GetCustomFormatOrder(string cultureTimeFormat, out string hourFormat)
        {
            hourFormat = string.Empty;
            List<int> formatOrder = new List<int>();

            // Extract parts of the format and map them to positions (0, 1, 2)
            string[] formatParts = cultureTimeFormat.Split(new char[] { ' ', ':', '\u202F' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < formatParts.Length; i++)
            {
                switch (formatParts[i])
                {
                    case "HH":
                    case "H":
                    case "h":
                    case "hh":
                        hourFormat = formatParts[i];
                        formatOrder.Add(0);
                        break;
                    case "mm":
                        formatOrder.Add(1);
                        break;
                    case "ss":
                        formatOrder.Add(2);
                        break;
                    case "tt":
                        formatOrder.Add(3);
                        break;
                    default:
                        // Handle unexpected cases or other format patterns
                        break;
                }
            }

            // Distinct method is used to store only unique values in the list
            formatOrder = formatOrder.Distinct().ToList();
            return formatOrder;
        }

        #endregion
    }
}