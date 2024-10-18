
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class StackedLine100Chart : SampleView
    {
        public StackedLine100Chart()
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