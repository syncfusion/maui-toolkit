using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{
    public partial class CenterElevation : SampleView
    {
        public CenterElevation()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
#if IOS
            if (IsCardView)
            {
                Chart1.WidthRequest = 350;
                Chart1.HeightRequest = 400;
                Chart1.VerticalOptions = LayoutOptions.Start;
            }
#endif
        }

    }
}

