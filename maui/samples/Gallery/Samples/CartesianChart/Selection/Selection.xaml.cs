using Syncfusion.Maui.ControlsGallery;
using Syncfusion.Maui.Toolkit.Charts;
using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class Selection : SampleView
    {
        public Selection()
        {
            InitializeComponent();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            chart.Handler?.DisconnectHandler();
        }

        private void checkbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            dataPointSelection.Type = e.Value ? ChartSelectionType.Multiple : ChartSelectionType.SingleDeselect;
            //series2.Fill = Color.FromArgb("#40314A6E");
        }
    }

    public class SelectionValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return ((DateTime)value).ToString("ddd-hh:mm");
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }

}