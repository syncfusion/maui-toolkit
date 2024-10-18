
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart;

    public partial class StackedLineChart : SampleView
    {
        public StackedLineChart()
        {
            InitializeComponent();
        }
        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart.Handler?.DisconnectHandler();
        }
    }