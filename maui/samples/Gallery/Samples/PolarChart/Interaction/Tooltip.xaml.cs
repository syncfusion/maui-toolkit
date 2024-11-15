
namespace Syncfusion.Maui.ControlsGallery.PolarChart.SfPolarChart
{
	public partial class Tooltip : SampleView
	{
		public Tooltip()
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