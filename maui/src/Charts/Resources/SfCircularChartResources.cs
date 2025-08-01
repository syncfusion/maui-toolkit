using System.Globalization;
using Syncfusion.Maui.Toolkit.Localization;

namespace Syncfusion.Maui.Toolkit.Charts
{

	/// <summary>
	/// Represents the localization resource accessor for the <see cref="SfCircularChart"/> control.
	/// This class is responsible for retrieving culture-specific localized strings based on the application's current UI culture settings.
	/// </summary>
	public class SfCircularChartResources : LocalizationResourceAccessor
	{
		internal static CultureInfo CultureInfo => CultureInfo.CurrentUICulture;

		internal static string Others
		{
			get
			{
				string? value = string.Empty;
				if (ResourceManager != null)
				{
					Culture = CultureInfo;
					value = GetString("Others");
				}
				return string.IsNullOrEmpty(value) ? "Others" : value;
			}
		}
	}
}