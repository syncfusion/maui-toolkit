using System.Collections.ObjectModel;
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class StackedArea100ViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> EmissionData { get; }
		public StackedArea100ViewModel()
		{
			EmissionData =
			[
				new ChartDataModel(20, 32, "2000", 16, 24 ),
				new ChartDataModel(13, 34, "2001", 17, 32 ),
				new ChartDataModel(14, 38, "2002", 31, 27),
				new ChartDataModel(17, 44, "2003", 27, 34),
				new ChartDataModel(16, 48, "2004", 28, 43),
				new ChartDataModel(23, 41, "2005", 32, 45),
				new ChartDataModel(23, 40, "2006", 46, 51),
				new ChartDataModel(27, 40, "2007", 47, 76),
				new ChartDataModel(27, 40, "2008", 51, 85),
				new ChartDataModel(32, 35, "2009", 55, 86),
				new ChartDataModel(40, 35, "2010", 58, 87)
			];
		}
	}
}