using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.ControlsGallery.Converters
{
    /// <summary>
    /// This class converts boolean value to inverted boolean and vice versa.
    /// </summary>
    public class InvertedBoolConverter : IValueConverter
    {
        /// <summary>
		/// Converts boolean value to an inverted boolean type.
        /// </summary>
        /// <param name="value">The value must be the type of boolean </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used </param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>Returns the inverted boolean value of the boolean</returns>
        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return InvertBool(value);
        }

        /// <summary>
		/// Converts back the boolean to inverted boolean value
        /// </summary>
        /// <param name="value">The value be the type of boolean </param>
        /// <param name="targetType"> The type of the target property</param>
        /// <param name="parameter"> An additional parameter for the converter to handle, not used</param>
        /// <param name="culture"> The culture to use in the converter, not used</param>
        /// <returns>Returns the boolean value of the inverted boolean</returns>
        public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return InvertBool(value);
        }

        /// <summary>
        /// Invert the given boolean value.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>Returns inverse of given boolean value.</returns>
        static object? InvertBool(object? value)
        {
            if (value is bool)
            {
                return !(bool)value;
            }

            return value;
        }
    }
}
