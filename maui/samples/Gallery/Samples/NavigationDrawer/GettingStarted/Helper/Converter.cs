using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.NavigationDrawer.NavigationDrawer
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
				return string.Empty;

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

	#region TextColorConverter

	/// <summary>
	/// Converter class helps to convert TextColor based on IsOpened property.
	/// </summary>
	public class TextColorConverter : IValueConverter
	{
		public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			var isOpened = (bool)value!;

			if (parameter != null)
			{
				if (!isOpened)
				{
					return Color.FromArgb("#6750A4");
				}
			}

			if (!isOpened)
			{
				return Color.FromArgb("#1C1B1F");
			}

			return Color.FromArgb("#49454F");
		}

		public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region GroupHeaderTextConverter

	/// <summary>
	/// Converter class helps to convert GroupHeader text based on Date property.
	/// </summary>
	public class GroupHeaderTextConverter : IValueConverter
	{
		public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			switch ((GroupName)value!)
			{
				case GroupName.Yesterday:
					return "Yesterday";
				case GroupName.ThisWeek:
					return "This Week";
				case GroupName.LastWeek:
					return "Last Week";
				case GroupName.ThisMonth:
					return "This Month";
				case GroupName.LastMonth:
					return "Last Month";
				case GroupName.Older:
					return "Older";
				default:
					return "";
			}
		}

		public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
#endregion
