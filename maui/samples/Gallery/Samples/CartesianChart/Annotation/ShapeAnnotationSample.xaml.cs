
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class ShapeAnnotationSample : SampleView
    {
        public ShapeAnnotationSample()
        {
            InitializeComponent();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            //Chart.Handler?.DisconnectHandler();
        }
    }
}