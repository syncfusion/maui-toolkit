using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class GroupingStackedColumn : SampleView
	{
		public GroupingStackedColumn()
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
			if (!IsCardView)
			{
				chart.Title = (Label)Resources["title"];
				yAxis.Title = new ChartAxisTitle() { Text = "Dollar (USD)" };
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			chart.Handler?.DisconnectHandler();
		}

	}
}
