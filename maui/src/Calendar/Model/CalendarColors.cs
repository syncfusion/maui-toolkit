namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Default Calendar Color for all UI elements
    /// </summary>
    internal static class CalendarColors
    {
        #region Internal Methods

        /// <summary>
        /// Default primary highlight color for UI elements
        /// </summary>
        /// <returns>primary color</returns>
        internal static Color GetPrimaryColor()
        {
            return Color.FromArgb("#6750A4");
        }

        /// <summary>
        /// Default text color for primary UI elements
        /// </summary>
        /// <returns>on primary color</returns>
        internal static Color GetOnPrimaryColor()
        {
            return Color.FromArgb("#FFFFFF");
        }

        /// <summary>
        /// Default text color for secondary UI elements(black with 11% lighter)
        /// </summary>
        /// <returns>on secondary color</returns>
        internal static Color GetOnSecondaryColor()
        {
            return Color.FromArgb("#49454F");
        }

        /// <summary>
        /// Default text color for invariant of secondary UI elements(secondary color with 0.6 opacity)
        /// </summary>
        /// <returns>on secondary variant color</returns>
        internal static Color GetOnSecondaryVariantColor()
        {
            return Color.FromArgb("#1C1B1F");
        }

        /// <summary>
        /// Default Calendar Color for disabled UI elements(secondary color with 0.3 opacity)
        /// </summary>
        /// <returns>disabled color</returns>
        internal static Color GetDisabledColor()
        {
            return Color.FromArgb("#611c1b1f");
        }

        /// <summary>
        /// Default Calendar brush for primary UI elements BG
        /// </summary>
        /// <returns>primary brush color</returns>
        internal static Brush GetPrimaryBrush()
        {
            return GetPrimaryColor();
        }

        #endregion
    }
}