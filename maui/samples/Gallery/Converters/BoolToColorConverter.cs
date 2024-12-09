using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.Converters
{
	/// <summary>
	/// 
	/// </summary>
	public class BoolToColorConverter : IValueConverter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			string? param = (parameter as String);

#if WINDOWS || MACCATALYST
            Color labelColor = Application.Current?.PlatformAppTheme == AppTheme.Light ? Color.FromArgb("#DE000000") : Colors.White;
#else
			Color labelColor = Application.Current?.PlatformAppTheme == AppTheme.Light ? Color.FromArgb("#49454F") : Color.FromArgb("#C4CAD0");
#endif
#if WINDOWS || MACCATALYST
            Color selectedLabelColor = Application.Current?.PlatformAppTheme == AppTheme.Light ? Color.FromArgb("#3B33DE") : Color.FromArgb("#D0BCFF");
#else
			Color selectedLabelColor = Application.Current?.PlatformAppTheme == AppTheme.Light ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#381E72");
#endif
			Color chipColor = Application.Current?.PlatformAppTheme == AppTheme.Light ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#1C1B1F");
#if WINDOWS || MACCATALYST
            Color selectedChipColor = Application.Current?.PlatformAppTheme == AppTheme.Light ? Color.FromArgb("#F1E8FF") : Color.FromArgb("#381E72");
#else
			Color selectedChipColor = Application.Current?.PlatformAppTheme == AppTheme.Light ? Color.FromArgb("#6750A4") : Color.FromArgb("#D0BCFF");
#endif
			Color chipBorderColor = Application.Current?.PlatformAppTheme == AppTheme.Light ? Color.FromArgb("#CAC4D0") : Color.FromArgb("#49454F");
			Color tabItem = Application.Current?.PlatformAppTheme == AppTheme.Light ? Color.FromArgb("#49454F") : Color.FromArgb("#CAC4D0");
			Color selectedTabItem = Application.Current?.PlatformAppTheme == AppTheme.Light ? Color.FromArgb("#6750A4") : Color.FromArgb("#D0BCFF");

			if (value != null && param != null)
			{
				if (param.ToUpperInvariant() == "LABEL")
				{
					return (bool)value ? selectedLabelColor : labelColor;
				}

				if (param.ToUpperInvariant() == "GRID")
				{
					return (bool)value ? Color.FromRgba("#F1E8FF") : Colors.White;
				}

				if (param.ToUpperInvariant() == "BOXVIEW")
				{
					return (bool)value ? Color.FromRgba("#EAEAEA") : Colors.White;
				}

				if (param.ToUpperInvariant() == "BORDER")
				{
					return (bool)value ? Color.FromRgba("#CCCCCC") : Color.FromRgba("#E1E1E1");
				}

				if (param.ToUpperInvariant() == "ISSELECTEDCHIP")
				{
					return (bool)value ? selectedChipColor : chipColor;
				}

				if (param.ToUpperInvariant() == "ISSELECTEDCHIPBORDER")
				{
					return (bool)value ? Colors.Transparent : chipBorderColor;
				}

				if (param.ToUpperInvariant() == "ISSELECTEDBOX")
				{
					return (bool)value ? selectedChipColor : Colors.Transparent;
				}

				if (param.ToUpperInvariant() == "ISSELECTEDLABEL")
				{
					return (bool)value ? selectedLabelColor : labelColor;
				}

				if (param.ToUpperInvariant() == "FRAME")
				{
					return (bool)value ? Colors.White : Color.FromRgba("#608C1B");
				}

				if (param.ToUpperInvariant() == "ISTABITEM")
				{
					return (bool)value ? selectedTabItem : tabItem;
				}
			}
			throw new ArgumentNullException("Value should not be null");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
