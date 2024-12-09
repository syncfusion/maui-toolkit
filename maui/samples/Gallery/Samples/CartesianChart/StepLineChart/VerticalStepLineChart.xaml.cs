using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class VerticalStepLineChart : SampleView
	{
		public VerticalStepLineChart()
		{
			InitializeComponent();
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
#if IOS
			if (IsCardView)
			{
				Chart2.WidthRequest = 350;
				Chart2.HeightRequest = 400;
				Chart2.VerticalOptions = LayoutOptions.Start;
			}
#endif

			hyperLinkLayout.IsVisible = !IsCardView;
			if (!IsCardView)
			{
				XAxis.Title = new ChartAxisTitle() { Text = "Year" };
				YAxis.Title = new ChartAxisTitle() { Text = "Carbon Intensity" };
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart2.Handler?.DisconnectHandler();
		}
	}
}