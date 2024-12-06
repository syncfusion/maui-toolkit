using System.Collections.ObjectModel;
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class StackedAreaViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> StackedAreaData { get; set; }
		public StackedAreaViewModel()
		{
			StackedAreaData =
			[
				new ChartDataModel(0.61, 0.03, 0.48, 0.23, "2000"),
				new ChartDataModel(0.81, 0.05, 0.53, 0.17, "2001"),
				new ChartDataModel(0.91, 0.06, 0.57, 0.17, "2002"),
				new ChartDataModel(1, 0.09, 0.61, 0.20, "2003"),
				new ChartDataModel(1.19, 0.14, 0.63, 0.23, "2004"),
				new ChartDataModel(1.47, 0.20, 0.64, 0.36, "2005"),
				new ChartDataModel(1.74, 0.29, 0.66, 0.43, "2006"),
				new ChartDataModel(1.98, 0.46, 0.76, 0.52, "2007"),
				new ChartDataModel(1.99, 0.64, 0.77, 0.72, "2008"),
				new ChartDataModel(1.70, 0.75, 0.55, 1.29, "2009"),
				new ChartDataModel(1.48, 1.06, 0.54, 1.38, "2010"),
				new ChartDataModel(1.38, 1.25, 0.57, 1.82, "2011"),
				new ChartDataModel(1.66, 1.55, 0.61, 2.16, "2012"),
				new ChartDataModel(1.66, 1.55, 0.67, 2.51, "2013"),
				new ChartDataModel(1.67, 1.65, 0.67, 2.61, "2014")
			];
		}
	}
}