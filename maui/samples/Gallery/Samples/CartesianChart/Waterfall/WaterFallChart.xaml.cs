using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class WaterFallChart : SampleView
    {
        public WaterFallChart()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            if (!IsCardView)
            {
                Chart1.Title = (Label)layout.Resources["title"];
                myYaxis.Title = new ChartAxisTitle() { Text = "Dollar (USD)" }; 
                myXAxis.Title= new ChartAxisTitle() { Text = "Month" };
            }
        }
        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart1.Handler?.DisconnectHandler();
        }
    }
}
