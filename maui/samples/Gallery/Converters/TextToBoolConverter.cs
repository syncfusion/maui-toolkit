using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class TextToBoolConverter : IValueConverter
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
            string? param = (parameter as String);
            if(param !=null && param.ToUpperInvariant() == "NAME")
            {
                if(value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return true;
                }
                return false;    
            }
            
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }
            return true;
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
