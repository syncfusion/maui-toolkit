using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class StepAreaChart : SampleView
	{
		int month = int.MaxValue;

		public StepAreaChart()
		{
			InitializeComponent();

			if ((BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile))
			{
				xAxis.AutoScrollingDelta = 18;
				xAxis.AutoScrollingMode = ChartAutoScrollingMode.Start;
			}
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

			if (!IsCardView)
			{
				yAxis.Title = new ChartAxisTitle() { Text = "Stock Price" };
				xAxis.Title = new ChartAxisTitle() { Text = "Date" };
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart.Handler?.DisconnectHandler();
		}

	}
}