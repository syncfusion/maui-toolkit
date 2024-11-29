using System.Globalization;

namespace Syncfusion.Maui.Toolkit.Localization
{
    /// <summary>
    /// A localization resource accessor that returns the localized string based on culture for the <see cref="SfCalendarResources"/>.
    /// </summary>
    public class SfCalendarResources : LocalizationResourceAccessor
    {
        #region Property

        /// <summary>
        /// Gets the culture info.
        /// </summary>
        /// <value>The culture.</value>
        internal static CultureInfo CultureInfo
        {
            get
            {
                return CultureInfo.CurrentUICulture;
            }
        }

        #endregion

        #region Internal Method

        /// <summary>
        /// Gets the localized string.
        /// </summary>
        /// <param name="text">Text type.</param>
        /// <returns>The string.</returns>
        internal static string GetLocalizedString(string text)
        {
            string? value = string.Empty;
            if (ResourceManager != null)
            {
                //// The current culture may be changed while calendar type is set. Hence the current culture is updated.
                Culture = CultureInfo.CurrentUICulture;
                value = GetString(text);
            }

            if (string.IsNullOrEmpty(value))
            {
                value = GetDefaultString(text);
            }

            return value;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Method to get the default custom string based on culture.
        /// </summary>
        /// <param name="text">The default local string to be localized based on culture.</param>
        /// <returns>The default localized string.</returns>
        static string GetDefaultString(string text)
        {
            string value = string.Empty;
            if (text == "OK")
            {
                value = "OK";
            }
            else if (text == "Cancel")
            {
                value = "Cancel";
            }
            else if (text == "Today")
            {
                value = "Today";
            }
            else if (text == "Special Date")
            {
                value = "Special Date";
            }
            else if (text == "Blackout Date")
            {
                value = "Blackout Date";
            }
            else if (text == "Disabled Date")
            {
                value = "Disabled Date";
            }
            else if (text == "Week")
            {
                value = "Week";
            }
            else if (text == "Disabled Cell")
            {
                value = "Disabled Cell";
            }
            else if (text == "To")
            {
                value = "To";
            }
            else if (text == "Backward")
            {
                value = "Backward";
            }
            else if (text == "Forward")
            {
                value = "Forward";
            }

            return value;
        }

        #endregion
    }
}
