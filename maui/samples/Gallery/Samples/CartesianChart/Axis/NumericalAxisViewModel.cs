using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class NumericalAxisViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> DataCollection1 { get; set; }
		public ObservableCollection<ChartDataModel> DataCollection2 { get; set; }
		public ObservableCollection<ChartDataModel> DataCollection3 { get; set; }

		public ObservableCollection<ChartDataModel> MultiAxisData { get; set; }
		public ObservableCollection<ChartDataModel> RangeStyle { get; set; }

		public ObservableCollection<ChartDataModel> InverseData { get; set; }
		public ObservableCollection<ChartDataModel> InverseData1 { get; set; }

		public ObservableCollection<ChartDataModel> CrossAxisData { get; set; }

		public NumericalAxisViewModel()
		{
			var date = new DateTime(2017, 01, 01);
			InverseData =
			[
				new ChartDataModel(date, 50, 38),
				new ChartDataModel(date.AddMonths(1), 43, 52),
				new ChartDataModel(date.AddMonths(2),42,54),
				new ChartDataModel(date.AddMonths(3),51,48),
				new ChartDataModel(date.AddMonths(4),52,46),
				new ChartDataModel(date.AddMonths(5),49,43),
				new ChartDataModel(date.AddMonths(6),39,52),
				new ChartDataModel(date.AddMonths(7),40,55),
				new ChartDataModel(date.AddMonths(8),47,52),
				new ChartDataModel(date.AddMonths(9),48,48),
				new ChartDataModel(date.AddMonths(10),54,46),
				new ChartDataModel(date.AddMonths(11),58,44),
			];

			InverseData1 =
			[
				new ChartDataModel( 2000, 50),
				new ChartDataModel( 2002, 65),
				new ChartDataModel( 2004, 42),
				new ChartDataModel( 2006, 64),
				new ChartDataModel( 2008, 42),
				new ChartDataModel( 2010, 20),
			];

			DataCollection1 =
			[
				new ChartDataModel(1,288, 254),
				new ChartDataModel(2,298, 299),
				new ChartDataModel(3,230, 234),
				new ChartDataModel(4,236, 240),
				new ChartDataModel(5,250, 242),
				new ChartDataModel(6,313, 281),
			];

			DataCollection2 =
			[
				new ChartDataModel(1,230),
				new ChartDataModel(2,228),
				new ChartDataModel(3,290),
				new ChartDataModel(4,348),
				new ChartDataModel(5,237),
			];

			DataCollection3 =
			[
				new ChartDataModel(0,32),
				new ChartDataModel(5,41),
				new ChartDataModel(10,50),
				new ChartDataModel(15,59),
				new ChartDataModel(20,68),
				new ChartDataModel(25,77),
				new ChartDataModel(30,86),
				new ChartDataModel(35,95),
				new ChartDataModel(40,104),
				new ChartDataModel(45,113),
				new ChartDataModel(50,122),
			];

			MultiAxisData =
			[
				new ChartDataModel( new DateTime(2019, 5, 1), 13, 69.8),
				new ChartDataModel( new DateTime(2019, 5, 2), 26, 87.8),
				new ChartDataModel( new DateTime(2019, 5, 3), 13, 78.8),
				new ChartDataModel( new DateTime(2019, 5, 4), 22, 75.2),
				new ChartDataModel( new DateTime(2019, 5, 5), 14, 68),
				new ChartDataModel( new DateTime(2019, 5, 6), 23, 78.8),
				new ChartDataModel( new DateTime(2019, 5, 7), 21, 80.6),
				new ChartDataModel( new DateTime(2019, 5, 8), 22, 73.4),
				new ChartDataModel( new DateTime(2019, 5, 9), 16, 78.8),
			];

			RangeStyle =
			[
			  new ChartDataModel( new DateTime(2018, 7, 1), 3.0),
			  new ChartDataModel( new DateTime(2018, 8, 1), 2.7),
			  new ChartDataModel( new DateTime(2018, 9, 1), 2.3),
			  new ChartDataModel( new DateTime(2018, 10, 1), 2.5),
			  new ChartDataModel( new DateTime(2018, 11, 1), 2.2),
			  new ChartDataModel( new DateTime(2018, 12, 1), 1.9),
			  new ChartDataModel( new DateTime(2019, 1, 1), 1.6),
			  new ChartDataModel( new DateTime(2019, 2, 1), 1.5),
			  new ChartDataModel( new DateTime(2019, 3, 1), 1.9),
			  new ChartDataModel( new DateTime(2019, 4, 1), 2),
			];

			CrossAxisData =
			[
				new ChartDataModel( -7,  -3),
				new ChartDataModel( -4.5,  -2),
				new ChartDataModel( -3.5,  0),
				new ChartDataModel( -3,  2),
				new ChartDataModel( 0,  7),
				new ChartDataModel( 3,  2),
				new ChartDataModel( 3.5,  0),
				new ChartDataModel( 4.5,  -2),
				new ChartDataModel( 7,  -3),
			];

		}
	}
}
