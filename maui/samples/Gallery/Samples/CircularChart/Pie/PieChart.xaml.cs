 
namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{
    public partial class PieChart : SampleView
    {
        public PieChart()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
#if IOS
            if (IsCardView)
            {
                Chart.WidthRequest = 350;
                Chart.HeightRequest = 400;
                Chart.VerticalOptions = LayoutOptions.Start;
            }
#endif
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart.Handler?.DisconnectHandler();
        }
    }
}
