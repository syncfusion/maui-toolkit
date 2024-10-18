using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.ControlsGallery.Converters
{
    /// <summary>
	/// This class converts brush type to color type and vice versa.
    /// </summary>
    public class BrushToColorConverter : IValueConverter
    {
        /// <summary>
		/// Converts brush value to an color type.
        /// </summary>
        /// <param name="value">The value must be the type of SolidColorBrush</param>
        /// <param name="targetType">The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used</param>
        /// <param name="culture">The culture to use in the converter, not used</param>
        /// <returns>Returns the color value of the brush</returns>
        /// <exception cref="ArgumentException">Exception is thrown when the value type is null or not a type of SolidColorBrush</exception>
        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (!Brush.IsNullOrEmpty(value as SolidColorBrush))
            {
                SolidColorBrush? brush = value as SolidColorBrush;
                return brush?.Color;
            }
            throw new ArgumentException("Expected value to be a type of brush", nameof(value));
        }

        /// <summary>
		/// Converts back the color to brush type.
        /// </summary>
        /// <param name="value">The value must be the type of color</param>
        /// <param name="targetType">The type of the target property</param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used</param>
        /// <param name="culture">The culture to use in the converter, not used</param>
        /// <returns>Returns the brush value of the color</returns>
        /// <exception cref="ArgumentException">Exception is thrown when the value type is null or not a type of Color</exception>
        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value != null && value is Color color)
            {
                return new SolidColorBrush(color);
            }
            throw new ArgumentException("Expected value to be a type of color", nameof(value));
        }
    }
}
