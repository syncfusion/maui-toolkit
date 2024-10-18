using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{

    public partial class RangeAreaChart : SampleView
    {
        int month = int.MaxValue;

        public RangeAreaChart()
        {
            InitializeComponent();
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

        public override void OnAppearing()
        {
            base.OnAppearing();
#if IOS
            if (IsCardView)
            {
                Chart.WidthRequest = 350;
                Chart.HeightRequest = 400;
                Chart.VerticalOptions = LayoutOptions.Start;
            }
#endif
            //hyperLinkLayout is stack layout, In that we added the source link.
            hyperLinkLayout.IsVisible = !IsCardView;
            if (!IsCardView)
            {
                yAxis.Title = new ChartAxisTitle() { Text = "Temperature [°C]" };
            }     
        }
    }
}