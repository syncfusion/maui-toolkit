using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class DateTimeAxisChart : SampleView
    {
        int month = int.MaxValue;
        public DateTimeAxisChart()
        {
            InitializeComponent();
            dateTimeChart.XAxes[0].LabelCreated += Primary_LabelCreated;
        }

        private void Primary_LabelCreated(object? sender, ChartAxisLabelEventArgs e)
        {
            DateTime baseDate = new(1899, 12, 30);
            var date = baseDate.AddDays(e.Position);
            if (date.Month != month)
            {
                ChartAxisLabelStyle labelStyle = new();
                labelStyle.LabelFormat = "MMM-dd";
                labelStyle.FontAttributes = FontAttributes.Bold;
                e.LabelStyle = labelStyle;

                month = date.Month;
            }
            else
            {
                ChartAxisLabelStyle labelStyle = new();
                labelStyle.LabelFormat = "dd";
                e.LabelStyle = labelStyle;
            }
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            dateTimeChart.Handler?.DisconnectHandler();
        }
    }
}
