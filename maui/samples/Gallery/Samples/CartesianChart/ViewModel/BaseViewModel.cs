using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class BaseViewModel : INotifyPropertyChanged
	{
		public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

		public event PropertyChangedEventHandler? PropertyChanged;

		public ObservableCollection<Brush> PaletteBrushes { get; set; }
		public ObservableCollection<Brush> CustomColor2 { get; set; }
		public ObservableCollection<Brush> CustomColor3 { get; set; }

		public ObservableCollection<Brush> CustomColor4 { get; set; }
		public ObservableCollection<Brush> AlterColor1 { get; set; }

		bool _enableAnimation = true;
		public bool EnableAnimation
		{
			get { return _enableAnimation; }
			set
			{
				_enableAnimation = value;
				OnPropertyChanged("EnableAnimation");
			}
		}

		public void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		public BaseViewModel()
		{

			PaletteBrushes =
			[
			   new SolidColorBrush(Color.FromArgb("#314A6E")),
				 new SolidColorBrush(Color.FromArgb("#48988B")),
				 new SolidColorBrush(Color.FromArgb("#5E498C")),
				 new SolidColorBrush(Color.FromArgb("#74BD6F")),
				 new SolidColorBrush(Color.FromArgb("#597FCA"))
			];

			CustomColor2 =
			[
				new SolidColorBrush(Color.FromArgb("#519085")),
				new SolidColorBrush(Color.FromArgb("#F06C64")),
				new SolidColorBrush(Color.FromArgb("#FDD056")),
				new SolidColorBrush(Color.FromArgb("#81B589")),
				new SolidColorBrush(Color.FromArgb("#88CED2"))
			];

			CustomColor3 =
			[
				new SolidColorBrush(Color.FromArgb("#04ABC1")),
				new SolidColorBrush(Color.FromArgb("#234A76")),
				new SolidColorBrush(Color.FromArgb("#DD6031")),
				new SolidColorBrush(Color.FromArgb("#7642A9")),
				new SolidColorBrush(Color.FromArgb("#495963"))
			];

			CustomColor4 =
			[
				new SolidColorBrush(Color.FromArgb("#95DB9C")),
				new SolidColorBrush(Color.FromArgb("#B95375")),
				new SolidColorBrush(Color.FromArgb("#56BBAF")),
				new SolidColorBrush(Color.FromArgb("#606D7F")),
				new SolidColorBrush(Color.FromArgb("#E99941")),
				new SolidColorBrush(Color.FromArgb("#327DBE")),
				new SolidColorBrush(Color.FromArgb("#E7695A")),
				new SolidColorBrush(Color.FromArgb("#2D4552")),
				new SolidColorBrush(Color.FromArgb("#4E9B8F")),
				new SolidColorBrush(Color.FromArgb("#E9A56C")),
			];

			AlterColor1 =
			[
				new SolidColorBrush(Color.FromArgb("#314A6E")),
				 new SolidColorBrush(Color.FromArgb("#48988B")),
			];
		}
	}

	public class TooltipValuesConverter : IValueConverter
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
					case "High":
						return model.High;
					case "Low":
						return model.Low;
					case "Size":
						return model.Size;
					case "Label":
						return model.Label;
					case "Data":
						return model.Data;
					case "Date":
						return model.Date;
					case "Year":
						return model.Year;
					case "Organic":
						return model.Organic;
					case "FairTrade":
						return model.FairTrade;
					case "VegAlternatives":
						return model.VegAlternatives;
					case "Others":
						return model.Others;
					case "Value1":
						return model.Value1;
					case "GrossLastYearDelta":
						return model.GrossLastYearDelta;
					case "Peru":
						return model.Peru;
					case "Ethiopia":
						return model.Ethiopia;
					case "Mali":
						return model.Mali;
					case "Canada":
						return model.Canada;
					case "Energy":
						return model.Energy;
					case "Department":
						return model.Department;
					case "TopMovie":
						return model.TopMovie;
					case "TotalGrossInBillion":
						return model.TotalGrossInBillion;

				}
			}
			else if (value is LineSeries series)
			{
				switch (parameter?.ToString())
				{
					case "Label":
						return series.Label;
					case "MarkerSettings.Stroke":
						return series.MarkerSettings.Stroke;

				}
				var line = value as LineSeriesExt;
				switch (parameter?.ToString())
				{
					case "PathData":
						return line?.PathData;				
				}
			}

			return value;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return value;
		}
	}
	
	public class ChartColorModel : ObservableCollection<Brush>
    {

	}
}
