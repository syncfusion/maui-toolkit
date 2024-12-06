namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class WaterFallChartIsTransposed : SampleView
	{
		public WaterFallChartIsTransposed()
		{
			InitializeComponent();
		}
		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart.Handler?.DisconnectHandler();
		}
		public override void OnAppearing()
		{
			base.OnAppearing();
			headingLabel.IsVisible = !IsCardView;
			firstLabel.IsVisible = !IsCardView;
			secondLabel.IsVisible = !IsCardView;
		}
	}
}
