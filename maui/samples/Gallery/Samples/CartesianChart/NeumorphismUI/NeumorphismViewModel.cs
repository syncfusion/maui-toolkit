using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class NeumorphismViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> NeumorphismColumnData { get; set; }
		public ObservableCollection<ChartDataModel> NeumorphismSpAreaData { get; set; }

		public NeumorphismViewModel()
		{
			NeumorphismColumnData =
			[
				new ChartDataModel("Jan", 51),
				new ChartDataModel("Feb", 81),
				new ChartDataModel("Mar", 45),
				new ChartDataModel("Apr", 33),
				new ChartDataModel("May", 61),           
#if WINDOWS || MACCATALYST
                new ChartDataModel("Jun", 81),
				new ChartDataModel("Jul", 45),
				new ChartDataModel("Aug", 33),
				new ChartDataModel("Sep", 60)
#endif

           ];

			NeumorphismSpAreaData =
			[
				new ChartDataModel("Jan", 5,15),
				new ChartDataModel("Feb", 25,91),
				new ChartDataModel("Mar", 40,70),
				new ChartDataModel("Apr", 110,35),
				new ChartDataModel("May", 55,45),
				new ChartDataModel("Jun", 25,91),
				new ChartDataModel("Jul", 60,21),
			];
		}
	}
}
