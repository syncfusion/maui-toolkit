 
namespace  Syncfusion.Maui.ControlsGallery.PyramidChart.SfPyramidChart
{
    public partial class Legend : SampleView
    {
        public Legend()
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
