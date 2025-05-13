using System.Collections.ObjectModel;
using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class TooltipViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> ChartData1 { get; set; }

		public TooltipViewModel()
		{
			ChartData1 =
			[
				new ChartDataModel(2004, 42.63, 34.73),
				new ChartDataModel( 2005,43.4, 43.4),
				new ChartDataModel( 2006,43.66, 38.09),
				new ChartDataModel( 2007,43.54, 44.71),
				new ChartDataModel( 2008,43.60, 45.32),
				new ChartDataModel( 2009,43.50, 46.20),
				new ChartDataModel( 2010,43.35, 46.99),
				new ChartDataModel( 2011,43.62, 49.17),
				new ChartDataModel( 2012,43.93, 50.64),
			];
		}
	}

	public class TooltipValuesConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is ChartDataModel model)
			{
				var param = parameter?.ToString();

				switch (param)
				{
					case "Value":
						return $": {model.Value}M";
					case "Size":
						return $": {model.Size}M";
					case "Value1":
						return model.Value1;
				}
			}

			return value;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
