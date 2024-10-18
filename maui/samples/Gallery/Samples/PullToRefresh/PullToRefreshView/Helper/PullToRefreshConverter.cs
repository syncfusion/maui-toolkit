using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.ControlsGallery.PullToRefresh
{
    /// <summary>
    /// Converter class helps to convert DateTime to string format.
    /// </summary>
    public class DateToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value == null) return "";

            var date = (DateTime)value;
            if (parameter == null)
            {
                return "1:23 PM, " + date.ToString("d MMMM yyyy");
            }
            else
            {
                return date.Date.CompareTo(DateTime.Today) == 0 ? "Today" : date.ToString("ddd d");
            }

        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
