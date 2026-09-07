using Syncfusion.Maui.Toolkit.SunburstChart;
using MAUIPicker = Microsoft.Maui.Controls.Picker;

namespace Syncfusion.Maui.ControlsGallery.SunburstChart.SfSunburstChart;

public partial class DrillDown : SampleView
{
    public DrillDown()
    {
        InitializeComponent();
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();

        sunburstChart.Handler?.DisconnectHandler();
    }
}
