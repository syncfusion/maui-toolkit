using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class DateTimeAxisChart : SampleView
	{
		int _month = int.MaxValue;
		public DateTimeAxisChart()
		{
			InitializeComponent();
			dateTimeChart.XAxes[0].LabelCreated += Primary_LabelCreated;
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

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			dateTimeChart.Handler?.DisconnectHandler();
		}
	}
}
