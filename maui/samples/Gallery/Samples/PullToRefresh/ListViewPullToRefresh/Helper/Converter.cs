using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.PullToRefresh
{
	#region DateTimeToStringConverter

	/// <summary>
	/// Converter class helps to convert DateTime to string format.
	/// </summary>
	public class DatetimeToStringConverter : IValueConverter
	{
		public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return string.Empty;
			}

			var datetime = (DateTime)value;
			int compare = datetime.Date.CompareTo(DateTime.Now.Date);

			if (compare == 0)
			{
				return datetime.ToLocalTime().ToString("t");
			}
			else if (datetime.Date.CompareTo(DateTime.Now.AddDays(-1).Date) == 0)
			{
				return "Yesterday";
			}
			else
			{
				return datetime.ToString("MMM dd");
			}
		}

		public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region FontAttributeConverter

	/// <summary>
	/// Converter class helps to convert FontAttribute based on IsOpened property.
	/// </summary>
	public class FontAttributeConverter : IValueConverter
	{
		public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			var isOpened = (bool)value!;

			if (!isOpened)
			{
				return FontAttributes.Bold;
			}

			return FontAttributes.None;
		}

		public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region TextOpacityConverter

	/// <summary>
	/// Converter class helps to convert TextOpacity based on IsOpened property.
	/// </summary>
	public class TextOpacityConverter : IValueConverter
	{
		public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			var isOpened = (bool)value!;

			if (parameter != null)
			{
				if (!isOpened)
				{
					return 1;
				}
			}

			if (!isOpened)
			{
				return 1;
			}

			return 0.8;
		}

		public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

}