
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class Column_RoundedEdges : SampleView
    {
        public Column_RoundedEdges()
        {
            InitializeComponent();
        }
        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart1.Handler?.DisconnectHandler();
        }
    }
}
