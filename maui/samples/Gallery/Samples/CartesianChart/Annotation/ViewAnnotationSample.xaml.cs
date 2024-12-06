
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{

	public partial class ViewAnnotationSample : SampleView
	{
		public ViewAnnotationSample()
		{
			InitializeComponent();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart.Handler?.DisconnectHandler();
		}
	}
}