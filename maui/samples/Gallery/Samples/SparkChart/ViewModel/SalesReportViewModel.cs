using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.SparkChart
{
	public class SalesReportViewModel : BaseViewModel
	{
		#region Properties

		private ObservableCollection<OrderInfo> orderInfo;
		public ObservableCollection<OrderInfo> OrderInfoCollection
		{
			get { return orderInfo; }
			set { orderInfo = value; }
		}

		#endregion

		#region Constructor

		public SalesReportViewModel()
		{
			orderInfo = new ObservableCollection<OrderInfo>();
			GenerateOrders();
		}

		#endregion

		#region Methods

		public void GenerateOrders()
		{
			orderInfo.Add(new OrderInfo("1001", "James", "Germany", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1002", "Emily", "Mexico", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1003", "Michael", "Mexico", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1004", "Sarah", "UK", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1005", "David", "Sweden", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1006", "Moore", "Germany", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1007", "White", "France", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1008", "Davis", "Spain", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1009", "Lewis", "Brazil", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1010", "Jessica", "USA", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1011", "Daniel", "USSR", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1012", "Harris", "India", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1013", "Clark", "Italy", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
			orderInfo.Add(new OrderInfo("1014", "Ryan", "Spain", GetRandomDoubleList(), GetWeeklyDataList(), GetRandomDoubleList(-5, 10)));
		}

		#endregion

	}
}