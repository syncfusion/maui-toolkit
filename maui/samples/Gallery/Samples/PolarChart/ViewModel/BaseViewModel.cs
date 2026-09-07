using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.PolarChart.SfPolarChart
{
	public partial class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		public ObservableCollection<Brush> CustomBrush1 { get; set; }
		public ObservableCollection<Brush> CustomBrush2 { get; set; }
		public ObservableCollection<Brush> CustomBrush3 { get; set; }
		public void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		public BaseViewModel()
		{
			CustomBrush1 =
			[
			   new SolidColorBrush(Color.FromArgb("#0DC920")),
			   new SolidColorBrush(Color.FromArgb("#F5921F")),
			   new SolidColorBrush(Color.FromArgb("#E64191")),
			];

			CustomBrush2 =
			[
			   new SolidColorBrush(Color.FromArgb("#0DC920")),
			   new SolidColorBrush(Color.FromArgb("#2A9AF3")),
			   new SolidColorBrush(Color.FromArgb("#F5921F"))
			];

			CustomBrush3 =
			[
			   new SolidColorBrush(Color.FromArgb("#A033F5")),
			   new SolidColorBrush(Color.FromArgb("#F5921F")),
			];
		}
	}

	public partial class ChartColorModel : ObservableCollection<Brush>
	{

	}

	public class TooltipValueConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is ChartDataModel model)
			{
				switch (parameter?.ToString())
				{
					case "Category":
						return model.Category;
					case "Value1":
						return model.Value1;
					case "Value2":
						return model.Value2;
				}
			}
			else if (value is PolarAreaSeries series)
			{
				return series.Label;
			}

			return value;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return value;
		}
	}
}