using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class RangeBarSerieViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> TemperatureData { get; set; }
		public RangeBarSerieViewModel()
		{
			TemperatureData =
		[
 #if ANDROID || IOS
                   new ChartDataModel("Jan",7, 3),
                   new ChartDataModel("Feb",8, 3),
                   new ChartDataModel("Mar",12, 5),
                   new ChartDataModel("Apr",16, 7),
                   new ChartDataModel("May",20, 11),
                   new ChartDataModel("Jun",23, 14),
                   new ChartDataModel("Jul",25, 16),
                   new ChartDataModel("Aug",25, 16),
                   new ChartDataModel("Sep",21, 13),
                   new ChartDataModel("Oct",16, 10),
                   new ChartDataModel("Nov",11, 6),
                   new ChartDataModel("Dec",8, 3),

#else
                   new ChartDataModel("January",7, 3),
				   new ChartDataModel("February",8, 3),
				   new ChartDataModel("March",12, 5),
				   new ChartDataModel("April",16, 7),
				   new ChartDataModel("May",20, 11),
				   new ChartDataModel("June",23, 14),
				   new ChartDataModel("July",25, 16),
				   new ChartDataModel("August",25, 16),
				   new ChartDataModel("September",21, 13),
				   new ChartDataModel("October",16, 10),
				   new ChartDataModel("November",11, 6),
				   new ChartDataModel("December",8, 3),
#endif
            ];
		}
	}
}
