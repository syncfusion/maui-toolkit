namespace Syncfusion.Maui.ControlsGallery.SunburstChart.SfSunburstChart;

public partial class DefaultSunburstChart : SampleView
{
    public DefaultSunburstChart()
    {
        InitializeComponent();
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();

        sunburstChart.Handler?.DisconnectHandler();
    }
}