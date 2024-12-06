namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class FastLineChart : SampleView
	{
		public FastLineChart()
		{
			InitializeComponent();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			fastLineChart.Handler?.DisconnectHandler();
		}
	}
}
