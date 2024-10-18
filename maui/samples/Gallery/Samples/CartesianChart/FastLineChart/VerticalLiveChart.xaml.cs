
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class VerticalLiveChart : SampleView
    {
        public VerticalLiveChart()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            realTimeVerticalChartViewModel.StartVerticalTimer();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            verticalLiveChart.Handler?.DisconnectHandler();
        }
    }
}
