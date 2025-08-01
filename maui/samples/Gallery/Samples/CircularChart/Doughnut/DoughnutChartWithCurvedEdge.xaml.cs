namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{
	public partial class DoughnutChartWithCurvedEdge : SampleView
	{
		public DoughnutChartWithCurvedEdge()
		{
			InitializeComponent();
			pathData.Data = doughnutViewModel.TruckPathData;
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			if (IsCardView)
			{
				series.ShowDataLabels = false;

#if IOS
        chart.WidthRequest = 350;
        chart.HeightRequest = 400;
        chart.VerticalOptions = LayoutOptions.Start;
#endif
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			chart.Handler?.DisconnectHandler();
		}
	}
}