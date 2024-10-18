using Syncfusion.Maui.Toolkit;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{
    public partial class CustomizedRadialBarChart : SampleView
    {
        public CustomizedRadialBarChart()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
#if IOS
            if (IsCardView)
            {
                chart.WidthRequest = 350;
                chart.HeightRequest = 400;
                chart.VerticalOptions = LayoutOptions.Start;
            }
#endif
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            chart.Handler?.DisconnectHandler();
        }
    }

    public class IndexToItemSourceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && value is LegendItem legendItem)
            {
                List<object?> collection = new List<object?>();
                collection.Add(legendItem.Item);
                return collection;
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

