using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{
	public partial class PieSeriesViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> PieSeriesData { get; set; }
		public ObservableCollection<ChartDataModel> SemiCircularData { get; set; }
		public ObservableCollection<ChartDataModel> GroupToData { get; set; }
		public ObservableCollection<ChartDataModel> GoldMedalCountData { get; set; }

		public PieSeriesViewModel()
		{
			PieSeriesData =
			[
				new ChartDataModel("David", 16.6),
				new ChartDataModel("Steve", 14.6),
				new ChartDataModel("Jack", 18.6),
				new ChartDataModel("John", 20.5),
				new ChartDataModel("Regev", 39.5),
		   ];

			SemiCircularData =
			[
				new ChartDataModel("Product A", 750),
				new ChartDataModel("Product B", 463),
				new ChartDataModel("Product C", 389),
				new ChartDataModel("Product D", 697),
				new ChartDataModel("Product E", 251)
			];

			GroupToData =
			[
				new ChartDataModel( "US",22.90,0.244),
				new ChartDataModel("China",16.90,0.179),
				new ChartDataModel( "Japan",5.10,0.054),
				new ChartDataModel("Germany",4.20,0.045),
				new ChartDataModel("UK",3.10,0.033),
				new ChartDataModel("India",2.90,0.031),
				new ChartDataModel("France",2.90,0.031),
				new ChartDataModel( "Italy",2.10,0.023),
				new ChartDataModel( "Canada",2.00,0.021),
				new ChartDataModel( "Korea",1.80,0.019),
				new ChartDataModel("Russia",1.60,0.017),
				new ChartDataModel("Brazil",1.60,0.017),
				new ChartDataModel("Australia",1.60,0.017),
				new ChartDataModel("Spain",1.40,0.015),
				new ChartDataModel("Mexico",1.30,0.014),
				new ChartDataModel("Indonesia",1.20,0.012),
				new ChartDataModel("Iran",1.10,0.011),
				new ChartDataModel("Netherlands",1.00,0.011),
				new ChartDataModel("Saudi Arabia",0.80,0.009),
				new ChartDataModel("Switzerland",0.80,0.009),
				new ChartDataModel("Turkey",0.80,0.008),
				new ChartDataModel("Taiwan",0.80,0.008),
				new ChartDataModel("Poland",0.70,0.007),
				new ChartDataModel("Sweden",0.60,0.007),
				new ChartDataModel("Belgium",0.60,0.006),
				new ChartDataModel("Thailand",0.50,0.006),
				new ChartDataModel("Ireland",0.50,0.005),
				new ChartDataModel("Austria",0.50,0.005),
				new ChartDataModel("Nigeria",0.50,0.005),
				new ChartDataModel("Israel",0.50,0.005),
				new ChartDataModel("Argentina",0.50,0.005),
				new ChartDataModel("Norway",0.40,0.005),
				new ChartDataModel("South Africa",0.40,0.004),
				new ChartDataModel("UAE",0.40,0.004),
				new ChartDataModel("Denmark",0.40,0.004),
				new ChartDataModel("Egypt",0.40,0.004),
				new ChartDataModel("Philippines",0.40,0.004),
				new ChartDataModel("Singapore",0.40,0.004),
				new ChartDataModel("Malaysia",0.40,0.004),
				new ChartDataModel("Hong Kong SAR",0.40,0.004),
				new ChartDataModel("Vietnam",0.40,0.004),
				new ChartDataModel("Bangladesh",0.40,0.004),
				new ChartDataModel("Chile",0.30,0.004),
				new ChartDataModel("Colombia",0.30,0.003),
				new ChartDataModel("Finland",0.30,0.003),
				new ChartDataModel("Romania",0.30,0.003),
				new ChartDataModel("Czech Republic",0.30,0.003),
				new ChartDataModel("Portugal",0.30,0.003),
				new ChartDataModel("Pakistan",0.30,0.003),
				new ChartDataModel("New Zealand",0.20,0.003),
			];

			GoldMedalCountData =
			[
				new ChartDataModel("USA", 46),
				new ChartDataModel("Great Britain", 27),
				new ChartDataModel("China", 26),
				new ChartDataModel("Russian Federation", 19),
				new ChartDataModel("Germany", 17),
				new ChartDataModel("Japan", 12),
				new ChartDataModel("France", 10),
				new ChartDataModel("Korea", 9),
				new ChartDataModel("Italy", 8),
				new ChartDataModel("Australia", 8 ),
				new ChartDataModel("Netherlands", 8 ),
				new ChartDataModel("Hungary", 8),
				new ChartDataModel("Brazil", 7),
				new ChartDataModel("Spain", 7),
				new ChartDataModel("Kenya", 6),
				new ChartDataModel("Jamaica", 6),
				new ChartDataModel("Croatia", 5),
				new ChartDataModel("Cuba", 5),
				new ChartDataModel("New Zealand", 4),
#if WINDOWS || MACCATALYST 
                new ChartDataModel("Canada", 4),
				new ChartDataModel("Uzbekistan", 4),
				new ChartDataModel("Kazakhstan", 3),
				new ChartDataModel("Colombia", 3),
				new ChartDataModel("Switzerland", 3),
#endif
            ];
		}
	}
}
