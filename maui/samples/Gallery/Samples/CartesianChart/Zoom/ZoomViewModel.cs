using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class ZoomViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> ZoomData { get; set; }

		public string[] ZoomModeType => ["X", "Y", "XY"];

		public ZoomViewModel()
		{
			DateTime date = new(1950, 3, 01);
			Random r = new();
			ZoomData = [];
			for (int i = 0; i < 20; i++)
			{
				ZoomData.Add(new ChartDataModel(date, r.Next(45, 75)));
				date = date.AddDays(5);
			}
		}
	}
}
