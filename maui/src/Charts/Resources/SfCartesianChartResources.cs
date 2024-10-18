using Syncfusion.Maui.Toolkit.Localization;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Localization resource for <see cref="SfCartesianChart"/>.
    /// </summary>
    public class SfCartesianChartResources : LocalizationResourceAccessor
    {
        internal static string High
        {
            get
            {
                var high = GetString("High");
                return string.IsNullOrEmpty(high) ? "High" : high;
            }
        }

        internal static string Low
        {
            get
            {
                var low = GetString("Low");
                return string.IsNullOrEmpty(low) ? "Low" : low;
            }
        }

        internal static string Open
        {
            get
            {
                var open = GetString("Open");
                return string.IsNullOrEmpty(open) ? "Open" : open;
            }
        }

        internal static string Close
        {
            get
            {
                var close = GetString("Close");
                return string.IsNullOrEmpty(close) ? "Close" : close;
            }
        }

        internal static string Maximum
        {
            get
            {
                var maximum = GetString("Maximum");
                return string.IsNullOrEmpty(maximum) ? "Maximum" : maximum;
            }
        }

        internal static string Minimum
        {
            get
            {
                var minimum = GetString("Minimum");
                return string.IsNullOrEmpty(minimum) ? "Minimum" : minimum;
            }
        }

        internal static string Median
        {
            get
            {
                var median = GetString("Median");
                return string.IsNullOrEmpty(median) ? "Median" : median;
            }
        }

        internal static string Q3
        {
            get
            {
                var upperQuartile = GetString("Q3");
                return string.IsNullOrEmpty(upperQuartile) ? "Q3" : upperQuartile;
            }
        }

        internal static string Q1
        {
            get
            {
                var lowerQuartile = GetString("Q1");
                return string.IsNullOrEmpty(lowerQuartile) ? "Q1" : lowerQuartile;
            }
        }

    }
}
