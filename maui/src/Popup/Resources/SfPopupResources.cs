using System.Globalization;
using Syncfusion.Maui.Toolkit.Localization;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// A resource accessor for localization that gives the localized string according to culture.
	/// </summary>
	public class SfPopupResources : LocalizationResourceAccessor
	{
		/// <summary>
		/// Gets the culture info.
		/// </summary>
		/// <value>The culture.</value>
		internal static CultureInfo CultureInfo
		{
			get => CultureInfo.CurrentUICulture;
		}

		/// <summary>
		/// Gets the localized string.
		/// </summary>
		/// <param name="text">Text type.</param>
		/// <returns>The string.</returns>
		internal static string GetLocalizedString(string text)
		{
			string? value = string.Empty;
			if (ResourceManager is not null)
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

		/// <summary>
		/// Method to get the default custom string based on culture.
		/// </summary>
		/// <param name="text">The default local string to be localized based on culture.</param>
		/// <returns>The default localized string.</returns>
		static string GetDefaultString(string text)
		{
			string value = string.Empty;
			if (text == "Title")
			{
				value = "Title";
			}
			else if (text == "Message")
			{
				value = "Popup Message";
			}
			else if (text == "DeclineButtonText")
			{
				value = "DECLINE";
			}
			else if (text == "AcceptButtonText")
			{
				value = "ACCEPT";
			}

			return value;
		}
	}
}