using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class FastScatterSeriesViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> TrainTravelData { get; set; }
		public ObservableCollection<ChartDataModel> AutomobileTravelData { get; set; }

		public FastScatterSeriesViewModel()
		{
			TrainTravelData = new ObservableCollection<ChartDataModel>();
			AutomobileTravelData = new ObservableCollection<ChartDataModel>();

			for (int i = 1; i <= 2000; i++)
			{
				double distance = i * 0.7 + ((i % 15 - 7) * (i / 400.0));

				double variationFactor = ((i / 5) % 5) * 3.0;
				double increaseFactor = (i / 150.0);

				double trainScatter = ((i % 9 - 4) * 3.2 + (i % 13 - 6) * 2.1 + variationFactor) * (1 + i / 800.0);
				double trainSpeed = 12 + (Math.Log(i + 1) * 9) + (Math.Sin(i * 0.1) * 12) + trainScatter + increaseFactor * 5;

				double autoScatter = ((i % 6 - 3) * 3.8 + (i % 10 - 5) * 2.5 + variationFactor) * (1 + i / 900.0);
				double autoSpeed = 35 + (Math.Log(i + 1) * 11) + (Math.Cos(i * 0.15) * 10) + autoScatter + increaseFactor * 6;

				TrainTravelData.Add(new ChartDataModel(distance, Math.Max(10, trainSpeed)));
				AutomobileTravelData.Add(new ChartDataModel(distance, Math.Max(20, autoSpeed)));
			}
		}
	}
}
