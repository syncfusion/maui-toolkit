using System.Globalization;
namespace Syncfusion.Maui.ControlsGallery.SunburstChart.SfSunburstChart;

public partial class ToolTip : SampleView
{
    public ToolTip()
    {
        InitializeComponent();
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();

        sunburstChart.Handler?.DisconnectHandler();
    }
}

public class SunburstItemIndexConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IList<object> list && parameter is string paramStr && int.TryParse(paramStr, out int index))
        {
            return list.Count > index ? list[index]?.ToString() : string.Empty;
        }

        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}