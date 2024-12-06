using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CandleChart : SampleView
	{
		public CandleChart()
		{
			InitializeComponent();
		}

		int _month = int.MaxValue;

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

			hyperLinkLayout.IsVisible = !IsCardView;

			if (!IsCardView)
			{
				Chart.Title = (Label)layout.Resources["title"];
				xAxis.Title = new ChartAxisTitle() { Text = "Year 2020" };
				YAxis.Title = new ChartAxisTitle() { Text = "Index Price" };
			}
		}
	}
}