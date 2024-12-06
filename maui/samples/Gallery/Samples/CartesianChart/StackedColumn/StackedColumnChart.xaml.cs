using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class StackedColumnChart : SampleView
	{
		public StackedColumnChart()
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
				chart.WidthRequest = 350;
				chart.HeightRequest = 400;
				chart.VerticalOptions = LayoutOptions.Start;
			}
#endif

			if (!IsCardView)
			{
				yAxis.Title = new ChartAxisTitle() { Text = "Revenue (EUR)" };
				xAxis.Title = new ChartAxisTitle() { Text = "Year" };
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			chart.Handler?.DisconnectHandler();
		}
	}
}
