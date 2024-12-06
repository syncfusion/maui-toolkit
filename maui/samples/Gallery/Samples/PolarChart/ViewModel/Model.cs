namespace Syncfusion.Maui.ControlsGallery.PolarChart.SfPolarChart
{
	public class ChartDataModel
	{
		public string? Category { get; set; }
		public double Value1 { get; set; }
		public double Value2 { get; set; }
		public double Value3 { get; set; }

		#region Constructor

		public ChartDataModel() { }

#pragma warning disable IDE0060 // Remove unused parameter
		public ChartDataModel(string category, double value1, double value2, double value3)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			Category = category;
			Value1 = value1;
			Value2 = value2;
			Value3 = value2;
		}

		public ChartDataModel(string category, double value1, double value2)
		{
			Category = category;
			Value1 = value1;
			Value2 = value2;
		}

		#endregion
	}
}