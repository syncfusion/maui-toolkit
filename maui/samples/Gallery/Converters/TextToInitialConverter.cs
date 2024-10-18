using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class TextToInitialConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string tagString = (string)value;
                if (!string.IsNullOrEmpty(tagString))
                    return tagString.FirstOrDefault();
            }

            return null;
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
