 using Syncfusion.Maui.Toolkit.Charts;
using System.Globalization;

namespace  Syncfusion.Maui.ControlsGallery.PolarChart.SfPolarChart;

public partial class PolarDataLabel : SampleView
{
    public PolarDataLabel()
    {
        InitializeComponent();
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();
        Chart.Handler?.DisconnectHandler();
    }
}

public class IconColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
       if (value is PolarSeries series)
        {
           if(series.Label == "Optimum Health")
            {
                return new SolidColorBrush(Color.FromRgb(0xDF, 0xD4, 0xFF));

            }
            else if (series.Label == "Actual Health")
            {
                return new SolidColorBrush(Color.FromRgb(0x97, 0x30, 0xE7));
            }
        }
      
        return Colors.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class PolarSeriesExt : PolarAreaSeries
{
    protected override void DrawDataLabel(ICanvas canvas, Brush? fillcolor, string label, PointF point, int index)
    {
        if(label != "53")
        {
            base.DrawDataLabel(canvas, fillcolor, label, point, index);
        }
    }
}
