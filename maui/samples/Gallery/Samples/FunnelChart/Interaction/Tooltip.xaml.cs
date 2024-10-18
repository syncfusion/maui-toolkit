
namespace  Syncfusion.Maui.ControlsGallery.FunnelChart.SfFunnelChart
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
