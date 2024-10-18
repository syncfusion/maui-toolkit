
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class Column_WidthCustomization : SampleView
    {
        public Column_WidthCustomization()
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
                Chart2.WidthRequest = 350;
                Chart2.HeightRequest = 400;
                Chart2.VerticalOptions = LayoutOptions.Start;
            }
#endif
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart2.Handler?.DisconnectHandler();
        }
    }
}
