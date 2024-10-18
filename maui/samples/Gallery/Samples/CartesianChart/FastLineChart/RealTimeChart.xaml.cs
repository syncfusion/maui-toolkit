
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class RealTimeChart : SampleView
    {
        public RealTimeChart()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            realTimeChartViewModel.StopTimer();
            realTimeChartViewModel.StartTimer();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            realTimeChartViewModel?.StopTimer();

            realTimeChart.Handler?.DisconnectHandler();
        }
    }
}
