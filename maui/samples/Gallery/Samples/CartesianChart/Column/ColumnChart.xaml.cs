
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class ColumnChart : SampleView
    {
        public ColumnChart()
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
