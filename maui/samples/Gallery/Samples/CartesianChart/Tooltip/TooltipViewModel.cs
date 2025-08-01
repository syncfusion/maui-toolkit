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
}
