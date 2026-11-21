using PickerControl = Microsoft.Maui.Controls.Picker;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class HorizontalPlotBand : SampleView
	{
		public HorizontalPlotBand()
		{
			InitializeComponent();
			YAxis.PlotBands = ViewModel.NumericalPlotBandFillCollection;
		}

		private void Picker_SelectedIndexChanged(object? sender, EventArgs e)
		{
			if (sender is not PickerControl picker)
			{
				return;
			}

			if (picker!.SelectedIndex == 0)
			{
				YAxis.PlotBands = ViewModel.NumericalPlotBandFillCollection;
			}
			else if (picker.SelectedIndex == 1)
			{
				YAxis.PlotBands = ViewModel.NumericalPlotBandLineCollection;
				YAxis.PlotOffsetEnd = 5;
			}
			else if (picker.SelectedIndex == 2)
			{
				YAxis.PlotBands = ViewModel.SegmentPlotBandCollection;
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			horizontalPlotBandChart.Handler?.DisconnectHandler();
		}
	}
}
