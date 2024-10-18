using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class DefaultRangeBar : SampleView
    {
        public DefaultRangeBar()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            hyperLinkLayout.IsVisible = !IsCardView;
            if (!IsCardView)
            {
                Chart.Title = (Label)layout.Resources["title"];
                yAxis.Title = new ChartAxisTitle() { Text = "Temperature [°C]" };
            }
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart.Handler?.DisconnectHandler();
        }
    }
}
