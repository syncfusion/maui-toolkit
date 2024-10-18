
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class Bar_WidthCustomization : SampleView
    {
        public Bar_WidthCustomization()
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
                Chart3.WidthRequest = 350;
                Chart3.HeightRequest = 400;
                Chart3.VerticalOptions = LayoutOptions.Start;
            }
#endif
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart3.Handler?.DisconnectHandler();
        }
    }
}
