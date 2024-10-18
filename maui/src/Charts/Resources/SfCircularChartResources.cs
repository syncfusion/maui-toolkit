using Syncfusion.Maui.Toolkit.Localization;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Localization resource for <see cref="SfCircularChart"/>.
    /// </summary>
    public class SfCircularChartResources : LocalizationResourceAccessor
    {
        internal static string Others
        {
            get
            {
                var others = GetString("Others");
                return string.IsNullOrEmpty(others) ? "Others" : others;
            }
        }
    }
}
