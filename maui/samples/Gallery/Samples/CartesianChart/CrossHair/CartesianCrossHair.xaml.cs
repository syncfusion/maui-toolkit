using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class CartesianCrossHair : SampleView
    {
        int _month = int.MaxValue;

        public CartesianCrossHair()
        {
            InitializeComponent();
        }

        // X axis label formatting logic
        private void Primary_LabelCreated(object sender, Syncfusion.Maui.Toolkit.Charts.ChartAxisLabelEventArgs e)
        {
            DateTime baseDate = new(2024, 09, 16);
            var date = baseDate.AddDays(e.Position);
            if (date.Month != _month)
            {
                ChartAxisLabelStyle labelStyle = new()
                {
                    LabelFormat = "MMM-dd",
                    FontAttributes = FontAttributes.Bold
                };
                e.LabelStyle = labelStyle;
                _month = date.Month;
            }
            else
            {
                ChartAxisLabelStyle labelStyle = new()
                {
                    LabelFormat = "dd"
                };
                e.LabelStyle = labelStyle;
            }
        }
    }
}
