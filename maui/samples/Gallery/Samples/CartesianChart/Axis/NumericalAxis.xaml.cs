namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class NumericalAxisChart : SampleView
    {
        public NumericalAxisChart()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            hyperLinkLayout.IsVisible = !IsCardView;
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            numericChart.Handler?.DisconnectHandler();
        }
    }
}
