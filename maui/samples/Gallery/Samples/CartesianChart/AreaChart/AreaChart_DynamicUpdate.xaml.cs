
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class AreaChart_DynamicUpdate : SampleView
    {
        public AreaChart_DynamicUpdate()
        {
            InitializeComponent();
            if (!(BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile))
                viewModel1.StartTimer();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            if (BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile)
            {
                viewModel1.StopTimer();
                viewModel1.StartTimer();
            }
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel1?.StopTimer();

            Chart1.Handler?.DisconnectHandler();
        }
    }
}
