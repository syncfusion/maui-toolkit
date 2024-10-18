using PickerControl = Microsoft.Maui.Controls.Picker;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class HorizontalPlotBandWindows : SampleView
    {
        public HorizontalPlotBandWindows()
        {
            InitializeComponent();
            YAxis.PlotBands = ViewModel.NumericalPlotBandFillCollection;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as PickerControl;
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
                YAxis.PlotBands = ViewModel.SegmentPlotBandCollectionWindows;
            }
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            horizontalPlotBandWindowsChart.Handler?.DisconnectHandler();
        }
    }
}
