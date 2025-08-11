using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
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
