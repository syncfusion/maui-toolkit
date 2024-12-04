using System.Globalization;
using Syncfusion.Maui.Toolkit.Localization;

namespace Syncfusion.Maui.Toolkit.NumericEntry
{
    /// <summary>
    /// 
    /// </summary>
    public class SfNumericEntryResources : LocalizationResourceAccessor
    {
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
                Culture = CultureInfo;
                value = GetString(text);
            }

            if (string.IsNullOrEmpty(value))
            {
                value = text;
            }

            return value;
        }
    }
}
