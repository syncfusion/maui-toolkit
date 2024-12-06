using System.Collections.ObjectModel;
using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.Converters
{
	/// <summary>
	/// 
	/// </summary>
	public class IsDirectSampleCategoryConverter : IValueConverter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is ObservableCollection<SampleSubCategoryModel> sampleSubCategoryModel)
			{
				if (sampleSubCategoryModel.Count == 1)
				{
					if (!sampleSubCategoryModel[0].IsApplicable)
					{
						if (sampleSubCategoryModel[0].CardLayouts?.Count == 1)
						{
							if (!sampleSubCategoryModel[0].CardLayouts![0].IsApplicable)
							{
								return false;
							}
						}
					}
				}
			}

			return true;
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
