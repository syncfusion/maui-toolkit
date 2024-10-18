
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart;

public partial class BarBackToBack : SampleView
{
    public BarBackToBack()
    {
        InitializeComponent();
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();
        Chart.Handler?.DisconnectHandler();
    }
}
