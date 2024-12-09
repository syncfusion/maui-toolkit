using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class StackedColumn100ViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> TeslaVehicleData { get; set; }
		public ObservableCollection<ChartDataModel> MacysSalesData { get; set; }
		public ObservableCollection<ChartDataModel> USElectricityData { get; set; }
		public ObservableCollection<ChartDataModel> UKElectricityData { get; set; }

		public StackedColumn100ViewModel()
		{
			TeslaVehicleData =
			[
				new ChartDataModel("2016",14.8,14.4,24.5,22.2),
				new ChartDataModel("2017",25,22,26.2,29.9),
				new ChartDataModel("2018",30,40.7,83.5,90.7),
				new ChartDataModel("2019",63,95.2,97,112),
				new ChartDataModel("2020",88.4,90.7,139.3,180.6),
				new ChartDataModel("2021",184.8,201.25,241.3,308.6),
				new ChartDataModel("2022",310.05,254.7,343.83,405.28)
			];

			MacysSalesData =
			[
				new ChartDataModel("2017",9444,5765,5610,4120),
				new ChartDataModel("2018",9457,5642,5699,4173),
				new ChartDataModel("2019",9454,5411,5628,4067),
				new ChartDataModel("2020",7206,2909,3486,3745),
				new ChartDataModel("2021",10119,4433,5252,4656)
			];

			USElectricityData =
			[
				new ChartDataModel("2017",62.68,19.86,17.45),
				new ChartDataModel("2018",63.34,19.21,17.45),
				new ChartDataModel("2019",62.24,19.46,18.24),
				new ChartDataModel("2020",60.15,19.54,20.32),
				new ChartDataModel("2021",60.57,18.74,20.75),

			];

			UKElectricityData =
			[
				new ChartDataModel("2017",49.51,20.99,29.50),
				new ChartDataModel("2018",47.02,19.69,33.29),
				new ChartDataModel("2019",45.08,17.47,37.46),
				new ChartDataModel("2020",40.60,16.54,42.86),
				new ChartDataModel("2021",44.95,15.26,39.78),
			];
		}
	}
}
