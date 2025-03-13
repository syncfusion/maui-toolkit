
using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public class EmptyPointViewModel : BaseViewModel
	{
		private EmptyPointMode emptypointMode;
		public EmptyPointMode EmptyPointMode
		{
			get => emptypointMode;
			set
			{
				if (emptypointMode != value)
				{
					emptypointMode = value;
					OnPropertyChanged(nameof(EmptyPointMode));
				}
			}
		}

		public string[] EmptyPointModeValues => ["None", "Zero", "Average"];

		public ObservableCollection<ChartDataModel> EmptyPointData { get; set; }

		public EmptyPointViewModel()
		{

			EmptyPointData =
			[
				new ChartDataModel("Tropical", 85, 70, 50),
				new ChartDataModel("Continental", 80, 65, 40),
				new ChartDataModel("Mediterranean", 82, 60, 30),
				new ChartDataModel("Arid", double.NaN, double.NaN, double.NaN),
				new ChartDataModel("Polar", double.NaN, double.NaN, double.NaN),
				new ChartDataModel("Temperate", 75, 50, 40),
				new ChartDataModel("Oceanic", 90, 65, 40),
				new ChartDataModel("Highland", 95, 60, 30),
			];
		}
	}
}
