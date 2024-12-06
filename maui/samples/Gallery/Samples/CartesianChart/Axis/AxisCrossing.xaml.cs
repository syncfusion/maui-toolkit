
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class AxisCrossing : SampleView
	{
		public AxisCrossing()
		{
			InitializeComponent();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			axisCrossingChart.Handler?.DisconnectHandler();
		}
	}
}
