using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart;

	public partial class DefaultBubbleChart : SampleView
	{
		public DefaultBubbleChart()
		{
			InitializeComponent();
		}

    public override void OnAppearing()
    {
        base.OnAppearing();
        hyperLinkLayout.IsVisible = !IsCardView;
        if (!IsCardView)
        {
            Chart.Title = (Label)layout.Resources["title"];
            yAxis.Title = new ChartAxisTitle() { Text = "GDP per capita" }; xAxis.Title= new ChartAxisTitle() { Text = "Literacy rate [%]" };
        }
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();
        Chart.Handler?.DisconnectHandler();
    }

    private void yAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
    {
        double position = e.Position;
        if (position >= 1000 && position <= 999999)
        {
            string text = (position / 1000).ToString("C0");
            e.Label = $"{text}K";
        }
        else
        {
            e.Label = $"{position:C0}K";
        }
    }

}
