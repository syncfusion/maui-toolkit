using System.Globalization;
using Syncfusion.Maui.Toolkit.Localization;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// A localization resource accessor that returns the localized string based on culture for the <see cref="SfPickerResources"/>.
    /// </summary>
    public class SfPickerResources : LocalizationResourceAccessor
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
                Culture = CultureInfo.CurrentUICulture;
                value = GetString(text);
            }

            if (string.IsNullOrEmpty(value))
            {
                return text;
            }

            return value;
        }

        #endregion
    }
}