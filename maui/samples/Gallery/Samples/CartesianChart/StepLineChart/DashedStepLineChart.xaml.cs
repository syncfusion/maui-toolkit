using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class DashedStepLineChart : SampleView
	{
		int _month = int.MaxValue;

		public DashedStepLineChart()
		{
			InitializeComponent();
		}

		private void Primary_LabelCreated(object? sender, ChartAxisLabelEventArgs e)
		{
			DateTime baseDate = new(1899, 12, 30);
			var date = baseDate.AddDays(e.Position);
			if (date.Month != _month)
			{
				ChartAxisLabelStyle labelStyle = new()
				{
					LabelFormat = "MMM-dd",
					FontAttributes = FontAttributes.Bold
				};
				e.LabelStyle = labelStyle;

				_month = date.Month;
			}
			else
			{
				ChartAxisLabelStyle labelStyle = new()
				{
					LabelFormat = "dd"
				};
				e.LabelStyle = labelStyle;
			}
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
#if IOS
			if (IsCardView)
			{
				Chart1.WidthRequest = 350;
				Chart1.HeightRequest = 400;
				Chart1.VerticalOptions = LayoutOptions.Start;
			}
#endif

			hyperLinkLayout.IsVisible = !IsCardView;
			if (!IsCardView)
			{
				XAxis.Title = new ChartAxisTitle() { Text = "Date" };
				YAxis.Title = new ChartAxisTitle() { Text = "Dollars per Gallon" };
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart1.Handler?.DisconnectHandler();
		}
	}
}
