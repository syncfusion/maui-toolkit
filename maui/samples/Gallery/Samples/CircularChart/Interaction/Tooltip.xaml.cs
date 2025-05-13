using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{
	public partial class Tooltip : SampleView
	{
		public Tooltip()
		{
			InitializeComponent();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart.Handler?.DisconnectHandler();
		}
	}

	public class TooltipValueConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is ChartDataModel model)
			{
				switch (parameter?.ToString())
				{
					case "Name":
						return model.Name;
					case "Value":
						return model.Value;
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
