namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class VerticalPlotBandWindows : SampleView
	{
		public VerticalPlotBandWindows()
		{
			InitializeComponent();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			verticalPlotBandWindowsChart.Handler?.DisconnectHandler();
		}
	}
}