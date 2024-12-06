using System.Globalization;

namespace Syncfusion.Maui.Toolkit
{
	/// <summary>
	/// Used to convert a legend item color with DisableBrush or IconBrush when toggle.
	/// </summary>
	internal class ToggleColorConverter : IValueConverter
	{
		public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			if (value != null && parameter != null)
			{
				bool toggled = (bool)value;

				if (parameter is SfShapeView shape)
				{
					if (shape.BindingContext is LegendItem item)
					{
						return toggled ? item.DisableBrush : item.IconBrush;
					}
				}

				if (parameter is Label label)
				{
					if (label.BindingContext is LegendItem item)
					{
						SolidColorBrush disableBrush = (SolidColorBrush)item.DisableBrush;
						return toggled ? disableBrush.Color : item.TextColor;
					}
				}
			}

			return Colors.Transparent;
		}

		public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			return value;
		}
	}

	//Used to get the legend item color with disable brush or selection brush.
	internal class MultiBindingIconBrushConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values != null && targetType.IsAssignableFrom(typeof(Brush)))
			{
				if (parameter is SfShapeView shape)
				{
					if (shape.BindingContext is LegendItem item)
					{
						return item.IsToggled ? item.DisableBrush : item.IconBrush;
					}
				}
			}

			if (values != null && targetType.IsAssignableFrom(typeof(Color)))
			{
				if (parameter is Label label)
				{
					if (label.BindingContext is LegendItem item)
					{
						var toggle = item.IsToggled;
						SolidColorBrush disableBrush = (SolidColorBrush)item.DisableBrush;
						return toggle ? disableBrush.Color : item.TextColor;
					}
				}
			}

			return new SolidColorBrush(Colors.Transparent);
		}

		public object[]? ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
