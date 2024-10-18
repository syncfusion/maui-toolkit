
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StackedArea100Chart : SampleView
    {
        public StackedArea100Chart()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            hyperLinkLayout.IsVisible = !IsCardView;
#if IOS
            if (IsCardView)
            {
                Chart.WidthRequest = 350;
                Chart.HeightRequest = 400;
                Chart.VerticalOptions = LayoutOptions.Start;
            }
#endif
            if (!IsCardView)
            {
                XAxis.Title = new Syncfusion.Maui.Toolkit.Charts.ChartAxisTitle { Text = "Year" };
            }
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart.Handler?.DisconnectHandler();
        }
    }
}