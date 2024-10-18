
namespace  Syncfusion.Maui.ControlsGallery.PyramidChart.SfPyramidChart
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
