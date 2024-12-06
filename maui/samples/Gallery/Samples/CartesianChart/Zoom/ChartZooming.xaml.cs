using Syncfusion.Maui.Toolkit.Charts;
using MAUIPicker = Microsoft.Maui.Controls.Picker;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class ChartZooming : SampleView
	{
		public ChartZooming()
		{
			InitializeComponent();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			chart.Handler?.DisconnectHandler();
		}

		private void ZoomModeChanged(object sender, EventArgs e)
		{
			var picker = (MAUIPicker)sender;
			int selectedIndex = picker.SelectedIndex;
			if (selectedIndex == 0)
			{
				zoomingBehavior.ZoomMode = ZoomMode.X;
			}
			else if (selectedIndex == 1)
			{
				zoomingBehavior.ZoomMode = ZoomMode.Y;
			}
			else if (selectedIndex == 2)
			{
				zoomingBehavior.ZoomMode = ZoomMode.XY;
			}
		}
	}
}
