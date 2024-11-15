namespace Syncfusion.Maui.Toolkit.Charts
{
    internal static class ChartColor
    {
        internal static bool IsEqual(Color sourceColor, Color targetColor)
        {
            return sourceColor.Equals(targetColor);
        }

        internal static bool IsEmpty(Color? color)
        {
            return color == null || color == Colors.Transparent;
        }

        /// <summary>
        /// Creates the empty color.
        /// </summary>
        /// <returns>The empty.</returns>
        internal static Color CreateEmpty()
        {
            return new Color(int.MaxValue);
        }

        /// <summary>
        /// Creates the transparent color.
        /// </summary>
        /// <returns>The transparent.</returns>
        internal static Color CreateTransparent()
        {
            return Colors.Transparent;
        }

        /// <summary>
        /// Values should be [0..1]
        /// </summary>
        /// <param name="r">The Red.</param>
        /// <param name="g">The Green.</param>
        /// <param name="b">The Blue.</param>
        /// <param name="a">The Alpha.</param>
        /// <returns>The color</returns>
        internal static Color CreateColor(int r, int g, int b, int a)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
        }

        /// <summary>
        /// Values should be [0..1]
        /// </summary>
        /// <param name="r">The Red.</param>
        /// <param name="g">The Green.</param>
        /// <param name="b">The Blue.</param>
        /// <param name="a">The Alpha.</param>
        /// <returns>The color</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static Color CreateColor(float r, float g, float b, float a)
        {
            return new Color(r, g, b, a);
        }
    }
}
