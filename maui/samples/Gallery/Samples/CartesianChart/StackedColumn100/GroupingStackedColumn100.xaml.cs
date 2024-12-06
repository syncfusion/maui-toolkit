using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GroupingStackedColumn100 : SampleView
	{
		public GroupingStackedColumn100()
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
				Chart1.WidthRequest = 350;
				Chart1.HeightRequest = 400;
				Chart1.VerticalOptions = LayoutOptions.Start;
			}
#endif

			if (!IsCardView)
			{
				XAxis.Title = new ChartAxisTitle() { Text = "Year" };
				YAxis.Title = new ChartAxisTitle() { Text = "Electricity Consumption" };
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart1.Handler?.DisconnectHandler();
		}
	}
}