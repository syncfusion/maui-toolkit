using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.Converters
{
	public class StringToVisibilityConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return !string.IsNullOrEmpty(value as string);
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return null;
		}

	}
}
