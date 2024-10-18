using System.Globalization;
using Syncfusion.Maui.Toolkit.Localization;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
    /// <summary>
    /// A localization resource accessor that returns the localized string based on culture for the <see cref="SfSegmentedResources"/>.
    /// </summary>
    public class SfSegmentedResources : LocalizationResourceAccessor
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

        #region Internal method

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

        #region Private method

        /// <summary>
        /// Method to get the default custom string based on culture.
        /// </summary>
        /// <param name="text">The default local string to be localized based on culture.</param>
        /// <returns>The default localized string.</returns>
        private static string GetDefaultString(string text)
        {
            string value = string.Empty;
            if (text == "Disabled")
            {
                value = "Disabled";
            }
            else if (text == "Selected")
            {
                value = "Selected";
            }

            return value;
        }

        #endregion
    }
}
