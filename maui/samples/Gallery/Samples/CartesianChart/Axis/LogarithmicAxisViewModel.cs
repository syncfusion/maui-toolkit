using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class LogarithmicAxisViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> DataCollection { get; set; }

		public LogarithmicAxisViewModel()
		{
			DataCollection =
			[
				new ChartDataModel("1995",80),
				new ChartDataModel("1996",200),
				new ChartDataModel("1997",400),
				new ChartDataModel("1998",600),
				new ChartDataModel("1999",700),
				new ChartDataModel("2000",1400),
				new ChartDataModel("2001",2000),
				new ChartDataModel("2002",4000),
				new ChartDataModel("2003",6000),
				new ChartDataModel("2004",8000),
				new ChartDataModel("2005",11000),
			];
		}
	}
}
