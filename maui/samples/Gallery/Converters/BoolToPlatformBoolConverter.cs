using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.Converters
{
	/// <summary>
	/// 
	/// </summary>
	public class BoolToPlatformBoolConverter : IValueConverter
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
			if (value != null && param != null)
			{
				if (BaseConfig.RunTimeDevicePlatform == SBDevice.Windows)
				{
					return (bool)value;
				}
				else
				{
					if (param.ToUpperInvariant() == "IMAGE")
					{
						return false;
					}
					else
					{
						return true;
					}
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
