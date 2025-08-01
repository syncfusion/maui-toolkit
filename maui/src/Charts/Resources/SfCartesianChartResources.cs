using Syncfusion.Maui.Toolkit.Localization;
using System.Globalization;

namespace Syncfusion.Maui.Toolkit.Charts
{

	/// <summary>
	/// Represents the localization resource accessor for the <see cref="SfCartesianChart"/> control.
	/// This class is responsible for retrieving culture-specific localized strings based on the application's current UI culture settings.
	/// </summary>
	public class SfCartesianChartResources : LocalizationResourceAccessor
	{
		internal static CultureInfo CultureInfo => CultureInfo.CurrentUICulture;

		private static string GetResourceString(string key, string defaultValue)
		{
			string? value = string.Empty;
			if (ResourceManager != null)
			{
				Culture = CultureInfo;
				value = GetString(key);
			}
			return string.IsNullOrEmpty(value) ? defaultValue : value;
		}

		internal static string High => GetResourceString("High", "High");

		internal static string Low => GetResourceString("Low", "Low");

		internal static string Open => GetResourceString("Open", "Open");

		internal static string Close => GetResourceString("Close", "Close");

		internal static string Maximum => GetResourceString("Maximum", "Maximum");

		internal static string Minimum => GetResourceString("Minimum", "Minimum");

		internal static string Median => GetResourceString("Median", "Median");

		internal static string Q3 => GetResourceString("Q3", "Q3");

		internal static string Q1 => GetResourceString("Q1", "Q1");
	}
}