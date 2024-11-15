using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class TagToPaddingConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (String.IsNullOrEmpty(value?.ToString()))
                return new Thickness(10, 0, 9, 0);
            else
                return new Thickness(10, 0, 27, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
