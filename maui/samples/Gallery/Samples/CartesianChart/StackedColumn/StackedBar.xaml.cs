using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class StackedBar : SampleView
	{
		public StackedBar()
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
			//hyperLinkLayout.IsVisible = !IsCardView;
			if (!IsCardView)
			{
				chart.Title = (Label)layout.Resources["title"];
				;
				yAxis.Title = new ChartAxisTitle() { Text = "Profit" };
				xAxis.Title = new ChartAxisTitle() { Text = "Product" };
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			chart.Handler?.DisconnectHandler();
		}
	}
}
