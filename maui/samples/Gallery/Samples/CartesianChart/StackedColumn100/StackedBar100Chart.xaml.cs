using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StackedBar100Chart : SampleView
	{
		public StackedBar100Chart()
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

			if (!IsCardView)
			{
				XAxis.Title = new ChartAxisTitle() { Text = "Year" };
				YAxis.Title = new ChartAxisTitle() { Text = "Sales Rate" };
			}
		}
	}
}