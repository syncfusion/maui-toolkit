using System;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Contains utility methods to manipulate data.
    /// </summary>
    internal class ChartDataUtils
    {
        ChartDataUtils()
        {
        }

        /// <summary>
        /// Gets the property from the specified object.
        /// </summary>
        /// <param name="obj">Object to retrieve a property.</param>
        /// <param name="path">Property name</param>
        /// <returns>The property.</returns>
        internal static PropertyInfo? GetPropertyInfo(object obj, string path)
        {
            //TODO: consider if it needed.
            //return obj.GetType().GetTypeInfo().GetDeclaredProperty(path);
            return obj.GetType().GetRuntimeProperty(path);
        }

        internal static bool EqualDoubleValues(double x, double y)
        {
            return x.ToString() == y.ToString();
        }

        internal static DateTime IncreaseInterval(DateTime date, double visibleInterval, DateTimeIntervalType actualIntervalType)
        {
            var interval = visibleInterval;
            var span = new TimeSpan(0);

            switch (actualIntervalType)
            {
                case DateTimeIntervalType.Days:
                    span = TimeSpan.FromDays(interval);
                    break;
                case DateTimeIntervalType.Hours:
                    span = TimeSpan.FromHours(interval);
                    break;
                case DateTimeIntervalType.Milliseconds:
                    span = TimeSpan.FromMilliseconds(interval);
                    break;
                case DateTimeIntervalType.Seconds:
                    span = TimeSpan.FromSeconds(interval);
                    break;
                case DateTimeIntervalType.Minutes:
                    span = TimeSpan.FromMinutes(interval);
                    break;
                case DateTimeIntervalType.Months:

                    date = date.AddMonths((int)interval);
                    double dayInterval = interval - (int)interval;

                    if (dayInterval > 0)
                    {
                        return AddMonth(date, dayInterval);
                    }
                    else
                    {
                        return date;
                    }

                case DateTimeIntervalType.Years:

                    date = date.AddYears((int)interval);
                    double monthInterval = interval - (int)interval;

                    if (monthInterval > 0)
                    {
                        return date.AddMonths((int)(monthInterval * 12));
                    }
                    else
                    {
                        return date;
                    }
            }

            return date.Add(span);
        }

        internal static DateTime AddMonth(DateTime date, double interval)
        {
            int days = 31;

            switch (date.Month)
            {
                case 2:
                case 4:
                case 7:
                case 9:
                    days = 30;
                    break;
                case 0:
                    int year = date.Year;
                    if ((year % 400 == 0) || ((year % 4 == 0) && (year % 100 != 0)))
                    {
                        days = 28;
                    }
                    else
                    {
                        days = 29;
                    }

                    break;
            }

            return date.AddMilliseconds((long)(60 * 60 * 24 * days * interval));
        }
    }
}
