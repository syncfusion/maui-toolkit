using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{
	public partial class DoughnutSeriesViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> DoughnutSeriesData { get; set; }
		public ObservableCollection<ChartDataModel> SemiCircularData { get; set; }
		public ObservableCollection<ChartDataModel> CenterElevationData { get; set; }
		public ObservableCollection<ChartDataModel> GroupToData { get; set; }
		public ObservableCollection<ChartDataModel> CapStyleData { get; set; }

		int _selectedIndex = 1;
		string _name = "";
		int _value1;
		int _total = 357580;

		public Geometry? TruckPathData { get; set; } = (Geometry?) (new PathGeometryConverter().ConvertFromInvariantString("M46.5 22.4813V22.4437C46.503 22.3992 46.4966 22.3545 46.4812 22.3125V22.2562C46.4812 22.2187 46.4625 22.1812 46.4625 22.1437V22.1063L46.425 21.9562H46.4062L43.7812 15.4313C43.5721 14.8602 43.1912 14.3679 42.691 14.0221C42.1907 13.6763 41.5956 13.4939 40.9875 13.5H34.5V12C34.5 11.6022 34.342 11.2206 34.0607 10.9393C33.7794 10.658 33.3978 10.5 33 10.5H4.5C3.70435 10.5 2.94129 10.8161 2.37868 11.3787C1.81607 11.9413 1.5 12.7044 1.5 13.5V34.5C1.5 35.2956 1.81607 36.0587 2.37868 36.6213C2.94129 37.1839 3.70435 37.5 4.5 37.5H6.9375C7.26795 38.7906 8.01855 39.9346 9.07096 40.7515C10.1234 41.5684 11.4177 42.0118 12.75 42.0118C14.0823 42.0118 15.3766 41.5684 16.429 40.7515C17.4814 39.9346 18.232 38.7906 18.5625 37.5H29.4375C29.7679 38.7906 30.5185 39.9346 31.571 40.7515C32.6234 41.5684 33.9177 42.0118 35.25 42.0118C36.5823 42.0118 37.8766 41.5684 38.929 40.7515C39.9814 39.9346 40.732 38.7906 41.0625 37.5H43.5C44.2956 37.5 45.0587 37.1839 45.6213 36.6213C46.1839 36.0587 46.5 35.2956 46.5 34.5V22.5V22.4813ZM34.5 16.5H40.9875L42.7875 21H34.5V16.5ZM4.5 13.5H31.5V25.5H4.5V13.5ZM12.75 39C12.1567 39 11.5766 38.8241 11.0833 38.4944C10.5899 38.1648 10.2054 37.6962 9.97836 37.1481C9.7513 36.5999 9.69189 35.9967 9.80764 35.4147C9.9234 34.8328 10.2091 34.2982 10.6287 33.8787C11.0482 33.4591 11.5828 33.1734 12.1647 33.0576C12.7467 32.9419 13.3499 33.0013 13.898 33.2284C14.4462 33.4554 14.9148 33.8399 15.2444 34.3333C15.5741 34.8266 15.75 35.4067 15.75 36C15.75 36.7956 15.4339 37.5587 14.8713 38.1213C14.3087 38.6839 13.5456 39 12.75 39ZM35.25 39C34.6567 39 34.0766 38.8241 33.5833 38.4944C33.0899 38.1648 32.7054 37.6962 32.4784 37.1481C32.2513 36.5999 32.1919 35.9967 32.3076 35.4147C32.4234 34.8328 32.7091 34.2982 33.1287 33.8787C33.5482 33.4591 34.0828 33.1734 34.6647 33.0576C35.2467 32.9419 35.8499 33.0013 36.398 33.2284C36.9462 33.4554 37.4148 33.8399 37.7444 34.3333C38.074 34.8266 38.25 35.4067 38.25 36C38.25 36.7956 37.9339 37.5587 37.3713 38.1213C36.8087 38.6839 36.0456 39 35.25 39Z"));
		
		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set
			{
				_selectedIndex = value;
				UpdateIndex(value);
				base.OnPropertyChanged("SelectedIndex");
			}
		}
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				base.OnPropertyChanged("Name");
			}
		}
		public int Value
		{
			get { return _value1; }
			set
			{
				_value1 = value;
				base.OnPropertyChanged("Value");
			}
		}

		public int Total
		{
			get { return _total; }
			set
			{
				_total = value;
			}
		}

		private void UpdateIndex(int value)
		{
			if (value >= 0 && value < DoughnutSeriesData.Count)
			{
				var model = DoughnutSeriesData[value];
				if (model != null && model.Name != null)
				{
					Name = model.Name;
					double sum = DoughnutSeriesData.Sum(item => item.Value);
					double SelectedItemsPercentage = model.Value / sum * 100;
					SelectedItemsPercentage = Math.Floor(SelectedItemsPercentage * 100) / 100;
					Value = (int)SelectedItemsPercentage;
				}
			}
		}

		public DoughnutSeriesViewModel()
		{
			DoughnutSeriesData =
			[
				new ChartDataModel("Labor", 10),
				new ChartDataModel("Legal", 8),
				new ChartDataModel("Production", 7),
				new ChartDataModel("License", 5),
				new ChartDataModel("Facilities", 10),
				new ChartDataModel("Taxes", 6),
				new ChartDataModel("Insurance", 18)
		    ];

			SemiCircularData =
			[
				new ChartDataModel("Product A", 750),
				new ChartDataModel("Product B", 463),
				new ChartDataModel("Product C", 389),
				new ChartDataModel("Product D", 697),
				new ChartDataModel("Product E", 251)
			];

			CenterElevationData =
			[
				new ChartDataModel("Agriculture",51),
				new ChartDataModel("Forest",30),
				new ChartDataModel("Water",5.2),
				new ChartDataModel("Others",14),
			];

			GroupToData =
			[
				new ChartDataModel("US",51.30,0.39),
				new ChartDataModel("China",20.90,0.16),
				new ChartDataModel("Japan",11.00,0.08),
				new ChartDataModel("France",4.40,0.03),
				new ChartDataModel("UK",4.30,0.03),
				new ChartDataModel ("Canada",4.00,0.03),
				new ChartDataModel("Germany",3.70,0.03),
				new ChartDataModel("Italy",2.90,0.02),
				new ChartDataModel("KY",2.70,0.02),
				new ChartDataModel("Brazil",2.40,0.02),
				new ChartDataModel("South Korea",2.20,0.02),
				new ChartDataModel("Australia",2.20,0.02),
				new ChartDataModel("Netherlands",1.90,0.01),
				new ChartDataModel("Spain",1.90,0.01),
				new ChartDataModel("India",1.30,0.01),
				new ChartDataModel("Ireland",1.00,0.01),
				new ChartDataModel("Mexico",1.00,0.01),
				new ChartDataModel("Luxembourg",0.90,0.01),
			];

			CapStyleData=
		    [
				new ChartDataModel("Delivered", 56),
				new ChartDataModel("Cancelled", 27),
				new ChartDataModel("Scheduled", 17),
		    ];
		}
	}
}
