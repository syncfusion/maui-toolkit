namespace Syncfusion.Maui.ControlsGallery.SparkChart
{
	public class Model
	{
		public double Performance { get; set; }
	}

	public class OrderInfo
	{
		private string? orderID;
		private string? customer;
		private string? shipCountry;
		private List<Model>? taxPerAnnum;
		private List<Model>? oneDayIndex;
		private List<Model>? yearGR;

		public string? OrderID
		{
			get { return orderID; }
			set { orderID = value; }
		}

		public string? ShipCountry
		{
			get { return shipCountry; }
			set { shipCountry = value; }
		}

		public string? Customer
		{
			get { return customer; }
			set { customer = value; }
		}

		public List<Model>? TaxPerAnnum
		{
			get { return taxPerAnnum; }
			set { taxPerAnnum = value; }
		}

		public List<Model>? OneDayIndex
		{
			get { return oneDayIndex; }
			set { oneDayIndex = value; }
		}

		public List<Model>? YearGR
		{
			get { return yearGR; }
			set { yearGR = value; }
		}

		public OrderInfo(string orderId, string customer, string country, List<Model> taxList, List<Model> indexList, List<Model> yearList)
		{
			OrderID = orderId;
			Customer = customer;
			ShipCountry = country;
			TaxPerAnnum = taxList;
			OneDayIndex = indexList;
			YearGR = yearList;
		}
	}
}