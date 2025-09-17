using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Provides utility and extension methods for internal sunburst chart calculations and styling.
    /// </summary>
    internal static class SunburstChartUtils
    {
        /// <summary>
        /// Method used to convert the start and end angle as degree to radian.
        /// </summary>
        /// <returns>Angle</returns>
        internal static double DegreeToRadianConverter(double degree)
        {
            return degree * Math.PI / 180;
        }

        /// <summary>
        /// Method used to convert the start and end angle as radian to degree.
        /// </summary>
        /// <returns>Angle</returns>
        internal static double RadianToDegreeConverter(double radian)
        {
            return radian * (float)(180 / Math.PI);
        }

        /// <summary>
        /// Gets the corresponding <see cref="Color"/> value from a given <see cref="Brush"/>, if available.
        /// </summary>
        /// <param name="brush">The brush to extract color from.</param>
        /// <returns>The color if found; otherwise, Transparent.</returns>
        internal static Color ToColor(Brush? brush)
        {
            if (brush is SolidColorBrush solidBrush)
                return solidBrush.Color;

            return Colors.Transparent;
        }

        /// <summary>
        /// Creates a lightweight clone of a data label style.
        /// Copies only the essential properties required for sunburst chart labels.
        /// </summary>
        /// <param name="labelStyle">The label style source.</param>
        /// <returns>A new <see cref="SunburstDataLabelSettings"/> instance with copied properties.</returns>
        internal static SunburstDataLabelSettings Clone(this ITextElement labelStyle)
        {
            var style = new SunburstDataLabelSettings();

            //Only returned values which help full to render chart label. 
            //TODO: Need to add all values when it use for other cases. 
            style.FontFamily = labelStyle.FontFamily;
            style.FontAttributes = labelStyle.FontAttributes;
            style.FontSize = labelStyle.FontSize;
            return style;
        }

        /// <summary>
        /// Gets either black or white, based on which offers the best contrast with the specified color.
        /// </summary>
        /// <param name="color">The color to check contrast against.</param>
        /// <returns>White for dark backgrounds, black for light backgrounds.</returns>
        internal static Color GetContrastColor(Color? color)
        {
            if (color == null)
                return Colors.Black;

            var isDark = (ToByte(color.Red * 255) * 0.299) + (ToByte(color.Green * 255) * 0.587) + (ToByte(color.Blue * 255) * 0.114) <= 186;
            return isDark ? Colors.White : Colors.Black;
        }

        /// <summary>
        /// Converts a float value (0-255) to a byte, clamped to valid range.
        /// </summary>
        static byte ToByte(float input)
        {
            var clampedInput = Math.Clamp(input, 0, 255);
            return (byte)Math.Round(clampedInput);
        }

        /// <summary>
        /// Determines if two segments are equal based on their item, index, and level.
        /// </summary>
        /// <param name="segment">The primary segment.</param>
        /// <param name="child">The segment to compare.</param>
        /// <returns>true if equal; otherwise, false.</returns>
        internal static bool EqualsTo(this SunburstSegment segment, SunburstSegment child)
        {
            if (segment.Item is List<object> list && child.Item is List<object> list1)
            {
                return list[0] == list1[0] && (double)list[1] == (double)list1[1] && segment.Index == child.Index && segment.CurrentLevel == child.CurrentLevel;
            }

            return false;
        }
    }
}
