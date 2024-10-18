
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class AutoScrolling : SampleView
    {
        public AutoScrolling()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            realTimeViewModel.StopTimer();
            realTimeViewModel.StartTimer();
        }
        
        public override void OnDisappearing()
        {
            base.OnDisappearing();
            realTimeViewModel?.StopTimer();

            categoryChart.Handler?.DisconnectHandler();
        }
    }
}
