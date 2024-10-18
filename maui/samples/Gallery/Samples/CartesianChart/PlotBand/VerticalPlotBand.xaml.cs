
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class VerticalPlotBand : SampleView
	{
		public VerticalPlotBand ()
		{
			InitializeComponent ();
		}

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            verticalPlotBandChart.Handler?.DisconnectHandler();
        }
    }
}